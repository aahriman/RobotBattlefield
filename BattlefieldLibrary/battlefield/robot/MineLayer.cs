using System.Collections.Generic;
using BaseLibrary;
using BaseLibrary.command;
using BaseLibrary.command.common;
using BaseLibrary.command.handshake;
using BaseLibrary.equip;
using JetBrains.Annotations;

namespace BattlefieldLibrary.battlefield.robot {
    public class MineLayer : BattlefieldRobot {

        public MineGun MineGun { get; set; }
        public int MinesNow { get; set; }

        public readonly IDictionary<int, Mine> MINES_BY_ID = new Dictionary<int, Mine>();
        
        public MineLayer(int teamId, int id, [NotNull] NetworkStream networkStream) : base(teamId, id, networkStream) {
            ROBOT_TYPE = RobotType.MINE_LAYER;
        }
    }
}
