using System;
using System.Collections.Generic;
using BaseLibrary.protocol;
using BaseLibrary.utils;
using BaseLibrary.visitors;

namespace BaseLibrary.command.miner {
    public class PutMineCommandV1_0 : PutMineCommand, ACommand.Sendable {

        private const string NAME = "PUT_MINE";
        public static readonly IFactory<ACommand.Sendable, ACommand> FACTORY = new CommandFactory();
        private sealed class CommandFactory : ACommandFactory {
            internal CommandFactory() : base() { }

            public override bool IsDeserializable(string s) {
                string[] rest;
                if (ProtocolV1_0Utils.GetParams(s, NAME, out rest)) {
                    cache.Cached(s, new PutMineCommandV1_0());
                    return true;
                }
                return false;
            }

            public override bool IsTransferable(ACommand c) {
                PutMineCommand command = c as PutMineCommand;
                if (command != null) {
                    cache.Cached(command, new PutMineCommandV1_0());
                    return true;
                }
                return false;
            }
        }


        public string Serialize() {
            return ProtocolV1_0Utils.SerializeParams(NAME);
        }
    }
}
