using System.Collections.Generic;
using BaseLibrary.equip;

namespace BaseLibrary.command.equipment {
    public class GetRepairToolsAnswerCommand : AEquipmentCommand {

        private static readonly List<ISubCommandFactory> SUB_COMMAND_FACTORIES = new List<ISubCommandFactory>();

        public static int RegisterSubCommandFactory(ISubCommandFactory subCommandFactory) {
            int position = SUB_COMMAND_FACTORIES.Count;
            SUB_COMMAND_FACTORIES.Add(subCommandFactory);
            return position;
        }

        /// <summary>
        /// Available repair tool to buy.
        /// </summary>
        /// <seealso cref="RepairTool"/>
        public RepairTool[] REPAIR_TOOLS { get; private set; }

        public GetRepairToolsAnswerCommand(RepairTool[] repairTools)
            : base() {
            REPAIR_TOOLS = repairTools;
        }
    }
}
