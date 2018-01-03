using BaseLibrary;
using BaseLibrary.command;
using BaseLibrary.command.common;
using BaseLibrary.command.handshake;
using BaseLibrary.equip;
using JetBrains.Annotations;

namespace BattlefieldLibrary.battlefield.robot {
    public class Repairman : BattlefieldRobot {

        public RepairTool RepairTool {get; set; }
        public int RepairToolUsed { get; set; }

        public Repairman(int teamId, int id, [NotNull] NetworkStream networkStream) : base(teamId, id, networkStream) {
            ROBOT_TYPE = RobotType.REPAIRMAN;
        }
    }
}
