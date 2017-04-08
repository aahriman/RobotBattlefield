using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using BaseCapcureBattlefield.battlefield;
using BaseLibrary;
using BaseLibrary.command;
using BaseLibrary.protocol;
using BattlefieldLibrary;
using BattlefieldLibrary.battlefield;
using ServerLibrary.config;
using ServerLibrary.protocol;

namespace BaseCapcureBattlefield {
    public class Server : AServer{
        public Server(int port) : base(port) {
        }

        public override GameTypeCommand GetGameTypeCommand(Battlefield battlefield) {
            return new GameTypeCommand(battlefield.ROBOTS_IN_TEAM, GameType.CAPTURE_BASE);
        }

        protected override Battlefield NewBattlefield(int ROBOT_TO_ONE_ARENA, int ROBOTS_IN_TEAM, params object[] more) {
            return new BaseCapture(ROBOT_TO_ONE_ARENA, ROBOTS_IN_TEAM, more);
        }

        protected override Battlefield NewBattlefield(int ROBOT_TO_ONE_ARENA, int ROBOTS_IN_TEAM, string EQUIPMENT_FILE, params object[] more) {
            return new BaseCapture(ROBOT_TO_ONE_ARENA, ROBOTS_IN_TEAM, EQUIPMENT_FILE, more);
        }
    }
}
