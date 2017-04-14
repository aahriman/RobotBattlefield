using System;
using System.Drawing;
using BaseLibrary.protocol;
using BaseLibrary.utils;

namespace ObtacleMod.obtacle {
    [ModDescription()]
    public class OuterShilding : IScanInfluence {

        public const string COMMAND_NAME = "OUTHER_SHILDING";
        public static readonly IFactory<IObtacle, IObtacle> FACTORY = new ObtacleFactory();
        private sealed class ObtacleFactory : AObtacleFactory {
            internal ObtacleFactory() { }
            public override Boolean IsDeserializable(String s) {
                string[] rest;
                if (ProtocolV1_0Utils.GetParams(s, COMMAND_NAME, out rest)) {
                    if (rest.Length == 2) {
                        int x, y;
                        if (int.TryParse(rest[0], out x) && int.TryParse(rest[1], out y)) {
                            cache.Cached(s, new OuterShilding(x, y));
                            return true;
                        }
                    }
                }
                return false;
            }

            public override bool IsTransferable(IObtacle c) {
                var c2 = c as OuterShilding;
                if (c2 != null) {
                    cache.Cached(c, new OuterShilding(c2.X, c2.Y));
                    return true;
                }
                return false;
            }

            public override bool IsSerializeable(IObtacle c) {
                OuterShilding c2 = c as OuterShilding;
                if (c2 != null) {
                    cacheForSerialize.Cached(c, ProtocolV1_0Utils.SerializeParams(COMMAND_NAME, c2.X, c2.Y));
                    return true;
                }
                return false;
            }
        }

        static OuterShilding() {
            ObtaclesInSight.OBTACLE_FACTORIES.RegisterCommand(FACTORY);
        }


        public string TypeName => this.GetType().ToString();

        public int X { get; }
        public int Y { get; }
        public bool Used { get; set; }

        public OuterShilding(int x, int y) {
            X = x;
            Y = y;
        }

        public bool CanScan(int turn, double fromX, double fromY, double toX, double toY) {
            return X < fromX && fromX < X + 1 && Y < fromY && fromY < Y + 1;
        }

        public void Draw(Graphics graphics, float xScale, float yScale) {
            // TODO
        }
    }
}
