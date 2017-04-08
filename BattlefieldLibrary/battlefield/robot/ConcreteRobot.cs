using System;
using BaseLibrary.equip;

namespace BattlefieldLibrary.battlefield.robot {
    public abstract class ConcreteRobot : BattlefieldRobot {
        protected readonly BattlefieldRobot robot;
        protected ConcreteRobot(BattlefieldRobot robot) : base(robot.TEAM_ID, robot.ID, robot.SuperNetworkStream) {
            this.robot = robot;
        }

        public override int HitPoints {
            get { return robot.HitPoints; }
            set { robot.HitPoints = value; }
        }
        public override int Score {
            get { return robot.Score; }
            set { robot.Score = value; }
        }

        public override int OldScore {
            get { return robot.OldScore; }
            set { robot.OldScore = value; }
        }

        public override int Gold {
            get { return robot.Gold; }
            set { robot.Gold = value; }
        }
        public override double X {
            get { return robot.X; }
            set { robot.X = value; }
        }
        public override double Y {
            get { return robot.Y; }
            set { robot.Y = value; }
        }
        public override double Power {
            get { return robot.Power; }
            set { robot.Power = value; }
        }
        public override double WantedPower {
            get { return robot.WantedPower; }
            set { robot.WantedPower = value; }
        }
        public override double AngleDrive {
            get { return robot.AngleDrive; }
            set { robot.AngleDrive = value; }
        }
        
        public override Motor Motor {
            get { return robot.Motor; }
            set { robot.Motor = value; }
        }
        public override Armor Armor {
            get { return robot.Armor; }
            set { robot.Armor = value; }
        }

        public override DateTime LastRequestAt {
            get { return robot.LastRequestAt; }
            set { robot.LastRequestAt = value; }
        }
    }
}
