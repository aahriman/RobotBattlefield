package BaseLibrary.utils;

import BaseLibrary.utils.Holder;

public class Cache<TOriginalType, TCachedType> {
	private TCachedType cached;
	private TOriginalType original;
	private final boolean useEquals;

	public Cache() {
		this(false);
	}

	public Cache(boolean useEquals) {
		this.useEquals = useEquals;
	}

	public void cached(TOriginalType original, TCachedType cached) {
		this.original = original;
		this.cached = cached;
	}

	public TCachedType GetCached() {
		return cached;
	}

	public boolean GetCached(TOriginalType probablyOriginal, Holder<TCachedType> cached) {
		if (IsCached(probablyOriginal)) {
			cached.value = this.cached;
			return true;
		} else {
			return false;
		}
	}

	public boolean IsCached(TOriginalType probablyOriginal) {
		boolean retValue = false;
		retValue = probablyOriginal == original; // compare ref
		return retValue || (useEquals && ((probablyOriginal == null && original == null)
				|| (probablyOriginal != null && probablyOriginal.equals(original))));
	}
}
