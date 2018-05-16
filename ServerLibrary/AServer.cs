using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using BaseLibrary.communication.command;
using BaseLibrary.communication.command.handshake;
using BaseLibrary.communication.protocol;
using BattlefieldLibrary.battlefield;
using ServerLibrary.protocol;
using NetworkStream = BaseLibrary.communication.NetworkStream;

namespace ServerLibrary {
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

        private static readonly String ERROR_FILE_NAME;

        static AServer () {
            if (!System.Diagnostics.Debugger.IsAttached) { // add handler for non debugging

                uint numberOfTry = 0;
                while (true) {
                    numberOfTry++;
                    try {
                        ERROR_FILE_NAME = errorName(numberOfTry);
                        Console.SetError(new IndentedTextWriter(File.AppendText(ERROR_FILE_NAME)));
                        break;
                    } catch {
                        // cannot open file, try next one
                        if (numberOfTry > 1000) {
                            Console.WriteLine("Cannot open error file. Application will be closed.");
                            Thread.Sleep(1000);
                            Environment.Exit(2);
                        }
                    }
                }

                AppDomain currentDomain = AppDomain.CurrentDomain;
                currentDomain.UnhandledException += new UnhandledExceptionEventHandler(MyHandler);
                Thread.GetDomain().UnhandledException += new UnhandledExceptionEventHandler(MyHandler);
            }
        }

        private static String errorName(uint numberOfTry) {
            return $"{System.Reflection.Assembly.GetEntryAssembly().GetName().Name}-error-{numberOfTry}.txt";
        }

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
            Battlefield = NewBattlefield(battlefieldConfig);

            Socket socketIPv6 = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
            Socket socketIPv4 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            myBindSocket(socketIPv4, IPAddress.Any, port);
            myBindSocket(socketIPv6, IPAddress.IPv6Any, port);

            Console.WriteLine("Arena runs on ip " + String.Join(" or ", (IEnumerable<IPAddress>)Dns.GetHostAddresses(Dns.GetHostName()).Where(a => a.AddressFamily == AddressFamily.InterNetwork || a.IsIPv6LinkLocal)) + " and port " + port);

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

        public static void MyHandler(object sender, UnhandledExceptionEventArgs e) {
            Console.Error.WriteLine(DateTime.Now);
            Console.Error.WriteLine(e.ExceptionObject);
            Console.Error.Flush();
            if (e.ExceptionObject is Exception ex) {
                Console.WriteLine("Some error occurs:'" + ex.Message + "'. Application store more information in " + ERROR_FILE_NAME + " and will be closed.");
            } else {
                Console.WriteLine("Some error occurs. Application store more information in " + ERROR_FILE_NAME + " and will be closed.");
            }
            Thread.Sleep(5000);
            Environment.Exit(1);
        }
    }
}
