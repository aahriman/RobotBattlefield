using System;
using BaseLibrary.battlefield;

namespace BaseLibrary.utils {
    public class RobotUtils {
        public static double getSpeedX(Robot r) {
            return r.Power * r.Motor.MAX_SPEED / 100.0 * Math.Cos(AngleUtils.ToRads(r.AngleDrive));
        }

        public static double getSpeedY(Robot r) {
            return r.Power * r.Motor.MAX_SPEED / 100.0 * Math.Sin(AngleUtils.ToRads(r.AngleDrive));
        }
    }
}
