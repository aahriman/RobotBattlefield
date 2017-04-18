using System.Linq;
using BaseLibrary.command;
using BattlefieldLibrary;
using BattlefieldLibrary.battlefield;
using FlagCapcureBattlefield.battlefield;
using ServerLibrary.config;

namespace FlagCapcureBattlefield {
    public class Server : AServer{
        public Server(int port) : base(port) {
        }

        public override GameTypeCommand GetGameTypeCommand(Battlefield battlefield) {
            return new GameTypeCommand(battlefield.ROBOTS_IN_TEAM, GameType.CAPTURE_FLAG);
        }

        protected override Battlefield NewBattlefield(BattlefieldConfig battlefieldConfig) {
            return new FlagCapture(FlagCaptureBattlefieldConfig.ConvertFromBattlefieldConfig(battlefieldConfig));
        }
    }
}
