using System;

namespace CommunicationLibrary.command {
    public class ErrorCommand : ACommand, ACommand.Sendable {
		public static readonly ICommandFactory FACTORY = new CommandFactory();
        private sealed class CommandFactory : ACommandFactory {
			internal CommandFactory() : base() { }

            public override Boolean IsDeserializable(String s) {
                if (s.StartsWith("ERROR")) {
                    var message = s.Substring(("ERROR").Length).Trim();
                    cache.Cached(s, new ErrorCommand(message));
                    return true;
                }
                return false;
            }

            public override bool IsTransferable(ACommand c) {
                if (c is ErrorCommand) {
                    var c1 = (ErrorCommand)c;
                    cache.Cached(c, new ErrorCommand(c1.MESSAGE));
                    return true;
                }
                return false;
            }
        }

        public String MESSAGE { get; private set; }

        public ErrorCommand(String message)
            : base() {
            MESSAGE = message;
        }

        public string serialize() {
            return "ERROR "+ MESSAGE;
        }

        public override void accept(AVisitorCommand accepter) {
            throw new NotImplementedException();
        }

        public override Output accept<Output>(AVisitorCommand<Output> accepter) {
            throw new NotImplementedException();
        }

        public override Output accept<Output, Input>(AVisitorCommand<Output, Input> accepter, params Input[] inputs) {
            throw new NotImplementedException();
        }

    }
}
