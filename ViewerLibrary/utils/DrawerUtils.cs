using System;
using System.Drawing;

namespace ViewerLibrary.utils {
    /// <summary>
    /// Utilities for drawing - transformation for points and Rectangles.
    /// </summary>
    public static class DrawerUtils {

        /// <summary>
        /// Rotate vector rotated-fromZero by angle with center of rotation in fromZero.
        /// </summary>
        /// <param name="angle">in radians</param>
        /// <param name="fromZero"></param>
        /// <param name="rotated"></param>
        /// <returns></returns>
        public static PointF Rotate(double angle, PointF fromZero, PointF rotated) {
            PointF p = new PointF(rotated.X - fromZero.X, rotated.Y - fromZero.Y);
            PointF result = new PointF((float)(Math.Cos(angle) * p.X - Math.Sin(angle) * p.Y + fromZero.X),
                (float)(Math.Sin(angle) * p.X + Math.Cos(angle) * p.Y + fromZero.Y));
            return result;
        }

        /// <summary>
        /// Rotate vectors rotated[i]-fromZero by angle with center of rotation in fromZero.
        /// </summary>
        /// <param name="angle">in radians</param>
        /// <param name="fromZero"></param>
        /// <param name="rotated"></param>
        /// <returns></returns>
        public static PointF[] Rotate(double angle, PointF fromZero, PointF[] rotated) {
            PointF[] ret = new PointF[rotated.Length];
            for (int i = 0; i < rotated.Length; i++) {
                ret[i] = Rotate(angle, fromZero, rotated[i]);
            }
            return ret;
        }

        /// <summary>
        /// Translate point translate by move.
        /// Add to point translate point move.
        /// </summary>
        /// <param name="move"></param>
        /// <param name="translate"></param>
        /// <returns></returns>
        public static PointF Translate(PointF move, PointF translate) {
            return new PointF(translate.X + move.X, translate.Y + move.Y);
        }

        /// <summary>
        /// Translate points translates by move.
        /// Add to points translates point move.
        /// </summary>
        /// <param name="move"></param>
        /// <param name="translates"></param>
        /// <returns></returns>
        public static PointF[] Translate(PointF move, PointF[] translates) {
            PointF[] ret = new PointF[translates.Length];
            for (int i = 0; i < translates.Length; i++) {
                ret[i] = Translate(move, translates[i]);
            }
            return ret;
        }

        /// <summary>
        /// Translate rectangle by move.
        /// Add to left top corner coordinates X and Y from point move. 
        /// </summary>
        /// <param name="move"></param>
        /// <param name="translate"></param>
        /// <returns></returns>
        public static RectangleF Translate(PointF move, RectangleF translate) {
            return new RectangleF(translate.X + move.X, translate.Y + move.Y, translate.Width, translate.Height);
        }

        /// <summary>
        /// Translate rectangles by move.
        /// Add to left top corner coordinates X and Y from point move. 
        /// </summary>
        /// <param name="move"></param>
        /// <param name="translates"></param>
        /// <returns></returns>
        public static  RectangleF[] Translate(PointF move, RectangleF[] translates) {
            RectangleF[] ret = new RectangleF[translates.Length];
            for (int i = 0; i < translates.Length; i++) {
                ret[i] = Translate(move, translates[i]);
            }
            return ret;
        }
    }
}
