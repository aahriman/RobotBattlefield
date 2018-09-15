
package BaseLibrary.communication.command.handshake;

import BaseLibrary.communication.command.ACommand;
import BaseLibrary.communication.command.ACommandFactory;
import BaseLibrary.communication.protocol.AProtocol;
import BaseLibrary.communication.protocol.IFactory;
import BaseLibrary.config.GameProperties;

public class OllehCommand extends AHandShakeCommand  {
	private static final String NAME = "OLLEH";

	static {
		AProtocol.registerForDeserialize(NAME, OllehCommand.class);
	}

	@Override
	public String getCommandName() {
		return NAME;
	}
	
	
	
	public static final IFactory<ACommand, ACommand> FACTORY = new OllehCommandFactory();

	

	private final static class OllehCommandFactory extends ACommandFactory {
		private OllehCommandFactory() {
		}

		@Override
		public boolean isDeserializeable(String s, AProtocol p) {
			if (s.startsWith("OLLEH " + GameProperties.APP_NAME)) {
				String protocol = s.substring(("OLLEH " + GameProperties.APP_NAME).length()).trim();
				if (!protocol.contains(" ")) {
					cache.cached(s, new OllehCommand(protocol));
					return true;
				}
			}
			return false;
		}

		@Override
		public boolean isTransferable(ACommand c) {
			if (c instanceof OllehCommand) {
				OllehCommand c1 = (OllehCommand) c;
				cache.cached(c, new OllehCommand(c1.PROTOCOL));
				return true;
			}
			return false;
		}
	}

	public final String PROTOCOL;

	public OllehCommand(String protocol) {
		if (protocol.contains(" ")) {
			throw new IllegalArgumentException("Invalid protocol.");
		}

		PROTOCOL = protocol;
		pending = false;
	}


	@Override
	public String serialize(AProtocol p) {
		return "OLLEH " + GameProperties.APP_NAME + PROTOCOL;
	}
}
