using System;
using CommunicationLibrary.utils;

namespace CommunicationLibrary.command.v1._0 {
    public class ShotCommandV1_0 : ShotCommand, ACommand.Sendable {
	    private const string NAME = "SHOT";
        public static readonly ICommandFactory FACTORY = new CommandFactory();
        private sealed class CommandFactory : ACommandFactory {
            internal CommandFactory() : base() { }

            public override Boolean IsDeserializable(String s) {
				string[] rest;
                if (ProtocolV1_0Utils.GetParams(s, NAME, out rest)) {
                    ProtocolDouble angle, range;
                    if (ProtocolDouble.TryParse(rest[0], out angle) && ProtocolDouble.TryParse(rest[1], out range)) {
                        cache.Cached(s, new ShotCommandV1_0(angle, range));
                        return true;
                    }
                }
                return false;
            }

            public override bool IsTransferable(ACommand c) {
	            ShotCommand command = c as ShotCommand;
	            if (command != null) {
					cache.Cached(command, new ShotCommandV1_0(command.ANGLE, command.RANGE));
                    return true;
                }
                return false;
            }
        }
 

        public ShotCommandV1_0(ProtocolDouble angle, ProtocolDouble range)
            : base(angle, range) {
        }

        public string serialize() {
	        return ProtocolV1_0Utils.SerializeParams(NAME, ANGLE, RANGE);
        }
    }
}
