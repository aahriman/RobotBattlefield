namespace BaseLibrary.equip
{
    public class Armor : IEquipment{
        /// <summary>
        /// How taught is armor. How many hit points can robot have.
        /// </summary>
        public int MAX_HP { get; private set; }

        /// <inheritdoc />
        public int COST { get; private set; }

        /// <inheritdoc />
        public int ID { get; private set; }

        public Armor(int ID, int COST, int MAX_HP) {
            this.MAX_HP = MAX_HP;
            this.COST = COST;
            this.ID = ID;
        }

        protected bool Equals(Armor other) {
            return MAX_HP == other.MAX_HP && COST == other.COST && ID == other.ID;
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Armor) obj);
        }

        public override int GetHashCode() {
            unchecked {
                int hashCode = MAX_HP;
                hashCode = (hashCode * 397) ^ COST;
                hashCode = (hashCode * 397) ^ ID;
                return hashCode;
            }
        }

        public override string ToString() {
            return $"{nameof(MAX_HP)}: {MAX_HP}, {nameof(COST)}: {COST}, {nameof(ID)}: {ID}";
        }
    }
}
