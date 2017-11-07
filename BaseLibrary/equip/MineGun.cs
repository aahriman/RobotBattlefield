using System;

namespace BaseLibrary.equip {
    public class MineGun : IClassEquipment{
        /// <inheritdoc />
        public int ID { get; private set; }

        /// <inheritdoc />
        public int COST { get; private set; }

        /// <summary>
        /// How many mines can be putted on map at the same time.
        /// </summary>
        public int MAX_MINES { get; private set; }

        /// <summary>
        /// Effect of mines. Their damaging and range of effect.
        /// </summary>
        public Zone[] ZONES { get; private set; }

        public MineGun(int ID, int COST, int MAX_MINES, Zone[] ZONES) {
            if (ZONES == null) throw new ArgumentNullException("zones");
            this.ID = ID;
            this.COST = COST;
            this.MAX_MINES = MAX_MINES;
            this.ZONES = new Zone[ZONES.Length];
            for (int i = 0; i < ZONES.Length; i++) {
                this.ZONES[i] = ZONES[i];
            }
        }

        /// <summary>
        /// Compare if both mine guns are same.
        /// </summary>
        /// <param name="other">Compared mine gun with this mine gun.</param>
        /// <returns></returns>
        protected bool Equals(MineGun other) {
            return ID == other.ID && COST == other.COST && MAX_MINES == other.MAX_MINES && Equals(ZONES, other.ZONES);
        }

        /// <inheritdoc />
        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((MineGun)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode() {
            unchecked {
                int hashCode = ID;
                hashCode = (hashCode * 397) ^ COST;
                hashCode = (hashCode * 397) ^ MAX_MINES;
                hashCode = (hashCode * 397) ^ (ZONES?.GetHashCode() ?? 0);
                return hashCode;
            }
        }

        /// <inheritdoc />
        public override string ToString() {
            return $"{nameof(ID)}: {ID}, {nameof(COST)}: {COST}, {nameof(MAX_MINES)}: {MAX_MINES}, {nameof(ZONES)}: [{string.Join<Zone>(";", ZONES)}]";
        }
    }
}
