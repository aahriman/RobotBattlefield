package BaseLibrary.communication.command;

import java.lang.reflect.Field;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

import BaseLibrary.communication.protocol.AProtocol;
import BaseLibrary.communication.protocol.AProtocol.Array;
import BaseLibrary.communication.protocol.IInnerObject;

public abstract class ACommand {
	
	/**
	 * If someone implements ACommand.Sendable it have to be child of ACommand
	 */
	public static interface Sendable {
		String Serialize();
	}

	/**
	 * true if request do not have valid data.
	 */
	protected boolean pending = true;

	/**
	 * For send mod extensions.
	 */
	public Object[] MORE;

	/**
	 * for storing mod factories
	 */
	private static final HashMap<Class<?>, List<ISubCommandFactory>> SUB_COMMAND_FACTORIES = new HashMap<>();

	public static <T> int RegisterSubCommandFactory(ISubCommandFactory subCommandFactory, Class<T> forCommandClass) {
		int position = SUB_COMMAND_FACTORIES.size();
		List<ISubCommandFactory> factories = SUB_COMMAND_FACTORIES.get(forCommandClass);
		if (factories == null) {
			factories = new ArrayList<>();
		}
		factories.add(subCommandFactory);
		SUB_COMMAND_FACTORIES.put(forCommandClass, factories);
		return position;
	}

	private Class<?> forCommandClass = this.getClass();

	protected ACommand() {
		List<ISubCommandFactory> factories = SUB_COMMAND_FACTORIES.get(forCommandClass);
		if (factories != null) {
			MORE = new Object[factories.size()];
		} else {
			MORE = new Object[0];
		}
	}

	@Override
	public String toString() {

		StringBuilder sb = new StringBuilder();
		sb.append(super.toString());
		sb.append("[");
		for (Field propertyInfo : this.getClass().getDeclaredFields()) {
			sb.append(propertyInfo.getName());
			sb.append(":");
			try {
				sb.append(propertyInfo.get(this));
			} catch (IllegalArgumentException | IllegalAccessException e) {
				e.printStackTrace();
			}
			sb.append(";");
		}
		sb.append("]");

		return sb.toString();
	}
	
	public abstract String getCommandName();
	
	public String serialize(AProtocol protocol) {
		return protocol.serialize(this);
	}
	
	public void deserializeMore(String serializedMore, AProtocol protocol) {	
		Array array = protocol.deserializeArray(serializedMore, IInnerObject.class);
		Object[] more = array.values;
		List<ISubCommandFactory> factories = SUB_COMMAND_FACTORIES.get(forCommandClass);
		if (factories != null) {
			for (Object m : more) {
				for (ISubCommandFactory subCommandFactory : factories) {
					if (subCommandFactory.insert(m, MORE)) {
						break;
					}
				}
			}
		}
	}
}
