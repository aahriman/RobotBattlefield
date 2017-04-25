using System.Threading.Tasks;
using BaseLibrary;
using BaseLibrary.command;
using BaseLibrary.command.common;
using BaseLibrary.command.handshake;
using BaseLibrary.command.tank;
using BaseLibrary.equip;

namespace ClientLibrary.robot {
    public class Tank : ClientRobot {

        public Gun GUN { get; private set; }

        public Tank() : base() {}
        public Tank(bool processStateAfterEveryCommand, bool processMerchant) : base(processStateAfterEveryCommand, processMerchant) {}

        public override RobotType GetRobotType() {
            return RobotType.TANK;
        }

        public ShotAnswerCommand Shot(double range, double angle) {
            ShotAnswerCommand answer = taskWait(ShotAsync(range, angle));
            return answer;
        }

        public async Task<ShotAnswerCommand> ShotAsync(double range, double angle) {
            await sendCommandAsync(new ShotCommand((ProtocolDouble)range, (ProtocolDouble)angle));
            var commnad = sns.RecieveCommand();
            var answerCommand = (ShotAnswerCommand) commnad;
            if (processStateAfterEveryCommand) {
                ProcessState(await StateAsync());
            }
            return answerCommand;
        }

        protected override void setClassEquip(int id) {
            GUN = GUNS_BY_ID[id];
        }

        protected override ClassEquipment getClassEquip() {
            return GUN;
        }
    }
}
