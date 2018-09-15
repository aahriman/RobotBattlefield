package BaseLibrary.communication.command.equipment;

import BaseLibrary.communication.protocol.AProtocol;

public class GetGunsCommand extends AEquipmentCommand {

	private static final String NAME = "GUNS";

	static {
		AProtocol.registerForDeserialize(NAME, GetGunsAnswerCommand.class);
	}

	@Override
	public String getCommandName() {
		return NAME;
	}
	
	public GetGunsCommand() {
		pending = false;
	}
}
