
package BaseLibrary.communication.command.tank;

import BaseLibrary.communication.protocol.AProtocol;

public class ShootAnswerCommand extends ATankCommand {

	private static final String NAME = "SHOOT_ANSWER";

	static {
		AProtocol.registerForDeserialize(NAME, ShootAnswerCommand.class);
	}

	@Override
	public String getCommandName() {
		return NAME;
	}
	
	private boolean success;

	public ShootAnswerCommand() {
	}

	public ShootAnswerCommand(boolean success) {
		setSuccess(success);
	}

	/**
	 * Fill command by data from another command (use-full for filling pending
	 * command by command with data).
	 */
	public void FillData(ShootAnswerCommand source) {
		setSuccess(source.success);
	}

	/**
	 * <ul>
	 * <li><description>True if shoot release bullet.</description></li>
	 * <li><description>False if shoot do not release bullet. No gun was
	 * loaded.</description></li>
	 * </ul>
	 * 
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
