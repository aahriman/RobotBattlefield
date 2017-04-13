using System;
using System.Collections.Generic;
using BaseLibrary.battlefield;
using BaseLibrary.command;
using BaseLibrary.equip;
using BattlefieldLibrary.battlefield;
using ServerLibrary.config;
using BattlefieldRobot = BattlefieldLibrary.battlefield.robot.BattlefieldRobot;


namespace BaseCapcureBattlefield.battlefield {
	public class BaseCapture : Battlefield {
	    private static int positionInBattleTurn;
	    static BaseCapture() {
            positionInBattleTurn = BattlefieldTurn.RegisterMore();
	    }

		public BaseCapture(BattlefieldConfig battlefielConfig)  : base(battlefielConfig) {
        }

        protected override RobotStateCommand AddToRobotStateCommand(RobotStateCommand robotStateCommand, BattlefieldRobot r) {
	        return robotStateCommand;
	    }

	    protected override InitAnswerCommand AddToInitAnswereCommand(InitAnswerCommand initAnswerCommand) {
	        return initAnswerCommand;
	    }

		protected override LapState NewLapState() {
			
			if (turn > MAX_TURN) {
				return LapState.LAP_OUT;
			}
			
			return LapState.NONE;
		}

        protected override void handleEndTurn() {
            // add flag position
            base.handleEndTurn();
        }
    }
}
