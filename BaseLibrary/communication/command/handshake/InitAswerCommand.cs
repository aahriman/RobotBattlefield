using System.Collections.Generic;

namespace BaseLibrary.communication.command.handshake {
    public class InitAnswerCommand : AHandShakeCommand {

        protected static readonly List<ISubCommandFactory> SUB_COMMAND_FACTORIES = new List<ISubCommandFactory>();

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
        public int MOTOR_ID { get; private set; }

        public InitAnswerCommand() { }

        public InitAnswerCommand(int maxTurn, int lapNumber, int maxLap, int robotId, int teamId, int classEquipmentId, int armorId, int motorId) {
            MAX_TURN = maxTurn;
            LAP_NUMBER = lapNumber;
            MAX_LAP = maxLap;
            ROBOT_ID = robotId;
            TEAM_ID = teamId;
			CLASS_EQUIPMENT_ID = classEquipmentId;
			ARMOR_ID = armorId;
            MOTOR_ID = motorId;
            MORE = new object[SUB_COMMAND_FACTORIES.Count];
            pending = false;
        }

        public void FillData(InitAnswerCommand source) {
            MAX_TURN = source.MAX_TURN;
            LAP_NUMBER = source.LAP_NUMBER;
            MAX_LAP = source.MAX_LAP;

            ROBOT_ID = source.ROBOT_ID;
            TEAM_ID = source.TEAM_ID;
            CLASS_EQUIPMENT_ID = source.CLASS_EQUIPMENT_ID;
            ARMOR_ID = source.ARMOR_ID;
            MOTOR_ID = source.MOTOR_ID;

            MORE = source.MORE;
            pending = false;
        }
    }
}
