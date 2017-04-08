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

        public Object[] MORE { get; protected set; }

        protected ACommand() { }

        public abstract void accept(ICommandVisitor accepter);

        public abstract Output accept<Output>(ICommandVisitor<Output> accepter);

        public abstract Output accept<Output,Input>(ICommandVisitor<Output,Input> accepter, params Input[] inputs);

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
