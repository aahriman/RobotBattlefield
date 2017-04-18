using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseLibrary;
using BaseLibrary.battlefield;
using BaseLibrary.command;
using BaseLibrary.equip;
using BaseLibrary.protocol;
using BaseLibrary.utils;
using BattlefieldLibrary.battlefield;
using ServerLibrary.config;
using BattlefieldRobot = BattlefieldLibrary.battlefield.robot.BattlefieldRobot;


namespace FlagCapcureBattlefield.battlefield {
    [ModDescription()]
	public class FlagCapture : Battlefield {
        public const int FLAG_PLACE_SIZE = 10;

        private static readonly int positionInBattleTurnFlagPlaces;
        private static readonly int positionInBattleTurnFlag;

        public static readonly int POSITION_IN_ROBOT_STATE_COMMAND;
        public static readonly int POSITION_IN_INIT_ASNWER_COMMAND;


        private static readonly ISubCommandFactory INIT_SUB_COMMAND_FACTORY = new SubCommandFactory();
        private class SubCommandFactory : ISubCommandFactory {
            private const string COMMAND_BASE_NAME = "FLAG_PLACE";
            internal SubCommandFactory() { }
            public bool Deserialize(string s, object[] commandsMore) {
                String[] basesString;
                if (ProtocolV1_0Utils.Deserialize(s, out basesString, ProtocolV1_0Utils.DEFAULT.NEXT.NEXT)) {

                    List<FlagPlace> flagPlaces = new List<FlagPlace>();
                    foreach (var baseString in basesString) {// parametrs are separated by DEFAULT.NEXT.NEXT.NEXT
                        String[] flagPlaceParams;

                        if (ProtocolV1_0Utils.GetParams(baseString, COMMAND_BASE_NAME,
                                                        ProtocolV1_0Utils.DEFAULT.NEXT.NEXT.NEXT, out flagPlaceParams)) {
                            if (flagPlaceParams.Length == 3) {
                                ProtocolDouble x, y;
                                int teamId;
                                if (ProtocolDouble.TryParse(flagPlaceParams[0], out x) &&
                                    ProtocolDouble.TryParse(flagPlaceParams[1], out y) &&
                                    int.TryParse(flagPlaceParams[2], out teamId) ) {

                                    FlagPlace flagPlace = new FlagPlace(x, y, teamId);
                                    flagPlaces.Add(flagPlace);
                                }
                            }
                        }
                    }
                    commandsMore[POSITION_IN_ROBOT_STATE_COMMAND] = flagPlaces.ToArray();
                }
                return false;
            }

            public bool Serialize(object singleMore, out string serializedSingleMore) {
                // this is use in RobotStateCommand (DEFAULT) in array (DEFAULT.NEXT) => separator for arrays items is DEAULT.NEXT.NEXT
                FlagPlace[] o = singleMore as FlagPlace[];
                if (o != null) {
                    StringBuilder sb = new StringBuilder();
                    if (0 < o.Length) {
                        sb.Append("[");
                    }
                    for (int i = 0; i < o.Length; i++) {
                        sb.Append(ProtocolV1_0Utils.ConvertToDeeper(ProtocolV1_0Utils.SerializeParams(COMMAND_BASE_NAME, o[i].X, o[i].Y, o[i].TEAM_ID),
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

        static FlagCapture() {
            positionInBattleTurnFlagPlaces = BattlefieldTurn.RegisterMore();
            positionInBattleTurnFlag = BattlefieldTurn.RegisterMore();
            POSITION_IN_INIT_ASNWER_COMMAND = InitAnswerCommand.RegisterSubCommandFactory(INIT_SUB_COMMAND_FACTORY);
        }

        private readonly Dictionary<int, FlagPlace> flagPlacesById = new Dictionary<int, FlagPlace>();
        private readonly Dictionary<int, List<FlagPlace>> flagPlacesByTeamId = new Dictionary<int, List<FlagPlace>>();
        private readonly List<Flag> flags = new List<Flag>();

	    public FlagCapture(FlagCaptureBattlefieldConfig battlefieldConfig) : base(battlefieldConfig) {
           
	        foreach (var flagPlace in battlefieldConfig.FlagsPlace) {
	            List<FlagPlace> flagPlaces;
	            if (!flagPlacesByTeamId.TryGetValue(flagPlace.TEAM_ID, out flagPlaces)) {
	                flagPlaces = new List<FlagPlace>();
                    flagPlacesByTeamId.Add(flagPlace.TEAM_ID, flagPlaces);
	            }
                flagPlaces.Add(flagPlace);
	            flagPlacesById.Add(flagPlace.ID, flagPlace);
                flags.Add(new Flag(flagPlace.ID));
            }
            check();
        }

	    protected override RobotStateCommand AddToRobotStateCommand(RobotStateCommand robotStateCommand, BattlefieldRobot r) {
	        robotStateCommand.MORE[POSITION_IN_ROBOT_STATE_COMMAND] = flags;
            return robotStateCommand;
	    }

	    protected override InitAnswerCommand AddToInitAnswereCommand(InitAnswerCommand initAnswerCommand) {
	        var o = flagPlacesById.Values.ToArray();
	        initAnswerCommand.MORE[POSITION_IN_INIT_ASNWER_COMMAND] = o;

            return initAnswerCommand;
	    }

		protected override LapState NewLapState() {
            return turn > MAX_TURN ? LapState.LAP_OUT : LapState.NONE;
        }

        protected override void afterProcessCommand() {
        }

        protected override void afterMovingAndDamaging() {
            foreach (var flag in flags) {
                FlagPlace flagPlace = flagPlacesById[flag.FROM_FLAGPLACE_ID];
                if (flag.RobotId == 0) {
                    double minDistance = Double.MaxValue;
                    foreach (BattlefieldRobot robot in robots) {
                        if (robot.TEAM_ID != flagPlace.TEAM_ID) {
                            double distance = EuclideanSpaceUtils.Distance(robot.GetPosition(),
                                                                           flagPlace.GetPosition());
                            if (distance < FLAG_PLACE_SIZE && distance < minDistance) {
                                flag.RobotId = robot.ID;
                                minDistance = distance;
                            }
                        }
                    }
                } else {
                    BattlefieldRobot robot;
                    if (robotsById.TryGetValue(flag.RobotId, out robot)) {
                        if (robot.HitPoints <= 0) {
                            flag.RobotId = 0;
                        } else {
                            List<FlagPlace> robotFlagPlaces = flagPlacesByTeamId[robot.TEAM_ID];
                            foreach (var robotFlagPlace in robotFlagPlaces) {
                                double distance = EuclideanSpaceUtils.Distance(robot.GetPosition(),
                                                                           robotFlagPlace.GetPosition());
                                if (distance < FLAG_PLACE_SIZE) {
                                    foreach (var robotInTeam in robotsByTeamId) {
                                        robotInTeam.SCORE += 10;
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            battlefieldTurn.AddMore(flags, positionInBattleTurnFlagPlaces);
        }

	    private void check() {
	        if (flagPlacesById.Count < MAX_ROBOTS / ROBOTS_IN_TEAM) {
	            throw new ArgumentException("Every team have to have at least one flag place.");
	        }
	    }

	    
	}
}
