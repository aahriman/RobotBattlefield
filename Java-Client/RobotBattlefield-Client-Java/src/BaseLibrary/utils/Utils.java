
package BaseLibrary.utils;

import BaseLibrary.battlefield.Robot;
import BaseLibrary.utils.euclidianSpaceStruct.Point;
import BaseLibrary.utils.euclidianSpaceStruct.Segment;

/**
 * Collection of all useful methods for programming robot.
 */
public final class Utils {
	
	private Utils() {}
	
	/**
	 * Convert angle in degree to angle in rads.
	 * 
	 * @param degree
	 *            Converted angle in degree.
	 * @return Return angle in rads
	 */
	public static double toRads(double degree) {
		return AngleUtils.toRads(degree);
	}

	/**
	 * Convert angle in rads to angle in degree.
	 * 
	 * @param rads
	 *            Converted angle in rads.
	 * @return Return angle in degree.
	 */
	public static double toDegree(double rads) {
		return AngleUtils.toDegree(rads);
	}

	/**
	 * Calc angle from position to to position via <code>Math.Atan2</code>
	 * 
	 * @see Math.atan2
	 * @see AngleUtils.angle
	 * @return angle in radians
	 */
	public static double angle(Point from, Point to) {
		return AngleUtils.angle(from.X, from.Y, to.X, to.Y);
	}

	/**
	 * Calc angle from position to to position via <code>Math.Atan2</code>
	 * 
	 * @see Math.atan2
	 * @see AngleUtils.angle
	 * @return angle in radians
	 */
	public static double angle(double fromX, double fromY, double toX, double toY) {
		return AngleUtils.angle(fromX, fromY, toX, toY);
	}

	/**
	 * Calc angle from position to to position via <code>Math.Atan2</code>
	 * 
	 * @see Math.atan2
	 * @see AngleUtils.AngleDegree
	 * @return angle in degree
	 */
	public static double angleDegree(Point from, Point to) {
		return AngleUtils.angleDegree(from.X, from.Y, to.X, to.Y);
	}

	/**
	 * Calc angle from position to to position via <code>Math.Atan2</code>
	 * 
	 * @see Math.atan2
	 * @see AngleUtils.AngleDegree
	 * @return angle in degree
	 */
	public static double angleDegree(double fromX, double fromY, double toX, double toY) {
		return AngleUtils.angleDegree(fromX, fromY, toX, toY);
	}

	/**
	 * Normalize angle in rads to be from 0 to 2PI.
	 * 
	 * @param rads
	 *            angle in rads for normalizing.
	 * @return Normalized angle.
	 */
	public static double normalizeAngle(double rads) {
		return AngleUtils.normalizeAngle(rads);
	}

	/**
	 * Normalize angle in degree to be from 0 to 360°.
	 * 
	 * @param degree
	 *            angle in degree for normalizing.
	 * @return Normalized angle.
	 */
	public static double normalizeDegree(double degree) {
		return AngleUtils.normalizeDegree(degree);
	}

	/**
	 * Calculate distance between two points.
	 * 
	 * @return Distance between points
	 */
	public static double distance(Point point1, Point point2) {
		return EuclideanSpaceUtils.distance(point1, point2);
	}

	/**
	 * Calculate distance between two object.
	 * 
	 * @param x1
	 *            - X-coordinate of first object.
	 * @param y1
	 *            - Y-coordinate of first object.
	 * @param x2
	 *            - X-coordinate of second object.
	 * @param y2
	 *            - Y-coordinate of second object.
	 * @return Distance between object
	 */
	public static double distance(double x1, double y1, double x2, double y2) {
		return EuclideanSpaceUtils.distance(x1, y1, x2, y2);
	}

	/**
	 * Find intersect of two segments.
	 * 
	 * @return True if this segments intersect each other.
	 */
	public static boolean findIntersection(Segment segment1, Segment segment2) {
		return EuclideanSpaceUtils.findIntersection(segment1, segment2);
	}

	/**
	 * Find intersect of two segments.
	 * 
	 * @param x
	 *            - X-coordinate where this segments intersect
	 * @param y
	 *            - Y-coordinate where this segments intersect
	 * @return True if this segments intersect each other.
	 */
	public static boolean findIntersection(Segment segment1, Segment segment2, Holder<Double> x, Holder<Double> y) {
		return EuclideanSpaceUtils.findIntersection(segment1, segment2, x, y);
	}

	/**
	 * Find nearest intersect from segment of default Point.
	 * 
	 * @param leadSegment
	 *            - segment witch you want to cross
	 * @param possibleSegments
	 *            - those segments can cross leadSegment
	 * @return Nearest intersect or null
	 */
	public static Point getNearestIntersect(Segment leadSegment, Segment[] possibleSegments) {
		return EuclideanSpaceUtils.getNearestIntersect(leadSegment, possibleSegments);
	}

	/**
	 * Get robots vertical speed.
	 * 
	 * @return Robot's vertical speed
	 */
	public static double getSpeedX(Robot r) {
		return RobotUtils.getSpeedX(r);
	}

	/**
	 * Get robots horizontal speed.
	 * 
	 * @returns Robot's horizontal speed
	 */
	public static double getSpeedY(Robot r) {
		return RobotUtils.getSpeedY(r);
	}
}
