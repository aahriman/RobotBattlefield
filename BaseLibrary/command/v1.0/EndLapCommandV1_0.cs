using System;
using BaseLibrary.battlefield;
using BaseLibrary.command.common;
using BaseLibrary.protocol;
using BaseLibrary.utils;
using BaseLibrary.utils.protocolV1_0Utils;

namespace BaseLibrary.command.v1._0 {
	internal class EndLapCommandV1_0 : EndLapCommand, ACommand.Sendable, InnerSerializerV1_0 {
	    public const string COMMAND_NAME = "END_LAP";

        public static bool Deserialize(string orig, Deep deep, out EndLapCommandV1_0 deserialized) {
            string[] rest;
            if (ProtocolV1_0Utils.GetParams(orig, COMMAND_NAME, deep, out rest)) {
                if (rest.Length == 3) {
                    int gold, score;
                    LapState lapState;

                    if (Enum.TryParse(rest[0], true, out lapState) && int.TryParse(rest[1], out gold) && int.TryParse(rest[2], out score)) {
                        deserialized = new EndLapCommandV1_0(lapState, gold, score);
                        return true;
                    }
                }
            }
            deserialized = null;
            return false;
        }

        public static readonly IFactory<ACommand.Sendable, ACommand> FACTORY = new CommandFactory();
        private sealed class CommandFactory : ACommandFactory {
            internal CommandFactory() : base() { }

            public override bool IsDeserializable(string s) {
                s = s.Trim();
                string[] rest;
                if (StringUtils.GetRestOfStringSplited(s, "END_LAP(", ")", out rest, ';')) {
                    if (rest.Length != 3) {
                        return false;
                    }
                    int gold, score;
                    LapState lapState;
                    
                    if (Enum.TryParse(rest[0], true, out lapState) && int.TryParse(rest[1], out gold) && int.TryParse(rest[2], out score)) {
                        cache.Cached(s, new EndLapCommandV1_0(lapState, gold, score));
                        return true;
                    }
                }
                return false;
            }

            public override bool IsTransferable(ACommand c) {
                if (c is EndLapCommand) {
                    var c2 = (EndLapCommand)c;
                    cache.Cached(c, new EndLapCommandV1_0(c2.STATE, c2.GOLD,c2.SCORE));
                    return true;
                }
                return false;
            }
        }

        public EndLapCommandV1_0(LapState lapState, int gold, int score) : base(lapState, gold, score) {}

        public string Serialize() {
            return ProtocolV1_0Utils.SerializeParams(COMMAND_NAME, STATE, GOLD, SCORE);
        }

	    public string Serialize(Deep deep) {
	        return ProtocolV1_0Utils.SerializeParams(COMMAND_NAME, deep, STATE, GOLD, SCORE);
	    }
	}
}
