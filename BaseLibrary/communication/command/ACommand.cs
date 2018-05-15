using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BaseLibrary.communication.command {
    public static class ACommandExtensions {
        public static async Task SendAsync(this ACommand.Sendable instance, NetworkStream s) {
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
            string Serialize();
        }

        /// <summary>
        /// true if request do not have valid data.
        /// </summary>
        protected bool pending = true;

        /// <summary>
        /// For send mod extensions.
        /// </summary>
        public Object[] MORE { get; protected set; }

        protected ACommand() {
            
        }

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
        protected string[] SerializeMore(IEnumerable<ISubCommandFactory> subCommandFactories) {
            string[] serializedMore = new string[MORE.Length];
            for (int i = 0; i < MORE.Length; i++) {
                foreach (var factory in subCommandFactories) {
                    if (factory.Serialize(MORE[i], out serializedMore[i])) {
                        break;
                    }

                }
            }
            return serializedMore;
        }

        protected void DeserializeMore(string[] serializedMore, object[] more, IEnumerable<ISubCommandFactory> subCommandFactories) {
            Console.WriteLine($"{GetType().Name} Deserialize - more.Length:{more.Length}");
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
