using System;
using BaseLibrary.command.tank;
using BaseLibrary.protocol;
using BaseLibrary.utils;

namespace BaseLibrary.command.v1._0.tank {
    public class ShootCommandV10 : ShootCommand, ACommand.Sendable {
	    private const string NAME = "SHOT";
        public static readonly IFactory<ACommand.Sendable, ACommand> FACTORY = new CommandFactory();
        private sealed class CommandFactory : ACommandFactory {
            internal CommandFactory() : base() { }

            public override bool IsDeserializeable(string s) {
				string[] rest;
                if (ProtocolV1_0Utils.GetParams(s, NAME, out rest)) {
                    ProtocolDouble angle, range;
                    if (ProtocolDouble.TryParse(rest[0], out range) && ProtocolDouble.TryParse(rest[1], out angle)) {
                        cache.Cached(s, new ShootCommandV10(range, angle));
                        return true;
                    }
                }
                return false;
            }

            public override bool IsTransferable(ACommand c) {
	            ShootCommand command = c as ShootCommand;
	            if (command != null) {
					cache.Cached(command, new ShootCommandV10(command.RANGE, command.ANGLE));
                    return true;
                }
                return false;
            }
        }
 

        public ShootCommandV10(double range, double angle)
            : base(range, angle) {
        }

        public string Serialize() {
	        return ProtocolV1_0Utils.SerializeParams(NAME, (ProtocolDouble)RANGE, (ProtocolDouble)ANGLE);
        }
    }
}
