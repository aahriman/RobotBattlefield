using System;
using BaseLibrary.command.common;
using BaseLibrary.protocol;
using BaseLibrary.utils;

namespace BaseLibrary.command.v1._0 {
    public class DriveCommandV1_0 : DriveCommand, ACommand.Sendable {
		private const string NAME = "DRIVE";

        public static readonly IFactory<ACommand.Sendable, ACommand> FACTORY = new CommandFactory();
        private sealed class CommandFactory : ACommandFactory {
            public override bool IsDeserializeable(string s) {
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

        public DriveCommandV1_0(double power, double angle) : base(power, angle) { }

        public string Serialize() {
	        return ProtocolV1_0Utils.SerializeParams(NAME, (ProtocolDouble) POWER, (ProtocolDouble) ANGLE);
        }

    }
}
