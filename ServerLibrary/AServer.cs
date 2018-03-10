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
	    private readonly List<NetworkStream> networkStreamPool = new List<NetworkStream>();
	    protected Battlefield Battlefield;
        protected int port;
        
        public AServer(int port) {
            this.port = port;
        }


        public void Close() {
            lock (networkStreamPool) {
                foreach (var i in networkStreamPool) {
                    i.Close();
                }
            }
        }

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

        protected async void disconnect(NetworkStream n, string message) {
            lock (networkStreamPool) {
                networkStreamPool.Remove(n);
            }
            await Task.Yield();
            ErrorCommand error = new ErrorCommand(message);
            await error.SendAsync(n);
            n.Close();
        }

        public Battlefield GetBattlefield(BattlefieldConfig battlefielConfig) {
            if (battlefielConfig.RANDOM_SEED == null) {
                int RANDOM_SEED = new Random().Next();
                battlefielConfig = new BattlefieldConfig(battlefielConfig.MAX_TURN, battlefielConfig.MAX_LAP, battlefielConfig.TEAMS, battlefielConfig.ROBOTS_IN_TEAM, battlefielConfig.RESPAWN_TIMEOUT, battlefielConfig.RESPAWN_ALLOWED, battlefielConfig.MATCH_SAVE_FILE, battlefielConfig.EQUIPMENT_CONFIG_FILE, battlefielConfig.OBSTACLE_CONFIG_FILE, battlefielConfig.WAITING_TIME_BETWEEN_TURNS, battlefielConfig.GUI, RANDOM_SEED, battlefielConfig.MORE);
                Console.WriteLine("Using random seed: " + RANDOM_SEED);
            }
            Battlefield = NewBattlefield(battlefielConfig);

            Socket socketIPv6 = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
            Socket socketIPv4 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            myBindSocket(socketIPv4, IPAddress.Any, port);
            myBindSocket(socketIPv6, IPAddress.IPv6Any, port);

            Console.WriteLine("Arena runs on " + String.Join(", ", (IEnumerable<IPAddress>)Dns.GetHostAddresses(Dns.GetHostName())) + " with port " + port);

            return Battlefield;
        }

        protected abstract Battlefield NewBattlefield(BattlefieldConfig battlefielConfig);

        public abstract GameTypeCommand GetGameTypeCommand(Battlefield Battlefield);
    }
}
