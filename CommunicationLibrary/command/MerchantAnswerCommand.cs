namespace CommunicationLibrary.command {
    public class MerchantAnswerCommand : ACommand {

        public int MOTOR_ID_BOUGHT { get; private set; }
        public int GUN_ID_BOUGHT { get; private set; }
        public int ARMOR_ID_BOUGHT { get; private set; }

        public MerchantAnswerCommand(int motorIdBought, int gunIdBought, int armorIdBought){
            MOTOR_ID_BOUGHT = motorIdBought;
            GUN_ID_BOUGHT = gunIdBought;
            ARMOR_ID_BOUGHT = armorIdBought;
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
