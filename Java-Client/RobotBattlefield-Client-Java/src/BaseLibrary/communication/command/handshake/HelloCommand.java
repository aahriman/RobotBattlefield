
package BaseLibrary.communication.command.handshake;

import java.util.Arrays;

import BaseLibrary.communication.command.ACommand;
import BaseLibrary.communication.command.ACommandFactory;
import BaseLibrary.communication.protocol.AProtocol;
import BaseLibrary.communication.protocol.IFactory;
import BaseLibrary.config.GameProperties;

public class HelloCommand extends AHandShakeCommand {
	private static final String NAME = "HELLO";

	static {
		AProtocol.registerForDeserialize(NAME, HelloCommand.class);
	}

	@Override
	public String getCommandName() {
		return NAME;
	}

	public static final IFactory<ACommand, ACommand> FACTORY = new HelloCommandFactory();

	private final static class HelloCommandFactory extends ACommandFactory {
		private HelloCommandFactory() {
		}

		@Override
		public boolean isDeserializeable(String s, AProtocol protocol) {
			if (s.startsWith("HELLO " + GameProperties.APP_NAME)) {
				String[] protocols = s.substring(("HELLO " + GameProperties.APP_NAME).length()).split(" ");
				if (protocols.length > 0) {
					cache.cached(s, new HelloCommand(protocols));
					return true;
				}
			}
			return false;
		}

		@Override
		public boolean isTransferable(ACommand c) {
			if (c instanceof HelloCommand) {
				HelloCommand c1 = (HelloCommand) c;
				cache.cached(c, new HelloCommand(c1.SUPPORTED_PROTOCOLS));
				return true;
			}
			return false;
		}
	}

	public final String[] SUPPORTED_PROTOCOLS;

	public HelloCommand(String[] supportedProtocols) {
		Arrays.sort(supportedProtocols);
		SUPPORTED_PROTOCOLS = supportedProtocols;
	}
	

	@Override
	public String serialize(AProtocol p) {
		StringBuilder text = new StringBuilder("HELLO " + GameProperties.APP_NAME);
		for (String s : SUPPORTED_PROTOCOLS) {
			text.append(" " + s.trim());
		}
		return text.toString();
	}
}
