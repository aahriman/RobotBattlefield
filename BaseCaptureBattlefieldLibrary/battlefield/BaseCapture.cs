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


namespace BaseCapcureBattlefieldLibrary.battlefield {
    [ModDescription()]
	public class BaseCapture  {
        public const int BASE_SIZE = 25;

        public static readonly int POSITION_IN_ROBOT_STATE_COMMAND;

        private static readonly ISubCommandFactory SUB_COMMAND_FACTORY = new SubCommandFactory();
        private class SubCommandFactory : ISubCommandFactory {
            private const string COMMAND_BASE_NAME = "BASE";
            internal SubCommandFactory() { }
            public bool Deserialize(string s, object[] commandsMore) {
                string[] basesString;
                if (ProtocolV1_0Utils.Deserialize(s, out basesString, ProtocolV1_0Utils.DEFAULT.NEXT.NEXT)) {

                    List<Base> bases = new List<Base>();
                    foreach (var baseString in basesString) {// parametrs are separated by DEFAULT.NEXT.NEXT.NEXT
                        string[] baseParam;

                        if (ProtocolV1_0Utils.GetParams(baseString, COMMAND_BASE_NAME,
                                                        ProtocolV1_0Utils.DEFAULT.NEXT.NEXT.NEXT, out baseParam)) {
                            if (baseParam.Length == 6) {
                                ProtocolDouble x, y;
                                int maxProgress, teamId, progress, progressTeamId;
                                if (ProtocolDouble.TryParse(baseParam[0], out x) &&
                                    ProtocolDouble.TryParse(baseParam[1], out y) &&
                                    int.TryParse(baseParam[2], out maxProgress) &&
                                    int.TryParse(baseParam[3], out progress) &&
                                    int.TryParse(baseParam[4], out teamId) &&
                                    int.TryParse(baseParam[5], out progressTeamId)) {

                                    Base @base = new Base(x, y, maxProgress);
                                    @base.Progress = progress;
                                    @base.TeamId = teamId;
                                    @base.ProgressTeamId = progressTeamId;

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
                        sb.Append(ProtocolV1_0Utils.ConvertToDeeper(ProtocolV1_0Utils.SerializeParams(COMMAND_BASE_NAME, (ProtocolDouble) o[i].X,  (ProtocolDouble) o[i].Y, o[i].MAX_PROGRESS, o[i].Progress, o[i].TeamId, o[i].ProgressTeamId),
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

        static BaseCapture() {
            POSITION_IN_ROBOT_STATE_COMMAND = RobotStateCommand.RegisterSubCommandFactory(SUB_COMMAND_FACTORY);
        }
        
        public static Base[] GetBases(RobotStateCommand state) {
            return (Base[]) state.MORE[POSITION_IN_ROBOT_STATE_COMMAND];
        }
    }
}
