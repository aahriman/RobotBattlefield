package BaseLibrary.utils;

import BaseLibrary.battlefield.Robot;

public final class RobotUtils {
	private RobotUtils() {}
	
	public static double getSpeedX(Robot r) {
		return r.Power * r.Motor.MAX_SPEED / 100.0 * Math.cos(AngleUtils.toRads(r.AngleDrive));
	}

	public static double getSpeedY(Robot r) {
		return r.Power * r.Motor.MAX_SPEED / 100.0 * Math.sin(AngleUtils.toRads(r.AngleDrive));
	}
}
