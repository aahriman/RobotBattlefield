package BaseLibrary.communication.protocol;

import java.lang.reflect.Constructor;
import java.lang.reflect.Field;
import java.lang.reflect.InvocationTargetException;
import java.lang.reflect.Modifier;
import java.lang.reflect.Parameter;
import java.util.ArrayList;
import java.util.List;

import BaseLibrary.communication.command.ACommand;

@ProtocolDescription(name = "v1.0")
public class ProtocolV1_0 extends AProtocol {

	public static class Deep {
		public static final Deep _8 = new Deep('*', null);
		public static final Deep _7 = new Deep('$', _8);
		public static final Deep _6 = new Deep('@', _7);
		public static final Deep _5 = new Deep('#', _6);
		public static final Deep _4 = new Deep('!', _5);
		public static final Deep _3 = new Deep('\\', _4);
		public static final Deep _2 = new Deep('/', _3);
		public static final Deep _1 = new Deep('|', _2);
		public static final Deep _0 = new Deep(';', _1);

		public static final Deep[] DEEPS = new Deep[] { _0, _1, _2, _3, _4, _5, _6, _7, _8 };
		public static final Deep[] DEEPS_REVER = new Deep[] { _8, _7, _6, _5, _4, _3, _2, _1, _0 };

		private Deep prev;
		public final Deep NEXT;
		public final char SEPARATOR;
		public final String SEPARATOR_STRING;

		private Deep(char separator, Deep next) {
			SEPARATOR = separator;
			SEPARATOR_STRING = "" + separator;
			NEXT = next;
			if (next != null) {
				next.prev = this;
			}
		}

		public Deep GetPrev() {
			return prev;
		}
	}

	public ProtocolV1_0() {
	}

	
	@Override
	protected String serializeInt(int i, String fieldName) {
		return String.valueOf(i);
	}

	@Override
	protected String serializeChar(char i, String fieldName) {
		return String.valueOf(i);
	}

	@Override
	protected String serializeBoolean(boolean i, String fieldName) {
		return i ? "1" : "0";
	}

	@Override
	protected String serializeProtocolDouble(ProtocolDouble i, String fieldName) {
		return i.Serialize();
	}

	@Override
	public String serializeString(String i, String fieldName) {
		for (Deep deep : Deep.DEEPS_REVER) {
			if (i.contains(deep.SEPARATOR_STRING)) {
				if (deep.NEXT != null) {
					i = i.replace(deep.SEPARATOR, deep.NEXT.SEPARATOR);
				} else {
					throw new RuntimeException("Too deep");
				}
			}
		}
		return i;
	}

	public String deserializeString(String string) {
		for (Deep deep : Deep.DEEPS) {
			if (string.contains(deep.SEPARATOR_STRING)) {
				if (deep.GetPrev() != null) {
					string = string.replace(deep.SEPARATOR, deep.GetPrev().SEPARATOR);
				}
			}
		}
		return string;
	}

	@Override
	public String serializeArray(Array array, String fieldName) {
		StringBuilder sb = new StringBuilder();
		sb.append('[');
		for (int i = 0; i < array.length; i++) {
			Object object = array.values[i];
			sb.append(serializeString(serializeObjectValue(object), null));
			if (i < array.length - 1) {
				sb.append(Deep._0.SEPARATOR);
			}
		}
		sb.append(']');
		return sb.toString();
	}

	@Override
	public String serializeObject(String objectName, Object object, Field... fields) {
		StringBuilder sb = new StringBuilder();
		
		ArrayList<Field> filteredFields = new ArrayList<>();
		
		for (int i = 0; i < fields.length; i++) {
			Field field = fields[i];
			if (!Modifier.isStatic(field.getModifiers()) && !field.getName().equals("MORE")) {
				filteredFields.add(field);
			}
		}
		
		sb.append(objectName);
		sb.append('(');
		for (int i = 0; i < filteredFields.size(); i++) {
			Field field = filteredFields.get(i);
			sb.append(serializeString(serialize(object, field), null));
			if (i < filteredFields.size() - 1) {
				sb.append(Deep._0.SEPARATOR);
			}
		}
		sb.append(')');
		return sb.toString();
	}

	@Override
	public String serializeInnerObject(IInnerObject object, String fieldName) {
		return serialize(object);
	}

