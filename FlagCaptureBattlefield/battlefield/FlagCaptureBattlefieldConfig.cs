using System;
using System.Linq;
using BattlefieldLibrary.battlefield;
using FlagCaptureLibrary.battlefield;

namespace FlagCaptureBattlefield.battlefield {
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
                    config = new FlagCaptureBattlefieldConfig(bc.MAX_TURN, bc.MAX_LAP, bc.TEAMS, bc.ROBOTS_IN_TEAM, bc.RESPAWN_TIMEOUT, bc.RESPAWN_ALLOWED, bc.MATCH_SAVE_FILE, bc.EQUIPMENT_CONFIG_FILE, bc.OBSTACLE_CONFIG_FILE, bc.WAITING_TIME_BETWEEN_TURNS, bc.GUI, bc.RANDOM_SEED, flagPlaces);
                } else {
                    throw new ArgumentException(nameof(bc) + "have to have same flags in more.");
                }
            }
            return config;
        }

        /// <summary>
        /// Places where flags are stored.
        /// </summary>
        public readonly FlagPlace[] FlagsPlaces;

        public FlagCaptureBattlefieldConfig(int MAX_TURN, int MAX_LAP, int TEAMS, int ROBOTS_IN_TEAM, int RESPAWN_TIMEOUT,
            bool RESPAWN_ALLOWED, string MATCH_SAVE_FILE, string EQUIPMENT_CONFIG_FILE, string obstacleConfigFile, int WAITING_TIME_BETWEEN_TURNS, bool GUI, int? RANDOM_SEED,
            FlagPlace[] flagPlaces)
            : base(MAX_TURN, MAX_LAP, TEAMS, ROBOTS_IN_TEAM, RESPAWN_TIMEOUT,
                RESPAWN_ALLOWED, MATCH_SAVE_FILE, EQUIPMENT_CONFIG_FILE, obstacleConfigFile, WAITING_TIME_BETWEEN_TURNS, GUI, RANDOM_SEED, flagPlaces)
        {
            FlagsPlaces = flagPlaces;
        }
    }
}
