
package ClientLibrary.config;

import java.util.ArrayList;
import java.util.List;

import BaseLibrary.communication.protocol.AProtocol;
import BaseLibrary.communication.protocol.ProtocolDescription;
import BaseLibrary.communication.protocol.ProtocolFactory;
import BaseLibrary.utils.ModUtils;

/**
 * Configuration for client side.
 */
public final class ClientConfig {

	private ClientConfig() {
	}

	/**
	 * Protocol witch is supported by client.
	 */
	public static final String[] SUPPORTED_PROTOCOLS;

	/**
	 * Protocol factory for making instance protocol by given protocol String.
	 */
	public static final ProtocolFactory PROTOCOL_FACTORY = new ProtocolFactory();

	static {
		int index = 0;
		KeyValuePair<String, AProtocol>[] supportedProtocols = getSupportedProcotols();
		SUPPORTED_PROTOCOLS = new String[supportedProtocols.length];
		for (KeyValuePair<String, AProtocol> pair : supportedProtocols) {
			SUPPORTED_PROTOCOLS[index++] = pair.Key;
			PROTOCOL_FACTORY.RegisterProtocol(pair.Key, pair.Value);
		}
	}

	@SuppressWarnings("unchecked")
	private static KeyValuePair<String, AProtocol>[] getSupportedProcotols() {
		List<KeyValuePair<String, AProtocol>> supportedProtocols = new ArrayList<KeyValuePair<String, AProtocol>>();

		List<Class<?>> classes = ModUtils.loadClassFromPackage("BaseLibrary.communication.protocol");
		for (Class<?> c : classes) {
			if (AProtocol.class.isAssignableFrom(c)) {
				ProtocolDescription description = c.getAnnotation(ProtocolDescription.class);
				if (description != null) {
					try {
						supportedProtocols.add(
								new KeyValuePair<String, AProtocol>(description.name(), (AProtocol) c.newInstance()));
					} catch (InstantiationException | IllegalAccessException e) {
						e.printStackTrace();
					}
				}
			}
		}

		return (KeyValuePair<String, AProtocol>[]) supportedProtocols.toArray(new KeyValuePair[0]);
	}

	private static class KeyValuePair<KEY, VALUE> {
		public final KEY Key;
		public final VALUE Value;

		public KeyValuePair(KEY key, VALUE value) {
			this.Key = key;
			this.Value = value;
		}
	}
}
