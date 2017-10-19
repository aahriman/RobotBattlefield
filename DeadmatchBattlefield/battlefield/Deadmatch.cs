using System;
using System.Collections.Generic;
using System.IO;
using BaseLibrary.battlefield;
using BaseLibrary.command;
using BaseLibrary.command.common;
using BaseLibrary.command.handshake;
using BaseLibrary.equip;
using BattlefieldLibrary.battlefield;
using BattlefieldRobot = BattlefieldLibrary.battlefield.robot.BattlefieldRobot;


namespace DeadmatchBattlefield.battlefield {
	public class Deadmatch : Battlefield {
		public Deadmatch(BattlefieldConfig battlefieldConfig)  : base(battlefieldConfig) {
        }

        protected override RobotStateCommand AddToRobotStateCommand(RobotStateCommand robotStateCommand, BattlefieldRobot r) {
	        return robotStateCommand;
	    }

	    protected override InitAnswerCommand AddToInitAnswereCommand(InitAnswerCommand initAnswerCommand) {
	        return initAnswerCommand;
	    }

		protected override LapState NewLapState() {
			if (Turn > MAX_TURN) {
				return LapState.LAP_OUT;
			}
            List<BattlefieldRobot> aliveRobots = getAliveRobots();
            if (aliveRobots.Count <= 1 && aliveRobots.Count != robots.Count) {
				return LapState.WIN;
			}
			return LapState.NONE;
		}

	    protected override void afterProcessCommand() {
	        // do nothing
	    }

	    protected override void afterMovingAndDamaging() {
	        // do nothing
	    }
	}
}
