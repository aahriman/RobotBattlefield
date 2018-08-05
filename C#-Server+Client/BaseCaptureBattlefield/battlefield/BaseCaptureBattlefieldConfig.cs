using System;
using System.Linq;
using BaseCaptureLibrary.battlefield;
using BattlefieldLibrary.battlefield;

namespace BaseCaptureBattlefield.battlefield {
    public class BaseCaptureBattlefieldConfig : BattlefieldConfig {
        public readonly Base[] BASES;


        public static BaseCaptureBattlefieldConfig ConvertFromBattlefieldConfig(BattlefieldConfig bc) {
            Base[] bases = basesFromMore(bc.MORE);
            if (bases.Length > 0) {
                return new BaseCaptureBattlefieldConfig(bc.MAX_TURN, bc.MAX_LAP, bc.TEAMS, bc.ROBOTS_IN_TEAM, bc.RESPAWN_TIMEOUT, bc.RESPAWN_ALLOWED, bc.MATCH_SAVE_FILE, bc.EQUIPMENT_CONFIG_FILE, bc.OBSTACLE_CONFIG_FILE,bc.WAITING_TIME_BETWEEN_TURNS, bc.GUI, bc.RANDOM_SEED, bases);
            } else {
                throw new ArgumentException("Config have to have some bases in more.");
            }
        }

        private static Base[] basesFromMore(object[] more) {
            return (from m in more
                    where m is Base
                    select m as Base).ToArray();
        }

        public BaseCaptureBattlefieldConfig(int MAX_TURN, int MAX_LAP, int TEAMS, int ROBOTS_IN_TEAM, int RESPAWN_TIMEOUT,
            bool RESPAWN_ALLOWED, string MATCH_SAVE_FILE, string EQUIPMENT_CONFIG_FILE, string obstacleConfigFile, int WAITING_TIME_BETWEEN_TURNS, bool GUI, int? RANDOM_SEED,
            Base[] bases)
            : base(MAX_TURN, MAX_LAP, TEAMS, ROBOTS_IN_TEAM, RESPAWN_TIMEOUT,
        RESPAWN_ALLOWED, MATCH_SAVE_FILE, EQUIPMENT_CONFIG_FILE, obstacleConfigFile, WAITING_TIME_BETWEEN_TURNS, GUI, RANDOM_SEED, bases) {
            BASES = bases;
        }
    }
}
