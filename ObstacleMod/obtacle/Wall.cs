using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseLibrary.battlefield;
using BaseLibrary.utils;
using BaseLibrary.utils.euclidianSpaceStruct;
using Point = BaseLibrary.utils.euclidianSpaceStruct.Point;
using PointF = System.Drawing.PointF;

namespace ObstacleMod.obtacle {
    public class Wall : IMoveInfluence, IShotInfluence {
        private static readonly Image icon = new Bitmap(11, 11);
        private static readonly RectangleF iconRectangle = new RectangleF(0, 0, icon.Width, icon.Height);

        static Wall() {
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
            graphics.DrawImage(icon, new RectangleF(leftUpCorner.X, leftUpCorner.Y, xScale, yScale), iconRectangle, GraphicsUnit.Pixel);
        }
    }
}
