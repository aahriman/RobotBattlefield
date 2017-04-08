using System.Linq;
using BaseLibrary.command;
using BattlefieldLibrary;
using BattlefieldLibrary.battlefield;
using FlagCapcureBattlefield.battlefield;
using ServerLibrary.config;

namespace FlagCapcureBattlefield {
    public class Server : AServer{
        public Server(int port) : base(port) {
        }

        public override GameTypeCommand GetGameTypeCommand(Battlefield battlefield) {
            return new GameTypeCommand(battlefield.ROBOTS_IN_TEAM, GameType.CAPTURE_FLAG);
        }

        protected override Battlefield NewBattlefield(int ROBOT_TO_ONE_ARENA, int ROBOTS_IN_TEAM, params object[] more) {
            return new FlagCapture(ROBOT_TO_ONE_ARENA, ROBOTS_IN_TEAM, fromMoreToFlags(more));
        }

        protected override Battlefield NewBattlefield(int ROBOT_TO_ONE_ARENA, int ROBOTS_IN_TEAM, string EQUIPMENT_FILE, params object[] more) {
            return new FlagCapture(ROBOT_TO_ONE_ARENA, ROBOTS_IN_TEAM, EQUIPMENT_FILE, fromMoreToFlags(more));
        }

        private Flag[] fromMoreToFlags(object[] more) {
            return (
                from m in more
                where (m is Flag)
                select m as Flag).ToArray();
        }
    }
}
