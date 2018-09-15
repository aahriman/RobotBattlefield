
package BaseLibrary.communication.command.handshake;

import BaseLibrary.communication.command.ACommand;
import BaseLibrary.communication.command.ACommandFactory;
import BaseLibrary.communication.protocol.AProtocol;
import BaseLibrary.communication.protocol.IFactory;

public final class AckCommand extends AHandShakeCommand {

	private static final String NAME = "ACK";

	static {
		AProtocol.registerForDeserialize(NAME, AckCommand.class);
	}

	@Override
	public String getCommandName() {
		return NAME;
	}

	public static final IFactory<ACommand, ACommand> FACTORY = new CommandFactory();

	private static final class CommandFactory extends ACommandFactory {
		private CommandFactory() {
		}

		@Override
		public boolean isDeserializeable(String s, AProtocol protocol) {
			s = s.trim();
			if ("ACK".equals(s)) {
				cache.cached(s, new AckCommand());
				return true;
			}
			return false;
		}

		@Override
		public boolean isTransferable(ACommand c) {
			if (c instanceof AckCommand) {
				cache.cached(c, new AckCommand());
				return true;
			}
			return false;
		}
	}

	public AckCommand() {
	}

	@Override
	public String serialize(AProtocol p) {
		return "ACK";
	}
}
