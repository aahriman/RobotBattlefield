using System;
using System.Collections.Generic;
using CommunicationLibrary.utils;

namespace CommunicationLibrary.command.v1._0 {
	public class InitAnswerCommandV1_0 : InitAnswerCommand, ACommand.Sendable {
        public static readonly ICommandFactory FACTORY = new CommandFactory();
        private sealed class CommandFactory : ACommandFactory {
            internal CommandFactory() : base() { }

            public override Boolean IsDeserializable(String s) {
                s = s.Trim();
                string[] rest;
				if (ProtocolV1_0Utils.GetParams(s, "INIT_ASNWER", out rest)) {
					if(rest.Length == 7){
						int[] param;
						if (Parser.TryParse(new ArraySegment<string>(rest, 0, 6), out param)) {
							Dictionary<int, int> motorIdForRobots = new Dictionary<int,int>();
							if (ProtocolV1_0Utils.Deserialize(rest[6], motorIdForRobots, ProtocolV1_0Utils.DEFAULT.NEXT)) {
								cache.Cached(s, new InitAnswerCommandV1_0(param[0], param[1], param[2], param[3], motorIdForRobots, param[4], param[5]));
								return true;
							}
						}
					}
                }
                return false;
            }

			public override bool IsTransferable(ACommand c) {
				if (c is InitAnswerCommand) {
					var c1 = (InitAnswerCommand)c;
					cache.Cached(c, new InitAnswerCommandV1_0(c1.MAX_LAP, c1.MATCH_NUMBER, c1.MATCHES, c1.ROBOT_ID, c1.MOTOR_ID_FOR_ROBOTS, c1.GUN_ID, c1.ARMOR_ID));
					return true;
				}
				return false;
			}
        }

		public InitAnswerCommandV1_0(int MAX_LAP, int MATCH_NUMBER, int MATCHES, int ROBOT_ID, Dictionary<int, int> MOTOR_ID_FOR_ROBOTS, int GUN_ID, int ARMOR_ID) :
			base(MAX_LAP, MATCH_NUMBER, MATCHES, ROBOT_ID, MOTOR_ID_FOR_ROBOTS, GUN_ID, ARMOR_ID) { }


        public string serialize() {
			return ProtocolV1_0Utils.SerializeParams("INIT_ASNWER", MAX_LAP, MATCH_NUMBER, MATCHES, ROBOT_ID, GUN_ID, ARMOR_ID, MOTOR_ID_FOR_ROBOTS);
        }
    }
}
