namespace CommunicationLibrary.command {
    public class DriveAnswerCommand : ACommand {

        public static DriveAnswerCommand GetInstance(bool succes) {
            return new DriveAnswerCommand(succes);
        }

        public bool SUCCES { get; private set; }

        public DriveAnswerCommand(bool succes) {
            SUCCES = succes;
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
