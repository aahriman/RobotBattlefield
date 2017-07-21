using System;

namespace BaseLibrary.utils {
	public static class AngleUtils {
        public static double ToRads(double degree) {
            return degree * Math.PI / 180.0;
        }

        public static double ToDegree(double rads) {
            return rads * 180 / Math.PI;
        }

        /// <summary>
        /// Calc angle from position to to position via Math.Atan2
        /// </summary>
        /// <param name="fromX"></param>
        /// <param name="fromY"></param>
        /// <param name="toX"></param>
        /// <param name="toY"></param>
        /// <returns>angle in rads</returns>
		public static double Angle(double fromX, double fromY, double toX, double toY) {
			return Math.Atan2(toY - fromY, toX - fromX);
		}

        /// <summary>
        /// Calc angle from position to to position via Math.Atan2
        /// </summary>
        /// <param name="fromX"></param>
        /// <param name="fromY"></param>
        /// <param name="toX"></param>
        /// <param name="toY"></param>
        /// <returns>angle in degree</returns>
		public static double AngleDegree(double fromX, double fromY, double toX, double toY) {
            return ToDegree(Math.Atan2(toY - fromY, toX - fromX));
        }

        public static double Normalize(double angle) {
            while (angle < 2*Math.PI) {
                angle += 2 * Math.PI;
            }
            return angle % (2 * Math.PI);
        }

        public static double NormalizeDegree(double angle) {
	        while (angle < 360) {
	            angle += 360;
	        }
	        return angle % 360;
	    }
    }
}
