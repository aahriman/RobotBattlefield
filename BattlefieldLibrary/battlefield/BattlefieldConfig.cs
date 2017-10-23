using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ViewerLibrary.serializer;

namespace BattlefieldLibrary.battlefield {
    public class BattlefieldConfig {
        public readonly int MAX_TURN;
        public readonly int MAX_LAP;
        public readonly int TEAMS;
        public readonly int ROBOTS_IN_TEAM;
        public readonly int RESPAWN_TIMEOUT;
        public readonly bool RESPAWN_ALLOWED;

        public readonly string MATCH_SAVE_FILE;
        public readonly string EQUIPMENT_CONFIG_FILE;
        public readonly string OBTACLE_CONFIG_FILE;

        public readonly int WAITING_TIME_BETWEEN_TURNS;
        public readonly bool GUI;

        public readonly object[] MORE;

        public BattlefieldConfig(int MAX_TURN, int MAX_LAP, int TEAMS, int ROBOTS_IN_TEAM, int RESPAWN_TIMEOUT,
                                 bool RESPAWN_ALLOWED, string MATCH_SAVE_FILE, string EQUIPMENT_CONFIG_FILE, string OBTACLE_CONFIG_FILE, int WAITING_TIME_BETWEEN_TURNS, bool GUI,
                                 object[] more) {
            this.TEAMS = TEAMS;
            this.MAX_TURN = MAX_TURN;
            this.MAX_LAP = MAX_LAP;
            this.ROBOTS_IN_TEAM = ROBOTS_IN_TEAM;
            this.RESPAWN_TIMEOUT = RESPAWN_TIMEOUT;
            this.RESPAWN_ALLOWED = RESPAWN_ALLOWED;
            this.EQUIPMENT_CONFIG_FILE = EQUIPMENT_CONFIG_FILE;
            this.OBTACLE_CONFIG_FILE = OBTACLE_CONFIG_FILE;
            this.MATCH_SAVE_FILE = MATCH_SAVE_FILE;
            this.WAITING_TIME_BETWEEN_TURNS = WAITING_TIME_BETWEEN_TURNS;
            this.GUI = GUI;
            MORE = more;
        }

        /// <summary>
        /// Serialize config to JSON and store it into the file specified by <code>filename</code>.
        /// </summary>
        /// <param name="filename"></param>
        public void Serialize(String filename) {
            // serialize JSON directly to a file
            using (StreamWriter file = File.CreateText(filename)) {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, this);
            }
        }

        /// <summary>
        /// Deserialize config from JSON stored in the file specified by <code>filename</code>.
        /// </summary>
        /// <param name="filename"></param>
        public static T  DeserializeFromFile<T>(String filename) where T : BattlefieldConfig {
            using (StreamReader file = File.OpenText(filename)) {
                JsonSerializer serializer = new JsonSerializer();
                return (T) serializer.Deserialize(file, typeof(T));
            }
        }
    }
}
