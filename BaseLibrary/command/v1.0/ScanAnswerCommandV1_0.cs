using System;
using BaseLibrary.command.common;
using BaseLibrary.protocol;
using BaseLibrary.utils;

namespace BaseLibrary.command.v1._0 {
    public class ScanAnswerCommandV1_0 : ScanAnswerCommand, ACommand.Sendable {
		private const string NAME = "SCAN_ANSWER";
        public static readonly IFactory<ACommand.Sendable, ACommand> FACTORY = new CommandFactory();
        private sealed class CommandFactory : ACommandFactory {
            internal CommandFactory() : base() { }

            public override bool IsDeserializeable(string s) {
                s = s.Trim();
				if (s.StartsWith(NAME+"(") && s.EndsWith(")")) {
					var rest = s.Substring(NAME.Length+1, s.Length - 2 - NAME.Length).Trim().Split(';');
                    double range; int enemyId;
                    if (double.TryParse(rest[0], out range) && int.TryParse(rest[1], out enemyId)) {
                        cache.Cached(s, new ScanAnswerCommandV1_0(range, enemyId));
                        return true;
                    }
                }
                return false;
            }

            public override bool IsTransferable(ACommand c) {
                if (c is ScanAnswerCommand) {
                    var c1 = (ScanAnswerCommand)c;
                    cache.Cached(c, new ScanAnswerCommandV1_0(c1.RANGE, c1.ENEMY_ID));
                    return true;
                }
                return false;
            }
        }

        public ScanAnswerCommandV1_0(double range, int enemyID)
            : base(range, enemyID) { }

        public string Serialize() {
            return ProtocolV1_0Utils.SerializeParams("SCAN_ANSWER", (ProtocolDouble) RANGE, ENEMY_ID);
        }
    }
}
