
package BaseLibrary.communication.command.tank;

import BaseLibrary.communication.protocol.AProtocol;

public class ShootCommand extends ATankCommand {
	
	private static final String NAME = "SHOOT";

	static {
		AProtocol.registerForDeserialize(NAME, ShootCommand.class);
	}

	@Override
	public String getCommandName() {
		return NAME;
	}

	/**
	 * How far from want to shot.
	 */
	private double range;
	
	/**
	 * In witch direction want to shot.
	 */
	private double angle;

	

	public ShootCommand(double range, double angle) {
		this.angle = angle;
		this.range = range;
		pending = false;
	}
	
	public double getAngle() {
		if (pending)
			throw new UnsupportedOperationException("Cannot access to property of pending request.");
		return angle;
	}
	
	public double getRange() {
		if (pending)
			throw new UnsupportedOperationException("Cannot access to property of pending request.");
		return range;
	}
}
