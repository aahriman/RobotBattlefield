using BaseLibrary.communication.command.equipment;
using BaseLibrary.communication.protocol;
using BaseLibrary.utils;

namespace BaseLibrary.communication.command.v1._0.equipment{
    public class GetGunsAnswerCommandV1_0 : GetGunsAnswerCommand, ACommand.Sendable {
        private const string NAME = "GUNS_ANSWER";
        public static readonly IFactory<ACommand.Sendable, ACommand> FACTORY = new CommandFactory();
        private sealed class CommandFactory : ACommandFactory {
            internal CommandFactory() : base() { }

            public override bool IsDeserializeable(string s) {
                s = s.Trim();
				string[] rest;
                if (ProtocolV1_0Utils.GetParams(s, NAME, ProtocolV1_0Utils.DEFAULT, out rest)) {
					if (rest.Length == 1 && ProtocolV1_0Utils.Deserialize(rest[0], out rest, ProtocolV1_0Utils.DEFAULT.NEXT)) {
						GunV1_0[] guns = new GunV1_0[rest.Length];
						for (int i = 0; i < guns.Length; i++) {
							if (!GunV1_0.Deserialize(rest[i], ProtocolV1_0Utils.DEFAULT.NEXT.NEXT, out guns[i])) {
								return false;
							}
						}
						cache.Cached(s, new GetGunsAnswerCommandV1_0(guns));
						return true;
					}
                }
                return false;
            }

            public override bool IsTransferable(ACommand c) {
				if (c is GetGunsAnswerCommand) {
					var c2 = (GetGunsAnswerCommand)c;
					GunV1_0[] guns = new GunV1_0[c2.GUNS.Length];
					for (int i = 0; i < guns.Length; i++) {
						guns[i] = new GunV1_0(c2.GUNS[i]);
					}
					cache.Cached(c, new GetGunsAnswerCommandV1_0(guns));
                    return true;
                }
                return false;
            }
        }
		public GetGunsAnswerCommandV1_0(GunV1_0[] GUNS) : base(GUNS) { }

        public string Serialize() {
			return ProtocolV1_0Utils.SerializeParams(NAME, ProtocolV1_0Utils.DEFAULT, new object[]{GUNS});
        }
	}

}