using System;
using System.Drawing;
using BaseLibrary;
using BaseLibrary.protocol;
using BaseLibrary.utils;

namespace ObstacleMod.obstacle {
    [ModDescription()]
    public class Shielding : IScanInfluence{
        internal const string COMMAND_NAME = "SHIELDING";

        public static readonly IFactory<IObstacle, IObstacle> FACTORY = new ObtacleFactory();
        private sealed class ObtacleFactory : AObstacleFactory {
            internal ObtacleFactory() {}
            public override Boolean IsDeserializable(String s) {
                string[] rest;
                if (ProtocolV1_0Utils.GetParams(s, COMMAND_NAME, out rest)) {
                    if (rest.Length == 2) {
                        int x, y;
                        if (int.TryParse(rest[0], out x) && int.TryParse(rest[1], out y)) {
                            cache.Cached(s, new Shielding(x, y));
                            return true;
                        }
                    }
                }
                return false;
            }

            public override bool IsTransferable(IObstacle c) {
                var c2 = c as Shielding;
                if (c2 != null) {
                    cache.Cached(c, new Shielding(c2.X, c2.Y));
                    return true;
                }
                return false;
            }

            public override bool IsSerializable(IObstacle c) {
                Shielding c2 = c as Shielding;
                if (c2 != null) {
                    cacheForSerialize.Cached(c,ProtocolV1_0Utils.SerializeParams(COMMAND_NAME, c2.X, c2.Y));
                    return true;
                }
                return false;
            }
        }


        private static readonly Image icon = new Bitmap(30, 30);
        private static readonly RectangleF iconRectangle = new RectangleF(0, 0, icon.Width, icon.Height);

        static Shielding() {
            ObstaclesAroundRobot.OBSTACLE_FACTORIES.RegisterCommand(FACTORY);

            Graphics graphics = Graphics.FromImage(icon);

            Pen darkGrayPen = new Pen(Color.DarkGray, 1);
            for (int size = 1; size < icon.Height; size += 5) {
                graphics.DrawArc(darkGrayPen, -size + icon.Width / 2, -size + icon.Height, size * 2, size * 2, 240, 60);
            }

            Pen redPen = new Pen(Color.Red, 1);
            graphics.DrawLine(redPen, icon.Width, 0, 0, icon.Height);
            graphics.DrawEllipse(redPen, 0, 0, icon.Width - 1, icon.Height - 1);
        }

        public string TypeName => this.GetType().ToString();

        public int X { get; }
        public int Y { get; }
        public bool Used { get; set; }

        public Shielding(int x, int y) {
            X = x;
            Y = y;
        }

        public bool CanScan(int turn, double fromX, double fromY, double toX, double toY) {
            return false;
        }

        public void Draw(Graphics graphics, float xScale, float yScale) {
            PointF leftUpCorner = new PointF(X * xScale, Y * yScale);
            graphics.DrawImage(icon, new RectangleF(leftUpCorner.X, leftUpCorner.Y, xScale, yScale), iconRectangle, GraphicsUnit.Pixel);
        }
    }
}
