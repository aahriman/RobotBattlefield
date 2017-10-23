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
                    config = new FlagCaptureBattlefieldConfig(bc.MAX_TURN, bc.MAX_LAP, bc.TEAMS, bc.ROBOTS_IN_TEAM, bc.RESPAWN_TIMEOUT, bc.RESPAWN_ALLOWED, bc.MATCH_SAVE_FILE, bc.EQUIPMENT_CONFIG_FILE, bc.OBTACLE_CONFIG_FILE, bc.WAITING_TIME_BETWEEN_TURNS, bc.GUI, flagPlaces);
                } else {
                    throw new ArgumentException(nameof(bc) + "have to have same flags in more.");
                }
            }
            return config;
        }

        public readonly FlagPlace[] FlagsPlaces;
        public FlagCaptureBattlefieldConfig(int MAX_TURN, int MAX_LAP, int TEAMS, int ROBOTS_IN_TEAM, int RESPAWN_TIMEOUT,
            bool RESPAWN_ALLOWED, string MATCH_SAVE_FILE, string EQUIPMENT_CONFIG_FILE, string OBTACLE_CONFIG_FILE, int WAITING_TIME_BETWEEN_TURNS, bool GUI,
            FlagPlace[] flagPlaces)
            : base(MAX_TURN, MAX_LAP, TEAMS, ROBOTS_IN_TEAM, RESPAWN_TIMEOUT,
                RESPAWN_ALLOWED, MATCH_SAVE_FILE, EQUIPMENT_CONFIG_FILE, OBTACLE_CONFIG_FILE, WAITING_TIME_BETWEEN_TURNS, GUI, flagPlaces)
        {
            FlagsPlaces = flagPlaces;
        }
    }
}
