using BaseLibrary.command;
using BaseLibrary.command.common;
using BaseLibrary.command.handshake;
using BaseLibrary.equip;

namespace BattlefieldLibrary.battlefield.robot {
    public class Tank : ConcreteRobot {

        public Gun Gun { get; set; }
        public int BulletsNow { get; set; }

        public Tank(BattlefieldRobot robot) : base(robot) {
            ROBOT_TYPE = RobotType.TANK;
            NAME = robot.NAME;
        }
    }
}
