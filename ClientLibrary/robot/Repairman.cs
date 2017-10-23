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
            return await recieveCommandAsync<RepairAnswerCommand>();
        }

        public async Task<RepairAnswerCommand> RepairAsync(int maxDistance) {
            await sendCommandAsync(new RepairCommand(maxDistance));
            return await recieveCommandAsync<RepairAnswerCommand>();
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
