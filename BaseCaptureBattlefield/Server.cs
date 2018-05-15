using BaseCaptureBattlefield.battlefield;
using BaseLibrary.communication.command.handshake;
using BattlefieldLibrary.battlefield;
using ServerLibrary;

namespace BaseCaptureBattlefield {
    public class Server : AServer{
        public Server(int port) : base(port) {
        }

        public override GameTypeCommand GetGameTypeCommand(Battlefield battlefield) {
            return new GameTypeCommand(battlefield.ROBOTS_IN_TEAM, GameType.CAPTURE_BASE);
        }

        protected override Battlefield NewBattlefield(BattlefieldConfig battlefieldConfig) {
            return new battlefield.BaseCaptureBattlefield(BaseCaptureBattlefieldConfig.ConvertFromBattlefieldConfig(battlefieldConfig));
        }
    }
}
