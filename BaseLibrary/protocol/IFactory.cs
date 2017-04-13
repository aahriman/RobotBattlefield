using System;

namespace BaseLibrary.protocol {
    public interface IFactory<TReturn, TTransfered> {

        TReturn Deserialize(String s);
        Boolean IsDeserializable(String s);

        TReturn Transfer(TTransfered c);
        Boolean IsTransferable(TTransfered c);

        String Serialize(TTransfered c);
        Boolean IsSerializeable(TTransfered c);
    }

    public abstract class AFactory<TReturn, TTransfered> : IFactory<TReturn, TTransfered> {
        protected Cache<Object, TReturn> cache = new Cache<Object, TReturn>();
        protected Cache<Object, String> cacheForSerialize = new Cache<Object, String>();

        public TReturn Deserialize(String s) {
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

        public String Serialize(TTransfered c) {
            if (cacheForSerialize.IsCached(c) || IsSerializeable(c)) {
                return cacheForSerialize.GetCached();
            } else {
                if (c == null) {
                    throw new ArgumentNullException("Command cant be null");
                }
                throw new ArgumentException("Command type: '" + c.GetType() + "' is not transferable via " + GetType());
            }
        }

        public abstract Boolean IsDeserializable(String s);

        public abstract Boolean IsTransferable(TTransfered c);

        public abstract Boolean IsSerializeable(TTransfered c);
    }
}
