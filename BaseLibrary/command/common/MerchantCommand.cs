using System.Collections.Generic;
using BaseLibrary.visitors;

namespace BaseLibrary.command.common {
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

        /// <summary>
        /// What motor want to robot buy.
        /// </summary>
        public int MOTOR_ID {get; private set;}

        /// <summary>
        /// What class equipment want to robot buy.
        /// </summary>
        public int CLASS_EQUIPMENT_ID { get; private set; }

        /// <summary>
        /// What armor want to robot buy.
        /// </summary>
        public int ARMOR_ID { get; private set; }

        /// <summary>
        /// How many HP want robot to fix.
        /// </summary>
        public int REPAIR_HP { get; private set; }

        public MerchantCommand(int motorId, int armorId, int classEquipmentId, int repairHp) {
            MOTOR_ID = motorId;
            CLASS_EQUIPMENT_ID = classEquipmentId;
            ARMOR_ID = armorId;
            REPAIR_HP = repairHp;
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
