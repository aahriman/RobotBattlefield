using System;
using BaseLibrary;
using BaseLibrary.equip;

namespace BattlefieldLibrary.battlefield.robot {
    public class DefaultRobot : BattlefieldRobot {
        public DefaultRobot(int id, SuperNetworkStream superNetworkStream) : base(id, superNetworkStream) {}
        
        public override int HitPoints { get; set; }
        public override int Score { get; set; }
        public override int OldScore { get; set; }
        public override int Gold { get; set; }
        public override double X { get; set; }
        public override double Y { get; set; }
        public override double Power { get; set; }
        public override double WantedPower { get; set; }
        public override double AngleDrive { get; set; }
        
        public override Motor Motor { get; set; }
        public override Armor Armor { get; set; }
        public override DateTime LastRequestAt { get; set; }
    }
}
