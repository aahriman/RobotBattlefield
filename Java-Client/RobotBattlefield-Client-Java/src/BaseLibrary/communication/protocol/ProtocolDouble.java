package BaseLibrary.communication.protocol;

public class ProtocolDouble {
	public static final ProtocolDouble ZERO = new ProtocolDouble(0.0);
	public static final ProtocolDouble _0 = ZERO;
	public static final ProtocolDouble _100 = new ProtocolDouble(100.0);

	public static final double MULTIPLE = 1000.0; // it have to be power of ten

	/**
	 * 
	 * @param s
	 * @return
	 * @throws NullPointerException if string is null
	 * @throws NumberFormatException if the string does not contain a parsable ProtocolDouble.
	 */
	public static ProtocolDouble Parse(String s) {
		int v = Integer.parseInt(s);
		return new ProtocolDouble(v);
	}
	
	
	/**
	 * @param s
	 * @return null if fail
	 */
	public static ProtocolDouble TryParse(String s) {
		ProtocolDouble result = null;
		try {
			int v = Integer.parseInt(s);
			result = new ProtocolDouble(v);
		} catch (NumberFormatException e) {
			result = null;
		}
		return result;
	}

	private int value;

	public ProtocolDouble(double d) {
		value = (int) (d * MULTIPLE);
	}

	public ProtocolDouble(ProtocolDouble d) {
		value = d.value;
	}

	private ProtocolDouble(int v) {
		value = v;
	}

	public double asDouble() {
		return this.value / MULTIPLE;
	}

	public int GetValue() {
		return value;
	}

	public String Serialize() {
		return String.valueOf(value);
	}

	@Override
	public String toString() {
		return "PD"+String.valueOf(value);
	}
}
