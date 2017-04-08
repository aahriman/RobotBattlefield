using System;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationLibrary.command {
    public static class ACommandExtensions {
        public static async Task SendAsync(this ACommand.Sendable instance, SuperNetworkStream s) {
			if (instance == null) {
				throw new ArgumentNullException("Instance can not be null.");
			}
            await s.WriteLineAsync(instance.serialize());
        }

        public static string ToString(this ACommand.Sendable instance) {
			if (instance == null) {
				throw new ArgumentNullException("Instance can not be null.");
			}
            return instance.serialize();
        }
    }

    public abstract class ACommand {

        /// <summary>
        /// If someone implements ACommand.Sendable it have to be child of ACommand
        /// </summary>
        public interface Sendable {
            String serialize();

        }
        protected ACommand() { }

        public abstract void accept(AVisitorCommand accepter);

        public abstract Output accept<Output>(AVisitorCommand<Output> accepter);

        public abstract Output accept<Output,Input>(AVisitorCommand<Output,Input> accepter, params Input[] inputs);

	    public override string ToString() {
		    ACommand.Sendable sendable = this as ACommand.Sendable;
		    if (sendable != null) {
			    return sendable.serialize();
		    } else {
				StringBuilder sb = new StringBuilder();
			    sb.Append(base.ToString());
				sb.Append("[");
				foreach (var propertyInfo in this.GetType().GetProperties()) {
					sb.Append(propertyInfo.Name);
					sb.Append(":");
					sb.Append(propertyInfo.GetValue(this));
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
    }
}
