package BaseLibrary.communication.protocol;

import java.lang.reflect.Field;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

import BaseLibrary.communication.command.ACommand;
import BaseLibrary.communication.command.handshake.ErrorCommand;
import BaseLibrary.utils.Holder;
import BaseLibrary.utils.ModUtils;

public abstract class AProtocol {

	protected static HashMap<String, Class<?>> classByObjectName = new HashMap<>();

	private static HashMap<Class<?>, Field[]> fieldsToDeserializeByClass = new HashMap<>();

	protected AProtocol() {
		ModUtils.loadClassFromPackage(ACommand.class.getPackage().getName()); // load all commands
	}

	public static void registerForDeserialize(String objectName, Class<?> objectClass) {
		classByObjectName.put(objectName, objectClass);
	}

	private static String firstLetterUppercase(String s) {
		String ret = String.valueOf(Character.toUpperCase(s.charAt(0)));
		if (s.length() > 1) {
			return ret + s.substring(1, s.length());
		} else {
			return ret;
		}
	}

	private static boolean getMethodExist(Class<?> clazz, Field f) {
		try {
			return clazz.getMethod("get" + firstLetterUppercase(f.getName())) != null;
		} catch (NoSuchMethodException | SecurityException e) {
			return false;
		}
	}

	private static Field[] getFields(Object object) {
		if (!fieldsToDeserializeByClass.containsKey(object.getClass())) {
			Field[] fields = object.getClass().getDeclaredFields();

			List<Field> fieldsToDeserialize = new ArrayList<>();

			for (Field f : fields) {
				if (getMethodExist(object.getClass(), f)) {
					fieldsToDeserialize.add(f);
				}
			}
			Field[] ret = fieldsToDeserialize.toArray(new Field[0]);
			setAccess(ret);
			fieldsToDeserializeByClass.put(object.getClass(), ret);

		}
		return fieldsToDeserializeByClass.get(object.getClass());
	}
	
	private static void setAccess(Field... fields) {
		for (Field f : fields) {
			f.setAccessible(true);
		}
	}

	public ACommand getCommand(String s) {
		if (ErrorCommand.FACTORY.isDeserializeable(s, this)) {
			return ErrorCommand.FACTORY.deserialize(s, this);
		}
		return (ACommand) deserializeObject(s);
	}

	public String serialize(ACommand command) {
		Field[] fields = getFields(command);

		return serializeObject(command.getCommandName(), command, fields);
	}

	public String serialize(IInnerObject innerObject) {
		Field[] fields = getFields(innerObject);

		return serializeObject(innerObject.getCommandName(), innerObject, fields);
	}

	protected String serialize(Object object, Field field) {

		Class<?> fieldType = field.getType();
		Holder<Array> holder = new Holder<>();
		try {
			if (field.get(object) == null) {
				return serializeNull(field.getName());
			} else if (fieldType == int.class) {
				return serializeInt(field.getInt(object), field.getName());
			} else if (fieldType == char.class) {
				return serializeChar(field.getChar(object), field.getName());
			} else if (fieldType == boolean.class) {
				return serializeBoolean(field.getBoolean(object), field.getName());
			} else if (fieldType == double.class) {
				return serializeProtocolDouble(new ProtocolDouble(field.getDouble(object)), field.getName());
			} else if (field.get(object) instanceof IInnerObject) {
				serializeInnerObject((IInnerObject) field.get(object), field.getName());
			} else if (fieldType == Integer.class) {
				return serializeInt((Integer) field.get(object), field.getName());
			} else if (fieldType == Character.class) {
				return serializeChar((Character) field.get(object), field.getName());
			} else if (fieldType == Boolean.class) {
				return serializeBoolean((Boolean) field.get(object), field.getName());
			} else if (fieldType == Double.class) {
				return serializeProtocolDouble(new ProtocolDouble((Double) field.get(object)), field.getName());
			} else if (fieldType == ProtocolDouble.class) {
				return serializeProtocolDouble((ProtocolDouble) field.get(object), field.getName());
			} else if (fieldType == String.class) {
				return serializeString((String) field.get(object), field.getName());
			} else if (Array.fromObjectArray(field.get(object), holder)) {
				return serializeArray(holder.value, null);
			} else if (Enum.class.isAssignableFrom(fieldType)) {
				return serializeString(field.get(object).toString(), field.getName());
			} else {
				throw new RuntimeException("Cannot serialize " + field.getName() + " type " + fieldType.getTypeName());
			}
		} catch (IllegalArgumentException | IllegalAccessException e) {
			e.printStackTrace();
		}
		return null;
	}

	protected abstract String serializeNull(String fieldName);

	protected abstract String serializeInt(int i, String fieldName);

	protected abstract String serializeChar(char i, String fieldName);

	protected abstract String serializeBoolean(boolean i, String fieldName);

	protected abstract String serializeProtocolDouble(ProtocolDouble i, String fieldName);

	protected abstract String serializeString(String i, String fieldName);

	public abstract String serializeArray(Array array, String fieldName);

	protected abstract String serializeObject(String objectName, Object object, Field... fields);

	public abstract String serializeInnerObject(IInnerObject object, String fieldName);

	public abstract Array deserializeArray(String serializedArray, Class<?> arrayType);

