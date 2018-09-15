
package BaseLibrary.communication.command.common;

import BaseLibrary.communication.protocol.AProtocol;

/**
 * Wait one turn or to until HitPoint > 0 or to the end of turn
 */
public class WaitCommand extends ACommonCommand {

	private static final String NAME = "WAIT";

	static {
		AProtocol.registerForDeserialize(NAME, WaitCommand.class);
	}

	@Override
	public String getCommandName() {
		return NAME;
	}

	public WaitCommand() {
		pending = false;
	}
}
