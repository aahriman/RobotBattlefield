using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseCapcureBattlefield.battlefield;
using BaseCapcureBattlefieldLibrary.battlefield;
using BaseLibrary.config;
using BattlefieldLibrary.battlefield;
using BattlefieldLibrary.config;

namespace BaseCapcureBattlefield {
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
            Battlefield arena;
            if (args.Length >= 2) {
                arena = server.GetBattlefield(BattlefieldConfig.DeserializeFromFile<BaseCaptureBattlefieldConfig>(args[1]));
            } else {
                arena = server.GetBattlefield(new BattlefieldConfig(4, ServerConfig.MAX_TURN, 1, 2, 20, true, "arena_match" + port + ".txt", null, null, new [] {new Base(500, 100, 30), new Base(500, 900, 30) }));
            }
            

            arena.RunEvent.WaitOne();
            arena.RunThread.Join();
        }
    }
}
