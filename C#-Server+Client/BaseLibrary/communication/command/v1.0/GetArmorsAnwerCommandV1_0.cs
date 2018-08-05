using System;
using BaseLibrary.utils;

namespace BaseLibrary.command.v1._0 {
	public class GetArmorsAnwerCommandV1_0 : GetArmorsAnwerCommand, ACommand.Sendable {
		private const string NAME = "ARMOR_ANSER";
        public static readonly ICommandFactory FACTORY = new CommandFactory();
        private sealed class CommandFactory : ACommandFactory {
            public override Boolean IsDeserializable(String s) {
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
						cache.Cached(s, new GetArmorsAnwerCommandV1_0(armors));
						return true;
					}
                }
                return false;
            }

            public override bool IsTransferable(ACommand c) {
	            GetArmorsAnwerCommand command = c as GetArmorsAnwerCommand;
	            if (command != null) {
					ArmorV1_0[] armors = new ArmorV1_0[command.ARMORS.Length];
					for (int i = 0; i < armors.Length; i++) {
						armors[i] = new ArmorV1_0(command.ARMORS[i]);
					}
					cache.Cached(command, new GetArmorsAnwerCommandV1_0(armors));
                    return true;
                }
                return false;
            }
        }
		public GetArmorsAnwerCommandV1_0(ArmorV1_0[] armors) : base(armors) { }

        public string serialize() {
			return ProtocolV1_0Utils.SerializeParams(NAME, ProtocolV1_0Utils.DEFAULT, new object[]{ARMORS});
        }
	}

}