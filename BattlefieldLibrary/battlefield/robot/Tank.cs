using BaseLibrary;
using BaseLibrary.command;
using BaseLibrary.command.common;
using BaseLibrary.command.handshake;
using BaseLibrary.equip;
using JetBrains.Annotations;

namespace BattlefieldLibrary.battlefield.robot {
    public class Tank : BattlefieldRobot {

        public Gun Gun { get; set; }
        public int GunsToLoad { get; set; }

        public Tank(int teamId, int id, [NotNull] NetworkStream networkStream) : base(teamId, id, networkStream) {
            ROBOT_TYPE = RobotType.TANK;
        }
    }
}
