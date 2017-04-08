using System;

namespace CommunicationLibrary.command {
    public class InitCommand : ACommand{
		public static int NAME_MAX_LENGTH = 10;
		public String NAME { get; private set; }

        public InitCommand(String name) {
			NAME = name.Substring(0, Math.Min(name.Length, NAME_MAX_LENGTH));
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
