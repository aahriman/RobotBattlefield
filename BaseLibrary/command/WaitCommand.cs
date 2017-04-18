using System.Collections.Generic;
using BaseLibrary.visitors;

namespace BaseLibrary.command {
    /// <summary>
    /// Wait one turn or to until HitPoint > 0 or to the end of turn
    /// </summary>
    public class WaitCommand : ACommand {

        private static readonly List<ISubCommandFactory> SUB_COMMAND_FACTORIES = new List<ISubCommandFactory>();

        public static int RegisterSubCommandFactory(ISubCommandFactory subCommandFactory) {
            int position = SUB_COMMAND_FACTORIES.Count;
            SUB_COMMAND_FACTORIES.Add(subCommandFactory);
            return position;
        }

        public WaitCommand() : base() { }

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