	public abstract Object deserializeObject(String serializedObject);

	protected abstract Integer deserializeInt(String integer);

	protected abstract Character deserializeChar(String character);

	protected abstract Boolean deserializeBoolean(String i);

	protected abstract ProtocolDouble deserializeProtocolDouble(String i);

	protected abstract String deserializeString(String i);

	public abstract IInnerObject deserializeInnerObject(String serializedInnerObject);

	protected String serializeObjectValue(Object object) {
		if (object == null) {
			return serializeNull(null);
		}

		Class<?> objectType = object.getClass();
		Holder<Array> holder = new Holder<>();

		if (objectType == Integer.class) {
			return serializeInt((Integer) object, null);
		} else if (objectType == Character.class) {
			return serializeChar((Character) object, null);
		} else if (objectType == Boolean.class) {
			return serializeBoolean((Boolean) object, null);
		} else if (objectType == Double.class) {
			return serializeProtocolDouble(new ProtocolDouble((Double) object), null);
		} else if (objectType == ProtocolDouble.class) {
			return serializeProtocolDouble(new ProtocolDouble((Double) object), null);
		} else if (objectType == String.class) {
			return serializeString((String) object, null);
		} else if (object instanceof IInnerObject) {
			return serialize((IInnerObject) object);
		} else if (Array.fromObjectArray(object, holder)) {
			return serializeArray(holder.value, null);
		} else {
			throw new RuntimeException("Cannot serialize " + objectType.getName());
		}
	}

	@SuppressWarnings({ "unchecked", "rawtypes" })
	protected Object deserializeValue(Class<?> objectType, String value) {
		if (objectType == Integer.class || objectType == int.class) {
			return deserializeInt(value);
		} else if (objectType == Character.class || objectType == char.class) {
			return deserializeChar(value);
		} else if (objectType == Boolean.class || objectType == boolean.class) {
			return deserializeBoolean(value);
		} else if (objectType == Double.class || objectType == double.class) {
			return deserializeProtocolDouble(value).asDouble();
		} else if (objectType == ProtocolDouble.class) {
			return deserializeProtocolDouble(value);
		} else if (objectType == String.class) {
			return deserializeString(value);
		} else if (IInnerObject.class.isAssignableFrom(objectType)) {
			return deserializeInnerObject(value);
		} else if (Array.fromObjectArray(objectType)) {
			return deserializeArray(value, objectType.getComponentType());
		} else if (objectType.isEnum()) {
			return Enum.valueOf((Class<Enum>) objectType, value);
		} else {
			throw new RuntimeException("Cannot deserialize " + objectType.getName());
		}
	}

	public static class Array {
		public final int length;

		public final Object[] values;

		public Array(Object[] array) {
			this.length = array.length;
			values = array;
		}

		public static boolean fromObjectArray(Object object, Holder<Array> holder) {

			if (object == null) {
				return false;
			}

			Class<?> oClass = object.getClass();
			if (oClass == java.lang.reflect.Array.class) {
				holder.value = Array.fromArray((java.lang.reflect.Array) object);
				return true;
			} else if (oClass == int[].class) {
				holder.value = Array.fromArray((int[]) object);
				return true;
			} else if (oClass == double[].class) {
				holder.value = Array.fromArray((double[]) object);
				return true;
			} else if (oClass == char[].class) {
				holder.value = Array.fromArray((char[]) object);
				return true;
			} else if (oClass == boolean[].class) {
				holder.value = Array.fromArray((boolean[]) object);
				return true;
			} else if (object instanceof Object[]) {
				holder.value = new Array((Object[]) object);
				return true;
			}
			return false;
		}

		public static boolean fromObjectArray(Class<?> oClass) {

			if (oClass == java.lang.reflect.Array.class) {
				return true;
			} else if (oClass == int[].class) {
				return true;
			} else if (oClass == double[].class) {
				return true;
			} else if (oClass == char[].class) {
				return true;
			} else if (oClass == boolean[].class) {
				return true;
			} else if (IInnerObject[].class.isAssignableFrom(oClass)) {
				return true;
			}
			return false;
		}

		public static Array fromArray(java.lang.reflect.Array array) {
			int length = java.lang.reflect.Array.getLength(array);
			Object[] o = new Object[length];
			for (int i = 0; i < length; i++) {
				o[i] = java.lang.reflect.Array.get(array, i);
			}

			return new Array(o);
		}

		public static Array fromArray(int[] array) {
			int length = array.length;
			Integer[] o = new Integer[length];
			for (int i = 0; i < length; i++) {
				o[i] = array[i];
			}

			return new Array(o);
		}

		public static Array fromArray(double[] array) {
			int length = array.length;
			Double[] o = new Double[length];
			for (int i = 0; i < length; i++) {
				o[i] = array[i];
			}

			return new Array(o);
		}

		public static Array fromArray(char[] array) {
			int length = array.length;
			Character[] o = new Character[length];
			for (int i = 0; i < length; i++) {
				o[i] = array[i];
			}

			return new Array(o);
		}

		public static Array fromArray(boolean[] array) {
			int length = array.length;
			Boolean[] o = new Boolean[length];
			for (int i = 0; i < length; i++) {
				o[i] = array[i];
			}

			return new Array(o);
		}
	}
}
