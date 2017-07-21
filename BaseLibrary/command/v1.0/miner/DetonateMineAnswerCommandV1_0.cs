using System;
using System.Collections.Generic;
using BaseLibrary.command.tank;
using BaseLibrary.command.v1._0.tank;
using BaseLibrary.protocol;
using BaseLibrary.utils;
using BaseLibrary.visitors;

namespace BaseLibrary.command.miner {
    public class DetonateMineAnswerCommandV1_0 : DetonateMineAnswerCommand, ACommand.Sendable {
        private const string NAME = "DETONATE_MINE_ANSWER";

        public static readonly IFactory<ACommand.Sendable, ACommand> FACTORY = new CommandFactory();

        private sealed class CommandFactory : ACommandFactory {

            public override Boolean IsDeserializable(String s) {
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
