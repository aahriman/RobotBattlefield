using System;
using BaseLibrary.command.tank;
using BaseLibrary.protocol;
using BaseLibrary.utils;

namespace BaseLibrary.command.v1._0.tank {
    public class ShotCommandV1_0 : ShotCommand, ACommand.Sendable {
	    private const string NAME = "SHOT";
        public static readonly IFactory<ACommand.Sendable, ACommand> FACTORY = new CommandFactory();
        private sealed class CommandFactory : ACommandFactory {
            internal CommandFactory() : base() { }

            public override bool IsDeserializable(string s) {
				string[] rest;
                if (ProtocolV1_0Utils.GetParams(s, NAME, out rest)) {
                    ProtocolDouble angle, range;
                    if (ProtocolDouble.TryParse(rest[0], out range) && ProtocolDouble.TryParse(rest[1], out angle)) {
                        cache.Cached(s, new ShotCommandV1_0(range, angle));
                        return true;
                    }
                }
                return false;
            }

            public override bool IsTransferable(ACommand c) {
	            ShotCommand command = c as ShotCommand;
	            if (command != null) {
					cache.Cached(command, new ShotCommandV1_0(command.RANGE, command.ANGLE));
                    return true;
                }
                return false;
            }
        }
 

        public ShotCommandV1_0(double range, double angle)
            : base(range, angle) {
        }

        public string Serialize() {
	        return ProtocolV1_0Utils.SerializeParams(NAME, (ProtocolDouble)RANGE, (ProtocolDouble)ANGLE);
        }
    }
}
