using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ViewerLibrary.serializers;

namespace BattlefieldLibrary.battlefield {
    public class BattlefieldConfig {
        public readonly int MAX_ROBOTS;
        public readonly int MAX_TURN;
        public readonly int MAX_LAP;
        public readonly int ROBOTS_IN_TEAM;
        public readonly int RESPAWN_TIMEOUT;
        public readonly bool RESPAWN_ALLOWED;

        public readonly String MATCH_SAVE_FILE;
        public readonly String EQUIPMENT_CONFIG_FILE;
        public readonly String OBTACLE_CONFIG_FILE;

        public readonly Object[] MORE;

        public BattlefieldConfig(int maxRobots, int maxTurn, int maxLap, int robotsInTeam, int respawnTimeout,
                                 bool respawnAllowed, string matchSaveFile, string equipmentConfigFile, string obtacleConfigFile,
                                 object[] more) {
            MAX_ROBOTS = maxRobots;
            MAX_TURN = maxTurn;
            MAX_LAP = maxLap;
            ROBOTS_IN_TEAM = robotsInTeam;
            RESPAWN_TIMEOUT = respawnTimeout;
            RESPAWN_ALLOWED = respawnAllowed;
            EQUIPMENT_CONFIG_FILE = equipmentConfigFile;
            OBTACLE_CONFIG_FILE = obtacleConfigFile;
            MATCH_SAVE_FILE = matchSaveFile;
            MORE = more;
        }

        public void Serialize(String filename) {
            // serialize JSON directly to a file
            using (StreamWriter file = File.CreateText(filename)) {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, this);
            }
        }

        public static T  DeserializeFromFile<T>(String filename) where T : BattlefieldConfig {
            using (StreamReader file = File.OpenText(filename)) {
                JsonSerializer serializer = new JsonSerializer();
                return (T) serializer.Deserialize(file, typeof(T));
            }
        }
    }
}
