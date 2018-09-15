package ClientLibrary;

import java.io.IOException;
import java.net.InetAddress;
import java.net.Socket;
import java.net.UnknownHostException;
import java.util.concurrent.CompletableFuture;

import BaseLibrary.communication.NetworkStream;
import BaseLibrary.communication.command.handshake.ErrorCommand;
import BaseLibrary.communication.command.handshake.GameTypeCommand;
import BaseLibrary.communication.protocol.HandShakeProtocol;

/**
 * Utils for connection to server.
 */
public class ConnectionUtil {
	/**
	 * Local ip address.
	 */
	public static final String LOCAL_ADDRESS = "::1";

	private Socket socket;
	/**
	 * Instance for communication with server.
	 */
	public NetworkStream COMMUNICATION;

	/**
	 * Create object witch store <code>NetworkStream</code>
	 */
	public ConnectionUtil() {
	}

	/**
	 * Close communication.
	 */
	public void close() {
		COMMUNICATION.close();
		try {
			socket.close();
		} catch (IOException e) {
			e.printStackTrace();
		}
	}

	/**
	 * Connect to server with specified url and port.
	 * 
	 * @param url
	 *            - Url where to connect. <code>IPAddress</code> is got by
	 *            <code>IPAddress.Parse(url)</code>.
	 * @param port
	 *            - Port where to connect.
	 * @throws UnknownHostException
	 * @see InetAddress
	 * @see InetAddress.getByName
	 */
	public CompletableFuture<GameTypeCommand> connectAsync(String url, int port) throws UnknownHostException {
		InetAddress ipAddress;
		try {
			ipAddress = InetAddress.getByName(url);
		} catch (UnknownHostException e) {
			throw new UnknownHostException(e.getMessage());
		}

		return CompletableFuture.supplyAsync(() -> {
			try {
				socket = new Socket(ipAddress, port);
			} catch (IOException e) {
				throw new RuntimeException("Cannot connect to server. Is server running?");
			}

			try {
				COMMUNICATION = new NetworkStream(socket);
			} catch (IOException e) {
				throw new RuntimeException("Cannot open streams.", e);
			}

			if (handShake().join()) {
				return (GameTypeCommand) COMMUNICATION.receiveCommandAsync().join();
			} else {
				return null;
			}
		});
	}

	/**
	 * Do handshake at client client side.
	 */
	private CompletableFuture<Boolean> handShake() {
		HandShakeProtocol handShakeProtocol = new HandShakeProtocol();
		return handShakeProtocol.handShakeClient(COMMUNICATION).thenApplyAsync((protocol) -> {

			if (protocol == null) {
				disconnect("Handshake fail.");
				return false;
			} else {
				COMMUNICATION.setProtocol(protocol);
				return true;
			}
		});
	}

	/**
	 * Disconnect client and print message.
	 */
	private void disconnect(String message) {
		System.err.println(message);
		ErrorCommand error = new ErrorCommand(message);
		try {
			COMMUNICATION.writeLineAsync(error.serialize(null)).get();
		} catch (Exception e) {
			throw new RuntimeException(e.getMessage(), e);
		}

		COMMUNICATION.close();
		throw new RuntimeException(message);
	}
}
