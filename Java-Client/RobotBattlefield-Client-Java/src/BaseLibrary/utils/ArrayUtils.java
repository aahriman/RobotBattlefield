package BaseLibrary.utils;

import java.lang.reflect.Array;

public final class ArrayUtils {
	private ArrayUtils() {}
	@SuppressWarnings("unchecked")
	public static  <E> E[] DeepCopy(E [] array){
        final E[] a = (E[]) Array.newInstance(array.getClass().getComponentType(), array.length);
		
        for(int i = 0; i < array.length; i++) {
        	a[i] = array[i];
        }
		return a;
	}
}
