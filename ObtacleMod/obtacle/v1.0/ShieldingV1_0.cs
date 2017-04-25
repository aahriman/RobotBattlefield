using System;
using System.Drawing;
using BaseLibrary.protocol;
using BaseLibrary.utils;
using BaseLibrary.utils.protocolV1_0Utils;

namespace ObstacleMod.obtacle.v1._0 {
    public class ShieldingV1_0 : Shielding, InnerSerializerV1_0{
        internal const string COMMAND_NAME = "SHIELDING";

        public static readonly IFactory<IObtacle, IObtacle> FACTORY = new ObtacleFactory();
        private sealed class ObtacleFactory : AObtacleFactory {

            public override Boolean IsDeserializable(String s) {
                string[] rest;
                if (ProtocolV1_0Utils.GetParams(s, COMMAND_NAME, out rest)) {
                    if (rest.Length == 2) {
                        int x, y;
                        if (int.TryParse(rest[0], out x) && int.TryParse(rest[1], out y)) {
                            cache.Cached(s, new ShieldingV1_0(x, y));
                        }
                    }
                }
                return false;
            }

            public override bool IsTransferable(IObtacle c) {
                var c2 = c as Shielding;
                if (c2 != null) {
                    cache.Cached(c, new ShieldingV1_0(c2.X, c2.Y));
                    return true;
                }
                return false;
            }
        }

        public ShieldingV1_0(int x, int y) : base(x, y) {
        }

        public string Serialize(Deep deep) {
            return ProtocolV1_0Utils.SerializeParams(COMMAND_NAME, deep, X, Y);
        }
    }
}
