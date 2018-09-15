package BaseLibrary.utils;

public final class StringUtils {
	private StringUtils() {
	}

	public static boolean GetRestOfString(String str, String start, String end, Holder<String> rest) {
		if (str.startsWith(start) && str.endsWith(end)) {
			rest.value = str.substring(start.length(), str.length() - end.length() - start.length());
			return true;
		} else {
			return false;
		}
	}

	public static boolean GetRestOfStringSplited(String str, String start, String end, Holder<String[]> rest,
			char... separator) {
		Holder<String> restStr = new Holder<>();

		if (GetRestOfString(str, start, end, restStr)) {
			rest.value = restStr.value.split(new String(separator));
			return true;
		} else {
			rest = null;
			return false;
		}
	}
}
