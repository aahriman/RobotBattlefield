using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattlefieldLibrary.battlefield {
    public class BattlefieldConfig {
        public readonly int MAX_ROBOTS;
        public readonly int MAX_TURN;
        public readonly int MAX_LAP;
        public readonly int ROBOTS_IN_TEAM;
        public readonly int RESPAWN_TIMEOUT;
        public readonly bool RESPAWN_ALLOWED;

        public readonly String EQUIPMENT_CONFIG_FILE;
        public readonly String OBTACLE_CONFIG_FILE;

        public readonly Object[] MORE;

        public BattlefieldConfig(int maxRobots, int maxTurn, int maxLap, int robotsInTeam, int respawnTimeout, bool respawnAllowed, string equipmentConfigFile, string obtacleConfigFile, object[] more) {
            MAX_ROBOTS = maxRobots;
            MAX_TURN = maxTurn;
            MAX_LAP = maxLap;
            ROBOTS_IN_TEAM = robotsInTeam;
            RESPAWN_TIMEOUT = respawnTimeout;
            RESPAWN_ALLOWED = respawnAllowed;
            EQUIPMENT_CONFIG_FILE = equipmentConfigFile;
            OBTACLE_CONFIG_FILE = obtacleConfigFile;
            MORE = more;
        }
    }
}
