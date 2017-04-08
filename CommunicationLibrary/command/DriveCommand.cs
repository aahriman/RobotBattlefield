namespace CommunicationLibrary.command {
    public class DriveCommand : ACommand {
        
        public static DriveCommand GetInstance(ProtocolDouble speed, ProtocolDouble angle) {
            return new DriveCommand(speed, angle);
        }
        public ProtocolDouble POWER { get; private set; }
        public ProtocolDouble ANGLE { get; private set; }

        public DriveCommand(ProtocolDouble speed, ProtocolDouble angle) {
            POWER = speed;
            ANGLE = angle;
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
