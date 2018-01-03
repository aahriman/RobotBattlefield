using System;
using BaseLibrary.command.equipment;
using BaseLibrary.protocol;

namespace BaseLibrary.command.v1._0.equipment {
	internal class GetMotorsCommandV1_0 : GetMotorsCommand, ACommand.Sendable {
        public static readonly IFactory<ACommand.Sendable, ACommand> FACTORY = new CommandFactory();
        private sealed class CommandFactory : ACommandFactory {
            internal CommandFactory() : base() { }

            public override bool IsDeserializeable(string s) {
                s = s.Trim();
                if (s.Equals("MOTORS()")) {
                    cache.Cached(s, new GetMotorsCommandV1_0());
                    return true;
                }
                return false;
            }

            public override bool IsTransferable(ACommand c) {
                if (c is GetMotorsCommand){
                    cache.Cached(c, new GetMotorsCommandV1_0());
                    return true;
                }
                return false;
            }
        }

        public GetMotorsCommandV1_0() : base() { }

        public string Serialize() {
            return "MOTORS()";
        }
	}
}
