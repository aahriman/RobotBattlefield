namespace CommunicationLibrary.command {
    public class ShotAnswerCommand : ACommand {
        public bool SUCCESS { get; private set; }

        public ShotAnswerCommand(bool success)
            : base() {
            SUCCESS = success;
        }

        public sealed override void accept(AVisitorCommand accepter) {
            accepter.visit(this);
        }

        public sealed override Output accept<Output>(AVisitorCommand<Output> accepter) {
            return accepter.visit(this);
        }

        public sealed override Output accept<Output, Input>(AVisitorCommand<Output, Input> accepter, params Input[] inputs) {
            return accepter.visit(this, inputs);
        }
    }
}
