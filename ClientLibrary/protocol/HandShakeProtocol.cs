using System;
using System.Threading.Tasks;
using BaseLibrary;
using BaseLibrary.command;
using BaseLibrary.command.common;
using BaseLibrary.command.handshake;
using BaseLibrary.protocol;
using ClientLibrary.config;

namespace ClientLibrary.protocol {
    /// <summary>
    /// Handshake protocol for client side.
    /// </summary>
    public class HandShakeProtocol : AProtocol {

        public HandShakeProtocol() : base(){
            commandsFactory.RegisterCommand(AckCommand.FACTORY);
            commandsFactory.RegisterCommand(HelloCommand.FACTORY);
            commandsFactory.RegisterCommand(OllehCommand.FACTORY);
            commandsFactory.RegisterCommand(ErrorCommand.FACTORY);
        }

        /// <summary>
        /// Do handshake for client side.
        /// </summary>
        /// <param name="clientSocket"> Socket from client side to server side</param>
        /// <returns>null if handShake fail otherwise return instance of AProtocol</returns>
        public async Task<AProtocol> HandShakeClient(NetworkStream clientSocket) {
            HelloCommand hello = new HelloCommand(ClientConfig.SUPPORTED_PROTOCOLS);
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

        private void printIfErrorElseSendMessage(ACommand command, string message, NetworkStream socket) {
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
