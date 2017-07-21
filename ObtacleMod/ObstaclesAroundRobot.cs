using System;
using System.Collections.Generic;
using System.Text;
using BaseLibrary;
using BaseLibrary.command;
using BaseLibrary.command.common;
using BaseLibrary.protocol;
using BaseLibrary.utils;

namespace ObstacleMod {
    [ModDescription()]
    public sealed class ObstaclesAroundRobot {
        private static readonly int COMMAND_MORE_OBTACLES_POSITION;
        private static readonly ISubCommandFactory SUB_COMMAND_FACTORY = new SubCommandFactory();

        public static Factories<IObstacle, IObstacle> OBSTACLE_FACTORIES = new Factories<IObstacle, IObstacle>();

        private sealed class SubCommandFactory : ISubCommandFactory {
            public bool Deserialize(string s, object[] commandsMore) {

                    String[] obtaclesString;
                    if (ProtocolV1_0Utils.Deserialize(s, out obtaclesString, ProtocolV1_0Utils.DEFAULT.NEXT.NEXT)) {
                        List<IObstacle> obtacles = new List<IObstacle>();
                        foreach (var obtacleString in obtaclesString) {
                            String obtacleStringDefaultDeep = obtacleString; // parametrs is separated by DEFAULT.NEXT.NEXT
                            obtacleStringDefaultDeep = ProtocolV1_0Utils.ConvertToShallowly(obtacleString, 3); // parametrs is separated by DEFAULT.NEXT
                            IObstacle o = OBSTACLE_FACTORIES.Deserialize(obtacleStringDefaultDeep);
                            if (o != default(IObstacle)) {
                                obtacles.Add(o);
                            }
                        }
                        commandsMore[COMMAND_MORE_OBTACLES_POSITION] = obtacles.ToArray();
                        return true;
                    }
                
                return false;
            }

            public bool Serialize(object singleMore, out string serializedSingleMore) {
                // this is use in RobotStateCommand (DEFAULT) in array (DEFAULT.NEXT) => separator for arrays items is DEAULT.NEXT.NEXT
                IObstacle[] o = singleMore as IObstacle[];
                if (o != null) {
                    StringBuilder sb = new StringBuilder();
                    if (0 < o.Length) {
                        sb.Append("[");
                    }
                    for (int i = 0; i < o.Length; i++) {
                        sb.Append(ProtocolV1_0Utils.ConvertToDeeper(OBSTACLE_FACTORIES.Serialize(o[i]),
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

        static ObstaclesAroundRobot() {
            COMMAND_MORE_OBTACLES_POSITION = RobotStateCommand.RegisterSubCommandFactory(SUB_COMMAND_FACTORY);
        }

        private readonly IObstacle[] OBTACLES;

        public ObstaclesAroundRobot(IObstacle[] obtacles) {
            OBTACLES = obtacles;
        }

        public void AddToRobotStateCommand(RobotStateCommand robotStateCommand) {
            robotStateCommand.MORE[COMMAND_MORE_OBTACLES_POSITION] = OBTACLES;
        }

        public static IObstacle[] GetFromState(RobotStateCommand robotStateCommand) {
            return robotStateCommand.MORE[COMMAND_MORE_OBTACLES_POSITION] as IObstacle[];
        }
    }
}
