namespace BaseLibrary.equipment {
	public class Motor : IEquipment{

        /// <summary>
        /// Max robot's speed in units. Arena is 1000 x 1000 units large.
        /// </summary>
		public double MAX_SPEED { get; private set; }

        /// <summary>
        /// In what power can robot make rotation.
        /// </summary>
		public double ROTATE_IN { get; private set; }

        /// <summary>
        /// Acceleration of motor in percent of power.
        /// </summary>
		public double ACCELERATION { get; private set; }

	    /// <summary>
	    /// Deceleration of motor in percent of power.
	    /// </summary>
		public double DECELERATION { get; private set; }

        /// <summary>
        /// Immediately performance from 0. What power motor immediately can get. Power is always in percentage. And robot go <code>robot.POWER*MAX_SPEED</code> units per turn.
        /// </summary>
		public double MAX_INITIAL_POWER { get; private set; }

        /// <inheritdoc />
		public int COST { get; private set; }

	    /// <inheritdoc />
		public int ID { get; private set; }

		public Motor(double MAX_SPEED, double ROTATE_IN, double ACCELERATION, double DECELERATION, double MAX_INITIAL_POWER, int COST, int ID) {
			this.MAX_SPEED = MAX_SPEED;
            this.ROTATE_IN = ROTATE_IN;
            this.ACCELERATION = ACCELERATION;
            this.DECELERATION = DECELERATION;
            this.MAX_INITIAL_POWER = MAX_INITIAL_POWER;
            this.COST = COST;
            this.ID = ID;
		}

        /// <inheritdoc />
		public override bool Equals(object obj) {
			Motor m = obj as Motor;
			return m != null &&
				MAX_SPEED == m.MAX_SPEED &&
				ROTATE_IN == m.ROTATE_IN &&
				ACCELERATION == m.ACCELERATION &&
				DECELERATION == m.DECELERATION &&
				MAX_INITIAL_POWER == m.MAX_INITIAL_POWER &&
				COST == m.COST &&
				ID == m.ID;
		}

	    /// <inheritdoc />
        public override int GetHashCode() {
            unchecked {
                var hashCode = MAX_SPEED.GetHashCode();
                hashCode = (hashCode * 397) ^ ROTATE_IN.GetHashCode();
                hashCode = (hashCode * 397) ^ ACCELERATION.GetHashCode();
                hashCode = (hashCode * 397) ^ DECELERATION.GetHashCode();
                hashCode = (hashCode * 397) ^ MAX_INITIAL_POWER.GetHashCode();
                hashCode = (hashCode * 397) ^ COST;
                hashCode = (hashCode * 397) ^ ID;
                return hashCode;
            }
        }

	    /// <inheritdoc />
	    public override string ToString() {
	        return $"{nameof(MAX_SPEED)}: {MAX_SPEED}, {nameof(ROTATE_IN)}: {ROTATE_IN}, {nameof(ACCELERATION)}: {ACCELERATION}, {nameof(DECELERATION)}: {DECELERATION}, {nameof(MAX_INITIAL_POWER)}: {MAX_INITIAL_POWER}, {nameof(COST)}: {COST}, {nameof(ID)}: {ID}";
	    }
	}
}
