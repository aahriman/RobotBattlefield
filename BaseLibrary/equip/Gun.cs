﻿using System;

namespace BaseLibrary.equip {
	public class Gun : ClassEquipment {
		
		public int ID { get; private set; }
		public int COST { get; private set; }
		public int MAX_BULLETS { get; private set; }
		public int MAX_RANGE { get; private set; }
		public double SHOT_SPEED { get; private set; }
		public Zone[] ZONES { get; private set; }

		public Gun(int ID, int COST, int MAX_BULLETS, int MAX_RANGE, double SHOT_SPEED, Zone[] ZONES) {
			if (ZONES == null) throw new ArgumentNullException("zones");
			this.ID = ID;
			this.COST = COST;
			this.MAX_BULLETS = MAX_BULLETS;
            this.MAX_RANGE = MAX_RANGE;
            this.SHOT_SPEED = SHOT_SPEED;
            this.ZONES = new Zone[ZONES.Length];
			for (int i = 0; i < ZONES.Length; i++) {
				this.ZONES[i] = ZONES[i];
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
				hashCode = (hashCode * 397) ^ (ZONES?.GetHashCode() ?? 0);
				return hashCode;
			}
		}
	}


}
