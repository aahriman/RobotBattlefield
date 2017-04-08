using System.Collections.Generic;

namespace CommunicationLibrary.command {
    public class InitAnswerCommand : ACommand{
        public int MAX_LAP {get; private set;}
        public int MATCH_NUMBER {get; private set;}
        public int MATCHES {get; private set;}
        
		// robotí instance
        public int ROBOT_ID {get; private set;}
		public int GUN_ID { get; private set; }
		public int ARMOR_ID { get; private set; }

        public Dictionary<int, int> MOTOR_ID_FOR_ROBOTS = new Dictionary<int,int>();

        public InitAnswerCommand(int maxLap, int matchNumber, int matches, int robotId, Dictionary<int,int> motorIdForRobots, int gunId, int armorId) {
            MAX_LAP = maxLap;
            MATCH_NUMBER = matchNumber;
            MATCHES = matches;
            ROBOT_ID = robotId;
            foreach (var pair in motorIdForRobots) {
                MOTOR_ID_FOR_ROBOTS[pair.Key] = pair.Value;
            }
			GUN_ID = gunId;
			ARMOR_ID = armorId;
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
