
package BaseLibrary.communication.command.common;

import BaseLibrary.communication.protocol.AProtocol;

public class ScanCommand extends ACommonCommand {
	
	private static final String NAME = "SCAN";

	static {
		AProtocol.registerForDeserialize(NAME, ScanCommand.class);
	}

	@Override
	public String getCommandName() {
		return NAME;
	}
	
	/**
	 * Max precision of scanner.
	 * 
	 * @see getPrecision
	 */
	public static final double MAX_SCAN_PRECISION = 10.0;

	private double precision;

	/**
	 * How wide is scan cone.
	 */
	public double getPrecision() {
		if (pending)
			throw new UnsupportedOperationException("Cannot access to property of pending request.");
		return precision;
	}

	private double angle;

	/**
	 * Direction to scan (0 to right, 90 down, 180 left, 270 up).
	 */
	public double getAngle() {
		if (pending)
			throw new UnsupportedOperationException("Cannot access to property of pending request.");
		return angle;
	}

	public ScanCommand(double precision, double angle) {
		this.angle = angle;
		precision = Math.max(0, precision);
		precision = Math.min(MAX_SCAN_PRECISION, precision);
		this.precision = precision;
		pending = false;
	}

}
