package BaseLibrary.communication;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.OutputStreamWriter;
import java.io.Reader;
import java.io.Writer;
import java.net.Socket;
import java.nio.charset.StandardCharsets;
import java.util.concurrent.CompletableFuture;
import java.util.concurrent.CompletionException;

import BaseLibrary.communication.command.ACommand;
import BaseLibrary.communication.command.handshake.ErrorCommand;
import BaseLibrary.communication.protocol.AProtocol;

/**
 * Stream for communication by sockets with server.
 */
public class NetworkStream {

	protected final Socket s;
	protected final Reader sr;
	protected final Writer sw;
	protected AProtocol protocol = null;

	public AProtocol getProtocol() {
		return protocol;
	}

	public void setProtocol(AProtocol protocol) {
		if (this.protocol == null || this.protocol.equals(protocol)) {
			this.protocol = protocol;
		} else {
			throw new RuntimeException("Protocol can be set only once.");
		}
	}

	/**
	 * Create instance from socket.
	 * 
	 * @throws IOException
	 */
	public NetworkStream(Socket s) throws IOException {
		this.s = s;
		sr = new BufferedReader(new InputStreamReader(s.getInputStream(), StandardCharsets.UTF_8));
		sw = new BufferedWriter(new OutputStreamWriter(s.getOutputStream(), StandardCharsets.UTF_8)) {
			@Override
			public void newLine() throws IOException {
				this.write('\n');
			}
		};

	}

	/**
	 * Read asynchronously one line from stream.
	 */
	public CompletableFuture<String> readLineAsync() {
		return CompletableFuture.supplyAsync(() -> {
			try {
				StringBuilder s = new StringBuilder();
				int character = 0;
				while ((character = sr.read()) != -1 && character != '\n') {
					if (character == Integer.parseInt("FEFF", 16)) {
						continue;
					}
					s.append((char) character);
				}
				return s.toString();
			} catch (IOException e) {
				throw new CompletionException(e);
			}
		});
	}

	/**
	 * Write asynchronously one line to stream.
	 */
	public CompletableFuture<Void> writeLineAsync(String s) {
		return CompletableFuture.runAsync(() -> {
			try {
				sw.write(s);
				sw.write('\n');
				sw.flush();
			} catch (IOException e) {
				throw new CompletionException(e.getMessage(), e);
			}
		});
	}

	/**
	 * Synchronously receive command from stream. That is mean read line and
	 * deserialize command by <code>PROTOCOL</code>.
	 * 
	 * @throws IllegalArgumentException
	 *             - When protocol can not deserialize line.
	 */
	public ACommand receiveCommand() throws IllegalArgumentException {
		return receiveCommandAsync().join();
	}

	/**
	 * Asynchronously receive command from stream. That is mean read line and
	 * deserialize command by <code>PROTOCOL</code>.
	 * 
	 * @throws RuntimeException
	 *             - When protocol can not deserialize line. - When protocol is not
	 *             set.
	 */
	public CompletableFuture<ACommand> receiveCommandAsync() {
		if (protocol == null) {
			throw new RuntimeException("Cannot read or write before set protocol.");
		}

		return readLineAsync().thenApplyAsync((s) -> {
				System.out.println(s);
				ACommand command = getProtocol().getCommand(s);
				
				if (command == null) {
					throw new RuntimeException(String.format("Protocol (%s) can not deserialize String '%s'",
							protocol.getClass().getName(), s));
				}

				if (command instanceof ErrorCommand) {
					System.err.println(((ErrorCommand) command).MESSAGE);
				}
				return command;	
			});
	}

	/**
	 * Synchronously send command.
	 */
	public void sendCommand(ACommand command) {
		sendCommandAsync(command).join();
	}

	/**
	 * Asynchronously send command.
	 */
	public CompletableFuture<Void> sendCommandAsync(ACommand command) {
		if (protocol == null) {
			throw new RuntimeException("Cannot read or write before set protocol.");
		}

		String serializedCommand = command.serialize(getProtocol());
		return writeLineAsync(serializedCommand);
	}

	private boolean close = false;

	/**
	 * Close stream.
	 */
	public void close() {
		if (!close) {
			try {
				sw.close();
			} catch (IOException e) {
			}
			try {
				sr.close();
			} catch (IOException e) {
			}
			try {
				s.close();
			} catch (IOException e) {
			}
			close = true;
		}
	}
}