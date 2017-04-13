using System;
using System.Collections.Generic;
using System.Linq;
using BaseLibrary.battlefield;
using BaseLibrary.command;
using BaseLibrary.equip;
using BattlefieldLibrary.battlefield;
using ServerLibrary.config;
using BattlefieldRobot = BattlefieldLibrary.battlefield.robot.BattlefieldRobot;


namespace FlagCapcureBattlefield.battlefield {
	public class FlagCapture : Battlefield {
        private static readonly int positionInBattleTurn;

        static FlagCapture() {
            positionInBattleTurn = BattlefieldTurn.RegisterMore();
        }

        private static Flag[] fromMoreToFlags(object[] more) {
            return (
                from m in more
                where (m is Flag)
                select m as Flag).ToArray();
        }

        private readonly Flag[] flags;

	    public FlagCapture(BattlefieldConfig battlefieldConfig) : base(battlefieldConfig) {
            this.flags = fromMoreToFlags(battlefieldConfig.MORE);
            check();
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
	        foreach (Flag flag in flags) {
                battlefieldTurn.AddMore(flag, positionInBattleTurn);
            }
	        base.handleEndTurn();
	    }

	    private void check() {
	        var teamsId = flags.GroupBy(flag => flag.TEAM_ID).Select(g => g.First()); // get distinc TEAM_ID
                          
	        if (teamsId.Count() < MAX_ROBOTS / ROBOTS_IN_TEAM) {
	            throw new ArgumentException("Every team have to have at least one flag.");
	        }
	    }
	}
}
