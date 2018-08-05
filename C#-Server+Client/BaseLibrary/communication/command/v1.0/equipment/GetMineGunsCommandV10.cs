using BaseLibrary.communication.command.equipment;
using BaseLibrary.communication.protocol;
using BaseLibrary.utils;

namespace BaseLibrary.communication.command.v1._0.equipment {
    class GetMineGunsCommandV10 : GetMineGunsCommand, ACommand.Sendable {
        public const string NAME = "MINE_GUNS";
        public static readonly IFactory<ACommand.Sendable, ACommand> FACTORY = new CommandFactory();
        private sealed class CommandFactory : ACommandFactory {
            internal CommandFactory() : base() { }

            public override bool IsDeserializeable(string s) {
                string rest;
                if (ProtocolV1_0Utils.GetParams(s, NAME, out rest)) {
                    cache.Cached(s, new GetMineGunsCommandV10());
                    return true;
                }
                return false;
            }

            public override bool IsTransferable(ACommand c) {
                if (c is GetMineGunsCommand) {
                    cache.Cached(c, new GetMineGunsCommandV10());
                    return true;
                }
                return false;
            }
        }

        public GetMineGunsCommandV10() : base() { }

        public string Serialize() {
            return ProtocolV1_0Utils.SerializeParams(NAME);
        }
    }
}
