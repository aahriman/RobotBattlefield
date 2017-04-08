using System;
using System.Collections.Generic;

namespace CommunicationLibrary.command {
    public class CommandsFactory {
        private readonly List<ICommandFactory> commandFactorys = new List<ICommandFactory>();

        public void registerCommand(ICommandFactory commandFactory) {
            commandFactorys.Add(commandFactory);
        }

        public void unregisterCommand(ICommandFactory commandFactory) {
            commandFactorys.Remove(commandFactory);
        }

        public ACommand.Sendable Deserialize(String commandString) {
            foreach (ICommandFactory commandFactory in commandFactorys) {
                if (commandFactory.IsDeserializable(commandString)) {
                    return commandFactory.Deserialize(commandString);
                }
            }
            return null;
        }

        public ACommand.Sendable Transfer(ACommand command) {
            foreach (ICommandFactory commandFactory in commandFactorys) {
                if (commandFactory.IsTransferable(command)) {
                    return commandFactory.Transfer(command);
                }
            }
            return null;
        }
    }
}
