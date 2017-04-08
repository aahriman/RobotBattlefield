using System;

namespace BaseLibrary.utils {
	public static class AngleUtils {
        public static double ToRads(double degree) {
            return degree * Math.PI / 180.0;
        }

        public static double ToDegree(double rads) {
            return rads * 180 / Math.PI;
        }

		public static double Angle(double fromX, double fromY, double toX, double toY) {
			return Math.Atan2(toY - fromY, toX - fromX);
		}
    }
}
