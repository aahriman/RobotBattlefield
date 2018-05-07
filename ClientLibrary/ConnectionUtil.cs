using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using BaseLibrary;
using BaseLibrary.battlefield;
using BaseLibrary.command;
using BaseLibrary.command.common;
using BaseLibrary.command.handshake;
using BaseLibrary.config;
using BaseLibrary.protocol;
using ClientLibrary.protocol;
using NetworkStream = BaseLibrary.NetworkStream;

namespace ClientLibrary {
    /// <summary>
    /// Utils for connection to server.
    /// </summary>
    public sealed class ConnectionUtil {
        /// <summary>
        /// Local ip address.
        /// </summary>
        public const string LOCAL_ADDRESS = "::1";

        private Socket socket;
        /// <summary>
        /// Instance for communication with server.
        /// </summary>
        public NetworkStream COMMUNICATION { get; private set; }
        
        public ConnectionUtil() { }

        /// <summary>
        /// Close communication.
        /// </summary>
        public void Close() {
            COMMUNICATION.Close();
            socket.Close();
        }

        /// <summary>
        /// Connect to server with specified url and port.
        /// </summary>
        /// <param name="url">Url where to connect. <code>IPAddress</code> is got by <code>IPAddress.Parse(url)</code>.</param>
        /// <param name="port">Port where to connect.</param>
        /// <see cref="IPAddress"/>
        /// <see cref="IPAddress.Parse"/>
        /// <returns></returns>
        public async Task<GameTypeCommand> ConnectAsync(string url, int port) {
            IPAddress ipAddress = IPAddress.Parse(url);
            if (ipAddress.AddressFamily == AddressFamily.InterNetworkV6) {
                socket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
            } else {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            }
            IPEndPoint ipe = new IPEndPoint(ipAddress, port);
            await Task.Yield();
            ManualResetEvent waitHandle = new ManualResetEvent(false);
            socket.BeginConnect(ipe, (ar) => {
                try {
                    socket.EndConnect(ar);
                } catch {
                    throw new Exception("Cannot connect to server. Is server running?");
                }
                try {
                    COMMUNICATION = new NetworkStream(socket);
                } catch (TypeInitializationException e) {
                    if (e.InnerException != null) throw e.InnerException;
                }
                handShake();
                waitHandle.Set();
            }, socket);
            waitHandle.WaitOne();
            return (GameTypeCommand) await COMMUNICATION.ReceiveCommandAsync();
        }

        /// <summary>
        /// Do handshake at client client side.
        /// </summary>
        private void handShake() {
            HandShakeProtocol handShakeProtocol = new HandShakeProtocol();
            AProtocol protocol = handShakeProtocol.HandShakeClient(COMMUNICATION).Result;
            if (protocol == null) {
                disconnect("Handshake fail.");
            } else {
                COMMUNICATION.PROTOCOL = protocol;
            }
        }

        /// <summary>
        /// Disconnect client and print message.
        /// </summary>
        /// <param name="message"></param>
        private async void disconnect(string message) {
            await Task.Yield();
            Console.Error.WriteLine(message);
            ErrorCommand error = new ErrorCommand(message);
            await COMMUNICATION.SendCommandAsync(error);
            COMMUNICATION.Close();
        }
    }
}
