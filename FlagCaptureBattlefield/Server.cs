using BaseLibrary.command.handshake;
using BattlefieldLibrary;
using BattlefieldLibrary.battlefield;
using FlagCaptureBattlefield.battlefield;

namespace FlagCaptureBattlefield {
    public class Server : AServer{
        public Server(int port) : base(port) {
        }

        public override GameTypeCommand GetGameTypeCommand(Battlefield battlefield) {
            return new GameTypeCommand(battlefield.ROBOTS_IN_TEAM, GameType.CAPTURE_FLAG);
        }

        protected override Battlefield NewBattlefield(BattlefieldConfig battlefieldConfig) {
            return new battlefield.FlagCaptureBattlefield(FlagCaptureBattlefieldConfig.ConvertFromBattlefieldConfig(battlefieldConfig));
        }
    }
}
