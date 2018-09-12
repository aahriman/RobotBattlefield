using BaseLibrary.communication.command.miner;
using BaseLibrary.communication.protocol;
using BaseLibrary.utils;

namespace BaseLibrary.communication.command.v1._0.miner {
    public class PutMineAnswerCommandV1_0 : PutMineAnswerCommand, ACommand.Sendable {

        private const string NAME = "PUT_MINE_ANSWER";
        public static readonly IFactory<ACommand.Sendable, ACommand> FACTORY = new CommandFactory();
        private sealed class CommandFactory : ACommandFactory {
            internal CommandFactory() : base() { }

            public override bool IsDeserializeable(string s) {
                if (ProtocolV1_0Utils.GetParams(s, NAME, out string[] rest) && rest.Length == 2) {
                    if ((rest[0].Equals("0") || rest[0].Equals("1")) && int.TryParse(rest[1], out int mineId)) {
                        cache.Cached(s, new PutMineAnswerCommandV1_0(rest[0].Equals("1"), mineId));
                        return true;
                    }
                }
                return false;
            }

            public override bool IsTransferable(ACommand c) {
                if (c is PutMineAnswerCommand command) {
                    cache.Cached(command, new PutMineAnswerCommandV1_0(command.SUCCESS, command.MINE_ID));
                    return true;
                }
                return false;
            }
        }

        public PutMineAnswerCommandV1_0(bool success, int mineId) : base(success, mineId) {
        }


        public string Serialize() {
            return ProtocolV1_0Utils.SerializeParams(NAME, SUCCESS, MINE_ID);
        }
    }
}
