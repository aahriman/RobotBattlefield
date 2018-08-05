using System;
using BaseLibrary.utils.euclidianSpaceStruct;

namespace BaseLibrary.utils {
    public static class EuclideanSpaceUtils {

        /// <summary>
        /// Epsilon for comparing doubles.
        /// </summary>
        private const double EqualsEpsilon = 0.000001;

        /// <summary>
        /// Calculate distance between two points.
        /// </summary>
        /// <param name="point1">First point.</param>
        /// <param name="point2">Second point.</param>
        /// <returns>Distance between object</returns>
        public static double Distance(Point point1, Point point2) {
            return Math.Sqrt(Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2));
        }

        /// <summary>
        /// Calculate distance between two object.
        /// </summary>
        /// <param name="x1">X-coordinate of first object.</param>
        /// <param name="y1">Y-coordinate of first object.</param>
        /// <param name="x2">X-coordinate of second object.</param>
        /// <param name="y2">Y-coordinate of second object.</param>
        /// <returns>Distance between object</returns>
        public static double Distance(double x1, double y1, double x2, double y2) {
            return Distance(new Point(x1, y1), new Point(x2, y2));
        }

        /// <summary>
        /// Find intersect of two segments.
        /// </summary>
        /// <param name="segment1">First segment.</param>
        /// <param name="segment2">Second segment.</param>
        /// <returns>True if this segments intersect each other.</returns>
        public static bool FindIntersection(Segment segment1, Segment segment2) {
            return FindIntersection(segment1, segment2, out double x, out double y);
        }

        /// <summary>
        /// Find intersect of two segments.
        /// </summary>
        /// <param name="segment1">First segment.</param>
        /// <param name="segment2">Second segment.</param>
        /// <returns>True if this segments intersect each other.</returns>
        public static bool FindIntersection(Segment segment1, Segment segment2, out double x, out double y) {
            double dX1 = segment1.To.X - segment1.From.X;
            double dY1 = segment1.To.Y - segment1.From.Y;

            double dX2 = segment2.To.X - segment2.From.X;
            double dY2 = segment2.To.Y - segment2.From.Y;

            if (dX1.DEquals(0)) {
                dX1 += EqualsEpsilon * EqualsEpsilon;
            }

            if ((dX1 * dY2).DEquals(dX2 * dY1)) {
                // segment1 is paralel to segment2
                dX2 += EqualsEpsilon * EqualsEpsilon;
                    // TODO zeptat se učitele, jestli to takto mohu nechat... když dx1 může být v mé implementaci max 1000
            }


            /*
             * x = dX1 * t + segment1.FromX = dX2 * u + segment2.From.X => matrix / dX1    -dX2 | segment2.FromX - segment1.From.X = dx \  k = -dY1/dX1  / dX1    -dX2        | dx        \     
             * y = dY1 * t + segment1.FromY = dY2 * u + segment2.From.Y           \ dY1    -dY2 | segment2.FromY - segment1.From.Y = dy /                \ 0    -dY2+k*(-dX2) | dy + k*dx /
            */

            double dx = segment2.From.X - segment1.From.X;
            double dy = segment2.From.X - segment1.From.X;
            double k = -dY1 / dX1;
            double u = (dy + k * dx) / (-dY2 + k * (-dX2));
            double t = (dx + dX2 * u) / dX1;

            x = dX1 * t + segment1.From.X;
            y = dY2 * u + segment2.From.Y;

            return Math.Abs(segment1.From.X - x) < Math.Abs(dX1) && Math.Abs(segment1.To.X - x) < Math.Abs(dX1) &&
                   Math.Abs(segment1.From.Y - y) < Math.Abs(dY1) && Math.Abs(segment1.To.Y - y) < Math.Abs(dY1) &&
                   Math.Abs(segment2.From.X - x) < Math.Abs(dX2) && Math.Abs(segment2.To.X - x) < Math.Abs(dX2) &&
                   Math.Abs(segment2.From.Y - y) < Math.Abs(dY2) && Math.Abs(segment2.To.Y - y) < Math.Abs(dY2);
        }

        /// <summary>
        /// Compare two doubles.
        /// </summary>
        /// <param name="self">Self (this) double.</param>
        /// <param name="compared">Compared double with self.</param>
        /// <returns>true if there are close enough. </returns>
        public static bool DEquals(this double self, double compared) {
            return Math.Abs(self - compared) < EqualsEpsilon;
        }

        /// <summary>
        /// Find nearest intersect from segment of default Point.
        /// </summary>
        /// <param name="leadSegment">This segment do you want to cross</param>
        /// <param name="possibleSegments">Those segments can cross leadSegment</param>
        /// <returns>Nearest intersect or default(Point)</returns>
        public static Point GetNearestIntersect(Segment leadSegment, Segment[] possibleSegments) {
            double minDistance = double.MaxValue;
            Point nearestIntersect = default(Point);
            foreach (var segment in possibleSegments) {
                if (EuclideanSpaceUtils.FindIntersection(leadSegment, segment, out double x, out double y)) {
                    Point intersect = new Point(x, y);
                    double distance = EuclideanSpaceUtils.Distance(leadSegment.From, intersect);
                    if (distance < minDistance) {
                        minDistance = distance;
                        nearestIntersect = intersect;
                    }
                }
            }
            return nearestIntersect;
        }
    }
}
