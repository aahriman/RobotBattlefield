using BaseLibrary;
using BaseLibrary.communication;
using BaseLibrary.communication.command.handshake;
using BaseLibrary.equipment;
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
