using CommunicationLibrary.equip;

namespace CommunicationLibrary.command {
    public class GetArmorsAnwerCommand : ACommand {
        
        public static GetArmorsAnwerCommand GetInstance(Armor[] armors) {
            return new GetArmorsAnwerCommand(armors);
        }

        public Armor[] ARMORS { get; private set; }

        public GetArmorsAnwerCommand(Armor[] armors)
            : base() {
            ARMORS = new Armor[armors.Length];
            for (int i = 0; i < ARMORS.Length; i++) {
                ARMORS[i] = armors[i];
            }
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
