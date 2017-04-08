using System;
using BaseLibrary.protocol;

namespace BaseLibrary.command.v1._0 {
    public class WaitCommandV1_0 : WaitCommand, ACommand.Sendable {
        public static readonly IFactory<ACommand.Sendable, ACommand> FACTORY = new CommandFactory();
        private sealed class CommandFactory : ACommandFactory {
            internal CommandFactory() : base() { }

            public override Boolean IsDeserializable(String s) {
                s = s.Trim();
                if (s.StartsWith("WAIT()")) {
                    cache.Cached(s, new WaitCommandV1_0());
                    return true;
                }
                return false;
            }

            public override bool IsTransferable(ACommand c) {
                if (c is WaitCommand) {
                    var c1 = (WaitCommand)c;
                    cache.Cached(c, new WaitCommandV1_0());
                    return true;
                }
                return false;
            }
        }

        public WaitCommandV1_0() : base() { }

        public string Serialize() {
            return "WAIT()";
        }
    }
}
