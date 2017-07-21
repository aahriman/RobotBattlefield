namespace BaseLibrary.equip {
	public class Motor : Equipment{
		public double MAX_SPEED { get; private set; }
		public double ROTATE_IN { get; private set; }
		public double SPEED_UP { get; private set; }
		public double SPEED_DOWN { get; private set; }
		public double SPEED_UP_TO { get; private set; }
		public int COST { get; private set; }
		public int ID { get; private set; }

		public Motor(double MAX_SPEED, double ROTATE_IN, double SPEED_UP, double SPEED_DOWN, double SPEED_UP_TO, int COST, int ID) {
			this.MAX_SPEED = MAX_SPEED;
            this.ROTATE_IN = ROTATE_IN;
            this.SPEED_UP = SPEED_UP;
            this.SPEED_DOWN = SPEED_DOWN;
            this.SPEED_UP_TO = SPEED_UP_TO;
            this.COST = COST;
            this.ID = ID;
		}

		public override bool Equals(object obj) {
			Motor m = obj as Motor;
			return m != null &&
				MAX_SPEED == m.MAX_SPEED &&
				ROTATE_IN == m.ROTATE_IN &&
				SPEED_UP == m.SPEED_UP &&
				SPEED_DOWN == m.SPEED_DOWN &&
				SPEED_UP_TO == m.SPEED_UP_TO &&
				COST == m.COST &&
				ID == m.ID;
		}

        public override int GetHashCode() {
            unchecked {
                var hashCode = MAX_SPEED.GetHashCode();
                hashCode = (hashCode * 397) ^ ROTATE_IN.GetHashCode();
                hashCode = (hashCode * 397) ^ SPEED_UP.GetHashCode();
                hashCode = (hashCode * 397) ^ SPEED_DOWN.GetHashCode();
                hashCode = (hashCode * 397) ^ SPEED_UP_TO.GetHashCode();
                hashCode = (hashCode * 397) ^ COST;
                hashCode = (hashCode * 397) ^ ID;
                return hashCode;
            }
        }

	    public override string ToString() {
	        return $"{nameof(MAX_SPEED)}: {MAX_SPEED}, {nameof(ROTATE_IN)}: {ROTATE_IN}, {nameof(SPEED_UP)}: {SPEED_UP}, {nameof(SPEED_DOWN)}: {SPEED_DOWN}, {nameof(SPEED_UP_TO)}: {SPEED_UP_TO}, {nameof(COST)}: {COST}, {nameof(ID)}: {ID}";
	    }
	}
}
