using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattlefieldLibrary.battlefield;

namespace FlagCapcureBattlefield.battlefield {
    public class FlagCaptureBattlefieldConfig : BattlefieldConfig {
        private static FlagPlace[] fromMoreToFlags(object[] more) {
            return (
                from m in more
                where (m is FlagPlace)
                select m as FlagPlace).ToArray();
        }

        public static FlagCaptureBattlefieldConfig ConvertFromBattlefieldConfig(BattlefieldConfig bc) {
            FlagCaptureBattlefieldConfig config = bc as FlagCaptureBattlefieldConfig;
            if (config == null) {
                FlagPlace[] flagPlaces = fromMoreToFlags(bc.MORE);
                if (flagPlaces.Length > 0) {
                    config = new FlagCaptureBattlefieldConfig(bc.MAX_ROBOTS, bc.MAX_TURN, bc.MAX_LAP, bc.ROBOTS_IN_TEAM, bc.RESPAWN_TIMEOUT, bc.RESPAWN_ALLOWED, bc.EQUIPMENT_CONFIG_FILE, bc.OBTACLE_CONFIG_FILE, flagPlaces);
                } else {
                    throw new ArgumentException(nameof(bc) + "have to have same bases in more.");
                }
            }
            return config;
        }

        public readonly FlagPlace[] FlagsPlace;
        public FlagCaptureBattlefieldConfig(int maxRobots, int maxTurn, int maxLap, int robotsInTeam, int respawnTimeout,
                                            bool respawnAllowed, string equipmentConfigFile, string obtacleConfigFile,
                                            FlagPlace[] flagsPlace)
            : base(
                   maxRobots, maxTurn, maxLap, robotsInTeam, respawnTimeout, respawnAllowed, equipmentConfigFile,
                   obtacleConfigFile, flagsPlace) {
            FlagsPlace = flagsPlace;
        }
    }
}
