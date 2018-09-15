
package BaseLibrary.communication.command.miner;

import BaseLibrary.communication.protocol.AProtocol;

public class PutMineAnswerCommand extends AMinerCommand {

	private static final String NAME = "PUT_MINE_ANSWER";

	static {
		AProtocol.registerForDeserialize(NAME, PutMineAnswerCommand.class);
	}

	@Override
	public String getCommandName() {
		return NAME;
	}
	
	/**
	 * Mine id if SUCCESS is false.
	 */
	public static final int FALSE_MINE_ID = 0;

	private boolean success;
	private int mineID;

	public PutMineAnswerCommand() {
	}

	public PutMineAnswerCommand(boolean success, int mineID) {
		this.success = success;
		this.mineID = mineID;
		pending = false;
	}

	/**
	 * Fill command by data from another command (use full for filling pending
	 * command by command with data).
	 */
	public void FillData(PutMineAnswerCommand source) {
		success = source.success;
		mineID = source.mineID;
		pending = false;
	}

	/**
	 * <ul>
	 * <li>true - mine was put.</li>
	 * <li>false - mine was not put. Robot reach maximum of put mines.</li>
	 * </ul>
	 */
	public boolean isSuccess() {
		if (pending)
			throw new UnsupportedOperationException("Cannot access to property of pending request.");
		return success;
	}

	/**
	 * Mine id for put mine.
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
