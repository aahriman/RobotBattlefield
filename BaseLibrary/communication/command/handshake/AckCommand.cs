using System.Collections.Generic;
using BaseLibrary.communication.protocol;

namespace BaseLibrary.communication.command.handshake {

    public sealed class AckCommand : AHandShakeCommand, ACommand.Sendable {

        private static readonly List<ISubCommandFactory> subCommandFactories = new List<ISubCommandFactory>();

        public static int RegisterSubCommandFactory(ISubCommandFactory subCommandFactory) {
            int position = subCommandFactories.Count;
            subCommandFactories.Add(subCommandFactory);
            return position;
        }

        public static readonly IFactory<ACommand.Sendable, ACommand> FACTORY = new CommandFactory();
        private sealed class CommandFactory : ACommandFactory {
            internal CommandFactory() : base() { }

            public override bool IsDeserializeable(string s) {
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

        string Sendable.Serialize() {
            return "ACK";
        }
    }
}
