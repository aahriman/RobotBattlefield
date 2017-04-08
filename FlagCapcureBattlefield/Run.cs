using System;
using System.Threading.Tasks;
using BaseLibrary.config;
using BattlefieldLibrary.battlefield;
using FlagCapcureBattlefield.battlefield;

namespace FlagCapcureBattlefield {
    public class Run {
        /// <summary>
        /// Start server with specific arguments
        /// </summary>
        /// <param name="args">[0] => port, [1] => number of robots, [2] => file to equipment</param>
        public static void Main(String[] args) {
            Console.WriteLine("Arena start.");
            Server server = new Server(GameProperties.DEFAULT_PORT);
            Battlefield arena;
            Object[] flags = {new Flag(500, 100, 1), new Flag(500, 900, 2)};
            if (args.Length >= 3) {
                arena = server.GetBattlefield(2, 1, args[2], flags);
            } else {
                arena = server.GetBattlefield(2, 1, flags);
            }



            while (!arena.End()) {
                Task.Yield();
            }
        }
    }
}
