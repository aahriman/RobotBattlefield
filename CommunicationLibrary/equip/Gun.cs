using System;

namespace CommunicationLibrary.equip {
	public class Gun {
		public class Zone {
			public int DISTANCE { get; private set; }
			public int DAMAGE { get; private set; }

			public Zone(int distance, int damage) {
				DISTANCE = distance;
				DAMAGE = damage;
			}

			protected bool Equals(Zone other) {
				return DISTANCE == other.DISTANCE && DAMAGE == other.DAMAGE;
			}

			public override bool Equals(object obj) {
				if (ReferenceEquals(null, obj)) return false;
				if (ReferenceEquals(this, obj)) return true;
				return obj.GetType() == GetType() && Equals((Zone)obj);
			}

			public override int GetHashCode() {
				unchecked {
					return (DISTANCE * 397) ^ DAMAGE;
				}
			}
		}
		public int ID { get; private set; }
		public int COST { get; private set; }
		public int MAX_BULLETS { get; private set; }
		public int MAX_RANGE { get; private set; }
		public double SHOT_SPEED { get; private set; }
		public Zone[] ZONES { get; private set; }

		public Gun(int id, int cost, int maxBullets, int maxRange, double shotSpeed, Zone[] zones) {
			if (zones == null) throw new ArgumentNullException("zones");
			ID = id;
			COST = cost;
			MAX_BULLETS = maxBullets;
			MAX_RANGE = maxRange;
			SHOT_SPEED = shotSpeed;
			ZONES = new Zone[zones.Length];
			for (int i = 0; i < zones.Length; i++) {
				ZONES[i] = zones[i];
			}
		}

		protected bool Equals(Gun other) {
			return ID == other.ID && COST == other.COST && MAX_BULLETS == other.MAX_BULLETS && MAX_RANGE == other.MAX_RANGE && SHOT_SPEED.Equals(other.SHOT_SPEED) && Equals(ZONES, other.ZONES);
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
				hashCode = (hashCode * 397) ^ MAX_BULLETS;
				hashCode = (hashCode * 397) ^ MAX_RANGE;
				hashCode = (hashCode * 397) ^ SHOT_SPEED.GetHashCode();
				hashCode = (hashCode * 397) ^ (ZONES != null ? ZONES.GetHashCode() : 0);
				return hashCode;
			}
		}
	}


}
