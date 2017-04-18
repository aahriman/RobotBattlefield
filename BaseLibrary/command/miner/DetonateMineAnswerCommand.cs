using System.Collections.Generic;
using BaseLibrary.visitors;

namespace BaseLibrary.command.miner {
    public class DetonateMineAnswerCommand : AMinerCommand {

        private static readonly List<ISubCommandFactory> SUB_COMMAND_FACTORIES = new List<ISubCommandFactory>();

        public static int RegisterSubCommandFactory(ISubCommandFactory subCommandFactory) {
            int position = SUB_COMMAND_FACTORIES.Count;
            SUB_COMMAND_FACTORIES.Add(subCommandFactory);
            return position;
        }

        public bool SUCCESS { get; private set; }

        public DetonateMineAnswerCommand(bool success) {
            SUCCESS = success;
        }

        public override void accept(IMinerCommandVisitor accepter) {
            accepter.visit(this);
        }

        public override Output accept<Output>(IMinerCommandVisitor<Output> accepter) {
            return accepter.visit(this);
        }

        public override Output accept<Output, Input>(IMinerCommandVisitor<Output, Input> accepter, Input input) {
            return accepter.visit(this, input);
        }
    }
}
