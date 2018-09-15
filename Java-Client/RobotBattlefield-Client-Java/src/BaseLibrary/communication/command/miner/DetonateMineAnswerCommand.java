
package BaseLibrary.communication.command.miner;

import BaseLibrary.communication.protocol.AProtocol;

public class DetonateMineAnswerCommand extends AMinerCommand {

	private static final String NAME = "DETONATE_MINE_ANSWER";

	static {
		AProtocol.registerForDeserialize(NAME, DetonateMineAnswerCommand.class);
	}

	@Override
	public String getCommandName() {
		return NAME;
	}
	
	private boolean success;

	public DetonateMineAnswerCommand() {
	}

	public DetonateMineAnswerCommand(boolean success) {
		setSuccess(success);
		pending = false;
	}

	/**
	 * Fill command by data from another command (use full for filling pending
	 * command by command with data).
	 */
	public void FillData(DetonateMineAnswerCommand source) {
		setSuccess(source.success);
	}

	/**
	 * <ul>
	 * <li>true - mine was detonate.</li>
	 * <li>false - mine was not detonate. Mine with specified id was not put.</li>
	 * </ul>
	 */
	public boolean isSuccess() {
		if (pending)
			throw new UnsupportedOperationException("Cannot access to property of pending request.");
		return success;
	}

	private void setSuccess(boolean success) {
		pending = false;
		this.success = success;
	}

}
