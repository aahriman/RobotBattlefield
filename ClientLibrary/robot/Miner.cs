using System.Threading.Tasks;
using BaseLibrary;
using BaseLibrary.command;
using BaseLibrary.command.common;
using BaseLibrary.command.handshake;
using BaseLibrary.command.miner;
using BaseLibrary.command.tank;
using BaseLibrary.equip;

namespace ClientLibrary.robot {
    class Miner : ClientRobot {
        public MineGun MINE_GUN { get; private set; }

        public Miner() : base() {}

        public Miner(bool processStateAfterEveryCommand, bool processMerchant)
            : base(processStateAfterEveryCommand, processMerchant) {}


        public PutMineAnswerCommand PutMine() {
            PutMineAnswerCommand answer = taskWait(PutMineAsync());
            return answer;
        }

        public async Task<PutMineAnswerCommand> PutMineAsync() {
            await sendCommandAsync(new PutMineCommand());
            var commnad = sns.RecieveCommand();
            var answerCommand = (PutMineAnswerCommand) commnad;
            if (processStateAfterEveryCommand) {
                ProcessState(await StateAsync());
            }
            return answerCommand;
        }

        public DetonateMineAnswerCommand DetonateMine(int mineId) {
            DetonateMineAnswerCommand answer = taskWait(DetonateMineAsync(mineId));
            return answer;
        }

        public async Task<DetonateMineAnswerCommand> DetonateMineAsync(int mineId) {
            await sendCommandAsync(new DetonateMineCommand(mineId));
            var commnad = sns.RecieveCommand();
            var answerCommand = (DetonateMineAnswerCommand) commnad;
            if (processStateAfterEveryCommand) {
                ProcessState(await StateAsync());
            }
            return answerCommand;
        }

        public override RobotType GetRobotType() {
            return RobotType.MINER;
        }

        protected override void setClassEquip(int id) {
            MINE_GUN = MINE_GUNS_BY_ID[id];
        }

        protected override ClassEquipment getClassEquip() {
            return MINE_GUN;
        }
    }
}
