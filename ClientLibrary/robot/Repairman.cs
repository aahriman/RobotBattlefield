using System.Threading.Tasks;
using BaseLibrary.command;
using BaseLibrary.command.common;
using BaseLibrary.command.handshake;
using BaseLibrary.command.miner;
using BaseLibrary.command.repairman;
using BaseLibrary.equip;

namespace ClientLibrary.robot {
    public class Repairman : ClientRobot {
        public RepairTool REPAIR_TOOL { get; private set; }

        public Repairman() : base() { }
        public Repairman(bool processStateAfterEveryCommand, bool processMerchant) : base(processStateAfterEveryCommand, processMerchant) { }


        public RepairAnswerCommand Repair() {
            RepairAnswerCommand answer = taskWait(RepairAsync());
            return answer;
        }

        public RepairAnswerCommand Repair(int maxDistance) {
            RepairAnswerCommand answer = taskWait(RepairAsync(maxDistance));
            return answer;
        }

        public async Task<RepairAnswerCommand> RepairAsync() {
            await sendCommandAsync(new RepairCommand());
            var commnad = sns.RecieveCommand();
            var answerCommand = (RepairAnswerCommand) commnad;
            if (processStateAfterEveryCommand) {
                ProcessState(await StateAsync());
            }
            return answerCommand;
        }

        public async Task<RepairAnswerCommand> RepairAsync(int maxDistance) {
            await sendCommandAsync(new RepairCommand(maxDistance));
            var commnad = sns.RecieveCommand();
            var answerCommand = (RepairAnswerCommand) commnad;
            if (processStateAfterEveryCommand) {
                ProcessState(await StateAsync());
            }
            return answerCommand;
        }

        public override RobotType GetRobotType() {
            return RobotType.REPAIRMAN;
        }

        protected override void setClassEquip(int id) {
            REPAIR_TOOL = REPAIR_TOOLS_BY_ID[id];
        }

        protected override ClassEquipment getClassEquip() {
            return REPAIR_TOOL;
        }
    }
}
