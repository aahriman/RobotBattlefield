package BaseLibrary.communication.protocol;

import BaseLibrary.utils.Cache;

public interface IFactory<TReturn, TTransfered> {

    public TReturn deserialize(String s, AProtocol protocol);
    public boolean isDeserializeable(String s, AProtocol protocol);

    public TReturn transfer(TTransfered c);
    public boolean isTransferable(TTransfered c);

    public String serialize(TTransfered c, AProtocol protocol);
    public boolean isSerializeable(TTransfered c, AProtocol protocol);
    
    public abstract class AFactory<TReturn, TTransfered> implements IFactory<TReturn, TTransfered> {
        protected Cache<Object, TReturn> cache = new Cache<Object, TReturn>();
        protected Cache<Object, String> cacheForSerialize = new Cache<Object, String>();

        @Override
        public TReturn deserialize(String s, AProtocol protocol) {
            if (cache.IsCached(s) || isDeserializeable(s, protocol)) {
                return cache.GetCached();
            } else {
                if (s == null) {
                    throw new IllegalArgumentException("Argument s cant be null");
                }
                throw new IllegalArgumentException("Argument s: '" + s + "' is not deseriazieble via " + getClass().getName());
            }
        }

        @Override
        public TReturn transfer(TTransfered c) {
            if (cache.IsCached(c) || isTransferable(c)) {
                return cache.GetCached();
            } else {
                if (c == null) {
                    throw new IllegalArgumentException("Command cant be null");
                }
                throw new IllegalArgumentException("Command type: '" + c.getClass().getName() + "' is not transferable via " + getClass().getName());
            }
        }

        @Override
        public String serialize(TTransfered c, AProtocol protocol) {
            if (cacheForSerialize.IsCached(c) || isSerializeable(c, protocol)) {
                return cacheForSerialize.GetCached();
            } else {
                if (c == null) {
                    throw new IllegalArgumentException("Command cant be null");
                }
                throw new IllegalArgumentException("Command type: '" + c.getClass().getName() + "' is not transferable via " + getClass().getName());
            }
        }
    }
}
