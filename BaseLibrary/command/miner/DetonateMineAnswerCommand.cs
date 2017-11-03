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

        public DetonateMineAnswerCommand() { }

        public DetonateMineAnswerCommand(bool success) {
            SUCCESS = success;
            pending = false;
        }

        public void FillData(DetonateMineAnswerCommand source) {
            SUCCESS = source.SUCCESS;
            pending = false;
        }

        public override void accept(IMinerVisitor accepter) {
            accepter.visit(this);
        }

        public override Output accept<Output>(IMinerVisitor<Output> accepter) {
            return accepter.visit(this);
        }

        public override Output accept<Output, Input>(IMinerVisitor<Output, Input> accepter, Input input) {
            return accepter.visit(this, input);
        }
    }
}
