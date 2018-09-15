package BaseLibrary.communication.protocol;

import java.util.concurrent.CompletableFuture;

import BaseLibrary.communication.NetworkStream;
import BaseLibrary.communication.command.ACommand;
import BaseLibrary.communication.command.handshake.AckCommand;
import BaseLibrary.communication.command.handshake.ErrorCommand;
import BaseLibrary.communication.command.handshake.HelloCommand;
import BaseLibrary.communication.command.handshake.OllehCommand;
import ClientLibrary.config.ClientConfig;

public class HandShakeProtocol {
	protected Factories<ACommand, ACommand> comandsFactory = new Factories<ACommand, ACommand>();

	public HandShakeProtocol() {
		comandsFactory.RegisterCommand(AckCommand.FACTORY);
		comandsFactory.RegisterCommand(HelloCommand.FACTORY);
		comandsFactory.RegisterCommand(OllehCommand.FACTORY);
		comandsFactory.RegisterCommand(ErrorCommand.FACTORY);
	}

	public ACommand GetCommand(String s) {
		return comandsFactory.Deserialize(s, null);
	}

	/// <summary>
	/// Do handshake for client side.
	/// </summary>
	/// <param name="clientSocket"> Socket from client side to server
	/// side</param>
	/// <returns>null if handShake fail otherwise return instance of
	/// AProtocol</returns>
	public CompletableFuture<AProtocol> handShakeClient(NetworkStream clientSocket) {
		
			HelloCommand hello = new HelloCommand(ClientConfig.SUPPORTED_PROTOCOLS);
			
			return clientSocket.writeLineAsync(hello.serialize(null)).thenApplyAsync((trash) -> {
				ACommand shouldBeOLLEH = GetCommand(clientSocket.readLineAsync().join());
				if (shouldBeOLLEH != null && shouldBeOLLEH instanceof OllehCommand) {
					OllehCommand olleh = (OllehCommand) shouldBeOLLEH;
					AProtocol protocol = ClientConfig.PROTOCOL_FACTORY.GetProtocol(olleh.PROTOCOL);
					if (protocol != null) {
						AckCommand ack = new AckCommand();
						clientSocket.writeLineAsync(ack.serialize(null)).join();
						return protocol;
					}
					ErrorCommand error = new ErrorCommand(
							String.format("Unsupported protocols '%s'. Handshake failed.", olleh.PROTOCOL));
					clientSocket.sendCommand(error);
				} else {
					printIfErrorElseSendMessage(shouldBeOLLEH,
							"Handshake error. Expected OllehCommand but receive " + shouldBeOLLEH.getClass().getName(),
							clientSocket);
				}
				return null;
			});
	}

	private void printIfErrorElseSendMessage(ACommand command, String message, NetworkStream socket) {
		if (command instanceof ErrorCommand) {
			System.err.println("ERROR: " + ((ErrorCommand) command).MESSAGE);
		} else {
			ErrorCommand error = new ErrorCommand(message);
			socket.sendCommandAsync(error);
			System.out.println(message);
		}
	}
}
