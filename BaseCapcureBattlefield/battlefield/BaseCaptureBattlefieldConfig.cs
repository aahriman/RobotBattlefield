using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseCapcureBattlefieldLibrary.battlefield;
using BattlefieldLibrary.battlefield;

namespace BaseCapcureBattlefield.battlefield {
    public class BaseCaptureBattlefieldConfig : BattlefieldConfig {
        public readonly Base[] BASES;


        public static BaseCaptureBattlefieldConfig ConvertFromBattlefieldConfig(BattlefieldConfig bc) {
            Base[] bases = basesFromMore(bc.MORE);
            if (bases.Length > 0) {
                return new BaseCaptureBattlefieldConfig(bc.MAX_ROBOTS, bc.MAX_TURN, bc.MAX_LAP, bc.ROBOTS_IN_TEAM, bc.RESPAWN_TIMEOUT, bc.RESPAWN_ALLOWED, bc.MATCH_SAVE_FILE, bc.EQUIPMENT_CONFIG_FILE, bc.OBTACLE_CONFIG_FILE, bases);
            } else {
                throw new ArgumentException(nameof(bc) + "have to have some bases in more.");
            }
        }

        private static Base[] basesFromMore(Object[] more) {
            return (from m in more
                    where m is Base
                    select m as Base).ToArray();
        }

        public BaseCaptureBattlefieldConfig(int maxRobots, int maxTurn, int maxLap, int robotsInTeam, int respawnTimeout,
                                            bool respawnAllowed, string matchSaveFile, string equipmentConfigFile, string obtacleConfigFile,
                                            Base[] bases)
            : base(
                   maxRobots, maxTurn, maxLap, robotsInTeam, respawnTimeout, respawnAllowed, matchSaveFile, equipmentConfigFile,
                   obtacleConfigFile, bases) {
            BASES = bases;
        }
    }
}
