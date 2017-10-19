using System;
using System.Collections.Generic;

namespace BaseLibrary.protocol {
    public class Factories<TResult, TTransfer> {
        private readonly List<IFactory<TResult, TTransfer>> commandFactories = new List<IFactory<TResult, TTransfer>>();

        public void RegisterCommand(IFactory<TResult, TTransfer> factory) {
            commandFactories.Add(factory);
        }

        public void UnregisterCommand(IFactory<TResult, TTransfer> factory) {
            commandFactories.Remove(factory);
        }

        public TResult Deserialize(string transferString) {
            foreach (IFactory<TResult, TTransfer> commandFactory in commandFactories) {
                if (commandFactory.IsDeserializable(transferString)) {
                    return commandFactory.Deserialize(transferString);
                }
            }
            return default(TResult);
        }

        public TResult Transfer(TTransfer transfer) {
            foreach (IFactory<TResult, TTransfer> commandFactory in commandFactories) {
                if (commandFactory.IsTransferable(transfer)) {
                    return commandFactory.Transfer(transfer);
                }
            }
            return default(TResult);
        }

        public string Serialize(TTransfer transfer) {
            foreach (IFactory<TResult, TTransfer> commandFactory in commandFactories) {
                if (commandFactory.IsSerializable(transfer)) {
                    return commandFactory.Serialize(transfer);
                }
            }
            return default(string);
        }
    }
}
