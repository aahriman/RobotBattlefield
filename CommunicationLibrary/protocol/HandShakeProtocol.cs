using System;
using System.Threading.Tasks;
using CommunicationLibrary.command;

namespace CommunicationLibrary.protocol {
    public class HandShakeProtocol : AProtocol {
        public HandShakeProtocol() : base(){
            comandsFactory.registerCommand(AckCommand.FACTORY);
            comandsFactory.registerCommand(HelloCommand.FACTORY);
            comandsFactory.registerCommand(OllehCommand.FACTORY);
            comandsFactory.registerCommand(ErrorCommand.FACTORY);
        }

        /// <summary>
        /// Do handshake for server side.
        /// </summary>
        /// <param name="serverSocket"> Socket from client side to server side</param>
        /// <returns>null if handShake fail otherwise retur instance of AProtocol</returns>
        public async Task<AProtocol> HandShakeServer(SuperNetworkStream serverSocket) {
            ACommand shoudBeHELLO = GetCommand(await serverSocket.ReadLineAsync());

            if (shoudBeHELLO is HelloCommand) {
                String selectedProtocolLabel; 
                HelloCommand hello = (HelloCommand)shoudBeHELLO;
                AProtocol protocol = ServerConfig.PROTOCOL_FACTORY.GetProtocol(out selectedProtocolLabel, hello.SUPPORTED_PROTOCOLS);
                if(protocol != null){
                    OllehCommand olleh = new OllehCommand(selectedProtocolLabel);
					await olleh.SendAsync(serverSocket);

                    ACommand shoudBeACK = GetCommand(await serverSocket.ReadLineAsync());
                    if (shoudBeACK is AckCommand) {
                        return protocol;
                    } else printIfErrorElseSendMessage(shoudBeACK, "Handshake error. Expected HelloCommand but receive:" + shoudBeACK.GetType().Name, serverSocket);
                }
                await serverSocket.SendCommandAsync(new ErrorCommand(String.Format("Unsupported protocols '{0}'. Handshake failed.", hello.SUPPORTED_PROTOCOLS)));
            } else printIfErrorElseSendMessage(shoudBeHELLO, "Handshake error. Expected HelloCommand but receive:" + shoudBeHELLO.GetType().Name, serverSocket);
            
            return null;
        }

        /// <summary>
        /// Do handshake for client side.
        /// </summary>
        /// <param name="clientSocket"> Socket from client side to server side</param>
        /// <returns>null if handShake fail otherwise return instance of AProtocol</returns>
        public async Task<AProtocol> HandShakeClient(SuperNetworkStream clientSocket) {
            HelloCommand hello = new HelloCommand(ClientConfig.SUPPERTED_PROTOCOLS);
            await hello.SendAsync(clientSocket);
            ACommand shouldBeOLLEH = GetCommand(await clientSocket.ReadLineAsync());
            if (shouldBeOLLEH is OllehCommand) {
                OllehCommand olleh = (OllehCommand)shouldBeOLLEH;
                AProtocol protocol = ClientConfig.PROTOCOL_FACTORY.GetProtocol(olleh.PROTOCOL);
                if (protocol != null) {
                    AckCommand ack = new AckCommand();
					await ack.SendAsync(clientSocket);
                    return protocol;
                }
				ErrorCommand error = new ErrorCommand(String.Format("Unsupported protocols '{0}'. Handshake failed.", olleh.PROTOCOL));
                await error.SendAsync(clientSocket);
            } else {
				printIfErrorElseSendMessage(shouldBeOLLEH, "Handshake error. Expected OllehCommand but receive " + shouldBeOLLEH.GetType().Name, clientSocket);
			}
            return null;
        }

        private void printIfErrorElseSendMessage(ACommand command, String message, SuperNetworkStream socket) {
            if (command is ErrorCommand) {
                Console.Out.WriteLine("ERROR: " + ((ErrorCommand)command).MESSAGE);
            } else {
                ErrorCommand error = new ErrorCommand(message);
                socket.SendCommandAsyncDontWait(error);
                Console.Out.WriteLine(message);
            }
        }
    }
}
