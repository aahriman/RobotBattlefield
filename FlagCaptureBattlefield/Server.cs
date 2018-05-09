using BaseLibrary.communication.command.handshake;
using BattlefieldLibrary;
using BattlefieldLibrary.battlefield;
using FlagCaptureBattlefield.battlefield;

namespace FlagCaptureBattlefield {
    public class Server : AServer{
        public Server(int port) : base(port) {
        }

        /// <inheritdoc />
        /// <summary>
        /// GameType is flagCapture.
        /// </summary>
        public override GameTypeCommand GetGameTypeCommand(Battlefield battlefield) {
            return new GameTypeCommand(battlefield.ROBOTS_IN_TEAM, GameType.CAPTURE_FLAG);
        }
        /// <inheritdoc />
        /// <summary>
        /// Create FlagCaptureBattlefield
        /// </summary>
        protected override Battlefield NewBattlefield(BattlefieldConfig battlefieldConfig) {
            return new battlefield.FlagCaptureBattlefield(FlagCaptureBattlefieldConfig.ConvertFromBattlefieldConfig(battlefieldConfig));
        }
    }
}
