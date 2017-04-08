namespace CommunicationLibrary.command {
    public class MerchantCommand : ACommand {
        public int MOTOR_ID {get; private set;}
        public int GUN_ID { get; private set; }
        public int ARMOR_ID { get; private set; }
        public int REPAIR_HP { get; private set; }

        public MerchantCommand(int motorId, int gunId, int armorId, int repairHp) {
            MOTOR_ID = motorId;
            GUN_ID = gunId;
            ARMOR_ID = armorId;
            REPAIR_HP = repairHp;
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
