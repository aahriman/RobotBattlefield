using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseLibrary.battlefield;
using BaseLibrary.utils.euclidianSpaceStruct;

namespace BaseLibrary.utils {

    /// <summary>
    /// Collection of all useful methods for programming robot.
    /// </summary>
    public static class Utils {
        /// <summary>
        /// Convert angle in degree to angle in rads.
        /// </summary>
        /// <param name="degree">Converted angle in degree.</param>
        /// <returns>Return angle in rads</returns>
        public static double ToRads(double degree) {
            return AngleUtils.ToRads(degree);
        }

        /// <summary>
        /// Convert angle in rads to angle in degree.
        /// </summary>
        /// <param name="rads">Converted angle in rads.</param>
        /// <returns>Return angle in degree.</returns>
        public static double ToDegree(double rads) {
            return AngleUtils.ToDegree(rads);
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
            return AngleUtils.Angle(fromX, fromY, toX, toY);
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
            return AngleUtils.AngleDegree(fromX, fromY, toX, toY);
        }

        /// <summary>
        /// Normalize angle in rads to be from 0 to 2PI.
        /// </summary>
        /// <param name="rads">Normalized angle in rads</param>
        /// <returns>Normalized angle.</returns>
        public static double NormalizeAngle(double rads) {
            return AngleUtils.NormalizeAngle(rads);
        }

        /// <summary>
        /// Normalize angle in degree to be from 0 to 360°.
        /// </summary>
        /// <param name="degree">Normalized angle in degree</param>
        /// <returns>Normalized angle.</returns>
        public static double NormalizeDegree(double degree) {
            return AngleUtils.NormalizeAngle(degree);
        }

        /// <summary>
        /// Calculate distance between two points.
        /// </summary>
        /// <param name="point1">First point.</param>
        /// <param name="point2">Second point.</param>
        /// <returns>Distance between object</returns>
        public static double Distance(Point point1, Point point2) {
            return EuclideanSpaceUtils.Distance(point1, point2);
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
            return EuclideanSpaceUtils.Distance(x1, y1, x2, y2);
        }

        /// <summary>
        /// Find intersect of two segments.
        /// </summary>
        /// <param name="segment1">First segment.</param>
        /// <param name="segment2">Second segment.</param>
        /// <returns>True if this segments intersect each other.</returns>
        public static bool FindIntersection(Segment segment1, Segment segment2) {
            return EuclideanSpaceUtils.FindIntersection(segment1, segment2, out double x, out double y);
        }

        /// <summary>
        /// Find intersect of two segments.
        /// </summary>
        /// <param name="segment1">First segment.</param>
        /// <param name="segment2">Second segment.</param>
        /// <returns>True if this segments intersect each other.</returns>
        public static bool FindIntersection(Segment segment1, Segment segment2, out double x, out double y) {
            return EuclideanSpaceUtils.FindIntersection(segment1, segment2, out x, out y);
        }

        /// <summary>
        /// Find nearest intersect from segment of default Point.
        /// </summary>
        /// <param name="leadSegment">This segment do you want to cross</param>
        /// <param name="possibleSegments">Those segments can cross leadSegment</param>
        /// <returns>Nearest intersect or default(Point)</returns>
        public static Point GetNearestIntersect(Segment leadSegment, Segment[] possibleSegments) {
            return EuclideanSpaceUtils.GetNearestIntersect(leadSegment, possibleSegments);
        }

        /// <summary>
        /// Get robots vertical speed.
        /// </summary>
        /// <param name="r"></param>
        /// <returns>Robot's vertical speed</returns>
        public static double GetSpeedX(Robot r) {
            return RobotUtils.GetSpeedX(r);
        }


        /// <summary>
        /// Get robots horizontal speed.
        /// </summary>
        /// <param name="r"></param>
        /// <returns>Robot's horizontal speed</returns>
        public static double GetSpeedY(Robot r) {
            return RobotUtils.GetSpeedY(r);
        }
    }
}
