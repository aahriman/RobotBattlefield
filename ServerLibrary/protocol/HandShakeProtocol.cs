using System;
using System.Threading.Tasks;
using BaseLibrary;
using BaseLibrary.command;
using BaseLibrary.command.common;
using BaseLibrary.command.handshake;
using BaseLibrary.config;
using BaseLibrary.protocol;
using BattlefieldLibrary.config;

namespace ServerLibrary.protocol {
    public class HandShakeProtocol : AProtocol {
        public HandShakeProtocol() : base(){
            comandsFactory.RegisterCommand(AckCommand.FACTORY);
            comandsFactory.RegisterCommand(HelloCommand.FACTORY);
            comandsFactory.RegisterCommand(OllehCommand.FACTORY);
            comandsFactory.RegisterCommand(ErrorCommand.FACTORY);
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

        private void printIfErrorElseSendMessage(ACommand command, String message, SuperNetworkStream socket) {
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
