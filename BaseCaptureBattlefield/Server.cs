﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using BaseCapcureBattlefield.battlefield;
using BaseLibrary;
using BaseLibrary.communication.command.handshake;
using BattlefieldLibrary;
using BattlefieldLibrary.battlefield;
using ServerLibrary.protocol;

namespace BaseCapcureBattlefield {
    public class Server : AServer{
        public Server(int port) : base(port) {
        }

        public override GameTypeCommand GetGameTypeCommand(Battlefield battlefield) {
            return new GameTypeCommand(battlefield.ROBOTS_IN_TEAM, GameType.CAPTURE_BASE);
        }

        protected override Battlefield NewBattlefield(BattlefieldConfig battlefieldConfig) {
            return new BaseCaptureBattlefield(BaseCaptureBattlefieldConfig.ConvertFromBattlefieldConfig(battlefieldConfig));
        }
    }
}
