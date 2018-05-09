using BaseLibrary.communication.command.common;
using BaseLibrary.communication.protocol;
using BaseLibrary.utils;

namespace BaseLibrary.communication.command.v1._0 {
    public class ScanCommandV1_0 : ScanCommand, ACommand.Sendable {
	    private const string NAME = "SCAN";
        public static readonly IFactory<ACommand.Sendable, ACommand> FACTORY = new CommandFactory();
        private sealed class CommandFactory : ACommandFactory {
            public override bool IsDeserializeable(string s) {
                if (ProtocolV1_0Utils.GetParams(s, NAME, out string[] rest)) {
                    if (Parser.TryParse(rest, out ProtocolDouble[] param)) {
                        cache.Cached(s, new ScanCommandV1_0(param[0], param[1]));
                        return true;
                    }
                }
                return false;
            }
            
            public override bool IsTransferable(ACommand c) {
                if (c is ScanCommand command) {
		            cache.Cached(c, new ScanCommandV1_0(command.PRECISION, command.ANGLE));
		            return true;
	            }
				return false;
            }
        }
        

        public ScanCommandV1_0(double precision, double angle)
            : base(precision, angle) { }

        public string Serialize() {
            return ProtocolV1_0Utils.SerializeParams(NAME, (ProtocolDouble) PRECISION, (ProtocolDouble)ANGLE);
        }
    }
}
