package BaseLibrary.communication.command.equipment;

import BaseLibrary.communication.protocol.AProtocol;

public class GetMineGunsCommand extends AEquipmentCommand {

	private static final String NAME = "MINE_GUNS";

	static {
		AProtocol.registerForDeserialize(NAME, GetMineGunsCommand.class);
	}

	@Override
	public String getCommandName() {
		return NAME;
	}
}