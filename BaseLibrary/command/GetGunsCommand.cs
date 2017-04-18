﻿using BaseLibrary.visitors;

namespace BaseLibrary.command {
    public class GetGunsCommand : ACommand {
        public GetGunsCommand() : base() { }

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
