using CommunicationLibrary.command;

namespace CommunicationLibrary.protocol {
    public abstract class AProtocol {
        protected CommandsFactory comandsFactory = new CommandsFactory();

	    protected AProtocol() {
            comandsFactory.registerCommand(ErrorCommand.FACTORY);
        }

        public virtual ACommand GetCommand(string s) {
			return (ACommand) comandsFactory.Deserialize(s);
        }

        public virtual ACommand GetCommand(ACommand s) {
            return (ACommand) comandsFactory.Transfer(s);
        }

        public virtual ACommand.Sendable GetSendableCommand(string s) {
            return comandsFactory.Deserialize(s);
        }

        public virtual ACommand.Sendable GetSendableCommand(ACommand s) {
            return comandsFactory.Transfer(s);
        }
    }
}
