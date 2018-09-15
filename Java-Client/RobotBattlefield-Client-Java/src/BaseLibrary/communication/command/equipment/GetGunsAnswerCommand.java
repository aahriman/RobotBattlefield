package BaseLibrary.communication.command.equipment;

import BaseLibrary.communication.protocol.AProtocol;
import BaseLibrary.equipment.Gun;

public class GetGunsAnswerCommand extends AEquipmentCommand {

	private static final String NAME = "GUNS_ANSWER";

	static {
		AProtocol.registerForDeserialize(NAME, GetGunsAnswerCommand.class);
	}

	@Override
	public String getCommandName() {
		return NAME;
	}
	
	private Gun[] guns;

	public GetGunsAnswerCommand(Gun[] guns) {
		setGuns(guns);
	}

	/**
	 * Available guns to buy.
	 * 
	 * @see Gun
	 */
	public Gun[] getGuns() {
		if (pending)
			throw new UnsupportedOperationException("Cannot access to property of pending request.");
		return guns;
	}

	private void setGuns(Gun[] guns) {
		this.guns = guns;
		pending = false;
	}
}
