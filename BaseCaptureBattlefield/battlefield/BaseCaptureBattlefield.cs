using System;
using System.Collections.Generic;
using System.Linq;
using BaseCaptureLibrary.battlefield;
using BaseLibrary.battlefield;
using BaseLibrary.communication.command.common;
using BaseLibrary.communication.command.handshake;
using BaseLibrary.utils;
using BaseLibrary.utils.euclidianSpaceStruct;
using BattlefieldLibrary.battlefield;
using BattlefieldRobot = BattlefieldLibrary.battlefield.robot.BattlefieldRobot;


namespace BaseCaptureBattlefield.battlefield {
	public class BaseCaptureBattlefield : Battlefield {
        public const int BASE_SIZE = 25;

        private static readonly int POSITION_IN_BATTLE_TURN;

        static BaseCaptureBattlefield() {
            POSITION_IN_BATTLE_TURN = BattlefieldTurn.RegisterMore();
        }

	    private readonly Base[] bases;
        private readonly Dictionary<Base, List<BattlefieldRobot>> aliveRobotsAtBaseByBase = new Dictionary<Base, List<BattlefieldRobot>>();

        public BaseCaptureBattlefield(BaseCaptureBattlefieldConfig battlefieldConfig)  : base(battlefieldConfig) {
            bases = battlefieldConfig.BASES;
            foreach (var @base in bases) {
                aliveRobotsAtBaseByBase.Add(@base, new List<BattlefieldRobot>());
            }
        }

        protected override RobotStateCommand addToRobotStateCommand(RobotStateCommand robotStateCommand, BattlefieldRobot r) {
	        robotStateCommand.MORE[BaseCapture.POSITION_IN_ROBOT_STATE_COMMAND] = bases.ToArray();
            return robotStateCommand;

        }

	    protected override InitAnswerCommand addToInitAnswerCommand(InitAnswerCommand initAnswerCommand) {
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
                        if (@base.TeamId != robot.TEAM_ID) {
                            robot.Score++;
                        }
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

                if (@base.Progress <= 0) {
                    @base.TeamId = 0;
                }

                @base.Progress = Math.Abs(@base.Progress);
                @base.Progress = Math.Min(@base.Progress, @base.MAX_PROGRESS);

                if (@base.Progress == @base.MAX_PROGRESS) {
                    @base.TeamId = @base.ProgressTeamId;
                }
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

        protected override LapState newLapState() {
            int teamId = bases.First().TeamId;
            if ((from @base in bases
                 where @base.TeamId == teamId
                 where @base.Progress > @base.MAX_PROGRESS / 2
                 select @base).Count() == bases.Length) {
                return LapState.SOMEONE_WIN;
            }

            return turn > MAX_TURN ? LapState.TURNS_OUT : LapState.NONE;
        }
    }
}
