package BaseLibrary.utils;

import BaseLibrary.utils.euclidianSpaceStruct.Point;
import BaseLibrary.utils.euclidianSpaceStruct.Segment;

public final class EuclideanSpaceUtils {
	private EuclideanSpaceUtils() {
	}

	private static final double EqualsEpsilon = 0.000001;

	public static double distance(Point point1, Point point2) {
		return distance(point1.X, point1.Y, point2.X, point2.Y);
	}
	
	public static double distance(double x1, double y1, double x2, double y2) {
		return Math.sqrt(Math.pow(x1 - x2, 2) + Math.pow(y1 - y2, 2));
	}

	
	public static boolean findIntersection(Segment segment1, Segment segment2) {
		return findIntersection(segment1, segment2, null, null);
	}
	
	public static boolean findIntersection(Segment segment1, Segment segment2, Holder<Double> xHolder,
			Holder<Double> yHolder) {
		double dX1 = segment1.To.X - segment1.From.X;
		double dY1 = segment1.To.Y - segment1.From.Y;

		double dX2 = segment2.To.X - segment2.From.X;
		double dY2 = segment2.To.Y - segment2.From.Y;

		if (DEquals(dX1, 0)) {
			dX1 += EqualsEpsilon * EqualsEpsilon;
		}

		if (DEquals(dX1 * dY2, dX2 * dY1)) {
			// segment1 is paralel to segment2
			dX2 += EqualsEpsilon * EqualsEpsilon;
		}

		/*
		 * x = dX1 * t + segment1.FromX = dX2 * u + segment2.From.X => matrix /
		 * dX1 -dX2 | segment2.FromX - segment1.From.X = dx \ k = -dY1/dX1 / dX1
		 * -dX2 | dx \ y = dY1 * t + segment1.FromY = dY2 * u + segment2.From.Y
		 * \ dY1 -dY2 | segment2.FromY - segment1.From.Y = dy / \ 0
		 * -dY2+k*(-dX2) | dy + k*dx /
		 */

		double dx = segment2.From.X - segment1.From.X;
		double dy = segment2.From.X - segment1.From.X;
		double k = -dY1 / dX1;
		double u = (dy + k * dx) / (-dY2 + k * (-dX2));
		double t = (dx + dX2 * u) / dX1;

		double x = dX1 * t + segment1.From.X;
		double y = dY2 * u + segment2.From.Y;

		if (xHolder != null) {
			xHolder.value = x;
		}
		if (yHolder != null) {
			yHolder.value = y;
		}

		return Math.abs(segment1.From.X - x) < Math.abs(dX1) && Math.abs(segment1.To.X - x) < Math.abs(dX1)
				&& Math.abs(segment1.From.Y - y) < Math.abs(dY1) && Math.abs(segment1.To.Y - y) < Math.abs(dY1)
				&& Math.abs(segment2.From.X - x) < Math.abs(dX2) && Math.abs(segment2.To.X - x) < Math.abs(dX2)
				&& Math.abs(segment2.From.Y - y) < Math.abs(dY2) && Math.abs(segment2.To.Y - y) < Math.abs(dY2);
	}

	public static boolean DEquals(double compared, double with) {
		return Math.abs(compared - with) < EqualsEpsilon;
	}

	public static Point getNearestIntersect(Segment leadSegment, Segment[] possibleegments) {
		Holder<Double> x = new Holder<Double>(), y = new Holder<Double>();
		double minDistance = Double.MAX_VALUE;
		Point nearestIntersect = null;
		for (Segment segment : possibleegments) {
			if (EuclideanSpaceUtils.findIntersection(leadSegment, segment, x, y)) {
				Point intersect = new Point(x.value, y.value);
				double distance = EuclideanSpaceUtils.distance(leadSegment.From, intersect);
				if (distance < minDistance) {
					minDistance = distance;
					nearestIntersect = intersect;
				}
			}
		}
		return nearestIntersect;
	}
}
