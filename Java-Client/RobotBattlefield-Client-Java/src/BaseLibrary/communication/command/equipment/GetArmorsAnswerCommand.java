package BaseLibrary.communication.command.equipment;

import BaseLibrary.communication.protocol.AProtocol;
import BaseLibrary.equipment.Armor;

public class GetArmorsAnswerCommand extends AEquipmentCommand {
	private static final String NAME = "ARMORS_ANSWER";

	static {
		AProtocol.registerForDeserialize(NAME, GetArmorsAnswerCommand.class);
	}

	@Override
	public String getCommandName() {
		return NAME;
	}
	
	private Armor[] armors;

	public GetArmorsAnswerCommand(Armor[] armors) {
		setArmors(armors);
	}

	/**
	 * Available armors to buy.
	 */
	public Armor[] getArmors() {
		if (pending)
			throw new UnsupportedOperationException("Cannot access to property of pending request.");
		return armors;
	}

	private void setArmors(Armor[] armors) {
		pending = false;
		this.armors = armors;
	}
}
