using System;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using BaseLibrary.command;
using BaseLibrary.protocol;
using BaseLibrary.utils;
using BaseLibrary.utils.protocolV1_0Utils;

namespace ObstacleMod.obtacle.v1._0 {
    public sealed class OuterShildingV1_0 : OuterShilding, InnerSerializerV1_0 {
        internal const string COMMAND_NAME = "OUTER_SHIELDING";

        public static readonly IFactory<IObtacle, IObtacle> FACTORY = new ObtacleFactory();
        private sealed class ObtacleFactory : AObtacleFactory {

            public override Boolean IsDeserializable(String s) {
                string[] rest;
                if (ProtocolV1_0Utils.GetParams(s, COMMAND_NAME, out rest)) {
                    if (rest.Length == 2) {
                        int x, y;
                        if (int.TryParse(rest[0], out x) && int.TryParse(rest[1], out y)) {
                            cache.Cached(s, new OuterShildingV1_0(x,y));
                        }
                    }
                }
                return false;
            }

            public override bool IsTransferable(IObtacle c) {
                OuterShilding c2 = c as OuterShilding;
                if (c2 != null) {
                    cache.Cached(c, new OuterShildingV1_0(c2.X, c2.Y));
                    return true;
                }
                return false;
            }
        }

        public OuterShildingV1_0(int x, int y) : base(x, y) {
        }

        public string Serialize(Deep deep) {
            return ProtocolV1_0Utils.SerializeParams(COMMAND_NAME, deep, X, Y);
        }
    }
}
