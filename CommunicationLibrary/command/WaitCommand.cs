namespace CommunicationLibrary.command {
    public class WaitCommand : ACommand {

        public WaitCommand() : base() { }

        public sealed override void accept(AVisitorCommand accepter) {
            accepter.visit(this);
        }

        public sealed override Output accept<Output>(AVisitorCommand<Output> accepter) {
            return accepter.visit(this);
        }

        public sealed override Output accept<Output, Input>(AVisitorCommand<Output, Input> accepter, params Input [] inputs) {
            return accepter.visit(this, inputs);
        }
    }
}
