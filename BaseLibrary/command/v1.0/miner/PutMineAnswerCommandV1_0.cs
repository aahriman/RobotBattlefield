using System;
using System.Collections.Generic;
using BaseLibrary.protocol;
using BaseLibrary.utils;
using BaseLibrary.visitors;

namespace BaseLibrary.command.miner {
    public class PutMineAnswerCommandV1_0 : PutMineAnswerCommand, ACommand.Sendable {

        private const string NAME = "PUT_MINE_ANSWER";
        public static readonly IFactory<ACommand.Sendable, ACommand> FACTORY = new CommandFactory();
        private sealed class CommandFactory : ACommandFactory {
            internal CommandFactory() : base() { }

            public override Boolean IsDeserializable(String s) {
                string[] rest;
                if (ProtocolV1_0Utils.GetParams(s, NAME, out rest) && rest.Length == 2) {
                    int mineId;
                    if ((rest[0].Equals("0") || rest[0].Equals("1")) && int.TryParse(rest[1], out mineId)) {
                        cache.Cached(s, new PutMineAnswerCommandV1_0(rest[0].Equals("1"), mineId));
                        return true;
                    }
                }
                return false;
            }

            public override bool IsTransferable(ACommand c) {
                PutMineAnswerCommand command = c as PutMineAnswerCommand;
                if (command != null) {
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
