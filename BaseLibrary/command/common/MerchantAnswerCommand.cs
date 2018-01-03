using System;
using System.Collections.Generic;

namespace BaseLibrary.command.common {
    /// <summary>
    /// Answer for merchant between turns.
    /// </summary>
    public class MerchantAnswerCommand : ACommonCommand {

        private static readonly List<ISubCommandFactory> SUB_COMMAND_FACTORIES = new List<ISubCommandFactory>();

        public static int RegisterSubCommandFactory(ISubCommandFactory subCommandFactory) {
            int position = SUB_COMMAND_FACTORIES.Count;
            SUB_COMMAND_FACTORIES.Add(subCommandFactory);
            return position;
        }

        private int _motorIdBought;
        /// <summary>
        /// What motor after buying robot has.
        /// </summary>
        public int MOTOR_ID_BOUGHT {
            get {
                if (pending)
                    throw new NotSupportedException("Cannot access to property of pending request.");
                return _motorIdBought;
            }
            private set => _motorIdBought = value;
        }

        private int _classEquipmentIdBought;
        /// <summary>
        /// What class equipment after buying robot has.
        /// </summary>
        public int CLASS_EQUIPMENT_ID_BOUGHT {
            get {
                if (pending)
                    throw new NotSupportedException("Cannot access to property of pending request.");
                return _classEquipmentIdBought;
            }
            private set => _classEquipmentIdBought = value;
        }

        private int _armorIdBought;
        /// <summary>
        /// What armor after buying robot has.
        /// </summary>
        public int ARMOR_ID_BOUGHT {
            get {
                if (pending)
                    throw new NotSupportedException("Cannot access to property of pending request.");
                return _armorIdBought;
            }
            private set => _armorIdBought = value;
        }

        public MerchantAnswerCommand() {
        }

        public MerchantAnswerCommand(int motorIdBought, int armorIdBought, int classEquipmentIdBought){
            pending = false;
            MOTOR_ID_BOUGHT = motorIdBought;
            ARMOR_ID_BOUGHT = armorIdBought;
            CLASS_EQUIPMENT_ID_BOUGHT = classEquipmentIdBought;
        }

        public void FillData(MerchantAnswerCommand source) {
            pending = true;
            MOTOR_ID_BOUGHT = source.MOTOR_ID_BOUGHT;
            ARMOR_ID_BOUGHT = source.ARMOR_ID_BOUGHT;
            CLASS_EQUIPMENT_ID_BOUGHT = source.CLASS_EQUIPMENT_ID_BOUGHT;
        }
    }
}
