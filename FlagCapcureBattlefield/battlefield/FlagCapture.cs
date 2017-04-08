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

        private readonly Flag[] flags;

	    public FlagCapture(int maxRobots, int robotsInTeam, Flag[] flags) : base(maxRobots, ServerConfig.MAX_LAP, robotsInTeam) {
            this.flags = flags;
            check();
        }

	    public FlagCapture(int maxRobots, int robotsInTeam, String equipmentConfigFile, Flag[] flags) : base(maxRobots, ServerConfig.MAX_LAP, robotsInTeam, equipmentConfigFile) {
	        this.flags = flags;
	        check();
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
