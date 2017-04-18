using System.Collections.Generic;
using BaseLibrary.equip;
using BaseLibrary.visitors;

namespace BaseLibrary.command.equipment {
    public class GetRepairToolAnswerCommand : AEquipmentCommand {

        private static readonly List<ISubCommandFactory> SUB_COMMAND_FACTORIES = new List<ISubCommandFactory>();

        public static int RegisterSubCommandFactory(ISubCommandFactory subCommandFactory) {
            int position = SUB_COMMAND_FACTORIES.Count;
            SUB_COMMAND_FACTORIES.Add(subCommandFactory);
            return position;
        }

        public RepairTool[] REPAIR_TOOLS { get; private set; }

        public GetRepairToolAnswerCommand(RepairTool[] repairTools)
            : base() {
            REPAIR_TOOLS = repairTools;
        }

        public sealed override void accept(ICommandVisitor accepter) {
            accepter.visit(this);
        }

        public sealed override Output accept<Output>(ICommandVisitor<Output> accepter) {
            return accepter.visit(this);
        }

        public sealed override Output accept<Output, Input>(ICommandVisitor<Output, Input> accepter, Input input) {
            return accepter.visit(this, input);
        }
    }
}
