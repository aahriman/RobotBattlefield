package BaseLibrary.utils;

public final class AngleUtils {
	private AngleUtils() {
	}

	public static double toRads(double degree) {
		return degree * Math.PI / 180.0;
	}

	public static double toDegree(double rads) {
		return rads * 180 / Math.PI;
	}

	public static double angle(double fromX, double fromY, double toX, double toY) {
		return Math.atan2(toY - fromY, toX - fromX);
	}

	public static double angleDegree(double fromX, double fromY, double toX, double toY) {
		return toDegree(angle(fromX, fromY, toX, toY));
	}

	public static double normalizeAngle(double rads) {
		if (rads > 0) {
			while (rads > 2 * Math.PI) {
				rads -= 2 * Math.PI;
			}
		} else {
			while (rads < 0) {
				rads += 2 * Math.PI;
			}
		}
		return rads;
	}
	
	public static double normalizeDegree(double degree) {
		if (degree > 0) {
			while (degree > 360) {
				degree -= 360;
			}
		} else {
			while (degree < 0) {
				degree += 360;
			}
		}
		return degree;
	}

}
