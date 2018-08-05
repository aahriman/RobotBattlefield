﻿using BaseLibrary.communication.command.miner;
using BaseLibrary.communication.protocol;
using BaseLibrary.utils;

namespace BaseLibrary.communication.command.v1._0.miner {
    public class DetonateMineCommandV1_0 : DetonateMineCommand, ACommand.Sendable {

        private const string NAME = "DETONATE_MINE";
        public static readonly IFactory<ACommand.Sendable, ACommand> FACTORY = new CommandFactory();
        private sealed class CommandFactory : ACommandFactory {
            internal CommandFactory() : base() { }

            public override bool IsDeserializeable(string s) {
                string[] rest;
                if (ProtocolV1_0Utils.GetParams(s, NAME, out rest) && rest.Length == 1) {
                    int mineId;
                    if (int.TryParse(rest[0], out mineId)) {
                        cache.Cached(s, new DetonateMineCommandV1_0(mineId));
                        return true;
                    }
                }
                return false;
            }

            public override bool IsTransferable(ACommand c) {
                DetonateMineCommand command = c as DetonateMineCommand;
                if (command != null) {
                    cache.Cached(command, new DetonateMineCommandV1_0(command.MINE_ID));
                    return true;
                }
                return false;
            }
        }


        public DetonateMineCommandV1_0(int mineId) : base(mineId) {
        }

        public string Serialize() {
            return ProtocolV1_0Utils.SerializeParams(NAME, MINE_ID);
        }

    }
}
