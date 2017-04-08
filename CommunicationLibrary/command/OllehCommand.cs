using System;
using System.Linq;

namespace CommunicationLibrary.command {
    public class OllehCommand : ACommand, ACommand.Sendable {
        public static readonly ICommandFactory FACTORY = new OllehCommandFactory();
        private sealed class OllehCommandFactory : ACommandFactory {
            internal OllehCommandFactory() : base() { }

            public override Boolean IsDeserializable(String s) {
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

        public String PROTOCOL { get; private set; }

        public OllehCommand(String protocol)
            : base() {
            if (protocol.Contains(' ')) {
                throw new ArgumentException("Invalid protocol.");
            }

            PROTOCOL = protocol;
        }

        public string serialize() {
            return "OLLEH " + GameProperties.APP_NAME + PROTOCOL;
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
