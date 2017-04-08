namespace CommunicationLibrary.command {
    public class ScanCommand : ACommand {
        public ProtocolDouble ANGLE {get; private set;}
        public ProtocolDouble PRECISION {get; private set;}

        public ScanCommand(ProtocolDouble angle, ProtocolDouble precision) : base() {
            ANGLE = angle;
            PRECISION = precision;
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
