package BaseLibrary.communication.command.equipment;

import BaseLibrary.communication.protocol.AProtocol;
import BaseLibrary.equipment.Motor;

public class GetMotorsAnswerCommand extends AEquipmentCommand {

	private static final String NAME = "MOTORS_ANSWER";

	static {
		AProtocol.registerForDeserialize(NAME, GetMotorsAnswerCommand.class);
	}

	@Override
	public String getCommandName() {
		return NAME;
	}
	
	private Motor[] motors;

	public GetMotorsAnswerCommand(Motor[] motors) {
		setMotors(motors);
	}
	
	/**
	 * Available motors to buy.
	 * 
	 * @see Motor
	 */
	public Motor[] getMotors() {
		if (pending)
			throw new UnsupportedOperationException("Cannot access to property of pending request.");
		return motors;
	}

	private void setMotors(Motor[] motors) {
		pending = false;
		this.motors = motors;
	}
}
