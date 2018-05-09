using System;
using BaseLibrary.utils;

namespace BaseLibrary.command.v1._0{
    public class GetGunsAnwerCommandV1_0 : GetGunsAnwerCommand, ACommand.Sendable {
        private const string NAME = "GUNS_ANSER";
        public static readonly ICommandFactory FACTORY = new CommandFactory();
        private sealed class CommandFactory : ACommandFactory {
            internal CommandFactory() : base() { }

            public override Boolean IsDeserializable(String s) {
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
						cache.Cached(s, new GetGunsAnwerCommandV1_0(guns));
						return true;
					}
                }
                return false;
            }

            public override bool IsTransferable(ACommand c) {
				if (c is GetGunsAnwerCommand) {
					var c2 = (GetGunsAnwerCommand)c;
					GunV1_0[] guns = new GunV1_0[c2.GUNS.Length];
					for (int i = 0; i < guns.Length; i++) {
						guns[i] = new GunV1_0(c2.GUNS[i]);
					}
					cache.Cached(c, new GetGunsAnwerCommandV1_0(guns));
                    return true;
                }
                return false;
            }
        }
		public GetGunsAnwerCommandV1_0(GunV1_0[] GUNS) : base(GUNS) { }

        public string serialize() {
			return ProtocolV1_0Utils.SerializeParams(NAME, ProtocolV1_0Utils.DEFAULT, new object[]{GUNS});
        }
	}

}