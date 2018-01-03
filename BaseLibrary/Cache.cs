using System;

namespace BaseLibrary {
    /// <summary>
    /// Cache to store some object of type <code>TCacheType</code>. Only one object can be store at the time.
    /// </summary>
    /// <typeparam name="TOriginalType"></typeparam>
    /// <typeparam name="TCachedType"></typeparam>
    public class Cache<TOriginalType, TCachedType> {
        private TCachedType cached;
        private TOriginalType original;
        private readonly bool useEquals;

        /// <inheritdoc />
        /// <summary>
        /// Construct cache instance. With useEquals false.
        /// </summary>
        public Cache() : this(false) { }

        /// <summary>
        /// Construct cache instance.
        /// </summary>
        /// <param name="useEquals">If reference are not equal but Equals return true and then you can return cached object.</param>
        public Cache(bool useEquals) {
            this.useEquals = useEquals;
        }

        /// <summary>
        /// Store cached object witch belongs to original
        /// </summary>
        /// <param name="original"></param>
        /// <param name="cached"></param>
        public void Cached(TOriginalType original, TCachedType cached) {
            this.original = original;
            this.cached = cached;
        }

        /// <summary>
        /// Return cached element.
        /// </summary>
        /// <returns></returns>
        public TCachedType GetCached() {
            return cached;
        }

        /// <summary>
        /// Return cache element (stored in <code>cached</code>) only if <code>probablyOriginal</code> is equals to <code>original</code>. Equals mean by reference or if useEquals is true then also by its Equals method.
        /// </summary>
        /// <param name="probablyOriginal"></param>
        /// <param name="cached"></param>
        /// <returns>true - if cached contains valid data, false - otherwise</returns>
        public bool TryGetCached(TOriginalType probablyOriginal, out TCachedType cached) {
            if (IsCached(probablyOriginal)) {
                cached = this.cached;
                return true;
            } else {
                cached = default(TCachedType);
                return false;
            }
        }

        /// <summary>
        /// Check if cached object belong to <code>probablyOriginal</code>.
        /// </summary>
        /// <param name="probablyOriginal"></param>
        /// <returns></returns>
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
