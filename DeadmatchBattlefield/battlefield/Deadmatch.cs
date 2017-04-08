using System;
using System.Collections.Generic;
using System.IO;
using BaseLibrary.battlefield;
using BaseLibrary.command;
using BaseLibrary.equip;
using BattlefieldLibrary.battlefield;
using ServerLibrary.config;
using BattlefieldRobot = BattlefieldLibrary.battlefield.robot.BattlefieldRobot;


namespace DeadmatchBattlefield.battlefield {
	public class Deadmatch : Battlefield {
		public Deadmatch(int maxRobots, int robotsInTeam)  : base(maxRobots, ServerConfig.MAX_LAP, robotsInTeam) {
        }

        public Deadmatch(int maxRobots, int robotsInTeam, string equipmentFileName) : base(maxRobots, ServerConfig.MAX_LAP, robotsInTeam, equipmentFileName) {
        }

        protected override RobotStateCommand AddToRobotStateCommand(RobotStateCommand robotStateCommand, BattlefieldRobot r) {
	        return robotStateCommand;
	    }

	    protected override InitAnswerCommand AddToInitAnswereCommand(InitAnswerCommand initAnswerCommand) {
	        return initAnswerCommand;
	    }

		protected override LapState newLapState() {
			if (turn > MAX_TURN) {
				return LapState.LAP_OUT;
			}
            List<BattlefieldRobot> aliveRobots = getAliveRobots();
            if (aliveRobots.Count <= 1 && aliveRobots.Count != robots.Count) {
				return LapState.WIN;
			}
			return LapState.NONE;
		}
	}
}
