using System.Collections.Generic;
using BaseLibrary.visitors;

namespace BaseLibrary.command {
    public class ScanCommand : ACommand {

        private static readonly List<ISubCommandFactory> SUB_COMMAND_FACTORIES = new List<ISubCommandFactory>();

        public static int RegisterSubCommandFactory(ISubCommandFactory subCommandFactory) {
            int position = SUB_COMMAND_FACTORIES.Count;
            SUB_COMMAND_FACTORIES.Add(subCommandFactory);
            return position;
        }

        public ProtocolDouble PRECISION { get; private set; }
        public ProtocolDouble ANGLE {get; private set;}

        public ScanCommand(ProtocolDouble precision, ProtocolDouble angle) : base() {
            ANGLE = angle;
            PRECISION = precision;
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
