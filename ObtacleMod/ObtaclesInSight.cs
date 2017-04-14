using System;
using System.Collections.Generic;
using System.Text;
using BaseLibrary.command;
using BaseLibrary.protocol;
using BaseLibrary.utils;

namespace ObtacleMod {
    [ModDescription()]
    public sealed class ObtaclesInSight {
        private const string COMMAND_NAME = "OBTACLES_SIGHT";
        private static readonly int COMMAND_MORE_OBTACLES_POSITION;
        private static readonly ISubCommandFactory SUB_COMMAND_FACTORY = new SubCommandFactory();

        public static Factories<IObtacle, IObtacle> OBTACLE_FACTORIES = new Factories<IObtacle, IObtacle>();

        private sealed class SubCommandFactory : ISubCommandFactory {
            public bool Deserialize(string s, object[] commandsMore) {
                String rest;
                if (ProtocolV1_0Utils.GetParams(s, COMMAND_NAME, out rest)) {
                    String[] obtaclesString;
                    if (ProtocolV1_0Utils.Deserialize(rest, out obtaclesString, ProtocolV1_0Utils.DEFAULT.NEXT.NEXT)) {
                        List<IObtacle> obtacles = new List<IObtacle>();
                        foreach (var obtacleString in obtaclesString) {
                            String obtacleStringDefaultDeep = obtacleString; // parametrs is separated by DEFAULT.NEXT.NEXT
                            obtacleStringDefaultDeep = ProtocolV1_0Utils.ConvertToShallowly(obtacleString, 3); // parametrs is separated by DEFAULT.NEXT
                            IObtacle o = OBTACLE_FACTORIES.Deserialize(obtacleStringDefaultDeep);
                            if (o != default(IObtacle)) {
                                obtacles.Add(o);
                            }
                        }
                        commandsMore[COMMAND_MORE_OBTACLES_POSITION] = new ObtaclesInSight(obtacles.ToArray());
                    }
                }
                return false;
            }

            public bool Serialize(object singleMore, out string serializedSingleMore) {
                // this is use in RobotStateCommand (DEFAULT) in array (DEFAULT.NEXT) => separator for arrays items is DEAULT.NEXT.NEXT
                ObtaclesInSight o = singleMore as ObtaclesInSight;
                if (o != null) {
                    StringBuilder sb = new StringBuilder();
                    if (0 < o.OBTACLES.Length) {
                        sb.Append("[");
                    }
                    for (int i = 0; i < o.OBTACLES.Length; i++) {
                        sb.Append(ProtocolV1_0Utils.ConvertToDeeper(OBTACLE_FACTORIES.Serialize(o.OBTACLES[i]),
                                                          ProtocolV1_0Utils.DEFAULT.NEXT.NEXT.NEXT));
                        if (i + 1 < o.OBTACLES.Length) {
                            sb.Append(ProtocolV1_0Utils.DEFAULT.NEXT.NEXT.SEPARATOR);
                        }
                    }
                    if (0 < o.OBTACLES.Length) {
                        sb.Append("]");
                    }
                    serializedSingleMore = ProtocolV1_0Utils.SerializeParams(COMMAND_NAME, ProtocolV1_0Utils.DEFAULT.NEXT,sb.ToString());
                    return true;
                }
                serializedSingleMore = null;
                return false;
            }
        }

        static ObtaclesInSight() {
            COMMAND_MORE_OBTACLES_POSITION = RobotStateCommand.RegisterSubCommandFactory(SUB_COMMAND_FACTORY);
        }

        public readonly IObtacle[] OBTACLES;

        public ObtaclesInSight(IObtacle[] obtacles) {
            OBTACLES = obtacles;
            // TODO použití
        }

        public void AddToRobotStateCommand(RobotStateCommand robotStateCommand) {
            robotStateCommand.MORE[COMMAND_MORE_OBTACLES_POSITION] = this;
        }

        public static ObtaclesInSight GetFromRobotStateCommand(RobotStateCommand robotStateCommand) {
            return robotStateCommand.MORE[COMMAND_MORE_OBTACLES_POSITION] as ObtaclesInSight;
        }
    }
}
