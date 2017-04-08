namespace CommunicationLibrary.command {
    public class EndMatchCommand : ACommand {
        public string FILE_URL { get; private set; }

        public EndMatchCommand(string fileUrl) {
            FILE_URL = fileUrl;
        }

        public override void accept(AVisitorCommand accepter) {
            accepter.visit(this);
        }

        public override Output accept<Output>(AVisitorCommand<Output> accepter) {
            return accepter.visit(this);
        }

        public override Output accept<Output, Input>(AVisitorCommand<Output, Input> accepter, params Input[] inputs) {
            return accepter.visit(this, inputs);
        }
    }
}
