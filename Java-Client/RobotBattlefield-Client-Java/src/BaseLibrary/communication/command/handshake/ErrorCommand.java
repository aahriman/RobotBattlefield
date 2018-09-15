package BaseLibrary.communication.command.handshake;

import BaseLibrary.communication.command.ACommand;
import BaseLibrary.communication.command.ACommandFactory;
import BaseLibrary.communication.protocol.AProtocol;
import BaseLibrary.communication.protocol.IFactory;

public final class ErrorCommand extends AHandShakeCommand {

	private static final String NAME = "ERROR";

	static {
		AProtocol.registerForDeserialize(NAME, ErrorCommand.class);
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
			if (s != null && s.startsWith("ERROR")) {
				String message = s.substring(("ERROR").length()).trim();
				cache.cached(s, new ErrorCommand(message));
				return true;
			}
			return false;
		}

		@Override
		public boolean isTransferable(ACommand c) {
			if (c instanceof ErrorCommand) {
				ErrorCommand c1 = (ErrorCommand) c;
				cache.cached(c, new ErrorCommand(c1.MESSAGE));
				return true;
			}
			return false;
		}
	}

	public final String MESSAGE;

	public ErrorCommand(String message) {
		MESSAGE = message;
	}

	@Override
	public String serialize(AProtocol p) {
		return "ERROR " + MESSAGE;
	}
}
