using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLibrary;
using BaseLibrary.command;
using BaseLibrary.command.common;
using BaseLibrary.command.handshake;
using BaseLibrary.command.miner;
using BaseLibrary.command.tank;
using BaseLibrary.equip;

namespace ClientLibrary.robot {
    public class Miner : ClientRobot {

        public struct Mine {
            public double X;
            public double Y;
            public int ID;
        }

        public MineGun MINE_GUN { get; private set; }

        public int PutedMines { get; private set; }

        public readonly List<Mine> PutedMinesList = new List<Mine>();

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

            if (answerCommand.SUCCESS) {
                PutedMinesList.Add(new Mine() {
                    ID = answerCommand.MINE_ID,
                    X = X,
                    Y = Y
                });
            }

            if (processStateAfterEveryCommand) {
                ProcessState(await StateAsync());
            }

            if (answerCommand.SUCCESS) {
                PutedMines++;
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

            if (answerCommand.SUCCESS) {
                PutedMines--;

                foreach (var mine in PutedMinesList) {
                    if (mine.ID == mineId) {
                        PutedMinesList.Remove(mine);
                        break;
                    }
                }
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
