using BaseLibrary.equip;
using BaseLibrary.utils;
using BaseLibrary.visitors;

namespace BaseLibrary.command {
    public class GetMotorsAnwerCommand : ACommand {

		public Motor[] MOTORS { get; private set; }

		public GetMotorsAnwerCommand(Motor[] motors)
			: base() {
				MOTORS = ArrayUtils.DeepCopy(motors);
        }

        public sealed override void accept(AVisitorCommand accepter) {
            accepter.visit(this);
        }

        public sealed override Output accept<Output>(AVisitorCommand<Output> accepter) {
            return accepter.visit(this);
        }

        public sealed override Output accept<Output, Input>(AVisitorCommand<Output, Input> accepter, Input input) {
            return accepter.visit(this, input);
        }
    }
}
