
package BaseLibrary.communication.command.repairman;

import BaseLibrary.communication.protocol.AProtocol;

public class RepairAnswerCommand extends ARepairmanCommand {

	private static final String NAME = "REPAIR_ANSWER";

	static {
		AProtocol.registerForDeserialize(NAME, RepairAnswerCommand.class);
	}

	@Override
	public String getCommandName() {
		return NAME;
	}
	
	private boolean success;

	public RepairAnswerCommand() {
	}

	public RepairAnswerCommand(boolean success) {
		setSuccess(success);
	}

	/**
	 * Fill command by data from another command (use-full for filling pending
	 * command by command with data).
	 */
	public void FillData(RepairAnswerCommand source) {
		setSuccess(source.success);
	}

	/**
	 * <ul>
	 * <li><description>true - robot use repair tool to repair robots in
	 * range.</description></li>
	 * <li><description>false - robot do not use repair tool to repair. Robot use
	 * repair tool too many times.</description></li>
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
