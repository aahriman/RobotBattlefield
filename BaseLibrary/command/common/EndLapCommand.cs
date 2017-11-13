using System.Collections.Generic;
using BaseLibrary.battlefield;
using BaseLibrary.visitors;

namespace BaseLibrary.command.common {
    /// <summary>
    /// Command at the end of lap. This is sub command to RobotStateCommand
    /// </summary>
    public class EndLapCommand : ACommonCommand {

        private static readonly List<ISubCommandFactory> SUB_COMMAND_FACTORIES = new List<ISubCommandFactory>();

        public static int RegisterSubCommandFactory(ISubCommandFactory subCommandFactory) {
            int position = SUB_COMMAND_FACTORIES.Count;
            SUB_COMMAND_FACTORIES.Add(subCommandFactory);
            return position;
        }

        /// <summary>
        /// Why lap end.
        /// </summary>
        public LapState STATE { get; private set; }

        /// <summary>
        /// How many gold robot has.
        /// </summary>
        public int GOLD { get; private set; }

        /// <summary>
        /// What robot score is.
        /// </summary>
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

        public sealed override Output accept<Output, Input>(ICommandVisitor<Output, Input> accepter, Input input) {
            return accepter.visit(this, input);
        }
    }
}
