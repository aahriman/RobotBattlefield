using System;
using System.Drawing;

namespace Viewer.utils {
    public static class DrawerUtils {
        public static PointF Transform(double angle, PointF fromZero, PointF transforming) {
            PointF p = new PointF(transforming.X - fromZero.X, transforming.Y - fromZero.Y);
            return new PointF((float)(Math.Cos(angle) * p.X - Math.Sin(angle) * p.Y + fromZero.X),
                              (float)(Math.Sin(angle) * p.X + Math.Cos(angle) * p.Y + fromZero.Y));
        }

        public static PointF[] Transform(double angle, PointF fromZero, PointF[] transforming) {
            PointF[] ret = new PointF[transforming.Length];
            for (int i = 0; i < transforming.Length; i++) {
                ret[i] = Transform(angle, fromZero, transforming[i]);
            }
            return ret;
        }

        public static PointF Translate(PointF move, PointF translate) {
            return new PointF(translate.X + move.X, translate.Y + move.Y);
        }

        public static PointF[] Translate(PointF move, PointF[] translates) {
            PointF[] ret = new PointF[translates.Length];
            for (int i = 0; i < translates.Length; i++) {
                ret[i] = Translate(move, translates[i]);
            }
            return ret;
        }

        public static RectangleF Translate(PointF move, RectangleF translate) {
            return new RectangleF(translate.X + move.X, translate.Y + move.Y, translate.Width, translate.Height);
        }

        public static  RectangleF[] Translate(PointF move, RectangleF[] translates) {
            RectangleF[] ret = new RectangleF[translates.Length];
            for (int i = 0; i < translates.Length; i++) {
                ret[i] = Translate(move, translates[i]);
            }
            return ret;
        }
    }
}
