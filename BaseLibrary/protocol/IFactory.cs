using System;

namespace BaseLibrary.protocol {
    public interface IFactory<TReturn, TTransfered> {

        TReturn Deserialize(string s);
        bool IsDeserializable(string s);

        TReturn Transfer(TTransfered c);
        bool IsTransferable(TTransfered c);

        string Serialize(TTransfered c);
        bool IsSerializable(TTransfered c);
    }

    public abstract class AFactory<TReturn, TTransfered> : IFactory<TReturn, TTransfered> {
        protected Cache<object, TReturn> cache = new Cache<object, TReturn>();
        protected Cache<object, string> cacheForSerialize = new Cache<object, string>();

        public TReturn Deserialize(string s) {
            if (cache.IsCached(s) || IsDeserializable(s)) {
                return cache.GetCached();
            } else {
                if (s == null) {
                    throw new ArgumentNullException("Argument s cant be null");
                }
                throw new ArgumentException("Argument s: '" + s + "' is not deseriazieble via " + GetType());
            }
        }

        public TReturn Transfer(TTransfered c) {
            if (cache.IsCached(c) || IsTransferable(c)) {
                return cache.GetCached();
            } else {
                if (c == null) {
                    throw new ArgumentNullException("Command cant be null");
                }
                throw new ArgumentException("Command type: '" + c.GetType() + "' is not transferable via " + GetType());
            }
        }

        public string Serialize(TTransfered c) {
            if (cacheForSerialize.IsCached(c) || IsSerializable(c)) {
                return cacheForSerialize.GetCached();
            } else {
                if (c == null) {
                    throw new ArgumentNullException("Command cant be null");
                }
                throw new ArgumentException("Command type: '" + c.GetType() + "' is not transferable via " + GetType());
            }
        }

        public abstract bool IsDeserializable(string s);

        public abstract bool IsTransferable(TTransfered c);

        public abstract bool IsSerializable(TTransfered c);
    }
}
