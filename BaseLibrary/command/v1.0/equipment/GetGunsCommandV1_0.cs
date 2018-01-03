using System;
using BaseLibrary.command.equipment;
using BaseLibrary.protocol;

namespace BaseLibrary.command.v1._0.equipment {
	public class GetGunsCommandV1_0 : GetGunsCommand, ACommand.Sendable {
        public static readonly IFactory<ACommand.Sendable, ACommand> FACTORY = new CommandFactory();
        private sealed class CommandFactory : ACommandFactory {
            internal CommandFactory() : base() { }

            public override bool IsDeserializeable(string s) {
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

        public string Serialize() {
            return "GUNS()";
        }
	}
}
