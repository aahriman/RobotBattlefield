using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using BaseCapcureBattlefieldLibrary.battlefield;
using BaseLibrary;
using BaseLibrary.battlefield;
using BaseLibrary.command;
using BaseLibrary.command.common;
using BaseLibrary.command.handshake;
using BaseLibrary.equip;
using BaseLibrary.protocol;
using BaseLibrary.utils;
using BaseLibrary.utils.euclidianSpaceStruct;
using BattlefieldLibrary.battlefield;
using BattlefieldRobot = BattlefieldLibrary.battlefield.robot.BattlefieldRobot;


namespace BaseCapcureBattlefield.battlefield {
	public class BaseCaptureBattlefield : Battlefield {
        public const int BASE_SIZE = 25;

        private static readonly int POSITION_IN_BATTLE_TURN;

        static BaseCaptureBattlefield() {
            POSITION_IN_BATTLE_TURN = BattlefieldTurn.RegisterMore();
        }

	    private readonly Base[] bases;
        private readonly Dictionary<Base, List<BattlefieldRobot>> aliveRobotsAtBaseByBase = new Dictionary<Base, List<BattlefieldRobot>>();

        public BaseCaptureBattlefield(BaseCaptureBattlefieldConfig battlefielConfig)  : base(battlefielConfig) {
            bases = battlefielConfig.BASES;
            foreach (var @base in bases) {
                aliveRobotsAtBaseByBase.Add(@base, new List<BattlefieldRobot>());
            }
        }

        protected override RobotStateCommand AddToRobotStateCommand(RobotStateCommand robotStateCommand, BattlefieldRobot r) {
	        robotStateCommand.MORE[BaseCapture.POSITION_IN_ROBOT_STATE_COMMAND] = bases.ToArray();
            return robotStateCommand;

        }

	    protected override InitAnswerCommand AddToInitAnswereCommand(InitAnswerCommand initAnswerCommand) {
	        return initAnswerCommand;
	    }

	    protected override void afterProcessCommand() {
	    }

	    protected override void afterMovingAndDamaging() {
	        capturingBases();
            battlefieldTurn.AddMore(bases, POSITION_IN_BATTLE_TURN);
	        repairRobotsInTheirBase();
	    }

        private void capturingBases() {
            foreach (var @base in bases) {
                aliveRobotsAtBaseByBase[@base].Clear();
                Dictionary<int, int> progressByTeamId = new Dictionary<int, int>();
                progressByTeamId[@base.ProgressTeamId] = @base.Progress;
                foreach (var robot in getAliveRobots()) {
                    if (EuclideanSpaceUtils.Distance(new Point(@base.X, @base.Y), new Point(robot.X, robot.Y)) < BASE_SIZE) {
                        aliveRobotsAtBaseByBase[@base].Add(robot);
                        robot.Score++;
                        int progressForTeam;
                        if (!progressByTeamId.TryGetValue(robot.TEAM_ID, out progressForTeam)) {
                            progressForTeam = 0;
                        }
                        progressForTeam++;
                        progressByTeamId[robot.TEAM_ID] = progressForTeam;
                    }
                }

                int maxTeamProgress = 0;
                int forTeamId = 0;
                foreach (var teamProgress in progressByTeamId) {
                    if (maxTeamProgress < teamProgress.Value) {
                        maxTeamProgress = teamProgress.Value;
                        forTeamId = teamProgress.Key;
                    }
                }

                int sumOtherProgress = 0;
                foreach (var teamProgress in progressByTeamId) {
                    if (teamProgress.Key != forTeamId) {
                        sumOtherProgress += teamProgress.Value;
                    }
                }

                @base.Progress = maxTeamProgress - sumOtherProgress;
                @base.ProgressTeamId = forTeamId;
                if (@base.Progress == @base.MAX_PROGRESS) {
                    @base.TeamId = @base.ProgressTeamId;
                }
                @base.Progress = Math.Max(@base.Progress, 0);
                @base.Progress = Math.Min(@base.Progress, @base.MAX_PROGRESS);

            }
        }

        private void repairRobotsInTheirBase() {
            foreach (KeyValuePair<Base, List<BattlefieldRobot>> keyValuePair in aliveRobotsAtBaseByBase) {
                Base @base = keyValuePair.Key;
                List<BattlefieldRobot> robotsInBase = keyValuePair.Value;
                foreach (var robot in robotsInBase) {
                    if (robot.TEAM_ID == @base.TeamId) {
                        robot.HitPoints += 5 * (@base.Progress / 100);
                    }   
                }
            }
        }

        protected override LapState NewLapState() {
            int teamId = bases.First().TeamId;
            if ((from @base in bases
                 where @base.TeamId == teamId
                 where @base.Progress > 0
                 select @base).Count() == bases.Length) {
                return LapState.WIN;
            }

            return turn > MAX_TURN ? LapState.LAP_OUT : LapState.NONE;
        }
    }
}
