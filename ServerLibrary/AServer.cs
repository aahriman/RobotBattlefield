using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using BaseLibrary;
using BaseLibrary.command;
using BaseLibrary.command.common;
using BaseLibrary.command.handshake;
using BaseLibrary.protocol;
using BattlefieldLibrary.battlefield;
using ServerLibrary.protocol;
using NetworkStream = BaseLibrary.NetworkStream;

namespace BattlefieldLibrary {
    public abstract class AServer {
        /// <summary>
        /// 
        /// </summary>
	    private readonly List<NetworkStream> networkStreamPool = new List<NetworkStream>();
        /// <summary>
        /// Created battlefield
        /// </summary>
	    protected Battlefield Battlefield;
        protected int port;
        
        protected AServer(int port) {
            this.port = port;
        }

        /// <summary>
        /// Close server - close all networkStream
        /// </summary>
        public void Close() {
            lock (networkStreamPool) {
                foreach (var i in networkStreamPool) {
                    i.Close();
                }
            }
        }

        /// <summary>
        /// Bind socket to listen at address and port.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        protected void myBindSocket(Socket socket, IPAddress ip, int port) {
            IPEndPoint ipeV6 = new IPEndPoint(ip, port);
            socket.Bind(ipeV6);
            socket.Listen(10);
            accept(socket);
        }

        protected async void accept(Socket socket) {
            await Task.Yield();
            socket.BeginAccept((ar) => {
                Socket s = socket.EndAccept(ar);
                NetworkStream sns = new NetworkStream(s);
                lock (networkStreamPool) {
                    networkStreamPool.Add(sns);
                }
                handshake(sns);
                accept(socket);
            }, socket);
        }

        protected async void handshake(NetworkStream sns) {
            HandShakeProtocol handshakeProtocol = new HandShakeProtocol();
            await Task.Yield();
            AProtocol protocol = await handshakeProtocol.HandShakeServer(sns);
            if (protocol == null) {
                disconnect(sns, "Handshake fail.");
            } else {
                sns.PROTOCOL = protocol;
                await sns.SendCommandAsync(GetGameTypeCommand(Battlefield));

                if (!Battlefield.AddRobot(sns)) {
                    disconnect(sns, "Arena is full.");
                }
            }
        }

        /// <summary>
        /// Disconnect networkStream from server and send message.
        /// </summary>
        /// <param name="n"></param>
        /// <param name="message"></param>
        protected async void disconnect(NetworkStream n, string message) {
            lock (networkStreamPool) {
                networkStreamPool.Remove(n);
            }
            await Task.Yield();
            ErrorCommand error = new ErrorCommand(message);
            await error.SendAsync(n);
            n.Close();
        }

        /// <summary>
        /// Create battlefield at server with config.
        /// </summary>
        /// <param name="battlefieldConfig"></param>
        /// <returns></returns>
        public Battlefield GetBattlefield(BattlefieldConfig battlefieldConfig) {
            if (battlefieldConfig.RANDOM_SEED == null) {
                int RANDOM_SEED = new Random().Next();
                battlefieldConfig = new BattlefieldConfig(battlefieldConfig.MAX_TURN, battlefieldConfig.MAX_LAP, battlefieldConfig.TEAMS, battlefieldConfig.ROBOTS_IN_TEAM, battlefieldConfig.RESPAWN_TIMEOUT, battlefieldConfig.RESPAWN_ALLOWED, battlefieldConfig.MATCH_SAVE_FILE, battlefieldConfig.EQUIPMENT_CONFIG_FILE, battlefieldConfig.OBSTACLE_CONFIG_FILE, battlefieldConfig.WAITING_TIME_BETWEEN_TURNS, battlefieldConfig.GUI, RANDOM_SEED, battlefieldConfig.MORE);
                Console.WriteLine("Using random seed: " + RANDOM_SEED);
            }
            Battlefield = NewBattlefield(battlefieldConfig);

            Socket socketIPv6 = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
            Socket socketIPv4 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            myBindSocket(socketIPv4, IPAddress.Any, port);
            myBindSocket(socketIPv6, IPAddress.IPv6Any, port);

            Console.WriteLine("Arena runs on " + String.Join(", ", (IEnumerable<IPAddress>)Dns.GetHostAddresses(Dns.GetHostName())) + " with port " + port);

            return Battlefield;
        }

        /// <summary>
        /// Create battlefield with config (used from GetBattlefield)
        /// </summary>
        /// <param name="battlefieldConfig"></param>
        /// <returns></returns>
        protected abstract Battlefield NewBattlefield(BattlefieldConfig battlefieldConfig);

        /// <summary>
        /// Get GameTypeCommand for battlefield (used after success connection).
        /// </summary>
        /// <param name="Battlefield"></param>
        /// <returns></returns>
        public abstract GameTypeCommand GetGameTypeCommand(Battlefield Battlefield);
    }
}
