using System;
using BaseLibrary.equipment;
using BaseLibrary.utils.euclidianSpaceStruct;

namespace BaseLibrary.battlefield {
    public abstract class Robot {

        /// <summary>
        /// Operator equal for robots.
        /// </summary>
        /// <param name="r1"></param>
        /// <param name="r2"></param>
        /// <returns></returns>
        public static bool operator ==(Robot r1, Robot r2) {
            if (!ReferenceEquals(null, r1)) {
                return r1.Equals((object) r2);
            } else if (ReferenceEquals(null, r2)) {
                return true;
            } else { 
                return false;
            }
            
        }

        /// <summary>
        /// Operator not equal for robots.
        /// </summary>
        /// <param name="r1"></param>
        /// <param name="r2"></param>
        /// <returns></returns>
        public static bool operator !=(Robot r1, Robot r2) {
            return !(r1 == r2);
        }

        /// <summary>
        /// Robot id.
        /// </summary>
        public int ID { get; protected set; }

        /// <summary>
        /// Robot team.
        /// </summary>
        public int TEAM_ID {
            get {
                if (_teamID == null) {
                    throw new NotSupportedException("Team id was not set, so cannot access it.");
                } else {
                    return (int) _teamID;
                }
            }
            set {
                if (_teamID == value) return;
                if (_teamID == null) {
                    _teamID = value;
                } else {
                    throw new NotSupportedException("Team id can be set only once.");
                }
            }
        }

        private int? _teamID = null;

        /// <summary>
        /// HitPoints which robot has.
        /// </summary>
        public int HitPoints { get; set; }

        /// <summary>
        /// Score which robot has.
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// Gold which robot has.
        /// </summary>
        public int Gold { get; set; }

        /// <summary>
        /// X-coordinate of robot.
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Y-coordinate of robot.
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Power which robot's motor currently use.
        /// </summary>
        public double Power { get; set; }

        /// <summary>
        /// Angle where robot goes.
        /// </summary>
        public double AngleDrive { get; set; }

        /// <summary>
        /// What motor do robot has.
        /// </summary>
        /// <seealso cref="Motor"/>
        public Motor Motor { get; set; }

        /// <summary>
        /// What armor do robot has.
        /// </summary>
        /// <seealso cref="Armor"/>
        public Armor Armor { get; set; }

        /// <summary>
        /// Get robot position (useful for <code>Utils</code>)
        /// </summary>
        /// <returns></returns>
        public Point Position => new Point(X, Y);
        

        /// <inheritdoc />
        public override string ToString() {
            return $"Robot: {{ ID: {ID}, HitPoints: {HitPoints}, Position: [{X},{Y}]";
        }

        /// <summary>
        /// Compare two instances of robots.
        /// </summary>
        /// <param name="other">Compared robot with this robot.</param>
        /// <returns></returns>
        protected bool Equals(Robot other) {
            return ID != default(int) && ID == other.ID;
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
