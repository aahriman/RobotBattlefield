
package BaseLibrary.communication.command.common;

import BaseLibrary.communication.protocol.AProtocol;

public class DriveCommand extends ACommonCommand {

	private static final String NAME = "DRIVE";
	
	static{
		AProtocol.registerForDeserialize(NAME, DriveCommand.class);
	}
	
	@Override
	public String getCommandName() {
		return NAME;
	}
	
	/**
	 * How fast robot wants to go.
	 */
	private double power;

	/**
	 * In witch direction robot wants to go.
	 */
	private double angle;

	public DriveCommand(double power, double angle) {
		power = Math.max(0, power);
		power = Math.min(100.0, power);
		this.power = power;
		this.angle = angle;
		pending = false;
	}

	/**
	 * How fast robot wants to go.
	 */
	public double getPower() {
		if (pending)
			throw new UnsupportedOperationException("Cannot access to property of pending request.");
		return power;
	}

	/**
	 * In witch direction robot wants to go.
	 */
	public double getAngle() {
		if (pending)
			throw new UnsupportedOperationException("Cannot access to property of pending request.");
		return angle;
	}

}
