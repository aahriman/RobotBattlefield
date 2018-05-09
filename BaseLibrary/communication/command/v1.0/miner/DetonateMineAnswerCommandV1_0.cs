using BaseLibrary.communication.command.miner;
using BaseLibrary.communication.protocol;
using BaseLibrary.utils;

namespace BaseLibrary.communication.command.v1._0.miner {
    public class DetonateMineAnswerCommandV1_0 : DetonateMineAnswerCommand, ACommand.Sendable {
        private const string NAME = "DETONATE_MINE_ANSWER";

        public static readonly IFactory<ACommand.Sendable, ACommand> FACTORY = new CommandFactory();

        private sealed class CommandFactory : ACommandFactory {

            public override bool IsDeserializeable(string s) {
                string rest;
                if (ProtocolV1_0Utils.GetParams(s, NAME, out rest)) {
                    if (rest.Equals("1") || rest.Equals("0")) {
                        cache.Cached(s, new DetonateMineAnswerCommandV1_0(rest.Equals("1")));
                        return true;
                    }
                }
                return false;
            }

            public override bool IsTransferable(ACommand c) {
                DetonateMineAnswerCommand command = c as DetonateMineAnswerCommand;
                if (command != null) {
                    cache.Cached(c, new DetonateMineAnswerCommandV1_0(command.SUCCESS));
                    return true;
                }
                return false;
            }
        }

        public DetonateMineAnswerCommandV1_0(bool success) : base(success){
        }

        public string Serialize() {
            return ProtocolV1_0Utils.SerializeParams(NAME, (SUCCESS ? 1 : 0));
        }
    }
}
