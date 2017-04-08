using BaseLibrary.command;

namespace BaseLibrary.protocol {
    public abstract class AProtocol {
        protected Factories<ACommand.Sendable, ACommand> comandsFactory = new Factories<ACommand.Sendable, ACommand>();

	    protected AProtocol() {
            comandsFactory.RegisterCommand(ErrorCommand.FACTORY);
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
