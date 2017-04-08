using System;
using BaseLibrary.command.equipment;
using BaseLibrary.protocol;
using BaseLibrary.utils;

namespace BaseLibrary.command.v1._0.equipment {
    class GetMineGunCommandV1_0 : GetMineGunCommand, ACommand.Sendable {
        public const String NAME = "MINE_GUN";
        public static readonly IFactory<ACommand.Sendable, ACommand> FACTORY = new CommandFactory();
        private sealed class CommandFactory : ACommandFactory {
            internal CommandFactory() : base() { }

            public override Boolean IsDeserializable(String s) {
                String rest;
                if (ProtocolV1_0Utils.GetParams(s, NAME, out rest)) {
                    cache.Cached(s, new GetMineGunCommandV1_0());
                    return true;
                }
                return false;
            }

            public override bool IsTransferable(ACommand c) {
                if (c is GetMineGunCommand) {
                    cache.Cached(c, new GetMineGunCommandV1_0());
                    return true;
                }
                return false;
            }
        }

        public GetMineGunCommandV1_0() : base() { }

        public string Serialize() {
            return ProtocolV1_0Utils.SerializeParams(NAME);
        }
    }
}
