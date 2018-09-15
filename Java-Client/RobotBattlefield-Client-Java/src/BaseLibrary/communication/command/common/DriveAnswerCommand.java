
package BaseLibrary.communication.command.common;

import BaseLibrary.communication.protocol.AProtocol;

/**
 * Answer for drive command.
 */
public class DriveAnswerCommand extends ACommonCommand {

	private static final String NAME = "DRIVE_ANSWER";
	
	static{
		AProtocol.registerForDeserialize(NAME, DriveAnswerCommand.class);
	}
	
	@Override
	public String getCommandName() {
		return NAME;
	}
	
	private boolean success;

		public DriveAnswerCommand() {
	}

	public DriveAnswerCommand(boolean success) {
		setSuccess(success);
	}

	/**
	 * Fill command by data from another command (use-full for filling pending
	 * command by command with data).
	 */
	public void FillData(DriveAnswerCommand source) {
		setSuccess(source.success);
	}

	/**
     * True if robot change direction.
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
