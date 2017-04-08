using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using BaseLibrary;
using BaseLibrary.command;
using BaseLibrary.protocol;
using BattlefieldLibrary.battlefield;
using ServerLibrary.protocol;

namespace BattlefieldLibrary {
    public abstract class AServer {
	    private readonly List<SuperNetworkStream> networkStreamPool = new List<SuperNetworkStream>();
	    protected Battlefield Battlefield;
        
        public AServer(int port) {
            Socket socketIPv6 = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
            Socket socketIPv4 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            
            myBindSocket(socketIPv4, IPAddress.Any, port);
            myBindSocket(socketIPv6, IPAddress.IPv6Any, port);
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
                SuperNetworkStream sns = new SuperNetworkStream(s);
                lock (networkStreamPool) {
                    networkStreamPool.Add(sns);
                }
                handshake(sns);
                accept(socket);
            }, socket);
        }

        protected async void handshake(SuperNetworkStream sns) {
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

        protected async void disconnect(SuperNetworkStream n, String message) {
            lock (networkStreamPool) {
                networkStreamPool.Remove(n);
            }
            await Task.Yield();
            ErrorCommand error = new ErrorCommand(message);
            await error.SendAsync(n);
            n.Close();
        }

        public Battlefield GetBattlefield(int ROBOT_TO_ONE_ARENA, int ROBOTS_IN_TEAM, params object[] more) {
            Battlefield = NewBattlefield(ROBOT_TO_ONE_ARENA, ROBOTS_IN_TEAM, more);
            return Battlefield;
        }

        public Battlefield GetBattlefield(int ROBOT_TO_ONE_ARENA, int ROBOTS_IN_TEAM, string EQUIPMENT_FILE, params object[] more) {
            Battlefield = NewBattlefield(ROBOT_TO_ONE_ARENA, ROBOTS_IN_TEAM, EQUIPMENT_FILE);
            return Battlefield;
        }

        protected abstract Battlefield NewBattlefield(int ROBOT_TO_ONE_ARENA, int ROBOTS_IN_TEAM, params object[] more);

        protected abstract Battlefield NewBattlefield(int ROBOT_TO_ONE_ARENA, int ROBOTS_IN_TEAM, string EQUIPMENT_FILE, params object[] more);

        public abstract GameTypeCommand GetGameTypeCommand(Battlefield Battlefield);
    }
}
