namespace CommunicationLibrary.command {
    public class ShotCommand : ACommand {   
        public ProtocolDouble ANGLE { get; private set; }
        public ProtocolDouble RANGE { get; private set; }

        public ShotCommand(ProtocolDouble angle, ProtocolDouble range)
            : base() {
                ANGLE = angle;
                RANGE = range;
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
