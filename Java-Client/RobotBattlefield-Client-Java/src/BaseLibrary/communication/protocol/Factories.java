package BaseLibrary.communication.protocol;

import java.util.ArrayList;
import java.util.List;

public class Factories<TResult, TTransfer> {
	private final List<IFactory<TResult, TTransfer>> commandFactorys = new ArrayList<IFactory<TResult, TTransfer>>();

	public void RegisterCommand(IFactory<TResult, TTransfer> factory) {
		commandFactorys.add(factory);
	}

	public void UnregisterCommand(IFactory<TResult, TTransfer> factory) {
		commandFactorys.remove(factory);
	}

	public TResult Deserialize(String transferString, AProtocol protocol) {
		for (IFactory<TResult, TTransfer> commandFactory : commandFactorys) {
			if (commandFactory.isDeserializeable(transferString, protocol)) {
				return commandFactory.deserialize(transferString, protocol);
			}
		}
		return null;
	}

	public String Serialize(TTransfer transfer, AProtocol protocol) {
		for (IFactory<TResult, TTransfer> commandFactory : commandFactorys) {
			if (commandFactory.isSerializeable(transfer, protocol)) {
				return commandFactory.serialize(transfer, protocol);
			}
		}
		return null;
	}
}
