using System;
using System.Threading.Tasks;
using BaseLibrary;
using BaseLibrary.command;
using BaseLibrary.command.common;
using BaseLibrary.command.handshake;
using BaseLibrary.protocol;
using ClientLibrary.config;

namespace ClientLibrary.protocol {
    public class HandShakeProtocol : AProtocol {
        public HandShakeProtocol() : base(){
            comandsFactory.RegisterCommand(AckCommand.FACTORY);
            comandsFactory.RegisterCommand(HelloCommand.FACTORY);
            comandsFactory.RegisterCommand(OllehCommand.FACTORY);
            comandsFactory.RegisterCommand(ErrorCommand.FACTORY);
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
				ErrorCommand error = new ErrorCommand(string.Format("Unsupported protocols '{0}'. Handshake failed.", olleh.PROTOCOL));
                await error.SendAsync(clientSocket);
            } else {
				printIfErrorElseSendMessage(shouldBeOLLEH, "Handshake error. Expected OllehCommand but receive " + shouldBeOLLEH.GetType().Name, clientSocket);
			}
            return null;
        }

        private void printIfErrorElseSendMessage(ACommand command, string message, SuperNetworkStream socket) {
            if (command is ErrorCommand) {
                Console.Out.WriteLine("ERROR: " + ((ErrorCommand)command).MESSAGE);
            } else {
                ErrorCommand error = new ErrorCommand(message);
                socket.SendCommand(error);
                Console.Out.WriteLine(message);
            }
        }
    }
}
