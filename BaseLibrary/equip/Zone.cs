namespace BaseLibrary.equip {
    public class Zone {

        public static Zone GetZoneByDistance(Zone[] zones, double distance) {
            foreach (Zone zone in zones) {
                if (zone.DISTANCE > distance) {
                    return zone;
                }
            }
            return NULL_ZONE;
        }

        public static readonly Zone NULL_ZONE = new Zone(int.MaxValue, 0);

        public int DISTANCE { get; private set; }
        public int EFFECT { get; private set; }

        public Zone(int distance, int effect) {
            DISTANCE = distance;
            EFFECT = effect;
        }

        protected bool Equals(Zone other) {
            return DISTANCE == other.DISTANCE && EFFECT == other.EFFECT;
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Zone)obj);
        }

        public override int GetHashCode() {
            unchecked {
                return (DISTANCE * 397) ^ EFFECT;
            }
        }

        public override string ToString() {
            return $"{nameof(DISTANCE)}: {DISTANCE}, {nameof(EFFECT)}: {EFFECT}";
        }
    }
}