	@Override
	public String serializeNull(String fieldName) {
		return "";
	}

	@Override
	public Array deserializeArray(String serializedArray, Class<?> arrayClass) {
		String[] arrayElement = splitArray(serializedArray);
		Object[] array = new Object[arrayElement.length];
		for (int i = 0; i < arrayElement.length; i++) {
			String e = arrayElement[i];
			if (e.startsWith("[")) {
				array[i] = deserializeArray(e, arrayClass.getComponentType()).values;
			} else {
				array[i] = deserializeValue(arrayClass, e);
			}
		}
		return new Array(array);
	}

	@Override
	public Object deserializeObject(String serializedObject) {

		if (serializedObject.equals("")) {
			return null;
		}
		String objectName = serializedObject.substring(0, serializedObject.indexOf('('));
		Object o = null;
		if (classByObjectName.get(objectName) != null) {
			try {
				Constructor<?>[] constructors = classByObjectName.get(objectName).getConstructors();
				
				String[] args = serializedObject.substring(objectName.length() + 1, serializedObject.length() - 1)
						.split(Deep._0.SEPARATOR_STRING);
				for (int i = 0; i < args.length; i++) {
					args[i] = deserializeString(args[i]);
				}

				Constructor<?> constructor = null;
				int argMoreIndex = -1;
				for (int i = 0; i < constructors.length; i++) {
					if (constructors[i].getParameterTypes().length == args.length) {
						constructor = constructors[i];
						break;
					} else if (ACommand.class.isAssignableFrom(classByObjectName.get(objectName)) &&  constructors[i].getParameterTypes().length == args.length - 1) {
						argMoreIndex = args.length - 1;
						constructor = constructors[i];
					}
				}
				
				if (constructor == null) {
					throw new RuntimeException("ProtocolV1_0 cannot find constructor for deserialize string " + serializedObject);
				}
				Parameter[] parameters = constructor.getParameters();
				
				List<Object> parameterValues = new ArrayList<>();
				for (int i = 0; i < parameters.length; i++) {
					Class<?> type = parameters[i].getType();
					
					Object value = deserializeValue(type, args[i]);
					if (value instanceof Array) {
						value = ConvertProtocolArrayToProperArray((Array) value, type);
					}
					parameterValues.add(value);
				}

				o = constructor.newInstance(parameterValues.toArray());

				if (argMoreIndex != -1 && ACommand.class.isAssignableFrom(classByObjectName.get(objectName)) && argMoreIndex < args.length) {
					((ACommand) o).deserializeMore(args[argMoreIndex], this);
				}

			} catch (SecurityException | IllegalArgumentException | InstantiationException
					| IllegalAccessException | InvocationTargetException e) {
				throw new RuntimeException(e.getMessage(), e);
			}
		} else {
			throw new RuntimeException("Cannot deserialize string " + serializedObject + " no class was found");
		}
		return o;
	}

	private String[] splitArray(String array) {
		if (array.equals("[]")) {
			return new String[0];
		}
		try {
		String[] arrayElements = array.substring(1, array.length() - 1).split(Deep._0.SEPARATOR_STRING);
		for (int i = 0; i < arrayElements.length; i++) {
			arrayElements[i] = deserializeString(arrayElements[i]);
		}
		return arrayElements;
		} catch(StringIndexOutOfBoundsException e) {
			throw e;
		}
	}

	@Override
	protected Integer deserializeInt(String integer) {
		return Integer.parseInt(integer);
	}

	@Override
	protected Character deserializeChar(String character) {
		return character.charAt(0);
	}

	@Override
	protected Boolean deserializeBoolean(String i) {
		return i.equals("1") ? true : false;
	}

	@Override
	protected ProtocolDouble deserializeProtocolDouble(String i) {
		return ProtocolDouble.Parse(i);
	}

	@Override
	public IInnerObject deserializeInnerObject(String serializedInnerObject) {
		return (IInnerObject) deserializeObject(serializedInnerObject);
	}
	
	private  Object ConvertProtocolArrayToProperArray(Array array, Class<?> properArray) {
		Object ret = java.lang.reflect.Array.newInstance(properArray.getComponentType(), array.length);
		for (int i = 0; i < array.length; i++){
			java.lang.reflect.Array.set(ret, i, array.values[i]);
		}
		return ret;
	}
}