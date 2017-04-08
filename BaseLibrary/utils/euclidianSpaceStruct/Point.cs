using System;

namespace BaseLibrary.utils.euclidianSpaceStruct {
    public struct Point : IComparable<Point> {

        public static bool operator ==(Point first, Point second) {
            return first.X.DEquals(second.X) && first.Y.DEquals(second.Y);
        }

        public static bool operator !=(Point first, Point second) {
            return !(first == second);
        }

        public double X;
        public double Y;

        public Point(double x, double y) {
            X = x;
            Y = y;
        }

        public bool Equals(Point other) {
            return this == other;
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Point && this == (Point)obj;
        }

        public int CompareTo(Point other) {
            var xComparison = X.CompareTo(other.X);
            if (xComparison != 0) return xComparison;
            return Y.CompareTo(other.Y);
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }

        public override string ToString() {
            return $"Point[x={X}, y={Y}]";
        }
    }
}
