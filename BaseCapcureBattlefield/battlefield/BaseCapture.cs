using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
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
using ServerLibrary.config;
using BattlefieldRobot = BattlefieldLibrary.battlefield.robot.BattlefieldRobot;


namespace BaseCapcureBattlefield.battlefield {
    [ModDescription()]
	public class BaseCapture : Battlefield {
        private const int BASE_SIZE = 25;

        private static readonly int POSITION_IN_BATTLE_TURN;
        private static readonly int POSITION_IN_ROBOT_STATE_COMMAND;

        private static readonly ISubCommandFactory SUB_COMMAND_FACTORY = new SubCommandFactory();
        private class SubCommandFactory : ISubCommandFactory {
            private const string COMMAND_BASE_NAME = "BASE";
            internal SubCommandFactory() {}
            public bool Deserialize(string s, object[] commandsMore) {
                String[] basesString;
                if (ProtocolV1_0Utils.Deserialize(s, out basesString, ProtocolV1_0Utils.DEFAULT.NEXT.NEXT)) {
                    
                        List<Base> bases = new List<Base>();
                        foreach (var baseString in basesString) {// parametrs are separated by DEFAULT.NEXT.NEXT.NEXT
                            String[] baseParam;

                            if (ProtocolV1_0Utils.GetParams(baseString, COMMAND_BASE_NAME,
                                                            ProtocolV1_0Utils.DEFAULT.NEXT.NEXT.NEXT, out baseParam)) {
                                if (baseParam.Length == 4) {
                                    ProtocolDouble x, y;
                                    int maxProgress, teamId, progress;
                                    if (ProtocolDouble.TryParse(baseParam[0], out x) &&
                                        ProtocolDouble.TryParse(baseParam[1], out y) &&
                                        int.TryParse(baseParam[2], out maxProgress) &&
                                        int.TryParse(baseParam[3], out progress) &&
                                        int.TryParse(baseParam[4], out teamId)) {

                                        Base @base = new Base(x, y, maxProgress);
                                        @base.Progress = progress;
                                        @base.TeamId = teamId;

                                        bases.Add(@base);
                                    }
                                }
                            }
                        }
                        commandsMore[POSITION_IN_ROBOT_STATE_COMMAND] = bases.ToArray();
                }
                return false;
            }

            public bool Serialize(object singleMore, out string serializedSingleMore) {
                // this is use in RobotStateCommand (DEFAULT) in array (DEFAULT.NEXT) => separator for arrays items is DEAULT.NEXT.NEXT
                Base[] o = singleMore as Base[];
                if (o != null) {
                    StringBuilder sb = new StringBuilder();
                    if (0 < o.Length) {
                        sb.Append("[");
                    }
                    for (int i = 0; i < o.Length; i++) {
                        sb.Append(ProtocolV1_0Utils.ConvertToDeeper(ProtocolV1_0Utils.SerializeParams(COMMAND_BASE_NAME, o[i].X, o[i].Y, o[i].MAX_PROGRESS, o[i].Progress, o[i].TeamId),
                                                          ProtocolV1_0Utils.DEFAULT.NEXT.NEXT.NEXT));
                        if (i + 1 < o.Length) {
                            sb.Append(ProtocolV1_0Utils.DEFAULT.NEXT.NEXT.SEPARATOR);
                        }
                    }
                    if (0 < o.Length) {
                        sb.Append("[");
                    }
                    serializedSingleMore = sb.ToString();
                    return true;
                }
                serializedSingleMore = null;
                return false;
            }
        }

        static BaseCapture() {
            POSITION_IN_BATTLE_TURN = BattlefieldTurn.RegisterMore();
            POSITION_IN_ROBOT_STATE_COMMAND = RobotStateCommand.RegisterSubCommandFactory(SUB_COMMAND_FACTORY);
        }

	    private readonly Base[] bases;
        private readonly Dictionary<Base, List<BattlefieldRobot>> aliveRobotsAtBaseByBase = new Dictionary<Base, List<BattlefieldRobot>>();

        public BaseCapture(BaseCaptureBattlefieldConfig battlefielConfig)  : base(battlefielConfig) {
            bases = battlefielConfig.BASES;
            foreach (var @base in bases) {
                aliveRobotsAtBaseByBase.Add(@base, new List<BattlefieldRobot>());
            }
        }

        protected override RobotStateCommand AddToRobotStateCommand(RobotStateCommand robotStateCommand, BattlefieldRobot r) {
	        robotStateCommand.MORE[POSITION_IN_ROBOT_STATE_COMMAND] = bases;
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
                progressByTeamId[@base.TeamId] = @base.Progress;
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
                @base.TeamId = forTeamId;
                @base.Progress = Math.Max(@base.Progress, 0);

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
                 select @base).Count() == bases.Length) {
                return LapState.WIN;
            }

            return turn > MAX_TURN ? LapState.LAP_OUT : LapState.NONE;
        }
    }
}
