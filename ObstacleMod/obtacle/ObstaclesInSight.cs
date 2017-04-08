using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseLibrary.command;
using BaseLibrary.protocol;
using BaseLibrary.utils;

namespace ObstacleMod.obtacle {
    public sealed class ObstaclesInSight {
        private const string COMMAND_NAME = "OBTACLES_SIGHT";
        private static readonly int COMMAND_MORE_OBTACLES_POSITION;
        private static ISubCommandFactory subCommandFactory = new SubCommandFactory();

        public static Factories<IObtacle, IObtacle> OBTACLE_FACTORIES = new Factories<IObtacle, IObtacle>();

        private sealed class SubCommandFactory : ISubCommandFactory {
            public bool Deserialize(string s, object[] commandsMore) {
                String rest;
                if (ProtocolV1_0Utils.GetParams(s, COMMAND_NAME, out rest)) {
                    String[] obtaclesString;
                    ProtocolV1_0Utils.Deserialize(rest, out obtaclesString, ProtocolV1_0Utils.DEFAULT.NEXT.NEXT);
                }
                return false;
            }

            public bool Serialize(object singleMore, out string serializedSingleMore) {
                // this is use in RobotStateCommand (DEFAULT) in array (DEFAULT.NEXT) => separator for array is DEAULT.NEXT.NEXT
                ObstaclesInSight o = singleMore as ObstaclesInSight;
                if (o != null) {
                    //OBTACLE_FACTORIES.
                    //serializedSingleMore = ProtocolV1_0Utils.SerializeArray(o.OBTACLES, ProtocolV1_0Utils.DEFAULT.NEXT.NEXT);
                    //return true;
                }
                serializedSingleMore = null;
                return false;
            }
        }

        static ObstaclesInSight() {
            COMMAND_MORE_OBTACLES_POSITION = RobotStateCommand.RegisterSubCommandFactory(subCommandFactory);
        }

        public readonly IObtacle[] OBTACLES;

        public ObstaclesInSight(IObtacle[] obtacles) {
            OBTACLES = obtacles;
        }
    }
}
