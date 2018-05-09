﻿using BaseLibrary.communication.command.equipment;
using BaseLibrary.communication.protocol;
using BaseLibrary.utils;

namespace BaseLibrary.communication.command.v1._0.equipment {
    public class GetMineGunsAnswerCommandV10 : GetMineGunsAnswerCommand, ACommand.Sendable{
        private const string NAME = "MINE_GUN_ANSWER";
        public static readonly IFactory<ACommand.Sendable, ACommand> FACTORY = new CommandFactory();
        private sealed class CommandFactory : ACommandFactory {
            internal CommandFactory() : base() { }

            public override bool IsDeserializeable(string s) {
                s = s.Trim();
                string[] rest;
                if (ProtocolV1_0Utils.GetParams(s, NAME, ProtocolV1_0Utils.DEFAULT, out rest)) {
                    if (rest.Length == 1 && ProtocolV1_0Utils.Deserialize(rest[0], out rest, ProtocolV1_0Utils.DEFAULT.NEXT)) {
                        MineGunV1_0[] mineGuns = new MineGunV1_0[rest.Length];
                        for (int i = 0; i < mineGuns.Length; i++) {
                            if (!MineGunV1_0.Deserialize(rest[i], ProtocolV1_0Utils.DEFAULT.NEXT.NEXT, out mineGuns[i])) {
                                return false;
                            }
                        }
                        cache.Cached(s, new GetMineGunsAnswerCommandV10(mineGuns));
                        return true;
                    }
                }
                return false;
            }

            public override bool IsTransferable(ACommand c) {
                var c2 = c as GetMineGunsAnswerCommand;
                if (c2 != null) {
                    cache.Cached(c, new GetMineGunsAnswerCommandV10(MineGunV1_0.Convert(c2.MINE_GUNS)));
                    return true;
                }
                return false;
            }
        }
        public GetMineGunsAnswerCommandV10(MineGunV1_0[] mineGuns) : base(mineGuns) { }

        public string Serialize() {
            return ProtocolV1_0Utils.SerializeParams(NAME, ProtocolV1_0Utils.DEFAULT, new object[] { MINE_GUNS});
        }
    }
}
