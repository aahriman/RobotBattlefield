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

        public Tank() : base() {}
        public Tank(bool processStateAfterEveryCommand, bool processMerchant) : base(processStateAfterEveryCommand, processMerchant) {}

        public override RobotType GetRobotType() {
            return RobotType.TANK;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="angle">in degree. 0 = 3 hour. 90 = 6 hour and so on.</param>
        /// <param name="range">how far this robot wants to shot</param>
        public ShotAnswerCommand Shot(double angle, double range) {
            ShotAnswerCommand answer = taskWait(ShotAsync(angle, range));
            return answer;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="angle">in degree. 0 = 3 hour. 90 = 6 hour and so on.</param>
        /// <param name="range">how far this robot wants to shot</param>
        public async Task<ShotAnswerCommand> ShotAsync(double angle, double range) {
            await sendCommandAsync(new ShotCommand(range, angle));
            var commnad = sns.RecieveCommand();
            var answerCommand = (ShotAnswerCommand) commnad;
            if (processStateAfterEveryCommand) {
                ProcessState(await StateAsync());
            }
            return answerCommand;
        }

        protected override void SetClassEquip(int id) {
            GUN = GUNS_BY_ID[id];
        }

        protected override ClassEquipment GetClassEquip() {
            return GUN;
        }
    }
}
