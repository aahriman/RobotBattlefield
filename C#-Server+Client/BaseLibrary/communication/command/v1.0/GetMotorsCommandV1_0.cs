using System;

namespace BaseLibrary.command.v1._0 {
	internal class GetMotorsCommandV1_0 : GetMotorsCommand, ACommand.Sendable {
        public static readonly ICommandFactory FACTORY = new CommandFactory();
        private sealed class CommandFactory : ACommandFactory {
            internal CommandFactory() : base() { }

            public override Boolean IsDeserializable(String s) {
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

        public string serialize() {
            return "MOTORS()";
        }
	}
}
