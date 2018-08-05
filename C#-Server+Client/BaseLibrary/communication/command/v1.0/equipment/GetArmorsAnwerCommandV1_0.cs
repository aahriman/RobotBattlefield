using BaseLibrary.communication.command.equipment;
using BaseLibrary.communication.protocol;
using BaseLibrary.utils;

namespace BaseLibrary.communication.command.v1._0.equipment {
	public class GetArmorsAnswerCommandV10 : GetArmorsAnswerCommand, ACommand.Sendable {
		private const string NAME = "ARMOR_ANSWER";
        public static readonly IFactory<ACommand.Sendable, ACommand> FACTORY = new CommandFactory();
        private sealed class CommandFactory : ACommandFactory {
            public override bool IsDeserializeable(string s) {
                s = s.Trim();
				string[] rest;
                if (ProtocolV1_0Utils.GetParams(s, NAME, ProtocolV1_0Utils.DEFAULT, out rest)) {
					if (rest.Length == 1 && ProtocolV1_0Utils.Deserialize(rest[0], out rest, ProtocolV1_0Utils.DEFAULT.NEXT)) {
						ArmorV1_0[] armors = new ArmorV1_0[rest.Length];
						for (int i = 0; i < armors.Length; i++) {
							if (!ArmorV1_0.Deserialize(rest[i], ProtocolV1_0Utils.DEFAULT.NEXT.NEXT, out armors[i])) {
								return false;
							}
						}
						cache.Cached(s, new GetArmorsAnswerCommandV10(armors));
						return true;
					}
                }
                return false;
            }

            public override bool IsTransferable(ACommand c) {
	            GetArmorsAnswerCommand command = c as GetArmorsAnswerCommand;
	            if (command != null) {
					ArmorV1_0[] armors = new ArmorV1_0[command.ARMORS.Length];
					for (int i = 0; i < armors.Length; i++) {
						armors[i] = new ArmorV1_0(command.ARMORS[i]);
					}
					cache.Cached(command, new GetArmorsAnswerCommandV10(armors));
                    return true;
                }
                return false;
            }
        }
		public GetArmorsAnswerCommandV10(ArmorV1_0[] armors) : base(armors) { }

        public string Serialize() {
			return ProtocolV1_0Utils.SerializeParams(NAME, ProtocolV1_0Utils.DEFAULT, new object[]{ARMORS});
        }
	}

}