using System;
using CommunicationLibrary.utils;

namespace CommunicationLibrary.command.v1._0 {
    public class DriveCommandV1_0 : DriveCommand, ACommand.Sendable {
		private const string NAME = "DRIVE";

        public static readonly ICommandFactory FACTORY = new DriverCommandFactory();
        private sealed class DriverCommandFactory : ACommandFactory {
            public override Boolean IsDeserializable(String s) {
	            string[] rest;
				if (ProtocolV1_0Utils.GetParams(s, NAME, out rest)) {
					ProtocolDouble[] param;
					if (rest.Length == 2 && Parser.TryParse(rest, out param)) {
						cache.Cached(s, new DriveCommandV1_0(param[0], param[1]));
						return true;
					}
                }
                return false;
            }

            public override bool IsTransferable(ACommand c) {
                if (c is DriveCommand) {
                    var c1 = (DriveCommand) c;
                    cache.Cached(c, new DriveCommandV1_0(c1.POWER, c1.ANGLE));
                    return true;
                }
                return false;
            }
        }

        public DriveCommandV1_0(ProtocolDouble power, ProtocolDouble angle) : base(power, angle) { }

        public string serialize() {
	        return ProtocolV1_0Utils.SerializeParams(NAME, POWER, ANGLE);
        }

    }
}
