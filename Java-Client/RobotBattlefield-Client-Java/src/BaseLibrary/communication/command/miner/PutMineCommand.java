
package BaseLibrary.communication.command.miner;

import BaseLibrary.communication.protocol.AProtocol;

public class PutMineCommand extends AMinerCommand {

	private static final String NAME = "PUT_MINE";

	static {
		AProtocol.registerForDeserialize(NAME, PutMineCommand.class);
	}

	@Override
	public String getCommandName() {
		return NAME;
	}
}
