using System.Collections.Generic;
using BaseLibrary.battlefield;
using BaseLibrary.visitors;

namespace BaseLibrary.command {
    public class EndLapCommand : ACommand{

        private static readonly List<ISubCommandFactory> SUB_COMMAND_FACTORIES = new List<ISubCommandFactory>();

        public static int RegisterSubCommandFactory(ISubCommandFactory subCommandFactory) {
            int position = SUB_COMMAND_FACTORIES.Count;
            SUB_COMMAND_FACTORIES.Add(subCommandFactory);
            return position;
        }

        public LapState STATE { get; private set; }
        public int GOLD { get; private set; }
        public int SCORE { get; private set; }

        public EndLapCommand(LapState state, int gold, int score) {
            STATE = state;
            GOLD = gold;
            SCORE = score;
        }


        public sealed override void accept(ICommandVisitor accepter) {
            accepter.visit(this);
        }

        public sealed override Output accept<Output>(ICommandVisitor<Output> accepter) {
            return accepter.visit(this);
        }

        public sealed override Output accept<Output, Input>(ICommandVisitor<Output, Input> accepter, params Input[] inputs) {
            return accepter.visit(this, inputs);
        }
    }
}
