using System;

namespace BaseLibrary.equip {
    public class RepairTool : IClassEquipment {
        /// <inheritdoc />
        public int ID { get; private set; }

        /// <inheritdoc />
        public int COST { get; private set; }

        /// <summary>
        /// How many times per lap can be this robot tool used.
        /// </summary>
        public int MAX_USAGES { get; private set; }

        /// <summary>
        /// Effect of reparation. Their hit point gain and range of effect.
        /// </summary>
        public Zone[] ZONES { get; private set; }

        public RepairTool(int ID, int COST, int MAX_USAGES, Zone[] ZONES) {
            if (ZONES == null) throw new ArgumentNullException("zones");
            this.ID = ID;
            this.COST = COST;
            this.MAX_USAGES = MAX_USAGES;
            this.ZONES = new Zone[ZONES.Length];
            for (int i = 0; i < ZONES.Length; i++) {
                this.ZONES[i] = ZONES[i];
            }
        }

        protected bool Equals(Gun other) {
            return ID == other.ID && COST == other.COST && MAX_USAGES == other.MAX_BULLETS && Equals(ZONES, other.ZONES);
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
                hashCode = (hashCode * 397) ^ MAX_USAGES;
                hashCode = (hashCode * 397) ^ (ZONES?.GetHashCode() ?? 0);
                return hashCode;
            }
        }

        public override string ToString() {
            return $"{nameof(ID)}: {ID}, {nameof(COST)}: {COST}, {nameof(MAX_USAGES)}: {MAX_USAGES}, {nameof(ZONES)}: [{string.Join<Zone>(";", ZONES)}]";
        }
    }
}
