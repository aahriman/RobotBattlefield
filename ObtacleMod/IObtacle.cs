using System.Drawing;
using BaseLibrary.utils.euclidianSpaceStruct;

namespace ObtacleMod {
    public interface IObtacle {
        string TypeName { get; }

        int X { get; }
        int Y { get; }


        bool Used { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="xScale">How many pixels is one unit</param>
        /// <param name="yScale">How many pixels is one unit</param>
        void Draw(Graphics graphics, float xScale, float yScale);
    }

    public static class ObtacleExtensions {
        public static Segment[] Segments(this IObtacle obtacle) {
            Segment obtacleSegment1 = new Segment(obtacle.X, obtacle.Y - 1, obtacle.X, obtacle.Y); // |<-
            Segment obtacleSegment2 = new Segment(obtacle.X, obtacle.Y, obtacle.X + 1, obtacle.Y); // ¯¯
            Segment obtacleSegment3 = new Segment(obtacle.X + 1, obtacle.Y, obtacle.X + 1, obtacle.Y - 1); //  ->|
            Segment obtacleSegment4 = new Segment(obtacle.X, obtacle.Y - 1,obtacle.X - 1, obtacle.Y - 1); // _

            return new [] {obtacleSegment1, obtacleSegment2, obtacleSegment3, obtacleSegment4};
        }

        public static bool Equals(this IObtacle self, IObtacle other) {
            return self.X == other.X && self.Y == other.Y && string.Equals(self.TypeName, other.TypeName);
        }

        public static bool Equals(this IObtacle self, object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(self, obj)) return true;
            if (obj.GetType() != self.GetType()) return false;
            return self.Equals((IObtacle) obj);
        }

        public static int GetHashCode(this IObtacle self) {
            unchecked {
                int hashCode = self.TypeName?.GetHashCode() ?? 0;
                hashCode = (hashCode * 397) ^ self.X;
                hashCode = (hashCode * 397) ^ self.Y;
                return hashCode;
            }
        }
    }
}
