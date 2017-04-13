using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using BaseLibrary;
using BaseLibrary.command;
using BaseLibrary.protocol;
using BattlefieldLibrary;
using BattlefieldLibrary.battlefield;
using DeadmatchBattlefield.battlefield;
using ServerLibrary.config;
using ServerLibrary.protocol;

namespace DeadmatchBattlefield {
    public class Server : AServer {
       
        public Server(int port) : base(port){
        }

        public override GameTypeCommand GetGameTypeCommand(Battlefield battlefield) {
            return new GameTypeCommand(battlefield.ROBOTS_IN_TEAM, GameType.DEADMATCH);
        }

        protected override Battlefield NewBattlefield(BattlefieldConfig battlefielConfig) {
            return new Deadmatch(battlefielConfig);
        }
    }
}
