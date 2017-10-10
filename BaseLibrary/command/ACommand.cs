using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BaseLibrary.visitors;

namespace BaseLibrary.command {
    public static class ACommandExtensions {
        public static async Task SendAsync(this ACommand.Sendable instance, SuperNetworkStream s) {
            if (instance == null) {
                throw new ArgumentNullException("Instance can not be null.");
            }
            await s.WriteLineAsync(instance.Serialize());
        }

        public static string ToString(this ACommand.Sendable instance) {
            if (instance == null) {
                throw new ArgumentNullException("Instance can not be null.");
            }
            return instance.Serialize();
        }
    }

    public abstract class ACommand {
        /// <summary>
        /// If someone implements ACommand.Sendable it have to be child of ACommand
        /// </summary>
        public interface Sendable {
            String Serialize();
        }

        protected bool pending = true;

        public Object[] MORE { get; protected set; }

        protected ACommand() {}

        public abstract void accept(ICommandVisitor accepter);

        public abstract Output accept<Output>(ICommandVisitor<Output> accepter);

        public abstract Output accept<Output, Input>(ICommandVisitor<Output, Input> accepter, Input input);

        public override string ToString() {
            ACommand.Sendable sendable = this as ACommand.Sendable;
            if (sendable != null) {
                return sendable.Serialize();
            } else {
                StringBuilder sb = new StringBuilder();
                sb.Append(base.ToString());
                sb.Append("[");
                foreach (var propertyInfo in this.GetType().GetProperties()) {
                    sb.Append(propertyInfo.Name);
                    sb.Append(":");
                    sb.Append(propertyInfo.GetValue(this, null));
                    sb.Append(";");
                }

                foreach (var fieldInfo in this.GetType().GetFields()) {
                    sb.Append(fieldInfo.Name);
                    sb.Append(":");
                    sb.Append(fieldInfo.GetValue(this));
                    sb.Append(";");
                }
                sb.Append("]");

                return sb.ToString();
            }
        }
        protected String[] SerializeMore(IEnumerable<ISubCommandFactory> subCommandFactories) {
            String[] serializedMore = new string[MORE.Length];
            for (int i = 0; i < MORE.Length; i++) {
                foreach (var factory in subCommandFactories) {
                    if (factory.Serialize(MORE[i], out serializedMore[i])) {
                        break;
                    }

                }
            }
            return serializedMore;
        }

        protected void DeserializeMore(String[] serializedMore, Object[] more, IEnumerable<ISubCommandFactory> subCommandFactories) {
            foreach (var moreString in serializedMore) {
                foreach (var subCommandFactory in subCommandFactories) {
                    if (subCommandFactory.Deserialize(moreString, more)) {
                        break;
                    }
                }
            }
        }
    }
}
