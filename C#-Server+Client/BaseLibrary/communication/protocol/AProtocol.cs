using System;
using BaseLibrary.communication.command;
using BaseLibrary.communication.command.handshake;

namespace BaseLibrary.communication.protocol {
    /// <summary>
    /// Abstract class for protocol.
    /// </summary>
    public abstract class AProtocol {
        protected Factories<ACommand.Sendable, ACommand> commandsFactory = new Factories<ACommand.Sendable, ACommand>();

	    protected AProtocol() {
            commandsFactory.RegisterCommand(ErrorCommand.FACTORY);
        }

        /// <summary>
        /// Command from protocol.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public virtual ACommand GetCommand(string s) {
			return (ACommand) commandsFactory.Deserialize(s);
        }

        public virtual string GetSendableCommand(ACommand s) {
            ACommand.Sendable commandSendable = commandsFactory.Transfer(s);
            if (commandSendable == null) {
                throw new ArgumentException("Protocol (" + this.GetType().Name + ") do not know command type: " + s.GetType().Name);
            }
            return commandSendable.Serialize();
        }
    }
}
