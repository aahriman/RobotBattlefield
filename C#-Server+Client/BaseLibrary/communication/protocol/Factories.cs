using System.Collections.Generic;

namespace BaseLibrary.communication.protocol {
    public class Factories<TResult, TTransfer> {
        private readonly List<IFactory<TResult, TTransfer>> commandFactories = new List<IFactory<TResult, TTransfer>>();

        public void RegisterCommand(IFactory<TResult, TTransfer> factory) {
            if (factory == null) return;
            commandFactories.Add(factory);
        }

        public void UnregisterCommand(IFactory<TResult, TTransfer> factory) {
            if (factory == null) return;
            commandFactories.Remove(factory);
        }

        public TResult Deserialize(string transferString) {
            if (transferString != null) {
                foreach (IFactory<TResult, TTransfer> commandFactory in commandFactories) {
                    if (commandFactory.IsDeserializeable(transferString)) {
                        return commandFactory.Deserialize(transferString);
                    }
                }
            }
            return default(TResult);
        }

        public TResult Transfer(TTransfer transfer) {
            if (transfer != null) {
                foreach (IFactory<TResult, TTransfer> commandFactory in commandFactories) {
                    if (commandFactory.IsTransferable(transfer)) {
                        return commandFactory.Transfer(transfer);
                    }
                }
            }
            return default(TResult);
        }

        public string Serialize(TTransfer transfer) {
            if (transfer != null) {
                foreach (IFactory<TResult, TTransfer> commandFactory in commandFactories) {
                    if (commandFactory.IsSerializable(transfer)) {
                        return commandFactory.Serialize(transfer);
                    }
                }
            }
            return default(string);
        }
    }
}
