using System;
using System.Collections.Generic;

namespace BaseLibrary.protocol {
    public class Factories<TResult, TTransfer> {
        private readonly List<IFactory<TResult, TTransfer>> commandFactorys = new List<IFactory<TResult, TTransfer>>();

        public void RegisterCommand(IFactory<TResult, TTransfer> factory) {
            commandFactorys.Add(factory);
        }

        public void UnregisterCommand(IFactory<TResult, TTransfer> factory) {
            commandFactorys.Remove(factory);
        }

        public TResult Deserialize(String commandString) {
            foreach (IFactory<TResult, TTransfer> commandFactory in commandFactorys) {
                if (commandFactory.IsDeserializable(commandString)) {
                    return commandFactory.Deserialize(commandString);
                }
            }
            return default(TResult);
        }

        public TResult Transfer(TTransfer command) {
            foreach (IFactory<TResult, TTransfer> commandFactory in commandFactorys) {
                if (commandFactory.IsTransferable(command)) {
                    return commandFactory.Transfer(command);
                }
            }
            return default(TResult);
        }
    }
}
