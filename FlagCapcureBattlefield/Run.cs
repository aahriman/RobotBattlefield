using System;
using System.Threading.Tasks;
using BaseLibrary.config;
using BattlefieldLibrary.battlefield;
using FlagCapcureBattlefield.battlefield;
using ServerLibrary.config;

namespace FlagCapcureBattlefield {
    public class Run {
        /// <summary>
        /// Start server with specific arguments
        /// </summary>
        /// <param name="args">[0] => port, [1] => number of robots, [2] => file to equipment</param>
        public static void Main(String[] args) {
            Console.WriteLine("Arena start.");
            Server server = new Server(GameProperties.DEFAULT_PORT);
            FlagPlace[] flags = {new FlagPlace(500, 100, 1), new FlagPlace(500, 900, 2)};
            FlagCaptureBattlefieldConfig flagCaptureBattlefieldConfig;
            if (args.Length >= 3) {
                flagCaptureBattlefieldConfig = new FlagCaptureBattlefieldConfig(2, ServerConfig.MAX_TURN, 1, 1, 0, true, args[2], null, flags);
            } else {
                flagCaptureBattlefieldConfig = new FlagCaptureBattlefieldConfig(2, ServerConfig.MAX_TURN, 1, 1, 0, true, null, null, flags);
            }
            
            Battlefield arena = server.GetBattlefield(flagCaptureBattlefieldConfig);

            while (!arena.End()) {
                Task.Yield();
            }
        }
    }
}
