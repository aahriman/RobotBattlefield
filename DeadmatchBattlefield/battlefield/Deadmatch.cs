using System.Collections.Generic;
using BaseLibrary.battlefield;
using BaseLibrary.communication.command.common;
using BaseLibrary.communication.command.handshake;
using BattlefieldLibrary.battlefield;
using BattlefieldRobot = BattlefieldLibrary.battlefield.robot.BattlefieldRobot;


namespace DeadmatchBattlefield.battlefield {
	public class Deadmatch : Battlefield {
	    /// <inheritdoc />
        public Deadmatch(BattlefieldConfig battlefieldConfig)  : base(battlefieldConfig) {
        }

	    /// <inheritdoc />
        protected override RobotStateCommand AddToRobotStateCommand(RobotStateCommand robotStateCommand, BattlefieldRobot r) {
	        return robotStateCommand;
	    }

        /// <inheritdoc />
	    protected override InitAnswerCommand AddToInitAnswerCommand(InitAnswerCommand initAnswerCommand) {
	        return initAnswerCommand;
	    }

	    /// <inheritdoc />
		protected override LapState NewLapState() {
			if (Turn > MAX_TURN) {
				return LapState.TURNS_OUT;
			}
            List<BattlefieldRobot> aliveRobots = getAliveRobots();
            if (aliveRobots.Count <= 1 && aliveRobots.Count != robots.Count) {
				return LapState.SOMEONE_WIN;
			}
			return LapState.NONE;
		}

	    /// <inheritdoc />
	    protected override void afterProcessCommand() {
	        // do nothing
	    }

	    /// <inheritdoc />
	    protected override void afterMovingAndDamaging() {
	        // do nothing
	    }
	}
}
