using System;

namespace BaseLibrary.equipment {
	public class Gun : IClassEquipment {

        /// <inheritdoc />
        public int ID { get; private set; }

	    /// <inheritdoc />
		public int COST { get; private set; }

        /// <summary>
        /// How many barrel this gun has. Every barrel can be use again after reload time (<code>Battlefield.RELOAD_TIME</code>)
        /// </summary>
        /// <seealso cref="BattlefieldLibrary.Battlefield.RELOAD_TIME"/>
		public int BARREL_NUMBER { get; private set; }

        /// <summary>
        /// How far can gun shoot.
        /// </summary>
		public int MAX_RANGE { get; private set; }

        /// <summary>
        /// How fast bullet fly.
        /// </summary>
		public double SHOT_SPEED { get; private set; }

	    /// <summary>
	    /// Effect of bullets. Their damaging and range of effect.
	    /// </summary>
		public Zone[] ZONES { get; private set; }

		public Gun(int ID, int COST, int BARREL_NUMBER, int MAX_RANGE, double SHOT_SPEED, Zone[] ZONES) {
			if (ZONES == null) throw new ArgumentNullException("zones");
			this.ID = ID;
			this.COST = COST;
			this.BARREL_NUMBER = BARREL_NUMBER;
            this.MAX_RANGE = MAX_RANGE;
            this.SHOT_SPEED = SHOT_SPEED;
            this.ZONES = new Zone[ZONES.Length];
			for (int i = 0; i < ZONES.Length; i++) {
				this.ZONES[i] = ZONES[i];
			}
		}

		protected bool Equals(Gun other) {
			return ID == other.ID && COST == other.COST && BARREL_NUMBER == other.BARREL_NUMBER && MAX_RANGE == other.MAX_RANGE && SHOT_SPEED.Equals(other.SHOT_SPEED) && Equals(ZONES, other.ZONES);
		}

		public override bool Equals(object obj) {
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != GetType()) return false;
			return Equals((Gun)obj);
		}

		public override int GetHashCode() {
			unchecked {
				int hashCode = ID;
				hashCode = (hashCode * 397) ^ COST;
				hashCode = (hashCode * 397) ^ BARREL_NUMBER;
				hashCode = (hashCode * 397) ^ MAX_RANGE;
				hashCode = (hashCode * 397) ^ SHOT_SPEED.GetHashCode();
				hashCode = (hashCode * 397) ^ (ZONES?.GetHashCode() ?? 0);
				return hashCode;
			}
		}

	    public override string ToString() {
	        return $"{nameof(ID)}: {ID}, {nameof(COST)}: {COST}, {nameof(BARREL_NUMBER)}: {BARREL_NUMBER}, {nameof(MAX_RANGE)}: {MAX_RANGE}, {nameof(SHOT_SPEED)}: {SHOT_SPEED}, {nameof(ZONES)}: [{string.Join<Zone>(";", ZONES)}]";
	    }
	}


}
