using System.Collections.Generic;
using BaseLibrary.visitors;

namespace BaseLibrary.command {
    public class GameTypeCommand : ACommand {

        private static readonly List<ISubCommandFactory> SUB_COMMAND_FACTORIES = new List<ISubCommandFactory>();

        public static int RegisterSubCommandFactory(ISubCommandFactory subCommandFactory) {
            int position = SUB_COMMAND_FACTORIES.Count;
            SUB_COMMAND_FACTORIES.Add(subCommandFactory);
            return position;
        }

        public int ROBOTS_IN_ONE_TEAM { get; private set; }
        public GameType GAME_TYPE { get; private set; }

        public GameTypeCommand(int ROBOTS_IN_ONE_TEAM, GameType GAME_TYPE) {
            this.ROBOTS_IN_ONE_TEAM = ROBOTS_IN_ONE_TEAM;
            this.GAME_TYPE = GAME_TYPE;
        }

        public override void accept(ICommandVisitor accepter) {
            accepter.visit(this);
        }

        public override Output accept<Output>(ICommandVisitor<Output> accepter) {
            return accepter.visit(this);
        }

        public override Output accept<Output, Input>(ICommandVisitor<Output, Input> accepter, Input input) {
            return accepter.visit(this, input);
        }
    }

    public enum GameType {
        DEADMATCH,
        CAPTURE_FLAG,
        CAPTURE_BASE
    }
}
