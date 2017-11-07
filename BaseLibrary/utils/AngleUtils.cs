using System;

namespace BaseLibrary.utils {
	public static class AngleUtils {
        /// <summary>
        /// Convert angle in degree to angle in rads.
        /// </summary>
        /// <param name="degree">Converted angle in degree.</param>
        /// <returns>Return angle in rads</returns>
        public static double ToRads(double degree) {
            return degree * Math.PI / 180.0;
        }

	    /// <summary>
	    /// Convert angle in rads to angle in degree.
	    /// </summary>
	    /// <param name="rads">Converted angle in rads.</param>
	    /// <returns>Return angle in degree.</returns>
        public static double ToDegree(double rads) {
            return rads * 180 / Math.PI;
        }

        /// <summary>
        /// Calc angle from position to to position via <code>Math.Atan2</code>
        /// </summary>
        /// <param name="fromX"></param>
        /// <param name="fromY"></param>
        /// <param name="toX"></param>
        /// <param name="toY"></param>
        /// <see cref="Math.Atan2"/>
        /// <returns>angle in rads</returns>
		public static double Angle(double fromX, double fromY, double toX, double toY) {
			return Math.Atan2(toY - fromY, toX - fromX);
		}

        /// <summary>
        /// Calc angle from position to to position via <code>Math.Atan2</code>
        /// </summary>
        /// <param name="fromX"></param>
        /// <param name="fromY"></param>
        /// <param name="toX"></param
        /// <param name="toY"></param>
        /// <see cref="Math.Atan2"/>
        /// <returns>angle in degree</returns>
		public static double AngleDegree(double fromX, double fromY, double toX, double toY) {
            return ToDegree(Math.Atan2(toY - fromY, toX - fromX));
        }

        /// <summary>
        /// Normalize angle in rads to be from 0 to 2PI.
        /// </summary>
        /// <param name="rads">Normalized angle in rads</param>
        /// <returns>Normalized angle.</returns>
        public static double NormalizeAngle(double rads) {
            while (rads < 2*Math.PI) {
                rads += 2 * Math.PI;
            }
            return rads % (2 * Math.PI);
        }

        /// <summary>
        /// Normalize angle in degree to be from 0 to 360°.
        /// </summary>
        /// <param name="degree">Normalized angle in degree</param>
        /// <returns>Normalized angle.</returns>
        public static double NormalizeDegree(double degree) {
	        while (degree < 360) {
	            degree += 360;
	        }
	        return degree % 360;
	    }
    }
}
