using System;
using System.Threading.Tasks;
using BaseLibrary.config;
using BattlefieldLibrary.battlefield;
using ServerLibrary.config;

namespace DeadmatchBattlefield {
    public class Run {
        /// <summary>
        /// Start server with specific arguments
        /// </summary>
        /// <param name="args">[0] => port, [1] => number of robots, [2] => file to equipment</param>
        public static void Main(String[] args) {
            Console.WriteLine("Arena start.");
            Server server = new Server(GameProperties.DEFAULT_PORT);

            Battlefield arena;
            if (args.Length >= 3) {
                arena = server.GetBattlefield(new BattlefieldConfig(2, ServerConfig.MAX_TURN, 1, 1, 20, true, args[2], null, new object[0]));
            } else {
                arena = server.GetBattlefield(new BattlefieldConfig(2, ServerConfig.MAX_TURN, 1, 1, 20, true, null, null, new object[0]));
            }

            while (!arena.End()) {
                Task.Yield();
            }
        }
    }
}
