using System;

namespace BaseLibrary {
    public class Cache<TOriginalType, TCachedType> {
        private TCachedType cached;
        private TOriginalType original;
        private readonly bool useEquals;

        public Cache() : this(false) { }

        public Cache(bool useEquals) {
            this.useEquals = useEquals;
        }

        public void Cached(TOriginalType original, TCachedType cached) {
            this.original = original;
            this.cached = cached;
        }

        public TCachedType GetCached() {
            return cached;
        }

        public bool GetCached(TOriginalType probablyOriginal, out TCachedType cached) {
            if (IsCached(probablyOriginal)) {
                cached = this.cached;
                return true;
            } else {
                cached = default(TCachedType);
                return false;
            }
        }

        public bool IsCached(TOriginalType probablyOriginal) {
            bool retValue = false;
            if (!typeof(TOriginalType).IsValueType) {
                retValue = ReferenceEquals(probablyOriginal, original);
            }
            return retValue || (useEquals && 
                    (
                        (probablyOriginal == null && original == null) || 
                        (probablyOriginal != null && probablyOriginal.Equals(original))
                    )
                );
        }
    }
}
