using BaseLibrary.command.handshake;
using BattlefieldLibrary;
using BattlefieldLibrary.battlefield;
using FlagCapcureBattlefield.battlefield;

namespace FlagCapcureBattlefield {
    public class Server : AServer{
        public Server(int port) : base(port) {
        }

        public override GameTypeCommand GetGameTypeCommand(Battlefield battlefield) {
            return new GameTypeCommand(battlefield.ROBOTS_IN_TEAM, GameType.CAPTURE_FLAG);
        }

        protected override Battlefield NewBattlefield(BattlefieldConfig battlefieldConfig) {
            return new FlagCaptureBattlefield(FlagCaptureBattlefieldConfig.ConvertFromBattlefieldConfig(battlefieldConfig));
        }
    }
}
