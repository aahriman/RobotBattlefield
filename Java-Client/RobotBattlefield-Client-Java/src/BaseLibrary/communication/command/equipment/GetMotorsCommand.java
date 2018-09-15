
package BaseLibrary.communication.command.equipment;

import BaseLibrary.communication.protocol.AProtocol;

public class GetMotorsCommand extends AEquipmentCommand {

	private static final String NAME = "MOTORS";

	static {
		AProtocol.registerForDeserialize(NAME, GetMotorsCommand.class);
	}

	@Override
	public String getCommandName() {
		return NAME;
	}
	
	
	public GetMotorsCommand() {
	}
}
