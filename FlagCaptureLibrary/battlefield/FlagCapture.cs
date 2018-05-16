using System;
using System.Collections.Generic;
using System.Text;
using BaseLibrary;
using BaseLibrary.communication.command;
using BaseLibrary.communication.command.common;
using BaseLibrary.communication.command.handshake;
using BaseLibrary.communication.protocol;
using BaseLibrary.utils;

namespace FlagCaptureLibrary.battlefield {
    [ModDescription()]
	public class FlagCapture {
        public const int FLAG_PLACE_SIZE = 10;

        public static readonly int POSITION_IN_ROBOT_STATE_COMMAND;
        public static readonly int POSITION_IN_INIT_ANSWER_COMMAND;


        private static readonly ISubCommandFactory INIT_SUB_COMMAND_FACTORY = new InitSubCommandFactory();
        private class InitSubCommandFactory : ISubCommandFactory {
            private const string COMMAND_BASE_NAME = "FLAG_PLACE";

            /// <inheritdoc />
            public override bool Equals(object obj) {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return true;
            }

            /// <inheritdoc />
            public override int GetHashCode() {
                return this.GetType().GetHashCode();
            }

            internal InitSubCommandFactory() { }
            public bool Deserialize(string s, object[] commandsMore) {
                if (ProtocolV1_0Utils.Deserialize(s, out string[] basesString, ProtocolV1_0Utils.DEFAULT.NEXT.NEXT)) {

                    List<FlagPlace> flagPlaces = new List<FlagPlace>();
                    foreach (string baseString in basesString) {// parameters are separated by DEFAULT.NEXT.NEXT.NEXT

                        if (ProtocolV1_0Utils.GetParams(baseString, COMMAND_BASE_NAME,
                                                        ProtocolV1_0Utils.DEFAULT.NEXT.NEXT.NEXT, out string[] flagPlaceParams)) {
                            if (flagPlaceParams.Length == 3) {
                                if (ProtocolDouble.TryParse(flagPlaceParams[0], out ProtocolDouble x) &&
                                    ProtocolDouble.TryParse(flagPlaceParams[1], out ProtocolDouble y) &&
                                    int.TryParse(flagPlaceParams[2], out int teamId) ) {

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
                if (singleMore is FlagPlace[] o) {
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

        private static readonly ISubCommandFactory STATE_SUB_COMMAND_FACTORY = new StateStateSubCommandFactory();
        private class StateStateSubCommandFactory : ISubCommandFactory {
            private const string COMMAND_BASE_NAME = "FLAG";

            /// <inheritdoc />
            public override bool Equals(object obj) {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return true;
            }

            /// <inheritdoc />
            public override int GetHashCode() {
                return this.GetType().GetHashCode();
            }

            internal StateStateSubCommandFactory() { }
            public bool Deserialize(string s, object[] commandsMore) {
                if (commandsMore.Length <= POSITION_IN_ROBOT_STATE_COMMAND) {
                    throw new NotSupportedException("Flags have no space for deserialize. Maybe load was corrupted.");
                }
                if (ProtocolV1_0Utils.Deserialize(s, out string[] basesString, ProtocolV1_0Utils.DEFAULT.NEXT.NEXT)) {

                    List<Flag> flags = new List<Flag>();
                    foreach (string baseString in basesString) {// parametrs are separated by DEFAULT.NEXT.NEXT.NEXT

                        if (ProtocolV1_0Utils.GetParams(baseString, COMMAND_BASE_NAME,
                                                        ProtocolV1_0Utils.DEFAULT.NEXT.NEXT.NEXT, out string[] flagParams)) {
                            if (flagParams.Length == 2) {
                                if (int.TryParse(flagParams[0], out int flagPlaceId) && int.TryParse(flagParams[1], out int RobotId)) {

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
            POSITION_IN_INIT_ANSWER_COMMAND = InitAnswerCommand.RegisterSubCommandFactory(INIT_SUB_COMMAND_FACTORY);
            POSITION_IN_ROBOT_STATE_COMMAND = RobotStateCommand.RegisterSubCommandFactory(STATE_SUB_COMMAND_FACTORY);
        }

        /// <summary>
        /// Return flag from <code>RobotStateCommand</code>
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static Flag[] GetFlags(RobotStateCommand state) {
            return (Flag[])state.MORE[POSITION_IN_ROBOT_STATE_COMMAND];
        }


        /// <summary>
        /// Return FlagPlaces from <code>InitAnswerCommand</code>
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static FlagPlace[] GetFlagPlaces(InitAnswerCommand state) {
            return (FlagPlace[])state.MORE[POSITION_IN_INIT_ANSWER_COMMAND];
        }

    }
}
