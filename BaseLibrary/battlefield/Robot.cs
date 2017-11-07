﻿using System;
using BaseLibrary.equip;
using BaseLibrary.utils.euclidianSpaceStruct;

namespace BaseLibrary.battlefield {
    public abstract class Robot {

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
        public abstract int HitPoints { get; set; }

        /// <summary>
        /// Score which robot has.
        /// </summary>
        public abstract int Score { get; set; }

        /// <summary>
        /// Gold which robot has.
        /// </summary>
        public abstract int Gold { get; set; }

        /// <summary>
        /// X-coordinate of robot.
        /// </summary>
        public abstract double X { get; set; }

        /// <summary>
        /// Y-coordinate of robot.
        /// </summary>
        public abstract double Y { get; set; }

        /// <summary>
        /// Power which robot's motor currently use.
        /// </summary>
        public abstract double Power { get; set; }

        /// <summary>
        /// Angle where robot goes.
        /// </summary>
        public abstract double AngleDrive { get; set; }

        /// <summary>
        /// What motor do robot has.
        /// </summary>
        /// <seealso cref="Motor"/>
        public abstract Motor Motor { get; set; }

        /// <summary>
        /// What armor do robot has.
        /// </summary>
        /// <seealso cref="Armor"/>
        public abstract Armor Armor { get; set; }

        /// <summary>
        /// Get robot position (useful for <code>Utils</code>)
        /// </summary>
        /// <returns></returns>
        public Point GetPosition() {
            return new Point(X, Y);
        }

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
