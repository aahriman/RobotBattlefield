using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using BaseLibrary;
using BaseLibrary.battlefield;
using BaseLibrary.command;
using BaseLibrary.config;
using BaseLibrary.protocol;
using HandShakeProtocol = ClientLibrary.protocol.HandShakeProtocol;

namespace ClientLibrary {
    public abstract class AClient : Robot {
        private const string LOCAL_ADDRES = "::1";
        private Socket socket;
        protected SuperNetworkStream sns;

        public AClient() { }

        public void Close() {
            sns.Close();
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
            IAsyncResult beginConnection = socket.BeginConnect(ipe, (ar) => {
                socket.EndConnect(ar);
                try {
                    sns = new SuperNetworkStream(socket);
                } catch (TypeInitializationException e) {
                    throw e.InnerException;
                }
                Task.WaitAll(handShake());
                waitHandle.Set();
            }, socket);
            waitHandle.WaitOne();
            return (GameTypeCommand) await sns.RecieveCommandAsync();
        }


        private async Task handShake() {
            await Task.Yield();
            HandShakeProtocol handShakeProtocol = new HandShakeProtocol();
            AProtocol protocol = await handShakeProtocol.HandShakeClient(sns);
            if (protocol == null) {
                disconnect("Handshake fail.");
            } else {
                sns.PROTOCOL = protocol;
            }
        }

        private async void disconnect(String message) {
            await Task.Yield();
            ErrorCommand error = new ErrorCommand(message);
            await sns.SendCommandAsync(error);
            sns.Close();
        }
    }
}
