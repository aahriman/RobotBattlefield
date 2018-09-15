
package BaseLibrary.communication.command.miner;

import BaseLibrary.communication.protocol.AProtocol;

public class DetonateMineCommand extends AMinerCommand {

	private static final String NAME = "DETONATE_MINE";

	static {
		AProtocol.registerForDeserialize(NAME, DetonateMineCommand.class);
	}

	@Override
	public String getCommandName() {
		return NAME;
	}
	
	private int mineID;

	public DetonateMineCommand(int mineID) {
		setMineID(mineID);
	}
	
	/**
	 * ID of mine which want to detonate.
	 */
	public int getMineID() {
		if (pending)
			throw new UnsupportedOperationException("Cannot access to property of pending request.");
		return mineID;
	}

	public void setMineID(int mineID) {
		this.mineID = mineID;
		pending = false;
	}

}
