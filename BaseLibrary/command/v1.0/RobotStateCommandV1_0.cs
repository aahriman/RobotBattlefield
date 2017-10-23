using System;
using System.Collections.Generic;
using BaseLibrary.command.common;
using BaseLibrary.protocol;
using BaseLibrary.utils;

namespace BaseLibrary.command.v1._0 {
	public class RobotStateCommandV1_0 : RobotStateCommand, ACommand.Sendable {
		private const string COMMAND_NAME = "STATE";

        public static readonly IFactory<ACommand.Sendable, ACommand> FACTORY = new CommandFactory();
        private sealed class CommandFactory : ACommandFactory {
			public override bool IsDeserializable(string s) {
				
				string[] rest;
				if (ProtocolV1_0Utils.GetParams(s, COMMAND_NAME, out rest)) {
					if (rest.Length == 10) {
						ProtocolDouble[] paramsPDoubles;
						int[] paramInts;
						int[] arrayIdsOfLifeRobots;
                        
					    if (Parser.TryParse(new int[] { 0, 1, 3 }, rest, out paramsPDoubles) &&
							Parser.TryParse(new int[] { 2, 4, 5, 6 }, rest, out paramInts) &&
							ProtocolV1_0Utils.Deserialize(rest[7], out arrayIdsOfLifeRobots, ProtocolV1_0Utils.DEFAULT.NEXT)
						) {
						    EndLapCommandV1_0 endLapCommand;
                            EndLapCommandV1_0.Deserialize(rest[8], ProtocolV1_0Utils.DEFAULT.NEXT, out endLapCommand);

                            RobotStateCommandV1_0 robotStateCommand = new RobotStateCommandV1_0(paramsPDoubles[0], paramsPDoubles[1],
						                                                                            paramInts[0], paramsPDoubles[2],
						                                                                            paramInts[1], paramInts[2],
						                                                                            paramInts[3],
						                                                                            arrayIdsOfLifeRobots, endLapCommand);
					        string[] moreString;
					        if (ProtocolV1_0Utils.Deserialize(rest[9], out moreString, ProtocolV1_0Utils.DEFAULT.NEXT)) {
					            robotStateCommand.DeserializeMore(moreString, robotStateCommand.MORE, SUB_COMMAND_FACTORIES);
					        }
					        cache.Cached(s, robotStateCommand);
							return true;
						}
					}
				}
				return false;
			}

			public override bool IsTransferable(ACommand c) {
				if (c is RobotStateCommand) {
					var c1 = (RobotStateCommand)c;
				    RobotStateCommandV1_0 c2 = new RobotStateCommandV1_0(c1.X, c1.Y, c1.HIT_POINTS, c1.POWER, c1.TURN, c1.MAX_TURN,
				                                                         c1.COUNT_OF_LIFE_ROBOTS, c1.ARRAY_IDS_OF_LIFE_ROBOTS,
				                                                         c1.END_LAP_COMMAND);
				    c2.MORE = c1.MORE;

                    cache.Cached(c, c2);
					return true;
				}
				return false;
			}
		}

		public RobotStateCommandV1_0(double x, double y, int hitPoints, double power, int turn, int maxTurn, int countOfLifeRobots, int[] arrayIdsOfLifeRobots, EndLapCommand endLapCommand) :
			base(x, y, hitPoints, power, turn, maxTurn, countOfLifeRobots, arrayIdsOfLifeRobots, endLapCommand) { }

        public string Serialize() {
		    EndLapCommandV1_0 endLapCommand = null;
		    if (END_LAP_COMMAND != null) {
                endLapCommand = (EndLapCommandV1_0) EndLapCommandV1_0.FACTORY.Transfer(this.END_LAP_COMMAND);
		    }
            return ProtocolV1_0Utils.SerializeParams(COMMAND_NAME, (ProtocolDouble)X, (ProtocolDouble)Y, HIT_POINTS, (ProtocolDouble)POWER, TURN, MAX_TURN, COUNT_OF_LIFE_ROBOTS, ARRAY_IDS_OF_LIFE_ROBOTS, endLapCommand, SerializeMore(SUB_COMMAND_FACTORIES));
		}
	}
}
