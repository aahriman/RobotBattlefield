using System;
using System.Collections.Generic;
using BaseLibrary.command.common;
using BaseLibrary.command.handshake;
using BaseLibrary.protocol;
using BaseLibrary.utils;

namespace BaseLibrary.command.v1._0 {
	public class InitAnswerCommandV1_0 : InitAnswerCommand, ACommand.Sendable {
        public static readonly IFactory<ACommand.Sendable, ACommand> FACTORY = new CommandFactory();
        private sealed class CommandFactory : ACommandFactory {
            internal CommandFactory() : base() { }

            public override bool IsDeserializeable(string s) {
                s = s.Trim();
                string[] rest;
				if (ProtocolV1_0Utils.GetParams(s, "INIT_ANSWER", out rest)) {
					if(rest.Length == 9){
						int[] param;
						if (Parser.TryParse(new ArraySegment<string>(rest, 0, 8), out param)) {
						    InitAnswerCommandV1_0 initAnswer = new InitAnswerCommandV1_0(param[0], param[1], param[2], param[3], param[4],
						                                                             param[5], param[6], param[7]);

                            

                            string[] moreString;
                            if (ProtocolV1_0Utils.Deserialize(rest[8], out moreString, ProtocolV1_0Utils.DEFAULT.NEXT)) {
                                initAnswer.DeserializeMore(moreString, initAnswer.MORE, SUB_COMMAND_FACTORIES);
                            }
                            cache.Cached(s, initAnswer);
                            return true;
						}
					}
                }
                return false;
            }

			public override bool IsTransferable(ACommand c) {
			    InitAnswerCommand c1 = c as InitAnswerCommand;
			    if (c1 != null) {
                    InitAnswerCommandV1_0 initAnswer = new InitAnswerCommandV1_0(c1.MAX_TURN, c1.LAP_NUMBER, c1.MAX_LAP, c1.ROBOT_ID, c1.TEAM_ID,
			                                  c1.CLASS_EQUIPMENT_ID, c1.ARMOR_ID, c1.MOTOR_ID);
			        initAnswer.MORE = c1.MORE;
                    cache.Cached(c, initAnswer);
					return true;
				}
				return false;
			}
        }

		public InitAnswerCommandV1_0(int maxTurn, int lapNumber, int maxLap, int robotId, int teamId, int classEquipmentId, int armorId, int motorId) :
			base(maxTurn, lapNumber, maxLap, robotId, teamId, classEquipmentId, armorId, motorId) { }


        public string Serialize() {
			return ProtocolV1_0Utils.SerializeParams("INIT_ANSWER", MAX_TURN, LAP_NUMBER, MAX_LAP, ROBOT_ID, TEAM_ID, CLASS_EQUIPMENT_ID, ARMOR_ID, MOTOR_ID, SerializeMore(SUB_COMMAND_FACTORIES));
        }
    }
}
