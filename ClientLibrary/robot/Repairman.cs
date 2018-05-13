using System;
using System.Threading.Tasks;
using BaseLibrary.communication.command.handshake;
using BaseLibrary.communication.command.repairman;
using BaseLibrary.equipment;

namespace ClientLibrary.robot {
    /// <summary>
    /// Instances represent robot who can repair other robots.
    /// </summary>
    public class Repairman : ClientRobot {
        /// <summary>
        /// Repair tool witch robot has.
        /// </summary>
        public RepairTool REPAIR_TOOL { get; private set; }

        /// <inheritdoc />
        public Repairman(String name) : base(name) { }

        /// <inheritdoc />
        public Repairman(String name, String robotTeamName) : base(name, robotTeamName) { }

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

        /// <summary>
        /// Repair robots in max range. And send it to server asynchronously.
        /// </summary>
        /// <param name="destination">Where to fill answer data.</param>
        /// <returns></returns>
        private async Task<RepairAnswerCommand> RepairAsync(RepairAnswerCommand destination) {
            await sendCommandAsync(new RepairCommand());
            RepairAnswerCommand answer = await receiveCommandAsync<RepairAnswerCommand>();
            destination.FillData(answer);
            return answer;
        }


        /// <summary>
        /// Repair robots closer then <code>maxDistance</code>. And send it to server asynchronously.
        /// </summary>
        /// <param name="destination">Where to fill answer data.</param>
        /// <param name="maxDistance">How far can be robots witch will be repaired.</param>
        /// <returns></returns>
        private async Task<RepairAnswerCommand> RepairAsync(RepairAnswerCommand destination, int maxDistance) {
            await sendCommandAsync(new RepairCommand(maxDistance));
            RepairAnswerCommand answer = await receiveCommandAsync<RepairAnswerCommand>();
            destination.FillData(answer);
            return answer;
        }

        /// <inheritdoc />
        public override RobotType GetRobotType() {
            return RobotType.REPAIRMAN;
        }

        /// <inheritdoc />
        protected override void SetClassEquip(int id) {
            REPAIR_TOOL = REPAIR_TOOLS_BY_ID[id];
        }

        /// <inheritdoc />
        protected override IClassEquipment GetClassEquip() {
            return REPAIR_TOOL;
        }
    }
}
