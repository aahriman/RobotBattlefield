using System.Collections.Generic;
using BaseLibrary.visitors;

namespace BaseLibrary.command.tank {
    public class ShotCommand : ATankCommand {

        private static readonly List<ISubCommandFactory> SUB_COMMAND_FACTORIES = new List<ISubCommandFactory>();

        public static int RegisterSubCommandFactory(ISubCommandFactory subCommandFactory) {
            int position = SUB_COMMAND_FACTORIES.Count;
            SUB_COMMAND_FACTORIES.Add(subCommandFactory);
            return position;
        }

        public ProtocolDouble ANGLE { get; private set; }
        public ProtocolDouble RANGE { get; private set; }

        public ShotCommand(ProtocolDouble range, ProtocolDouble angle)
            : base() {
                ANGLE = angle;
                RANGE = range;
        }

        public sealed override void accept(ITankCommandVisitor accepter) {
            accepter.visit(this);
        }

        public sealed override Output accept<Output>(ITankCommandVisitor<Output> accepter) {
            return accepter.visit(this);
        }

        public sealed override Output accept<Output, Input>(ITankCommandVisitor<Output, Input> accepter, params Input[] inputs) {
            return accepter.visit(this, inputs);
        }
    }
}
