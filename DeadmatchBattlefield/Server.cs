using BaseLibrary.command.handshake;
using BattlefieldLibrary;
using BattlefieldLibrary.battlefield;
using DeadmatchBattlefield.battlefield;


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
