using System;
using System.Collections.Generic;

namespace BaseLibrary.communication.command.common {
    /// <summary>
    /// Command for specific what robot want to buy.
    /// </summary>
    public class MerchantCommand : ACommonCommand {

        private static readonly List<ISubCommandFactory> SUB_COMMAND_FACTORIES = new List<ISubCommandFactory>();

        public static int RegisterSubCommandFactory(ISubCommandFactory subCommandFactory) {
            int position = SUB_COMMAND_FACTORIES.Count;
            SUB_COMMAND_FACTORIES.Add(subCommandFactory);
            return position;
        }

        private int _motorId;
        /// <summary>
        /// What motor want to robot buy.
        /// </summary>
        public int MOTOR_ID {
            get {
                if (pending)
                    throw new NotSupportedException("Cannot access to property of pending request.");
                return _motorId;
            }
            private set => _motorId = value;
        }

        private int _classEquipmentId;
        /// <summary>
        /// What class equipment want to robot buy.
        /// </summary>
        public int CLASS_EQUIPMENT_ID {
            get {
                if (pending)
                    throw new NotSupportedException("Cannot access to property of pending request.");
                return _classEquipmentId;
            }
            private set => _classEquipmentId = value;
        }

        private int _armorID;
        /// <summary>
        /// What armor want to robot buy.
        /// </summary>
        public int ARMOR_ID {
            get {
                if (pending)
                    throw new NotSupportedException("Cannot access to property of pending request.");
                return _armorID;
            }
            private set => _armorID = value;
        }

        /// <summary>
        /// How many HP want robot to fix.
        /// </summary>
        public int REPAIR_HP { get; private set; }

        public MerchantCommand(int motorId, int armorId, int classEquipmentId, int repairHp) {
            MOTOR_ID = motorId;
            CLASS_EQUIPMENT_ID = classEquipmentId;
            ARMOR_ID = armorId;
            REPAIR_HP = repairHp;
            pending = false;
        }
    }
}
