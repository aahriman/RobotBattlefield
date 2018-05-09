using System;
using BaseLibrary.battlefield;

namespace BaseLibrary.utils {
    public static class RobotUtils {

        /// <summary>
        /// Get robots vertical speed.
        /// </summary>
        /// <param name="r"></param>
        /// <returns>Robot's vertical speed</returns>
        public static double GetSpeedX(Robot r) {
            return r.Power * r.Motor.MAX_SPEED / 100.0 * Math.Cos(AngleUtils.ToRads(r.AngleDrive));
        }


        /// <summary>
        /// Get robots horizontal speed.
        /// </summary>
        /// <param name="r"></param>
        /// <returns>Robot's horizontal speed</returns>
        public static double GetSpeedY(Robot r) {
            return r.Power * r.Motor.MAX_SPEED / 100.0 * Math.Sin(AngleUtils.ToRads(r.AngleDrive));
        }
    }
}
