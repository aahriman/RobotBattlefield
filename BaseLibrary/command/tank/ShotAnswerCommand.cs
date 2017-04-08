using System.Collections.Generic;
using BaseLibrary.visitors;

namespace BaseLibrary.command.tank {
    public class ShotAnswerCommand : ATankCommand {

        private static readonly List<ISubCommandFactory> SUB_COMMAND_FACTORIES = new List<ISubCommandFactory>();

        public static int RegisterSubCommandFactory(ISubCommandFactory subCommandFactory) {
            int position = SUB_COMMAND_FACTORIES.Count;
            SUB_COMMAND_FACTORIES.Add(subCommandFactory);
            return position;
        }

        public bool SUCCESS { get; private set; }

        public ShotAnswerCommand(bool success)
            : base() {
            SUCCESS = success;
        }

        public sealed override void accept(ITankCommandVisitor accepter) {
            accepter.visit(this);
        }

        public sealed override Output accept<Output>(ITankCommandVisitor<Output> accepter) {
            return accepter.visit(this);
        }

        public sealed override Output accept<Output, Input>(ITankCommandVisitor<Output, Input> accepter, params Input[] inputs) {
            return accepter.visit(this, inputs);
        }
    }
}
