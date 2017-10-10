using System;
using System.Threading.Tasks;
using BaseLibrary;
using BaseLibrary.command;
using BaseLibrary.command.common;
using BaseLibrary.command.handshake;
using BaseLibrary.command.tank;
using BaseLibrary.equip;
using BaseLibrary.protocol;

namespace ClientLibrary.robot {
    public class Tank : ClientRobot {

        public Gun GUN { get; private set; }

        public Tank(String name, String teamName) : base(name, teamName) {}

        public override RobotType GetRobotType() {
            return RobotType.TANK;
        }

        /// <summary>
        /// Shoot bullet.
        /// </summary>
        /// <param name="angle">in degree. 0 = 3 hour. 90 = 6 hour and so on.</param>
        /// <param name="range">how far this robot wants to shot</param>
        public ShotAnswerCommand Shot(double angle, double range) {
            ShotAnswerCommand answer = taskWait(ShotAsync(angle, range));
            return answer;
        }

        /// <summary>
        /// Shoot bullet and immediattely return. For answer you have to wait.
        /// </summary>
        /// <param name="angle">in degree. 0 = 3 hour. 90 = 6 hour and so on.</param>
        /// <param name="range">how far this robot wants to shot</param>
        public async Task<ShotAnswerCommand> ShotAsync(double angle, double range) {
            await sendCommandAsync(new ShotCommand(range, angle));
            return recieveCommand<ShotAnswerCommand>();
        }

        protected override void setClassEquip(int id) {
            GUN = GUNS_BY_ID[id];
        }

        protected override ClassEquipment getClassEquip() {
            return GUN;
        }
    }
}
