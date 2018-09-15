
package BaseLibrary.communication.command.equipment;

import BaseLibrary.communication.protocol.AProtocol;

public class GetRepairToolsCommand extends AEquipmentCommand {

	private static final String NAME = "REPAIR_TOOLS";

	static {
		AProtocol.registerForDeserialize(NAME, GetRepairToolsCommand.class);
	}

	@Override
	public String getCommandName() {
		return NAME;
	}
}
