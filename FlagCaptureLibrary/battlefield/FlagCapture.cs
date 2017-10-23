using System;
using System.Collections.Generic;
using System.Text;
using BaseLibrary;
using BaseLibrary.command;
using BaseLibrary.command.common;
using BaseLibrary.command.handshake;
using BaseLibrary.protocol;
using BaseLibrary.utils;

namespace FlagCaptureLibrary.battlefield {
    [ModDescription()]
	public class FlagCapture {
        public const int FLAG_PLACE_SIZE = 10;

        public static readonly int POSITION_IN_ROBOT_STATE_COMMAND;
        public static readonly int POSITION_IN_INIT_ASNWER_COMMAND;


        private static readonly ISubCommandFactory INIT_SUB_COMMAND_FACTORY = new SubCommandFactory();
        private class SubCommandFactory : ISubCommandFactory {
            private const string COMMAND_BASE_NAME = "FLAG_PLACE";
            internal SubCommandFactory() { }
            public bool Deserialize(string s, object[] commandsMore) {
                string[] basesString;
                if (ProtocolV1_0Utils.Deserialize(s, out basesString, ProtocolV1_0Utils.DEFAULT.NEXT.NEXT)) {

                    List<FlagPlace> flagPlaces = new List<FlagPlace>();
                    foreach (var baseString in basesString) {// parametrs are separated by DEFAULT.NEXT.NEXT.NEXT
                        string[] flagPlaceParams;

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
                // this is use in InitAnswerCommand (DEFAULT) in array (DEFAULT.NEXT) => separator for arrays items is DEAULT.NEXT.NEXT
                FlagPlace[] o = singleMore as FlagPlace[];
                if (o != null) {
                    StringBuilder sb = new StringBuilder();
                    if (0 < o.Length) {
                        sb.Append("[");
                    }
                    for (int i = 0; i < o.Length; i++) {
                        sb.Append(ProtocolV1_0Utils.ConvertToDeeper(ProtocolV1_0Utils.SerializeParams(COMMAND_BASE_NAME, (ProtocolDouble) o[i].X, (ProtocolDouble) o[i].Y, o[i].TEAM_ID),
                                                          ProtocolV1_0Utils.DEFAULT.NEXT.NEXT.NEXT));
                        if (i + 1 < o.Length) {
                            sb.Append(ProtocolV1_0Utils.DEFAULT.NEXT.NEXT.SEPARATOR);
                        }
                    }
                    if (0 < o.Length) {
                        sb.Append("]");
                    }
                    serializedSingleMore = sb.ToString();
                    return true;
                }
                serializedSingleMore = null;
                return false;
            }
        }

        private static readonly ISubCommandFactory STATE_SUB_COMMAND_FACTORY = new StateSubCommandFactory();
        private class StateSubCommandFactory : ISubCommandFactory {
            private const string COMMAND_BASE_NAME = "FLAG";
            internal StateSubCommandFactory() { }
            public bool Deserialize(string s, object[] commandsMore) {
                string[] basesString;
                if (ProtocolV1_0Utils.Deserialize(s, out basesString, ProtocolV1_0Utils.DEFAULT.NEXT.NEXT)) {

                    List<Flag> flags = new List<Flag>();
                    foreach (var baseString in basesString) {// parametrs are separated by DEFAULT.NEXT.NEXT.NEXT
                        string[] flagParams;

                        if (ProtocolV1_0Utils.GetParams(baseString, COMMAND_BASE_NAME,
                                                        ProtocolV1_0Utils.DEFAULT.NEXT.NEXT.NEXT, out flagParams)) {
                            if (flagParams.Length == 2) {
                                int flagPlaceId, RobotId;
                                if (int.TryParse(flagParams[0], out flagPlaceId) && int.TryParse(flagParams[1], out RobotId)) {

                                    Flag flag = new Flag(flagPlaceId, RobotId);
                                    flags.Add(flag);
                                }
                            }
                        }
                    }
                    commandsMore[POSITION_IN_ROBOT_STATE_COMMAND] = flags.ToArray();
                }
                return false;
            }

            public bool Serialize(object singleMore, out string serializedSingleMore) {
                // this is use in RobotStateCommand (DEFAULT) in array (DEFAULT.NEXT) => separator for arrays items is DEAULT.NEXT.NEXT
                Flag[] o = singleMore as Flag[];
                if (o != null) {
                    StringBuilder sb = new StringBuilder();
                    if (0 < o.Length) {
                        sb.Append("[");
                    }
                    for (int i = 0; i < o.Length; i++) {
                        sb.Append(ProtocolV1_0Utils.ConvertToDeeper(ProtocolV1_0Utils.SerializeParams(COMMAND_BASE_NAME, o[i].FROM_FLAGPLACE_ID, o[i].RobotId),
                                                          ProtocolV1_0Utils.DEFAULT.NEXT.NEXT.NEXT));
                        if (i + 1 < o.Length) {
                            sb.Append(ProtocolV1_0Utils.DEFAULT.NEXT.NEXT.SEPARATOR);
                        }
                    }
                    if (0 < o.Length) {
                        sb.Append("]");
                    }
                    serializedSingleMore = sb.ToString();
                    return true;
                }
                serializedSingleMore = null;
                return false;
            }
        }

        static FlagCapture() {
            POSITION_IN_INIT_ASNWER_COMMAND = InitAnswerCommand.RegisterSubCommandFactory(INIT_SUB_COMMAND_FACTORY);
            POSITION_IN_ROBOT_STATE_COMMAND = RobotStateCommand.RegisterSubCommandFactory(STATE_SUB_COMMAND_FACTORY);
        }

        private readonly Dictionary<int, FlagPlace> flagPlacesById = new Dictionary<int, FlagPlace>();
        private readonly Dictionary<int, List<FlagPlace>> flagPlacesByTeamId = new Dictionary<int, List<FlagPlace>>();
        private readonly List<Flag> flags = new List<Flag>();

        public static Flag[] GetFlags(RobotStateCommand state) {
            return (Flag[])state.MORE[POSITION_IN_ROBOT_STATE_COMMAND];
        }

        public static FlagPlace[] GetFlagPlaces(InitAnswerCommand state) {
            return (FlagPlace[])state.MORE[POSITION_IN_INIT_ASNWER_COMMAND];
        }

    }
}
