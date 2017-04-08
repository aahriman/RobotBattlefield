namespace CommunicationLibrary.utils {
    public class ArrayUtils {

        public static E[] DeepCopy<E>(E[] input) {
	        if (input == null) {
		        return null;
	        }
            E[] output = new E[input.Length];
            for (int i = 0; i < input.Length; i++) {
                output[i] = input[i];
            }
            return output;
        }

		public static E[] FromParams<E>(params E[] array) {
			return array;
		}
    }
}
