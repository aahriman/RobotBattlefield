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
        public static void Main(String[] args) {
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
                battlefieldConfig = new FlagCaptureBattlefieldConfig(2, ServerConfig.MAX_TURN, 1, 1, 20, true, "arena_match" + port + ".txt", null, null, new [] { new FlagPlace(500, 200, 1), new FlagPlace(500, 800, 2)});
            }
            Battlefield arena = server.GetBattlefield(battlefieldConfig);

            arena.RunEvent.WaitOne();
            arena.RunThread.Join();
        }
    }
}
