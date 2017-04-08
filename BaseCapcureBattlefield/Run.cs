using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLibrary.config;
using BattlefieldLibrary.battlefield;
using ServerLibrary.config;

namespace BaseCapcureBattlefield {
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
                arena = server.GetBattlefield(2, 1, args[2]);
            } else {
                arena = server.GetBattlefield(2, 1);
            }
            
            
            
            while (!arena.End()) {
                Task.Yield();
            }
        }
    }
}
