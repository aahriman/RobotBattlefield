using System;

namespace BaseLibrary.command.v1._0 {
	internal class GetGunsCommandV1_0 : GetGunsCommand, ACommand.Sendable {
        public static readonly ICommandFactory FACTORY = new CommandFactory();
        private sealed class CommandFactory : ACommandFactory {
            internal CommandFactory() : base() { }

            public override Boolean IsDeserializable(String s) {
                s = s.Trim();
                if (s.Equals("GUNS()")) {
                    cache.Cached(s, new GetGunsCommandV1_0());
                    return true;
                }
                return false;
            }

            public override bool IsTransferable(ACommand c) {
                if (c is GetGunsCommand){
                    cache.Cached(c, new GetGunsCommandV1_0());
                    return true;
                }
                return false;
            }
        }

        public GetGunsCommandV1_0() : base() { }

        public string serialize() {
            return "GUNS()";
        }
	}
}
