using BaseLibrary.communication.command.repairman;
using BaseLibrary.communication.protocol;
using BaseLibrary.utils;

namespace BaseLibrary.communication.command.v1._0.repairman {
    public class RepairCommandV1_0 : RepairCommand, ACommand.Sendable {

        private const string NAME = "REPAIR";
        public static readonly IFactory<ACommand.Sendable, ACommand> FACTORY = new CommandFactory();
        private sealed class CommandFactory : ACommandFactory {
            internal CommandFactory() : base() { }

            public override bool IsDeserializeable(string s) {
                string[] rest;
                if (ProtocolV1_0Utils.GetParams(s, NAME, out rest) && rest.Length == 1) {
                    int maxDistance;
                    if (int.TryParse(rest[0], out maxDistance)) {
                        cache.Cached(s, new RepairCommandV1_0(maxDistance));
                        return true;
                    }
                }
                return false;
            }

            public override bool IsTransferable(ACommand c) {
                RepairCommand command = c as RepairCommand;
                if (command != null) {
                    cache.Cached(command, new RepairCommandV1_0(command.MAX_DISTANCE));
                    return true;
                }
                return false;
            }
        }


        public RepairCommandV1_0(int maxDistance) : base(maxDistance){
        }

        public string Serialize() {
            return ProtocolV1_0Utils.SerializeParams(NAME, MAX_DISTANCE);
        }
    }
}
