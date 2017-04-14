using System;
using System.Drawing;
using BaseLibrary;
using BaseLibrary.battlefield;
using BaseLibrary.protocol;
using BaseLibrary.utils;
using BaseLibrary.utils.euclidianSpaceStruct;
using Point = BaseLibrary.utils.euclidianSpaceStruct.Point;
using PointF = System.Drawing.PointF;

namespace ObtacleMod.obtacle {
    [ModDescription()]
    public class Wall : IMoveInfluence, IShotInfluence {
        public const string COMMAND_NAME = "WALL";
        private static readonly AObtacleFactory FACTORY = new ObtacleFactory();
        private class ObtacleFactory : AObtacleFactory {
            internal ObtacleFactory() {}
            public override bool IsDeserializable(string s) {
                string[] rest;
                if (ProtocolV1_0Utils.GetParams(s, COMMAND_NAME, out rest)) {
                    int x, y;
                    if (rest.Length == 2 && int.TryParse(rest[0], out x) && int.TryParse(rest[1], out y)) {
                        cache.Cached(s, new Wall(x,y));
                        return true;
                    }
                }
                return false;
            }

            public override bool IsTransferable(IObtacle c) {
                Wall c2 = c as Wall;
                if (c2 != null) {
                    cache.Cached(c, c2);
                    return true;
                }
                return false;
            }

            public override bool IsSerializeable(IObtacle c) {
                Wall c2 = c as Wall;
                if (c2 != null) {
                    String serialized = ProtocolV1_0Utils.SerializeParams(COMMAND_NAME, c2.Y, c2.X);
                    cacheForSerialize.Cached(c, serialized);
                    return true;
                }
                return false;
            }
        }

        private static readonly Cache<PointF, Image> CACHE = new Cache<PointF, Image>(true);
        static Wall() {
            ObtaclesInSight.OBTACLE_FACTORIES.RegisterCommand(FACTORY);
        }

        public string TypeName => this.GetType().ToString();

        public int X { get; }
        public int Y { get; }
        public bool Used { get; set; }

        public Wall(int x, int y) {
            X = x;
            Y = y;
        }

        void IMoveInfluence.Change(Robot robot, int turn, ref double fromX, ref double fromY, ref double toX, ref double toY) {
            robot.HitPoints -= (int) Math.Max(1, Math.Round(5 * robot.Power * robot.Motor.MAX_SPEED / 100));
            robot.Power = 0;
            toY = robot.Y;
            toX = robot.X;
            fromX = robot.X;
            fromY = robot.Y;
        }

        bool IShotInfluence.Change(int turn, double fromX, double fromY, ref double toX, ref double toY) {
            Segment shotSegment = new Segment(fromX, fromY, toX, toY);

            Point intersect = EuclideanSpaceUtils.GetNearestIntersect(shotSegment, this.Segments());
            toY = intersect.Y;
            toX = intersect.X;

            return true;
        }

        public void Draw(Graphics graphics, float xScale, float yScale) {
            PointF leftUpCorner = new PointF(X * xScale, Y * yScale);
            PointF scale = new PointF(xScale, yScale);
            Image drawingIcon;
            if (!CACHE.GetCached(scale, out drawingIcon)) {
                drawingIcon = new Bitmap((int)xScale,  (int)yScale);
                Image icon = drawIcon();
                Graphics.FromImage(drawingIcon)
                        .DrawImage(icon, new RectangleF(0, 0, xScale, yScale),
                                   new RectangleF(0, 0, icon.Width, icon.Height), GraphicsUnit.Pixel);
                CACHE.Cached(scale, icon);
            }
            graphics.DrawImage(drawingIcon, leftUpCorner);
        }

        private Image drawIcon() {
            Image icon = new Bitmap(11, 11);
            Graphics graphics = Graphics.FromImage(icon);
            graphics.FillRectangle(new SolidBrush(Color.Red), 0, 0, icon.Width, icon.Height);
            Pen black = new Pen(Color.Black, 1);

            int x = 0;

            for (int y = 0; y < icon.Height; y += 3) {
                graphics.DrawLine(black, 0, y, icon.Width, y);
                for (x = x % icon.Width; x < icon.Width; x += 4) {
                    graphics.DrawLine(black, x, y + 0, x, y + 3);
                }
            }
            return icon;
        }
    }
}
