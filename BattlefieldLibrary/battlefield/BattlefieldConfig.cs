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
        /// <summary>
        /// Max turn in one lap.
        /// </summary>
        public readonly int MAX_TURN;
        /// <summary>
        /// Max lap in match.
        /// </summary>
        public readonly int MAX_LAP;
        /// <summary>
        /// How many teams will fight in match.
        /// </summary>
        public readonly int TEAMS;
        /// <summary>
        /// How many robots in one team.
        /// </summary>
        public readonly int ROBOTS_IN_TEAM;
        /// <summary>
        /// How many turn to reborn in same lap.
        /// </summary>
        public readonly int RESPAWN_TIMEOUT;
        /// <summary>
        /// Is reborn available.
        /// </summary>
        public readonly bool RESPAWN_ALLOWED;

        /// <summary>
        /// Path to file where record of match will be saved.
        /// </summary>
        public readonly string MATCH_SAVE_FILE;
        /// <summary>
        /// File with equipments (can be null -> default equipment will be used).
        /// </summary>
        public readonly string EQUIPMENT_CONFIG_FILE;

        /// <summary>
        /// File with obstacles (can be null -> no obstacle at map).
        /// </summary>
        public readonly string OBSTACLE_CONFIG_FILE;

        /// <summary>
        /// How long (in ms) want to wait for collection robots command. -1 is until get command from every robot.
        /// </summary>
        public readonly int WAITING_TIME_BETWEEN_TURNS;

        /// <summary>
        /// Want to show gui.
        /// </summary>
        public readonly bool GUI;

        /// <summary>
        /// Random seed for random things in area. (When null - random seed will be choose randomly).
        /// </summary>
        public readonly int RANDOM_SEED;

        /// <summary>
        /// Some more object for battlefield (like flag, bases etc.)
        /// </summary>
        public readonly object[] MORE;

        public BattlefieldConfig(int MAX_TURN, int MAX_LAP, int TEAMS, int ROBOTS_IN_TEAM, int RESPAWN_TIMEOUT,
                                 bool RESPAWN_ALLOWED, string MATCH_SAVE_FILE, string EQUIPMENT_CONFIG_FILE, string obstacleConfigFile, int WAITING_TIME_BETWEEN_TURNS, bool GUI, int? RANDOM_SEED,
                                 object[] more) {
            this.TEAMS = TEAMS;
            this.MAX_TURN = MAX_TURN;
            this.MAX_LAP = MAX_LAP;
            this.ROBOTS_IN_TEAM = ROBOTS_IN_TEAM;
            this.RESPAWN_TIMEOUT = RESPAWN_TIMEOUT;
            this.RESPAWN_ALLOWED = RESPAWN_ALLOWED;
            this.EQUIPMENT_CONFIG_FILE = EQUIPMENT_CONFIG_FILE;
            this.OBSTACLE_CONFIG_FILE = obstacleConfigFile;
            this.MATCH_SAVE_FILE = MATCH_SAVE_FILE;
            this.WAITING_TIME_BETWEEN_TURNS = WAITING_TIME_BETWEEN_TURNS;
            this.GUI = GUI;
            if (RANDOM_SEED == null) {
                this.RANDOM_SEED = new Random().Next();
                Console.WriteLine("Used random seed:" + this.RANDOM_SEED);
            } else {
                this.RANDOM_SEED = (int) RANDOM_SEED;
            }
            MORE = more;
        }

        /// <summary>
        /// Serialize config to JSON and store it into the file specified by <code>filename</code>.
        /// </summary>
        /// <param name="filename"></param>
        public void Serialize(String filename) {
            // serialize JSON directly to a file
            using (var file = new JsonTextWriter(File.CreateText("equipment.json"))) {
                file.Formatting = Formatting.Indented;
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
