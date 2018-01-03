using System;
using BaseLibrary.command;
using BaseLibrary.command.common;
using BaseLibrary.command.handshake;

namespace BaseLibrary.protocol {
    /// <summary>
    /// Abstract class for protocol.
    /// </summary>
    public abstract class AProtocol {
        protected Factories<ACommand.Sendable, ACommand> comandsFactory = new Factories<ACommand.Sendable, ACommand>();

	    protected AProtocol() {
            comandsFactory.RegisterCommand(ErrorCommand.FACTORY);
        }

        /// <summary>
        /// Command from protocol.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public virtual ACommand GetCommand(string s) {
			return (ACommand) comandsFactory.Deserialize(s);
        }

        public virtual string GetSendableCommand(ACommand s) {
            ACommand.Sendable commandSendable = comandsFactory.Transfer(s);
            if (commandSendable == null) {
                throw new ArgumentException("Protocol (" + this.GetType().Name + ") do not know command type: " + s.GetType().Name);
            }
            return commandSendable.Serialize();
        }
    }
}
