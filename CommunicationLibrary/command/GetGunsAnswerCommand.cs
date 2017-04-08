using CommunicationLibrary.equip;
using CommunicationLibrary.utils;

namespace CommunicationLibrary.command {
    public class GetGunsAnwerCommand : ACommand {
        public Gun[] GUNS { get; private set; }

        public GetGunsAnwerCommand(Gun[] guns)
            : base() {
				GUNS = ArrayUtils.DeepCopy(guns);
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
