using System;
using BaseLibrary.equip;
using BaseLibrary.utils.euclidianSpaceStruct;

namespace BaseLibrary.battlefield {
    public abstract class Robot {
        public int ID { get; protected set; }

        public int TEAM_ID {
            get { return _teamID; }
            set {
                if (_teamID != value) {
                    if (_teamID == default(int)) {
                        _teamID = value;
                    } else {
                        throw new NotSupportedException("Team id can be set only once.");
                    }
                }
            }
        }

        private int _teamID = default(int);

        public abstract int HitPoints { get; set; }
        public abstract int Score { get; set; }
        public abstract int Gold { get; set; }
        public abstract double X { get; set; }
        public abstract double Y { get; set; }
        public abstract double Power { get; set; }
        public abstract double AngleDrive { get; set; }

        public abstract Motor Motor { get; set; }
        public abstract Armor Armor { get; set; }

        public Point GetPosition() {
            return new Point(X, Y);
        }

        /// <inheritdoc />
        public override string ToString() {
            return $"Robot: {{ ID: {ID}, HitPoints: {HitPoints}, Position: [{X},{Y}]";
        }

        protected bool Equals(Robot other) {
            return ID == other.ID;
        }

        /// <inheritdoc />
        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Robot) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode() {
            return ID;
        }
    }
}
