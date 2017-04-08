using System.Collections.Generic;
using BaseLibrary.command;
using BaseLibrary.equip;

namespace BattlefieldLibrary.battlefield.robot {
    public class Miner : ConcreteRobot {

        public MineGun MineGun { get; set; }
        public int MinesNow { get; set; }

        public readonly IDictionary<int, Mine> MINES_BY_ID = new Dictionary<int, Mine>();

        public Miner(BattlefieldRobot robot) : base(robot) {
            ROBOT_TYPE = RobotType.MINER;
            NAME = robot.NAME;
        }
    }
}
