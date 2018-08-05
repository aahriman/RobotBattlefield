using BaseLibrary.communication.command.miner;
using BaseLibrary.communication.protocol;
using BaseLibrary.utils;

namespace BaseLibrary.communication.command.v1._0.miner {
    public class PutMineCommandV1_0 : PutMineCommand, ACommand.Sendable {

        private const string NAME = "PUT_MINE";
        public static readonly IFactory<ACommand.Sendable, ACommand> FACTORY = new CommandFactory();
        private sealed class CommandFactory : ACommandFactory {
            internal CommandFactory() : base() { }

            public override bool IsDeserializeable(string s) {
                string[] rest;
                if (ProtocolV1_0Utils.GetParams(s, NAME, out rest)) {
                    cache.Cached(s, new PutMineCommandV1_0());
                    return true;
                }
                return false;
            }

            public override bool IsTransferable(ACommand c) {
                PutMineCommand command = c as PutMineCommand;
                if (command != null) {
                    cache.Cached(command, new PutMineCommandV1_0());
                    return true;
                }
                return false;
            }
        }


        public string Serialize() {
            return ProtocolV1_0Utils.SerializeParams(NAME);
        }
    }
}
