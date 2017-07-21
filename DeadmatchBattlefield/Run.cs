using System;
using System.Threading.Tasks;
using BaseLibrary.config;
using BattlefieldLibrary.battlefield;
using BattlefieldLibrary.config;

namespace DeadmatchBattlefield {
    public class Run {
        /// <summary>
        /// Start server with specific arguments
        /// </summary>
        /// <param name="args">[0] => port, [1] => number of robots, [2] => number of teams, [3] => file to equipment</param>
        public static void Main(String[] args) {
            Console.WriteLine("Arena start.");

            int port;
            if (args.Length < 1 || !int.TryParse(args[0], out port)) {
                port = GameProperties.DEFAULT_PORT;
            }
            Server server = new Server(port);
            Battlefield arena;
            if (args.Length >= 2) {
                arena = server.GetBattlefield(BattlefieldConfig.DeserializeFromFile<BattlefieldConfig>(args[1]));
            } else {
                arena = server.GetBattlefield(new BattlefieldConfig(2, ServerConfig.MAX_TURN, 1, 1, 20, false, "arena_match" + port +".txt", null, null, new object[0]));
            }

            arena.RunEvent.WaitOne();
            arena.RunThread.Join();
        }
    }
}
