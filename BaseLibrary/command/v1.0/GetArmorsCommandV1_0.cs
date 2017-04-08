using System;

namespace BaseLibrary.command.v1._0 {
	internal class GetArmorsCommandV1_0 : GetArmorsCommand, ACommand.Sendable {
        public static readonly ICommandFactory FACTORY = new CommandFactory();
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

        public string serialize() {
            return "ARMORS()";
        }
	}
}
