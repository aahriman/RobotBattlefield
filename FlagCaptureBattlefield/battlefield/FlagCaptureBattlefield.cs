using System;
using System.Collections.Generic;
using System.Linq;
using BaseLibrary.battlefield;
using BaseLibrary.communication.command.common;
using BaseLibrary.communication.command.handshake;
using BaseLibrary.utils;
using BattlefieldLibrary.battlefield;
using FlagCaptureLibrary.battlefield;
using BattlefieldRobot = BattlefieldLibrary.battlefield.robot.BattlefieldRobot;


namespace FlagCaptureBattlefield.battlefield {
	public class FlagCaptureBattlefield : Battlefield {
        /// <summary>
        /// How far from flag center of flag place have to robot be to capture the flag.
        /// </summary>
        public const int FLAG_PLACE_SIZE = FlagCapture.FLAG_PLACE_SIZE;

        private static readonly int POSITION_IN_BATTLE_TURN_FLAG_PLACES;
        private static readonly int POSITION_IN_BATTLE_TURN_FLAG;

        static FlagCaptureBattlefield() {
            POSITION_IN_BATTLE_TURN_FLAG_PLACES = BattlefieldTurn.RegisterMore();
            POSITION_IN_BATTLE_TURN_FLAG = BattlefieldTurn.RegisterMore();
        }

        private readonly Dictionary<int, FlagPlace> flagPlacesById = new Dictionary<int, FlagPlace>();
        private readonly Dictionary<int, List<FlagPlace>> flagPlacesByTeamId = new Dictionary<int, List<FlagPlace>>();
        private readonly List<Flag> flags = new List<Flag>();

	    public FlagCaptureBattlefield(FlagCaptureBattlefieldConfig battlefieldConfig) : base(battlefieldConfig) {
           
	        foreach (var flagPlace in battlefieldConfig.FlagsPlaces) {
	            if (!flagPlacesByTeamId.TryGetValue(flagPlace.TEAM_ID, out List<FlagPlace> flagPlaces)) {
	                flagPlaces = new List<FlagPlace>();
                    flagPlacesByTeamId.Add(flagPlace.TEAM_ID, flagPlaces);
                }
                flagPlaces.Add(flagPlace);
	            flagPlacesById.Add(flagPlace.ID, flagPlace);
                flags.Add(new Flag(flagPlace.ID));
            }
            check();

            
        }

        /// <inheritdoc />
	    protected override RobotStateCommand AddToRobotStateCommand(RobotStateCommand robotStateCommand, BattlefieldRobot r) {
	        robotStateCommand.MORE[FlagCapture.POSITION_IN_ROBOT_STATE_COMMAND] = flags.ToArray();
            return robotStateCommand;
	    }

	    /// <inheritdoc />
	    protected override InitAnswerCommand AddToInitAnswerCommand(InitAnswerCommand initAnswerCommand) {
	        initAnswerCommand.MORE[FlagCapture.POSITION_IN_INIT_ASNWER_COMMAND] = flagPlacesById.Values.ToArray();
            return initAnswerCommand;
	    }

	    /// <inheritdoc />
		protected override LapState NewLapState() {
            return Turn > MAX_TURN ? LapState.TURNS_OUT : LapState.NONE;
        }

	    /// <inheritdoc />
        protected override void afterProcessCommand() {
        }

	    /// <inheritdoc />
        protected override void afterMovingAndDamaging() {
            foreach (var flag in flags) {
                FlagPlace flagPlace = flagPlacesById[flag.FROM_FLAGPLACE_ID];
                if (flag.RobotId == 0) {
                    double minDistance = double.MaxValue;
                    foreach (BattlefieldRobot robot in robots) {
                        if (robot.TEAM_ID != flagPlace.TEAM_ID) {
                            double distance = EuclideanSpaceUtils.Distance(robot.Position,
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
                                double distance = EuclideanSpaceUtils.Distance(robot.Position,
                                                                           robotFlagPlace.GetPosition());
                                if (distance < FLAG_PLACE_SIZE) {
                                    foreach (var robotInTeam in robotsByTeamId[robot.TEAM_ID]) {
                                        robotInTeam.Score += 10;
                                        flag.RobotId = 0;
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            battlefieldTurn.AddMore(ConvertFlag(flags), POSITION_IN_BATTLE_TURN_FLAG);
            battlefieldTurn.AddMore(flagPlacesById.Values.ToArray(), POSITION_IN_BATTLE_TURN_FLAG_PLACES);
        }

	    private void check() {
	        if (flagPlacesById.Count < TEAMS) {
	            throw new ArgumentException("Every team have to have at least one flag place.");
	        }
	    }

        /// <summary>
        /// Convert flag to ViewerFlag format in field flags.
        /// </summary>
        /// <param name="flags"></param>
        /// <returns></returns>
	    public ViewerFlag[] ConvertFlag(Flag[] flags) {
	        ViewerFlag[] viewerFlags = new ViewerFlag[flags.Length];

	        for (int i = 0; i < viewerFlags.Length; i++) {
	            Robot robot = robotsById[flags[i].RobotId];
	            viewerFlags[i] = new ViewerFlag(robot.X, robot.Y, robot.TEAM_ID);
	        }
	        return viewerFlags;
	    }


        /// <summary>
        /// Convert flag to ViewerFlag format in list flags.
        /// </summary>
        /// <param name="flags"></param>
        /// <returns></returns>
        public ViewerFlag[] ConvertFlag(List<Flag> flags) {
            List<ViewerFlag> viewerFlags = new List<ViewerFlag>();

            foreach (Flag flag in flags) {
                if (robotsById.TryGetValue(flag.RobotId, out BattlefieldRobot robot)) {
                    int teamId = flagPlacesById[flag.FROM_FLAGPLACE_ID].TEAM_ID;
                    viewerFlags.Add(new ViewerFlag(robot.X, robot.Y, teamId));
                }
            }
            return viewerFlags.ToArray();
        }
    }
}
