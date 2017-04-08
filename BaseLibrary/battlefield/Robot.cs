﻿using System;
using BaseLibrary.equip;

namespace BaseLibrary.battlefield {
    public abstract class Robot {
        public int ID { get; protected set; }

        public int TEAM_ID {
            get { return _teamID; }
            set {
                if (_teamID == default(int)) {
                    _teamID = value;
                } else {
                    throw new NotSupportedException("Team id can be set only once.");
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
    }
}
