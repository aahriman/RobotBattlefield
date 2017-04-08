namespace CommunicationLibrary.command {
    public class EndLapCommand : ACommand{
        public LapStates STATE { get; private set; }
        public int GOLD { get; private set; }

        public EndLapCommand(LapStates state, int gold) {
            STATE = state;
            GOLD = gold;
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
