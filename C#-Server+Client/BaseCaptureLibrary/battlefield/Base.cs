namespace BaseCaptureLibrary.battlefield {
    public class Base {
        /// <summary>
        /// X-coordinate of center of base
        /// </summary>
        public readonly double X;

        /// <summary>
        /// Y-coordinate of center of base
        /// </summary>
        public readonly double Y;

        /// <summary>
        /// How height have to be progress to capture this base.
        /// </summary>
        public readonly int MAX_PROGRESS;

        /// <summary>
        /// Type name for de-serialization used.
        /// </summary>
        public readonly string TYPE_NAME;

        /// <summary>
        /// How much is this base captured.
        /// </summary>
        public int Progress { get; set; }

        /// <summary>
        /// Witch team owns this base.
        /// </summary>
        public int TeamId { get; set; }

        /// <summary>
        /// Witch team is capturing the base.
        /// </summary>
        public int ProgressTeamId { get; set; }

        public Base(double x, double y, int MAX_PROGRESS) {
            X = x;
            Y = y;
            this.MAX_PROGRESS = MAX_PROGRESS;
            TYPE_NAME = this.GetType().ToString();
        }

        protected bool Equals(Base other) {
            return Equals(X, other.X) && Equals(Y, other.Y) && MAX_PROGRESS == other.MAX_PROGRESS;
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Base) obj);
        }

        public override int GetHashCode() {
            unchecked {
                int hashCode = X.GetHashCode();
                hashCode = (hashCode * 397) ^ Y.GetHashCode();
                hashCode = (hashCode * 397) ^ MAX_PROGRESS;
                return hashCode;
            }
        }
    }
}
