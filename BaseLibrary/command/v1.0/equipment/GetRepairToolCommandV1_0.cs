using System;
using BaseLibrary.command.equipment;
using BaseLibrary.protocol;
using BaseLibrary.utils;

namespace BaseLibrary.command.v1._0.equipment {
    class GetRepairToolCommandV1_0 : GetRepairToolCommand, ACommand.Sendable {
        public const string NAME = "REPAIR_TOOLS";
        public static readonly IFactory<ACommand.Sendable, ACommand> FACTORY = new CommandFactory();
        private sealed class CommandFactory : ACommandFactory {
            internal CommandFactory() : base() {}

            public override bool IsDeserializable(string s) {
                string rest;
                if (ProtocolV1_0Utils.GetParams(s, NAME, out rest)) {
                    cache.Cached(s, new GetRepairToolCommandV1_0());
                    return true;
                }
                return false;
            }

            public override bool IsTransferable(ACommand c) {
                if (c is GetRepairToolCommand) {
                    cache.Cached(c, new GetRepairToolCommandV1_0());
                    return true;
                }
                return false;
            }
        }

        public GetRepairToolCommandV1_0() : base() {}

        public string Serialize() {
            return ProtocolV1_0Utils.SerializeParams(NAME);
        }
    }
}
