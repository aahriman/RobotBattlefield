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
        
        public int ROBOT_ID {get; private set;}
        public int TEAM_ID { get; private set; }
        public int CLASS_EQUIPMENT_ID { get; private set; }
		public int ARMOR_ID { get; private set; }

        public InitAnswerCommand(int maxTurn, int lapNumber, int maxLap, int robotId, int teamId, int classEquipmentId, int armorId) {
            MAX_TURN = maxTurn;
            LAP_NUMBER = lapNumber;
            MAX_LAP = maxLap;
            ROBOT_ID = robotId;
            TEAM_ID = teamId;
			CLASS_EQUIPMENT_ID = classEquipmentId;
			ARMOR_ID = armorId;
            MORE = new object[SUB_COMMAND_FACTORIES.Count];
        }

        public sealed override void accept(ICommandVisitor accepter) {
            accepter.visit(this);
        }

        public sealed override Output accept<Output>(ICommandVisitor<Output> accepter) {
            return accepter.visit(this);
        }

        public sealed override Output accept<Output, Input>(ICommandVisitor<Output, Input> accepter, Input input) {
            return accepter.visit(this, input);
        }
    }
}
