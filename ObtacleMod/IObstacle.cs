using System.Drawing;
using BaseLibrary.utils.euclidianSpaceStruct;

namespace ObstacleMod {
    /// <summary>
    /// Generic obstacle interface
    /// </summary>
    public interface IObstacle {
        /// <summary>
        /// Type name for used in de-serialization
        /// </summary>
        string TypeName { get; }

        /// <summary>
        /// X - coordinate of obstacle
        /// </summary>
        int X { get; }
        /// <summary>
        /// Y - coordinate of obstacle
        /// </summary>
        int Y { get; }


        /// <summary>
        /// Flag if this obstacle was used to avoid cycle.
        /// </summary>
        bool Used { get; set; }

        /// <summary>
        /// Draw this obstacle at correct position.
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="xScale">How many pixels is one unit</param>
        /// <param name="yScale">How many pixels is one unit</param>
        void Draw(Graphics graphics, float xScale, float yScale);
    }

    public static class ObtacleExtensions {
        public static Segment[] Segments(this IObstacle obtacle) {
            Segment obtacleSegment1 = new Segment(obtacle.X, obtacle.Y - 1, obtacle.X, obtacle.Y); // |<-
            Segment obtacleSegment2 = new Segment(obtacle.X, obtacle.Y, obtacle.X + 1, obtacle.Y); // ¯¯
            Segment obtacleSegment3 = new Segment(obtacle.X + 1, obtacle.Y, obtacle.X + 1, obtacle.Y - 1); //  ->|
            Segment obtacleSegment4 = new Segment(obtacle.X, obtacle.Y - 1,obtacle.X - 1, obtacle.Y - 1); // _

            return new [] {obtacleSegment1, obtacleSegment2, obtacleSegment3, obtacleSegment4};
        }

        public static bool Equals(this IObstacle self, IObstacle other) {
            return self.X == other.X && self.Y == other.Y && string.Equals(self.TypeName, other.TypeName);
        }

        public static bool Equals(this IObstacle self, object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(self, obj)) return true;
            if (obj.GetType() != self.GetType()) return false;
            return self.Equals((IObstacle) obj);
        }

        public static int GetHashCode(this IObstacle self) {
            unchecked {
                int hashCode = self.TypeName?.GetHashCode() ?? 0;
                hashCode = (hashCode * 397) ^ self.X;
                hashCode = (hashCode * 397) ^ self.Y;
                return hashCode;
            }
        }
    }
}
