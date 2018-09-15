
package BaseLibrary.communication.command.equipment;

import BaseLibrary.communication.protocol.AProtocol;

public class GetArmorsCommand extends AEquipmentCommand {

	private static final String NAME = "ARMORS";

	static {
		AProtocol.registerForDeserialize(NAME, GetArmorsCommand.class);
	}

	@Override
	public String getCommandName() {
		return NAME;
	}
	
	public GetArmorsCommand() {
		pending = false;
	}
}
