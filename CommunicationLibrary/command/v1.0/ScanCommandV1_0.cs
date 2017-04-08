using System;
using CommunicationLibrary.utils;

namespace CommunicationLibrary.command.v1._0 {
    public class ScanCommandV1_0 : ScanCommand, ACommand.Sendable {
	    private const string NAME = "SCAN";
        public static readonly ICommandFactory FACTORY = new ScanCommandFactory();
        private sealed class ScanCommandFactory : ACommandFactory {
            public override Boolean IsDeserializable(String s) {
	            string[] rest;
				if (ProtocolV1_0Utils.GetParams(s, NAME, out rest)) {
                    ProtocolDouble[] param;
					if (Parser.TryParse(rest, out param)) {
                        cache.Cached(s, new ScanCommandV1_0(param[0], param[1]));
                        return true;
                    }
                }
                return false;
            }
            
            public override bool IsTransferable(ACommand c) {
				ScanCommand command = c as ScanCommand;
	            if (command != null) {
		            cache.Cached(c, new ScanCommandV1_0(command.ANGLE, command.PRECISION));
		            return true;
	            }
				return false;
            }
        }
        

        public ScanCommandV1_0(ProtocolDouble angle, ProtocolDouble precision)
            : base(angle, precision) { }

        public string serialize() {
            return ProtocolV1_0Utils.SerializeParams(NAME, ANGLE, PRECISION);
        }
    }
}
