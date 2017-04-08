using System.Collections.Generic;
using BaseLibrary.visitors;

namespace BaseLibrary.command.repairman {
    public class RepairCommand : ARepairmanCommand {

        private static readonly List<ISubCommandFactory> SUB_COMMAND_FACTORIES = new List<ISubCommandFactory>();

        public static int RegisterSubCommandFactory(ISubCommandFactory subCommandFactory) {
            int position = SUB_COMMAND_FACTORIES.Count;
            SUB_COMMAND_FACTORIES.Add(subCommandFactory);
            return position;
        }

        public int MAX_DISTANCE { get; private set; }

        public RepairCommand(int maxDistance) {
            this.MAX_DISTANCE = maxDistance;
        }

        public override void accept(IRepairmanCommandVisitor accepter) {
            accepter.visit(this);
        }

        public override Output accept<Output>(IRepairmanCommandVisitor<Output> accepter) {
            return accepter.visit(this);
        }

        public override Output accept<Output, Input>(IRepairmanCommandVisitor<Output, Input> accepter, params Input[] inputs) {
            return accepter.visit(this, inputs);
        }
    }
}
