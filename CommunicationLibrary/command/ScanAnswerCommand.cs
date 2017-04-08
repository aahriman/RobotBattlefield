namespace CommunicationLibrary.command {
    public class ScanAnswerCommand : ACommand{
        
        public static ScanAnswerCommand GetInstance(ProtocolDouble range, int enemyID) {
            return new ScanAnswerCommand(range, enemyID);
        }

        public ProtocolDouble RANGE {get; private set;}
        public int ENEMY_ID { get; private set; }

        public ScanAnswerCommand(ProtocolDouble range, int enemyID)
            : base() {
                ENEMY_ID = enemyID;
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
