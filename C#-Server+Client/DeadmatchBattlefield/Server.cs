using BaseLibrary.communication.command.handshake;
using BattlefieldLibrary;
using BattlefieldLibrary.battlefield;
using DeadmatchBattlefield.battlefield;
using ServerLibrary;


namespace DeadmatchBattlefield {
    public class Server : AServer {

        /// <inheritdoc />
        public Server(int port) : base(port){
        }

        /// <inheritdoc />
        public override GameTypeCommand GetGameTypeCommand(Battlefield battlefield) {
            return new GameTypeCommand(battlefield.ROBOTS_IN_TEAM, GameType.DEADMATCH);
        }

        /// <inheritdoc />
        /// Create Deadmatch
        /// </summary>
        protected override Battlefield NewBattlefield(BattlefieldConfig battlefieldConfig) {
            return new Deadmatch(battlefieldConfig);
        }
    }
}
