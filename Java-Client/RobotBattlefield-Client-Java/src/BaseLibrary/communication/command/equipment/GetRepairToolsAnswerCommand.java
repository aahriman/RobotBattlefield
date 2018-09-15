
package BaseLibrary.communication.command.equipment;

import BaseLibrary.communication.protocol.AProtocol;
import BaseLibrary.equipment.RepairTool;

public class GetRepairToolsAnswerCommand extends AEquipmentCommand {

	private static final String NAME = "REPAIR_TOOLS_ANSWER";

	static {
		AProtocol.registerForDeserialize(NAME, GetRepairToolsAnswerCommand.class);
	}

	@Override
	public String getCommandName() {
		return NAME;
	}
	
	private RepairTool[] repairTools;

	public GetRepairToolsAnswerCommand(RepairTool[] repairTools) {
		setRepairTools(repairTools);
	}
	
	/**
	 * Available repair tool to buy.
	 * 
	 * @see RepairTool
	 */
	public RepairTool[] getRepairTools() {
		if (pending)
			throw new UnsupportedOperationException("Cannot access to property of pending request.");
		return repairTools;
	}

	public void setRepairTools(RepairTool[] repairTools) {
		pending = false;
		this.repairTools = repairTools;
	}
}
