using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLibrary;
using BaseLibrary.communication.command.handshake;
using BaseLibrary.communication.command.miner;
using BaseLibrary.equipment;
using BaseLibrary.utils.euclidianSpaceStruct;

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
            public readonly int ID;

            /// <summary>
            /// Mine position.
            /// </summary>
            public Point Position => new Point(X, Y);

            public Mine(int id) {
                ID = id;
                X = default(double);
                Y = default(double);
            }
        }

        /// <summary>
        /// Mine gun witch robot has.
        /// </summary>
        public MineGun MINE_GUN { get; private set; }

        /// <summary>
        /// Number of put mines on map.
        /// </summary>
        public int PutMines { get; private set; }

        /// <summary>
        /// List of available mines put on map.
        /// </summary>
        public readonly List<Mine> PutMinesList = new List<Mine>();

        /// <summary>
        /// Create new mineLayer with name.
        /// </summary>
        /// <param name="name"></param>
        public MineLayer(String name)
            : base(name) { }

        /// <summary>
        /// Create new mineLayer with name and into team specified by teamName
        /// </summary>
        /// <param name="name"></param>
        /// <param name="teamName"></param>
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

            Point position = Position;
            PutMineAnswerCommand answerCommand = await receiveCommandAsync<PutMineAnswerCommand>();

            destinaton.FillData(answerCommand);
            if (answerCommand.SUCCESS) {
                PutMinesList.Add(new Mine(id: answerCommand.MINE_ID) {
                    Y = position.X,
                    X = position.Y
                });
            }

            if (answerCommand.SUCCESS) {
                PutMines++;
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
                PutMines--;

                foreach (var mine in PutMinesList) {
                    if (mine.ID == mineId) {
                        PutMinesList.Remove(mine);
                        break;
                    }
                }
            }
            return answerCommand;
        }

        /// <inheritdoc />
        public override RobotType GetRobotType() {
            return RobotType.MINE_LAYER;
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
