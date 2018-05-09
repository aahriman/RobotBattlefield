using System;
using BaseLibrary.utils;

namespace BaseLibrary.communication.protocol {
    /// <summary>
    /// Factory to <code>Deserialize</code> from string to <code>TReturn</code> and <code>serialize</code> from <code>TTransfered</code>, or <code>Tranfer</code> <code>TTransfered<code> to <code>TReturn</code>.
    /// </summary>
    /// <typeparam name="TReturn"></typeparam>
    /// <typeparam name="TTransfered"></typeparam>
    public interface IFactory<TReturn, TTransfered> {

        /// <summary>
        /// Convert string <code>s</code> to <code>TReturn</code> type.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        TReturn Deserialize(string s);

        /// <summary>
        /// Can deserialize <code>s</code> to <code>TReturn</code>
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        bool IsDeserializeable(string s);

        /// <summary>
        /// Convert <code>TTranfered</code> to <code>TReturn</code>
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        TReturn Transfer(TTransfered c);

        /// <summary>
        /// Can convert <code>TTranfered</code> to <code>TReturn</code>
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        bool IsTransferable(TTransfered c);

        /// <summary>
        /// Serialize <code>TTranfered</code> to <code>String</code> representation.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        string Serialize(TTransfered c);


        /// <summary>
        /// Can serialize <code>TTranfered</code> to <code>String</code> representation.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        bool IsSerializable(TTransfered c);
    }

    public abstract class AFactory<TReturn, TTransfered> : IFactory<TReturn, TTransfered> {
        protected Cache<object, TReturn> cache = new Cache<object, TReturn>();
        protected Cache<object, string> cacheForSerialize = new Cache<object, string>();

        /// <inheritdoc />
        public TReturn Deserialize(string s) {
            if (cache.IsCached(s) || IsDeserializeable(s)) {
                return cache.GetCached();
            } else {
                if (s == null) {
                    throw new ArgumentNullException("Argument s can not be null");
                }
                throw new ArgumentException("Argument s: '" + s + "' is not deseriazieble via " + GetType());
            }
        }

        /// <inheritdoc />
        public TReturn Transfer(TTransfered c) {
            if (cache.IsCached(c) || IsTransferable(c)) {
                return cache.GetCached();
            } else {
                if (c == null) {
                    throw new ArgumentNullException("Command can not be null");
                }
                throw new ArgumentException("Command type: '" + c.GetType() + "' is not transferable via " + GetType());
            }
        }

        /// <inheritdoc />
        public string Serialize(TTransfered c) {
            if (cacheForSerialize.IsCached(c) || IsSerializable(c)) {
                return cacheForSerialize.GetCached();
            } else {
                if (c == null) {
                    throw new ArgumentNullException("Command can not be null");
                }
                throw new ArgumentException("Command type: '" + c.GetType() + "' is not transferable via " + GetType());
            }
        }

        /// <inheritdoc />
        public abstract bool IsDeserializeable(string s);

        /// <inheritdoc />
        public abstract bool IsTransferable(TTransfered c);

        /// <inheritdoc />
        public abstract bool IsSerializable(TTransfered c);
    }
}
