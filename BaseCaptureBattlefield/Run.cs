using System;
using BaseCapcureBattlefieldLibrary.battlefield;
using BaseLibrary.config;
using BattlefieldLibrary.battlefield;
using BattlefieldLibrary.config;

namespace BaseCapcureBattlefield {
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
            BattlefieldConfig battlefielConfig;
            if (args.Length >= 2) {
                battlefielConfig = BattlefieldConfig.DeserializeFromFile<BattlefieldConfig>(args[1]);
            } else {
                battlefielConfig = new BattlefieldConfig(MAX_ROBOTS: 4, MAX_TURN: ServerConfig.MAX_TURN, MAX_LAP: 1, ROBOTS_IN_TEAM: 2, RESPAWN_TIMEOUT: 20, RESPAWN_ALLOWED: true, MATCH_SAVE_FILE: "arena_match" + port + ".txt", EQUIPMENT_CONFIG_FILE: null, OBTACLE_CONFIG_FILE: null, WAITING_TIME_BETWEEN_TURNS: -1, GUI: true, more: new [] {new Base(500, 100, 30), new Base(500, 900, 30) });
            }

            Battlefield arena = server.GetBattlefield(battlefielConfig);

            arena.RunEvent.WaitOne();
            arena.RunThread.Join();
        }
    }
}
