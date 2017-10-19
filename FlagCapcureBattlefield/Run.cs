using System;
using System.Threading.Tasks;
using BaseLibrary.config;
using BattlefieldLibrary.battlefield;
using BattlefieldLibrary.config;
using FlagCapcureBattlefield.battlefield;
using FlagCaptureLibrary.battlefield;

namespace FlagCapcureBattlefield {
    public class Run {
        /// <summary>
        /// Start server with specific arguments
        /// </summary>
        /// <param name="args">[0] => port, [1] => number of robots, [2] => file to equipment</param>
        public static void Main(string[] args) {
            Console.WriteLine("Arena start.");

            int port;
            if (args.Length < 1 || !int.TryParse(args[0], out port)) {
                port = GameProperties.DEFAULT_PORT;
            }
            Server server = new Server(port);
            BattlefieldConfig battlefieldConfig;
            if (args.Length >= 2) {
                battlefieldConfig = BattlefieldConfig.DeserializeFromFile<FlagCaptureBattlefieldConfig>(args[1]);
            } else {
                battlefieldConfig = new BattlefieldConfig(MAX_ROBOTS: 2, MAX_TURN: ServerConfig.MAX_TURN, MAX_LAP: 1, ROBOTS_IN_TEAM: 1, RESPAWN_TIMEOUT: 20, RESPAWN_ALLOWED: true, MATCH_SAVE_FILE: "arena_match" + port + ".txt", EQUIPMENT_CONFIG_FILE: null, OBTACLE_CONFIG_FILE: null, WAITING_TIME_BETWEEN_TURNS: -1, GUI: true, more: new [] { new FlagPlace(500, 200, 1), new FlagPlace(500, 800, 2)});
            }
            Battlefield arena = server.GetBattlefield(battlefieldConfig);

            arena.RunEvent.WaitOne();
            arena.RunThread.Join();
        }
    }
}
