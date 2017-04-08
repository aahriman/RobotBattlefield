using System;

namespace CommunicationLibrary.command {
    public interface ICommandFactory {

        ACommand.Sendable Deserialize(String s);
        Boolean IsDeserializable(String s);

        ACommand.Sendable Transfer(ACommand c);
        Boolean IsTransferable(ACommand c);
    }

    public abstract class ACommandFactory : ICommandFactory {
        protected Cache<Object, ACommand.Sendable> cache = new Cache<Object, ACommand.Sendable>();

        public ACommand.Sendable Deserialize(String s) {
            if (cache.IsCached(s) || IsDeserializable(s)) { // if it wanst cached after call IsDeserializable it is cached
                return cache.GetCached();
            } else {
                if (s == null) {
                    throw new ArgumentNullException("Argument s cant be null");
                }
                throw new ArgumentException("Argument s: '"+s+"' is not deseriazieble via " + GetType());
            }
        }

        public ACommand.Sendable Transfer(ACommand c) {
            if (cache.IsCached(c) || IsTransferable(c)) { // if it wanst cached after call IsDeserializable it is cached
                return cache.GetCached();
            } else {
				if (c == null) {
					throw new ArgumentNullException("Command cant be null");
				}
                throw new ArgumentException("Command type: '"+c.GetType()+"' is not transferable via " + GetType());
            }
        }

        public abstract Boolean IsDeserializable(String s);

		public abstract Boolean IsTransferable(ACommand c);
    }
}
