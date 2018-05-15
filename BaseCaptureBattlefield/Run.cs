using System;
using BaseCaptureLibrary.battlefield;
using BaseLibrary.config;
using BattlefieldLibrary.battlefield;
using ServerLibrary.config;

namespace BaseCaptureBattlefield {
    public class Run {
        /// <summary>
        /// Start server with specific arguments
        /// </summary>
        /// <param name="args">[0] => port, [2] => config file </param>
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
                battlefieldConfig = new BattlefieldConfig(MAX_TURN: ServerConfig.MAX_TURN, MAX_LAP: 1, TEAMS: 2, ROBOTS_IN_TEAM: 2, RESPAWN_TIMEOUT: 20, RESPAWN_ALLOWED: true, MATCH_SAVE_FILE: "arena_match" + port + ".txt", EQUIPMENT_CONFIG_FILE: null, obstacleConfigFile: null, WAITING_TIME_BETWEEN_TURNS: -1, GUI: true, RANDOM_SEED: null, more: new [] {new Base(500, 100, 100), new Base(500, 900, 100) });
            }

            Battlefield arena = server.GetBattlefield(battlefieldConfig);

            arena.RunEvent.WaitOne();
            arena.RunThread.Join();
        }
    }
}
