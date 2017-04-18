using System;
using System.Collections.Generic;
using BaseLibrary.protocol;
using BaseLibrary.utils;

namespace BaseLibrary.command.v1._0 {
	public class InitAnswerCommandV1_0 : InitAnswerCommand, ACommand.Sendable {
        public static readonly IFactory<ACommand.Sendable, ACommand> FACTORY = new CommandFactory();
        private sealed class CommandFactory : ACommandFactory {
            internal CommandFactory() : base() { }

            public override Boolean IsDeserializable(String s) {
                s = s.Trim();
                string[] rest;
				if (ProtocolV1_0Utils.GetParams(s, "INIT_ASNWER", out rest)) {
					if(rest.Length == 7){
						int[] param;
						if (Parser.TryParse(new ArraySegment<string>(rest, 0, 7), out param)) {
							cache.Cached(s, new InitAnswerCommandV1_0(param[0], param[1], param[2], param[3], param[4], param[5], param[6]));
							return true;
						}
					}
                }
                return false;
            }

			public override bool IsTransferable(ACommand c) {
			    InitAnswerCommand c1 = c as InitAnswerCommand;
			    if (c1 != null) {
					cache.Cached(c, new InitAnswerCommandV1_0(c1.MAX_TURN, c1.LAP_NUMBER, c1.MAX_LAP, c1.ROBOT_ID, c1.TEAM_ID, c1.CLASS_EQUIPMENT_ID, c1.ARMOR_ID));
					return true;
				}
				return false;
			}
        }

		public InitAnswerCommandV1_0(int maxTurn, int lapNumber, int maxLap, int robotId, int teamId, int classEquipmentId, int armorId) :
			base(maxTurn, lapNumber, maxLap, robotId, teamId, classEquipmentId, armorId) { }


        public string Serialize() {
			return ProtocolV1_0Utils.SerializeParams("INIT_ASNWER", MAX_TURN, LAP_NUMBER, MAX_LAP, ROBOT_ID, TEAM_ID, CLASS_EQUIPMENT_ID, ARMOR_ID);
        }
    }
}
