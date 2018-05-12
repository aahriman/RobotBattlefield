using System.Collections.Generic;
using BaseLibrary.communication.protocol;

namespace BaseLibrary.communication.command.handshake {
    public class ErrorCommand : AHandShakeCommand, ACommand.Sendable {

        private static readonly List<ISubCommandFactory> SUB_COMMAND_FACTORIES = new List<ISubCommandFactory>();

        public static int RegisterSubCommandFactory(ISubCommandFactory subCommandFactory) {
            int position = SUB_COMMAND_FACTORIES.Count;
            SUB_COMMAND_FACTORIES.Add(subCommandFactory);
            return position;
        }

        public static readonly IFactory<ACommand.Sendable, ACommand> FACTORY = new CommandFactory();
        private sealed class CommandFactory : ACommandFactory {

            internal CommandFactory() : base() { }

            public override bool IsDeserializeable(string s) {
                if (s != null && s.StartsWith("ERROR")) {
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

        public string MESSAGE { get; private set; }

        public ErrorCommand(string message)
            : base() {
            MESSAGE = message;
        }

        public string Serialize() {
            return "ERROR "+ MESSAGE;
        }
    }
}
