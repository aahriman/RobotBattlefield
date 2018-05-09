using BaseLibrary.communication.command.common;
using BaseLibrary.communication.protocol;
using BaseLibrary.utils;

namespace BaseLibrary.communication.command.v1._0 {
    public class DriveAnswerCommandV1_0 : DriveAnswerCommand, ACommand.Sendable {
	    private const string NAME = "DRIVE_ANSWER";
        public static readonly IFactory<ACommand.Sendable, ACommand> FACTORY = new CommandFactory();
        private sealed class CommandFactory : ACommandFactory {

            public override bool IsDeserializeable(string s) {
	            string rest;
                if (ProtocolV1_0Utils.GetParams(s, NAME, out rest)) {
                    if (rest.Equals("1") || rest.Equals("0")) {
                        cache.Cached(s, new DriveAnswerCommandV1_0(rest.Equals("1")));
                        return true;
                    }
                }
                return false;
            }

            public override bool IsTransferable(ACommand c) {
                if (c is DriveAnswerCommand) {
                    cache.Cached(c, new DriveAnswerCommandV1_0(((DriveAnswerCommand)c).SUCCESS));
                    return true;
                }
                return false;
            }
        }

        public DriveAnswerCommandV1_0(bool success) : base(success){ }

        public string Serialize() {
            return ProtocolV1_0Utils.SerializeParams(NAME, (SUCCESS? 1 : 0));
        }
    }
}
