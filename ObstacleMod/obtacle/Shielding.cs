using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObstacleMod.obtacle {
    public class Shielding : IScanInfluence{
        private static readonly Image icon = new Bitmap(30, 30);
        private static readonly RectangleF iconRectangle = new RectangleF(0, 0, icon.Width, icon.Height);

        static Shielding() {
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
