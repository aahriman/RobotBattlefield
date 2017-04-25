using System;
using System.Collections.Generic;
using BaseLibrary.command.common;
using BaseLibrary.protocol;
using BaseLibrary.utils;
using BaseLibrary.utils.protocolV1_0Utils;

namespace BaseLibrary.command.v1._0 {
	public class RobotStateCommandV1_0 : RobotStateCommand, ACommand.Sendable {
		private const string COMMAND_NAME = "STATE";

        public static readonly IFactory<ACommand.Sendable, ACommand> FACTORY = new CommandFactory();
        private sealed class CommandFactory : ACommandFactory {
			public override Boolean IsDeserializable(String s) {
				
				string[] rest;
				if (ProtocolV1_0Utils.GetParams(s, COMMAND_NAME, out rest)) {
					if (rest.Length == 10) {
						ProtocolDouble[] paramsPDoubles;
						int[] paramInts;
						int[] arrayIdsOfLifeRobots;
                        /*InnerSerializerV1_0[] more = new InnerSerializerV1_0[subCommandFactories.Count];

					    if (rest.Length == 10) {
					        String[] moreStrings;
					        if (ProtocolV1_0Utils.Deserialize(rest[9], out moreStrings, ProtocolV1_0Utils.DEFAULT.NEXT)) {
					            foreach (var moreString in moreStrings) {
					                foreach (var subCommandFactory in subCommandFactories) {
					                    if (subCommandFactory.Deserialize(moreString, more)) {
					                        break;
					                    }
					                }
					            }   
					        }
					    }*/
					    Object more = new Object[0];
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
					cache.Cached(c, new RobotStateCommandV1_0(c1.X, c1.Y, c1.HIT_POINTS, c1.POWER, c1.TURN, c1.MAX_TURN, c1.COUNT_OF_LIFE_ROBOTS, c1.ARRAY_IDS_OF_LIFE_ROBOTS, c1.END_LAP_COMMAND));
					return true;
				}
				return false;
			}
		}

		public RobotStateCommandV1_0(ProtocolDouble x, ProtocolDouble y, int hitPoints, ProtocolDouble power, int turn, int maxTurn, int countOfLifeRobots, int[] arrayIdsOfLifeRobots, EndLapCommand endLapCommand) :
			base(x, y, hitPoints, power, turn, maxTurn, countOfLifeRobots, arrayIdsOfLifeRobots, endLapCommand) { }

        public string Serialize() {
		    EndLapCommandV1_0 endLapCommand = null;
		    if (END_LAP_COMMAND != null) {
                endLapCommand = (EndLapCommandV1_0) EndLapCommandV1_0.FACTORY.Transfer(this.END_LAP_COMMAND);
		    }
			return ProtocolV1_0Utils.SerializeParams(COMMAND_NAME, X, Y, HIT_POINTS, POWER, TURN, MAX_TURN, COUNT_OF_LIFE_ROBOTS, ARRAY_IDS_OF_LIFE_ROBOTS, endLapCommand, MORE);
		}
	}
}
