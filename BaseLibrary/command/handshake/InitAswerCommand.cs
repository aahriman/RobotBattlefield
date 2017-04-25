using System.Collections.Generic;
using BaseLibrary.command.common;
using BaseLibrary.visitors;

namespace BaseLibrary.command.handshake {
    public class InitAnswerCommand : AHandShakeCommand {

        private static readonly List<ISubCommandFactory> SUB_COMMAND_FACTORIES = new List<ISubCommandFactory>();

        public static int RegisterSubCommandFactory(ISubCommandFactory subCommandFactory) {
            int position = SUB_COMMAND_FACTORIES.Count;
            SUB_COMMAND_FACTORIES.Add(subCommandFactory);
            return position;
        }

        public int MAX_TURN {get; private set;}
        public int LAP_NUMBER {get; private set;}
        public int MAX_LAP {get; private set;}
        
		// robotí instance
        public int ROBOT_ID {get; private set;}
		public int CLASS_EQUIPMENT_ID { get; private set; }
		public int ARMOR_ID { get; private set; }

        public Dictionary<int, int> MOTOR_ID_FOR_ROBOTS = new Dictionary<int,int>();

        public InitAnswerCommand(int maxTurn, int lapNumber, int maxLap, int robotId, Dictionary<int,int> motorIdForRobots, int classEquipmentId, int armorId) {
            MAX_TURN = maxTurn;
            LAP_NUMBER = lapNumber;
            MAX_LAP = maxLap;
            ROBOT_ID = robotId;
            foreach (var pair in motorIdForRobots) {
                MOTOR_ID_FOR_ROBOTS[pair.Key] = pair.Value;
            }
			CLASS_EQUIPMENT_ID = classEquipmentId;
			ARMOR_ID = armorId;
        }

        public sealed override void accept(ICommandVisitor accepter) {
            accepter.visit(this);
        }

        public sealed override Output accept<Output>(ICommandVisitor<Output> accepter) {
            return accepter.visit(this);
        }

        public sealed override Output accept<Output, Input>(ICommandVisitor<Output, Input> accepter, params Input[] inputs) {
            return accepter.visit(this, inputs);
        }
    }
}
