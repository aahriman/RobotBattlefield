using System.Collections.Generic;
using BaseLibrary.visitors;

namespace BaseLibrary.command {
    public class MerchantAnswerCommand : ACommand {

        private static readonly List<ISubCommandFactory> SUB_COMMAND_FACTORIES = new List<ISubCommandFactory>();

        public static int RegisterSubCommandFactory(ISubCommandFactory subCommandFactory) {
            int position = SUB_COMMAND_FACTORIES.Count;
            SUB_COMMAND_FACTORIES.Add(subCommandFactory);
            return position;
        }

        public int MOTOR_ID_BOUGHT { get; private set; }
        public int CLASS_EQUIPMENT_ID_BOUGHT { get; private set; }
        public int ARMOR_ID_BOUGHT { get; private set; }

        public MerchantAnswerCommand(int motorIdBought, int armorIdBought, int classEquipmentIdBought){
            MOTOR_ID_BOUGHT = motorIdBought;
            ARMOR_ID_BOUGHT = armorIdBought;
            CLASS_EQUIPMENT_ID_BOUGHT = classEquipmentIdBought;
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
