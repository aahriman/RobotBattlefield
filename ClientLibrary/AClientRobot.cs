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
using HandShakeProtocol = ClientLibrary.protocol.HandShakeProtocol;

namespace ClientLibrary {
    public sealed class ConnectionUtil {
        public const string LOCAL_ADDRES = "::1";
        private Socket socket;
        public SuperNetworkStream COMMUNICATION { get; private set; }

        public ConnectionUtil() { }

        public void Close() {
            COMMUNICATION.Close();
            socket.Close();
        }

        public async Task<GameTypeCommand> ConnectAsync() {
            return await ConnectAsync(GameProperties.DEFAULT_PORT);
        }

        public async Task<GameTypeCommand> ConnectAsync(int port) {
            return await ConnectAsync(LOCAL_ADDRES, port);
        }

        public async Task<GameTypeCommand> ConnectAsync(String url, int port) {
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
                socket.EndConnect(ar);
                try {
                    COMMUNICATION = new SuperNetworkStream(socket);
                } catch (TypeInitializationException e) {
                    throw e.InnerException;
                }
                handShake();
                waitHandle.Set();
            }, socket);
            waitHandle.WaitOne();
            return (GameTypeCommand) await COMMUNICATION.RecieveCommandAsync();
        }


        private void handShake() {
            HandShakeProtocol handShakeProtocol = new HandShakeProtocol();
            AProtocol protocol = handShakeProtocol.HandShakeClient(COMMUNICATION).Result;
            if (protocol == null) {
                disconnect("Handshake fail.");
            } else {
                COMMUNICATION.PROTOCOL = protocol;
            }
        }

        private async void disconnect(String message) {
            await Task.Yield();
            ErrorCommand error = new ErrorCommand(message);
            await COMMUNICATION.SendCommandAsync(error);
            COMMUNICATION.Close();
        }
    }
}
