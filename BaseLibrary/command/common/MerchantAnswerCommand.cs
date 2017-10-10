using System.Collections.Generic;
using BaseLibrary.visitors;

namespace BaseLibrary.command.common {
    public class MerchantAnswerCommand : ACommonCommand {

        private static readonly List<ISubCommandFactory> SUB_COMMAND_FACTORIES = new List<ISubCommandFactory>();

        public static int RegisterSubCommandFactory(ISubCommandFactory subCommandFactory) {
            int position = SUB_COMMAND_FACTORIES.Count;
            SUB_COMMAND_FACTORIES.Add(subCommandFactory);
            return position;
        }

        public int MOTOR_ID_BOUGHT { get; private set; }
        public int CLASS_EQUIPMENT_ID_BOUGHT { get; private set; }
        public int ARMOR_ID_BOUGHT { get; private set; }

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
