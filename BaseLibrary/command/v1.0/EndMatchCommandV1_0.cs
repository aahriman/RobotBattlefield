using System;
using BaseLibrary.command.common;
using BaseLibrary.protocol;
using BaseLibrary.utils;

namespace BaseLibrary.command.v1._0 {
	internal class EndMatchCommandV1_0 : EndMatchCommand, ACommand.Sendable {
	    public static readonly IFactory<ACommand.Sendable, ACommand> FACTORY = new CommandFactory();
        private sealed class CommandFactory : ACommandFactory {
            internal CommandFactory() : base() { }

            public override bool IsDeserializeable(string s) {
                s = s.Trim();
                string rest;
				if (StringUtils.GetRestOfString(s, "END_MATCH(", ")", out rest)) {
                    cache.Cached(s, new EndMatchCommandV1_0(rest));
                    return true;
                }
                return false;
            }

            public override bool IsTransferable(ACommand c) {
                if (c is EndMatchCommand) {
                    var c2 = (EndMatchCommand)c;
                    cache.Cached(c, new EndMatchCommandV1_0(c2.FILE_URL));
                    return true;
                }
                return false;
            }
        }

        public EndMatchCommandV1_0(string fileUrl) : base(fileUrl) { }


        public string Serialize() {
            return string.Format("END_MATCH({0})", FILE_URL);
        }
    }
}
