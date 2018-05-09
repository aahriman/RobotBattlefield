using System;
using System.Collections.Specialized;
using BaseLibrary.config;
using BaseLibrary.utils;
using BattlefieldLibrary.battlefield;
using BattlefieldLibrary.battlefield.robot;
using ServerLibrary.config;

namespace DeadmatchBattlefield {
    public class Run {

        /// <summary>
        /// Start server with specific arguments
        /// </summary>
        /// <param name="args">[0] => port, [1] => config file</param>
        [STAThread]
        public static void Main(string[] args) {
            Console.WriteLine("Arena start.");

            int port;
            if (args.Length < 1 || !int.TryParse(args[0], out port)) {
                port = GameProperties.DEFAULT_PORT;
            }
            Server server = new Server(port);
            BattlefieldConfig battlefieldConfig;
            if (args.Length >= 2) {
                battlefieldConfig = BattlefieldConfig.DeserializeFromFile<BattlefieldConfig>(args[1]);
            } else {
                battlefieldConfig = new BattlefieldConfig(MAX_TURN: ServerConfig.MAX_TURN, MAX_LAP: 1, TEAMS: 2, ROBOTS_IN_TEAM: 1, RESPAWN_TIMEOUT: 20, RESPAWN_ALLOWED: false, MATCH_SAVE_FILE: "arena_match" + port +".txt", EQUIPMENT_CONFIG_FILE: null, obstacleConfigFile: null, WAITING_TIME_BETWEEN_TURNS: -1, GUI: true, RANDOM_SEED:null, more: new object[0]);
            }

            Battlefield arena = server.GetBattlefield(battlefieldConfig);
  
            arena.RunEvent.WaitOne();
            arena.RunThread.Join();
        }
    }
}
