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
            
            BattlefieldConfig battlefieldConfig = new BattlefieldConfig(2, 5000, 1, 1, null, @"C:\Users\vojtech\Desktop\test", new Object[0]{});
            Battlefield arena = server.GetBattlefield(battlefieldConfig);
            
            while (!arena.End()) {
                Task.Yield();
            }
        }
    }
}
