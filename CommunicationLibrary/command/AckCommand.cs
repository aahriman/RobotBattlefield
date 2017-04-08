using System;

namespace CommunicationLibrary.command {
    public class AckCommand : ACommand, ACommand.Sendable {
        public static readonly ICommandFactory FACTORY = new CommandFactory();
        private sealed class CommandFactory : ACommandFactory {
            internal CommandFactory() : base() { }

            public override Boolean IsDeserializable(String s) {
                s = s.Trim();
                if (s.Equals("ACK")) {
                    cache.Cached(s, new AckCommand());
                    return true;
                }
                return false;
            }

            public override bool IsTransferable(ACommand c) {
                if (c is AckCommand) {
                    cache.Cached(c, new AckCommand());
                    return true;
                }
                return false;
            }
        }

        public AckCommand() : base() { }

        string Sendable.serialize() {
            return "ACK";
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
