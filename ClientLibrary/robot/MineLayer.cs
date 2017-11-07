using System;
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

    /// <summary>
    /// Instances represent robot who can put mines on map.
    /// </summary>
    public class MineLayer : ClientRobot {

        /// <summary>
        /// Structure for information about mine.
        /// </summary>
        public struct Mine {
            /// <summary>
            /// X coordinate on map.
            /// </summary>
            public double X;

            /// <summary>
            /// Y coordinate on map.
            /// </summary>
            public double Y;

            /// <summary>
            /// Mine id witch is useful for detonation.
            /// </summary>
            public int ID;
        }

        /// <summary>
        /// Mine gun witch robot has.
        /// </summary>
        public MineGun MINE_GUN { get; private set; }

        /// <summary>
        /// Number of putted
        /// </summary>
        public int PuttedMines { get; private set; }

        /// <summary>
        /// List of available mines putted on map.
        /// </summary>
        public readonly List<Mine> PuttedMinesList = new List<Mine>();

        public MineLayer(String name, String teamName)
            : base(name, teamName) {}


        /// <summary>
        /// Put mine on robot position.
        /// </summary>
        public PutMineAnswerCommand PutMine() {
            PutMineAnswerCommand answer = new PutMineAnswerCommand();
            addRobotTask(PutMineAsync(answer));
            return answer;
        }

        /// <summary>
        /// Put mine on robot position.
        /// </summary>
        /// <param name="destination">Where to fill answer data.</param>
        private async Task<PutMineAnswerCommand> PutMineAsync(PutMineAnswerCommand destinaton) {
            await sendCommandAsync(new PutMineCommand());
            PutMineAnswerCommand answerCommand = await receiveCommandAsync<PutMineAnswerCommand>();

            destinaton.FillData(answerCommand);
            if (answerCommand.SUCCESS) {
                PuttedMinesList.Add(new Mine() {
                    ID = answerCommand.MINE_ID,
                    X = X,
                    Y = Y
                });
            }

            if (answerCommand.SUCCESS) {
                PuttedMines++;
            }
            return answerCommand;
        }

        /// <summary>
        /// Detonate specified mine.
        /// </summary>
        /// <param name="mineId">witch mine robot wants to detonate.</param>
        /// <returns></returns>
        public DetonateMineAnswerCommand DetonateMine(int mineId) {
            DetonateMineAnswerCommand answer = new DetonateMineAnswerCommand();
            addRobotTask(DetonateMineAsync(answer, mineId));
            return answer;
        }

        /// <summary>
        /// Detonate specified mine. Send action to erver asynchronously.
        /// </summary>
        /// <param name="destination">Where to fill answer data.</param>
        /// <param name="mineId">witch mine robot wants to detonate.</param>
        /// <returns></returns>
        private async Task<DetonateMineAnswerCommand> DetonateMineAsync(DetonateMineAnswerCommand destination, int mineId) {
            await sendCommandAsync(new DetonateMineCommand(mineId));
            var answerCommand = await receiveCommandAsync<DetonateMineAnswerCommand>();

            destination.FillData(answerCommand);

            if (answerCommand.SUCCESS) {
                PuttedMines--;

                foreach (var mine in PuttedMinesList) {
                    if (mine.ID == mineId) {
                        PuttedMinesList.Remove(mine);
                        break;
                    }
                }
            }
            return answerCommand;
        }

        /// <inheritdoc />
        public override RobotType GetRobotType() {
            return RobotType.MINER;
        }

        /// <inheritdoc />
        protected override void SetClassEquip(int id) {
            MINE_GUN = MINE_GUNS_BY_ID[id];
        }

        /// <inheritdoc />
        protected override IClassEquipment GetClassEquip() {
            return MINE_GUN;
        }
    }
}
