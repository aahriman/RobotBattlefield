using System;
using System.Linq;
using BaseLibrary.config;
using BaseLibrary.protocol;
using BaseLibrary.visitors;

namespace BaseLibrary.command.handshake {
    public class OllehCommand : AHandShakeCommand, ACommand.Sendable {
        public static readonly IFactory<ACommand.Sendable, ACommand> FACTORY = new OllehCommandFactory();
        private sealed class OllehCommandFactory : ACommandFactory {
            internal OllehCommandFactory() : base() { }

            public override bool IsDeserializable(string s) {
                if (s.StartsWith("OLLEH "+ GameProperties.APP_NAME)) {
                    var protocol = s.Substring(("OLLEH "+GameProperties.APP_NAME).Length).Trim();
                    if (!protocol.Contains(' ')) {
                        cache.Cached(s, new OllehCommand(protocol));
                        return true;
                    }
                }
                return false;
            }

            public override bool IsTransferable(ACommand c) {
                if (c is OllehCommand) {
                    var c1 = (OllehCommand)c;
                    cache.Cached(c, new OllehCommand(c1.PROTOCOL));
                    return true;
                }
                return false;
            }
        }

        public string PROTOCOL { get; private set; }

        public OllehCommand(string protocol)
            : base() {
            if (protocol.Contains(' ')) {
                throw new ArgumentException("Invalid protocol.");
            }

            PROTOCOL = protocol;
        }

        public string Serialize() {
            return "OLLEH " + GameProperties.APP_NAME + PROTOCOL;
        }

        public override void accept(ICommandVisitor accepter) {
            throw new NotImplementedException();
        }

        public override Output accept<Output>(ICommandVisitor<Output> accepter) {
            throw new NotImplementedException();
        }

        public override Output accept<Output, Input>(ICommandVisitor<Output, Input> accepter, Input input) {
            throw new NotImplementedException();
        }
    }
}
