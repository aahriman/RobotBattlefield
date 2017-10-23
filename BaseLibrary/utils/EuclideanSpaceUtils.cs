using System;
using BaseLibrary.utils.euclidianSpaceStruct;

namespace BaseLibrary.utils {
    public static class EuclideanSpaceUtils {
        private const double EqualsEpsilon = 0.000001;

        public static bool FindIntersenction(Segment segment1, Segment segment2) {
            double x, y;
            return FindIntersenction(segment1, segment2, out x, out y);
        }

        public static double Distance(Point point1, Point point2) {
            return Math.Sqrt(Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2));
        }

        public static double Distance(double x1, double y1, double x2, double y2) {
            return Distance(new Point(x1, y1), new Point(x2, y2));
        }

        public static bool FindIntersenction(Segment segment1, Segment segment2, out double x, out double y) {
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

        public static bool DEquals(this double compared, double with) {
            return Math.Abs(compared - with) < EqualsEpsilon;
        }

        public static Point GetNearestIntersect(Segment leadSegment, Segment[] possibleegments) {
            double x, y;
            double minDistance = double.MaxValue;
            Point nearestIntersect = default(Point);
            foreach (var segment in possibleegments) {
                if (EuclideanSpaceUtils.FindIntersenction(leadSegment, segment, out x, out y)) {
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
