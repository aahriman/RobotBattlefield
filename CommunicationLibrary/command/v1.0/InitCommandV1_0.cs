using System;
using CommunicationLibrary.utils;

namespace CommunicationLibrary.command.v1._0 {
	public class InitCommandV1_0 : InitCommand, ACommand.Sendable {
        public static readonly ICommandFactory FACTORY = new CommandFactory();
        private sealed class CommandFactory : ACommandFactory {
            internal CommandFactory() : base() { }

            public override Boolean IsDeserializable(String s) {
                s = s.Trim();
                string rest;
				if (StringUtils.GetRestOfString(s, "INIT(", ")", out rest)) {
                    cache.Cached(s, new InitCommandV1_0(rest));
					return true;
                }
                return false;
            }

            public override bool IsTransferable(ACommand c) {
                if (c is InitCommand) {
                    var c2 = (InitCommand)c;
                    cache.Cached(c, new InitCommandV1_0(c2.NAME));
                    return true;
                }
                return false;
            }
        }

		public InitCommandV1_0(String name) : base(name) { }


        public string serialize() {
            return String.Format("INIT({0})", NAME);
        }
    }
}
