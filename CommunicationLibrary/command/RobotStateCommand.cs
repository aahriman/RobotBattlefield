namespace CommunicationLibrary.command {
    public class RobotStateCommand : ACommand{
        public ProtocolDouble X { get; private set; }
        public ProtocolDouble Y { get; private set; }
        public int HIT_POINTS { get; private set; }
        public ProtocolDouble POWER { get; private set; }
        public int LAP { get; private set; }
        public int MAX_LAP { get; private set; }
        public int COUNT_OF_LIFE_ROBOTS { get; private set; }
        public int[] ARRAY_IDS_OF_LIFE_ROBOTS { get; private set; }

        public RobotStateCommand(ProtocolDouble x, ProtocolDouble y, int hitPoints, ProtocolDouble power, int lap, int maxLap, int countOfLefeRobots, int[] arrayIdsOfLifeRobots) {
            X = x;
            Y = y;
            HIT_POINTS = hitPoints;
            POWER = power;
            LAP = lap;
            MAX_LAP = maxLap;
            COUNT_OF_LIFE_ROBOTS = countOfLefeRobots;
            ARRAY_IDS_OF_LIFE_ROBOTS = (int[]) arrayIdsOfLifeRobots.Clone();
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
