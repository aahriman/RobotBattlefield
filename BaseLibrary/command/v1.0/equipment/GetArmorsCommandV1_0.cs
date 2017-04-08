using System;
using BaseLibrary.command.equipment;
using BaseLibrary.protocol;

namespace BaseLibrary.command.v1._0.equipment {
	internal class GetArmorsCommandV1_0 : GetArmorsCommand, ACommand.Sendable {
        public static readonly IFactory<ACommand.Sendable, ACommand> FACTORY = new CommandFactory();
        private sealed class CommandFactory : ACommandFactory {
            internal CommandFactory() : base() { }

            public override Boolean IsDeserializable(String s) {
                s = s.Trim();
                if (s.Equals("ARMORS()")) {
                    cache.Cached(s, new GetArmorsCommandV1_0());
                    return true;
                }
                return false;
            }

            public override bool IsTransferable(ACommand c) {
                if (c is GetArmorsCommand){
                    cache.Cached(c, new GetArmorsCommandV1_0());
                    return true;
                }
                return false;
            }
        }

        public GetArmorsCommandV1_0() : base() { }

        public string Serialize() {
            return "ARMORS()";
        }
	}
}
