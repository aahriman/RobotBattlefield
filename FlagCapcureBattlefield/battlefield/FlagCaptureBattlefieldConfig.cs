using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattlefieldLibrary.battlefield;
using FlagCaptureLibrary.battlefield;

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
                    config = new FlagCaptureBattlefieldConfig(bc.MAX_TURN, bc.MAX_LAP, bc.TEAMS, bc.ROBOTS_IN_TEAM, bc.RESPAWN_TIMEOUT, bc.RESPAWN_ALLOWED, bc.MATCH_SAVE_FILE, bc.EQUIPMENT_CONFIG_FILE, bc.OBTACLE_CONFIG_FILE, flagPlaces);
                } else {
                    throw new ArgumentException(nameof(bc) + "have to have same flags in more.");
                }
            }
            return config;
        }

        public readonly FlagPlace[] FlagsPlace;
        public FlagCaptureBattlefieldConfig(int maxTurn, int maxLap, int teams, int robotsInTeam, int respawnTimeout,
                                            bool respawnAllowed, string matchSaveFile, string equipmentConfigFile, string obtacleConfigFile,
                                            FlagPlace[] flagsPlace)
            : base(maxTurn, maxLap, teams, robotsInTeam, respawnTimeout, respawnAllowed, matchSaveFile, equipmentConfigFile,
                   obtacleConfigFile, flagsPlace) {
            FlagsPlace = flagsPlace;
        }
    }
}
