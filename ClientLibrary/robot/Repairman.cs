using System;
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

        public Repairman(String name, String teamName) : base(name, teamName) { }

        /// <summary>
        /// Repair robots in max range.
        /// </summary>
        /// <returns></returns>
        public RepairAnswerCommand Repair() {
            RepairAnswerCommand answer = new RepairAnswerCommand();
            addRobotTask(RepairAsync(answer));
            return answer;
        }

        /// <summary>
        /// Repair robots closer then <code>maxDistance</code>.
        /// </summary>
        /// <param name="maxDistance">How far can be robots witch will be repaired.</param>
        /// <returns></returns>
        public RepairAnswerCommand Repair(int maxDistance) {
            RepairAnswerCommand answer = new RepairAnswerCommand();
            addRobotTask(RepairAsync(answer, maxDistance));
            return answer;
        }

        private async Task<RepairAnswerCommand> RepairAsync(RepairAnswerCommand destination) {
            await sendCommandAsync(new RepairCommand());
            RepairAnswerCommand answer = await receiveCommandAsync<RepairAnswerCommand>();
            destination.FillData(answer);
            return answer;
        }

        private async Task<RepairAnswerCommand> RepairAsync(RepairAnswerCommand destination, int maxDistance) {
            await sendCommandAsync(new RepairCommand(maxDistance));
            RepairAnswerCommand answer = await receiveCommandAsync<RepairAnswerCommand>();
            destination.FillData(answer);
            return answer;
        }

        public override RobotType GetRobotType() {
            return RobotType.REPAIRMAN;
        }

        protected override void SetClassEquip(int id) {
            REPAIR_TOOL = REPAIR_TOOLS_BY_ID[id];
        }

        protected override ClassEquipment GetClassEquip() {
            return REPAIR_TOOL;
        }
    }
}
