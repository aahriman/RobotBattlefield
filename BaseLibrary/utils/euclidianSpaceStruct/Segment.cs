namespace BaseLibrary.utils.euclidianSpaceStruct {
    public struct Segment {

        public static bool operator ==(Segment first, Segment second) {
            return first.From == second.From && first.To == second.To; ;
        }

        public static bool operator !=(Segment first, Segment second) {
            return !(first == second);
        }


        public Point From;
        public Point To;

        public Segment(double fromX, double fromY, double toX, double toY) : this(new Point(fromX, fromY), new Point(toX, toY))  {}

        public Segment(Point from, Point to) {
            From = from;
            To = to;
        }

        public bool Equals(Segment other) {
            return this == other;
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Segment && this == ((Segment)obj);
        }

        public override int GetHashCode() {
            unchecked {
                return (From.GetHashCode() * 397) ^ To.GetHashCode();
            }
        }

        public override string ToString() {
            return $"Segment{{From={From}, To={To}}}";
        }
    }
}
