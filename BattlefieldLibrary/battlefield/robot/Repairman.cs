using BaseLibrary.command;
using BaseLibrary.command.common;
using BaseLibrary.command.handshake;
using BaseLibrary.equip;

namespace BattlefieldLibrary.battlefield.robot {
    public class Repairman : ConcreteRobot {

        public RepairTool RepairTool {get; set; }
        public int RepairToolUsed { get; set; }

        public Repairman(BattlefieldRobot robot) : base(robot) {
            ROBOT_TYPE = RobotType.REPAIRMAN;
            NAME = robot.NAME;
        }
    }
}
