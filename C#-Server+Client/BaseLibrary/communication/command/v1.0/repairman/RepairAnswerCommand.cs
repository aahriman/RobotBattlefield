using BaseLibrary.communication.command.repairman;
using BaseLibrary.communication.protocol;
using BaseLibrary.utils;

namespace BaseLibrary.communication.command.v1._0.repairman {
    public class RepairAnswerCommandV1_0 : RepairAnswerCommand, ACommand.Sendable {

        private const string NAME = "REPAIR_ANSWER";
        public static readonly IFactory<ACommand.Sendable, ACommand> FACTORY = new CommandFactory();
        private sealed class CommandFactory : ACommandFactory {
            internal CommandFactory() : base() { }

            public override bool IsDeserializeable(string s) {
                string[] rest;
                if (ProtocolV1_0Utils.GetParams(s, NAME, out rest) && rest.Length == 1) {
                    if (rest[0].Equals("0") || rest[0].Equals("1")) {
                        cache.Cached(s, new RepairAnswerCommandV1_0(rest[0].Equals("1")));
                        return true;
                    }
                }
                return false;
            }

            public override bool IsTransferable(ACommand c) {
                RepairAnswerCommand command = c as RepairAnswerCommand;
                if (command != null) {
                    cache.Cached(command, new RepairAnswerCommandV1_0(command.SUCCESS));
                    return true;
                }
                return false;
            }
        }


        
        public RepairAnswerCommandV1_0(bool success) : base(success){
        }

        public string Serialize() {
            return ProtocolV1_0Utils.SerializeParams(NAME, SUCCESS ? "1" : "0");
        }

    }
}
