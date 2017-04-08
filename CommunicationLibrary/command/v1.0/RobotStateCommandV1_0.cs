using System;
using CommunicationLibrary.utils;

namespace CommunicationLibrary.command.v1._0 {
	public class RobotStateCommandV1_0 : RobotStateCommand, ACommand.Sendable {
		private const string NAME = "STATE";
		public static readonly ICommandFactory FACTORY = new CommandFactory();
		private sealed class CommandFactory : ACommandFactory {
			public override Boolean IsDeserializable(String s) {
				
				string[] rest;
				if (ProtocolV1_0Utils.GetParams(s, NAME, out rest)) {
					if (rest.Length == 8) {
						ProtocolDouble[] paramsPDoubles;
						int[] paramInts;
						int[] arrayIdsOfLifeRobots;
						if (Parser.TryParse(new int[] { 0, 1, 3 }, rest, out paramsPDoubles) &&
							Parser.TryParse(new int[] { 2, 4, 5, 6 }, rest, out paramInts) &&
							ProtocolV1_0Utils.Deserialize(rest[7], out arrayIdsOfLifeRobots, ProtocolV1_0Utils.DEFAULT.NEXT)
						) {
							cache.Cached(s, new RobotStateCommandV1_0(paramsPDoubles[0], paramsPDoubles[1], paramInts[0], paramsPDoubles[2], paramInts[1], paramInts[2], paramInts[3], arrayIdsOfLifeRobots));
							return true;
						}
					}
				}
				return false;
			}

			public override bool IsTransferable(ACommand c) {
				if (c is RobotStateCommand) {
					var c1 = (RobotStateCommand)c;
					cache.Cached(c, new RobotStateCommandV1_0(c1.X, c1.Y, c1.HIT_POINTS, c1.POWER, c1.LAP, c1.MAX_LAP, c1.COUNT_OF_LIFE_ROBOTS, c1.ARRAY_IDS_OF_LIFE_ROBOTS));
					return true;
				}
				return false;
			}
		}

		public RobotStateCommandV1_0(ProtocolDouble x, ProtocolDouble y, int hitPoints, ProtocolDouble power, int lap, int maxLap, int countOfLifeRobots, int[] arrayIdsOfLifeRobots) :
			base(x, y, hitPoints, power, lap, maxLap, countOfLifeRobots, arrayIdsOfLifeRobots) { }

		public string serialize() {
			return ProtocolV1_0Utils.SerializeParams("STATE", X, Y, HIT_POINTS, POWER, LAP, MAX_LAP, COUNT_OF_LIFE_ROBOTS, ARRAY_IDS_OF_LIFE_ROBOTS);
		}
	}
}
