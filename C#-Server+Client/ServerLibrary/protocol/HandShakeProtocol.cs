﻿using System;
using System.Threading.Tasks;
using BaseLibrary;
using BaseLibrary.communication;
using BaseLibrary.communication.command;
using BaseLibrary.communication.command.handshake;
using BaseLibrary.communication.protocol;
using ServerLibrary.config;

namespace ServerLibrary.protocol {
    /// <summary>
    /// Handle handshake for server side.
    /// </summary>
    public class HandShakeProtocol : AProtocol {
        public HandShakeProtocol() : base(){
            commandsFactory.RegisterCommand(AckCommand.FACTORY);
            commandsFactory.RegisterCommand(HelloCommand.FACTORY);
            commandsFactory.RegisterCommand(OllehCommand.FACTORY);
            commandsFactory.RegisterCommand(ErrorCommand.FACTORY);
        }

        /// <summary>
        /// Do handshake for server side.
        /// </summary>
        /// <param name="serverSocket"> Socket from client side to server side</param>
        /// <returns>null if handShake fail otherwise retur instance of AProtocol</returns>
        public async Task<AProtocol> HandShakeServer(NetworkStream serverSocket) {
            ACommand shoudBeHELLO = GetCommand(await serverSocket.ReadLineAsync());

            if (shoudBeHELLO is HelloCommand) {
                string selectedProtocolLabel; 
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
                await serverSocket.SendCommandAsync(new ErrorCommand(string.Format("Unsupported protocols '{0}'. Handshake failed.", hello.SUPPORTED_PROTOCOLS)));
            } else printIfErrorElseSendMessage(shoudBeHELLO, "Handshake error. Expected HelloCommand but receive:" + shoudBeHELLO.GetType().Name, serverSocket);
            
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
