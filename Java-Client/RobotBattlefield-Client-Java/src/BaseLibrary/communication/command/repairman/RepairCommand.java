
package BaseLibrary.communication.command.repairman;

import BaseLibrary.communication.protocol.AProtocol;

public class RepairCommand extends ARepairmanCommand {

	private static final String NAME = "REPAIR";

	static {
		AProtocol.registerForDeserialize(NAME, RepairCommand.class);
	}

	@Override
	public String getCommandName() {
		return NAME;
	}
	
	private double maxDistance;

	public RepairCommand() {
		this(10000);
	}

	public RepairCommand(double maxDistance) {
		setMaxDistance(maxDistance);
	}

	/**
	 * Distance how far repair.
	 */
	public double getMaxDistance() {
		if (pending)
			throw new UnsupportedOperationException("Cannot access to property of pending request.");
		return maxDistance;
	}

	private void setMaxDistance(double maxDistance) {
		this.maxDistance = maxDistance;
		pending = false;
	}
}
