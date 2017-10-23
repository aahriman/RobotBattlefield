using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BaseCapcureBattlefieldLibrary.battlefield;
using BaseLibrary.command;
using BaseLibrary.command.common;
using BaseLibrary.command.miner;
using BaseLibrary.command.repairman;
using BaseLibrary.equip;
using BaseLibrary.utils;
using BaseLibrary.utils.euclidianSpaceStruct;
using ClientLibrary.robot;

namespace MinerForBaseCapture {
    class Program {

        static String TEAM_ID = Guid.NewGuid().ToString();
        private static Base[] bases;

        static private ScanAnswerCommand scan1;
        static private ScanAnswerCommand scan2;

        static Base capturedBase = null;
                
        static MyMiner miner = new MyMiner("mines", TEAM_ID);
        static Repairman repairman = new Repairman("repairman", TEAM_ID);

        class MyMiner : Miner {

            public MyMiner(String name, String teamName) : base(name, teamName) {}

            public int previousHitpoints = 0;

            protected override void ProcessState(RobotStateCommand state) {
                base.ProcessState(state);
                bases = BaseCapture.GetBases(state);

                foreach (var @base in bases) {
                    if (@base.TeamId != miner.TEAM_ID) {
                        capturedBase = @base;
                        break;
                    }
                }
            }
        }

        enum RepairmenState {
            GO_TO_BASE,
            GO_AROUND,
            REPAIR
        }

        enum MinerState {
            GO_TO_BASE,
            PUT_MINE,
            GO_AROUND,
            DETONATE,
            GO_TO_REPAIRMAN,
        }

        enum SubState {
            SCAN,
            DRIVE
        }

        private static Task<DriveAnswerCommand> robotDriveToBase(ClientRobot robot) {
            double angle = AngleUtils.AngleDegree(robot.X, robot.Y, capturedBase.X, capturedBase.Y);

            if (robot.Power > robot.Motor.ROTATE_IN && Math.Abs(angle - robot.AngleDrive) > 1){
                return robot.DriveAsync(robot.AngleDrive, robot.Motor.ROTATE_IN);
            } else {
               return robot.DriveAsync(angle, 100); 
            }
        }

        private static Task<DriveAnswerCommand> robotDriveAround(ClientRobot robot) {
            if (robot.Power > robot.Motor.ROTATE_IN) {
                return robot.DriveAsync(robot.AngleDrive, robot.Motor.ROTATE_IN);
            } else {
                if (0 <= robot.AngleDrive && robot.AngleDrive < 90) {
                    return robot.DriveAsync(180, 100);
                } else if (90 <= robot.AngleDrive && robot.AngleDrive < 180) {
                    return robot.DriveAsync(270, 100);
                } else if (180 <= robot.AngleDrive && robot.AngleDrive < 270) {
                    return robot.DriveAsync(0, 100);
                } else if (270 <= robot.AngleDrive && robot.AngleDrive < 360) {
                    return robot.DriveAsync(90, 100);
                }
            }
            return null;
        }

        private static bool ScanSeeEnemy(ScanAnswerCommand scan) {
            return scan != null && scan.ENEMY_ID != miner.ID && scan.ENEMY_ID != repairman.ID;
        }

        private static RepairmenState getRepairmenState() {
            if (
                EuclideanSpaceUtils.Distance(new Point(repairman.X, repairman.Y),
                                             new Point(capturedBase.X, capturedBase.Y)) >
                BaseCapture.BASE_SIZE / 2.0) {
                return RepairmenState.GO_TO_BASE;
            } else if (miner.HitPoints < 100 &&
                       EuclideanSpaceUtils.Distance(repairman.X, repairman.Y, miner.X, miner.Y) <
                       repairman.REPAIR_TOOL.ZONES[0].DISTANCE) {
                return RepairmenState.REPAIR;
            } else {
                return RepairmenState.GO_AROUND;
            }
        }

        private static MinerState getMinerState() {
            if (EuclideanSpaceUtils.Distance(new Point(miner.X, miner.Y), new Point(capturedBase.X, capturedBase.Y)) >
                BaseCapture.BASE_SIZE) {
                return MinerState.GO_TO_BASE;
            } else if (miner.PutedMines < miner.MINE_GUN.MAX_MINES) {
                return MinerState.PUT_MINE;
            } else if (miner.HitPoints < miner.previousHitpoints) {
                return MinerState.GO_TO_REPAIRMAN;
            } else if (ScanSeeEnemy(scan1) || ScanSeeEnemy(scan2)) {
                return MinerState.DETONATE;
            } else {
                return MinerState.GO_AROUND;
            }
        }


        public static void Main(string[] args) {
            while (true) {

                RepairmenState repairmenState = getRepairmenState();
                MinerState minetState = getMinerState();

                SubState minerSubState = default(SubState);
                SubState repairmenSubState = default(SubState);
                List<Task> taskToWait = new List<Task>();

                Task<DriveAnswerCommand> minerDriveTask = null;
                Task<PutMineAnswerCommand> minerPutTask = null;
                Task<DetonateMineAnswerCommand> minerDetonateTask = null;
                Task<ScanAnswerCommand> minerScanTask = null;
                
                switch (minetState) {
                    case MinerState.DETONATE:
                        minerDetonateTask = miner.DetonateMineAsync(miner.PutedMinesList[0].ID);
                        taskToWait.Add(minerDetonateTask);
                        break;
                    case MinerState.PUT_MINE:
                        minerPutTask = miner.PutMineAsync();
                        taskToWait.Add(minerPutTask);
                        break;
                    case MinerState.GO_TO_REPAIRMAN:
                        minerDriveTask =
                            miner.DriveAsync(AngleUtils.AngleDegree(miner.X, miner.Y, repairman.X, repairman.Y),
                                             miner.Motor.ROTATE_IN);
                        taskToWait.Add(minerDriveTask);
                        break;
                    case MinerState.GO_TO_BASE:
                        minerDriveTask = robotDriveToBase(miner);
                        taskToWait.Add(minerDriveTask);
                        break;
                    case MinerState.GO_AROUND:
                        if (EuclideanSpaceUtils.Distance(miner.X, miner.Y, capturedBase.X, capturedBase.Y) < BaseCapture.BASE_SIZE  * 3.0 / 4.0) {
                            minerSubState = SubState.SCAN;
                            minerScanTask = miner.ScanAsync(AngleUtils.AngleDegree(miner.X, miner.Y, capturedBase.X, capturedBase.Y), 10);
                            taskToWait.Add(minerScanTask);
                        } else {

                            minerSubState = SubState.DRIVE;
                            minerDriveTask = robotDriveAround(miner);

                            
                            taskToWait.Add(minerDriveTask);
                        }
                        break;
                }

                Task<DriveAnswerCommand> repairmenDriveTask = null;
                Task<ScanAnswerCommand> repairmenScanTask = null;
                Task<RepairAnswerCommand> repairmenRepairTask = null;

                switch (repairmenState) {
                    case RepairmenState.GO_AROUND:
                        if (EuclideanSpaceUtils.Distance(miner.X, miner.Y, capturedBase.X, capturedBase.Y) <
                            BaseCapture.BASE_SIZE * 3.0 / 4.0) {
                            repairmenSubState = SubState.SCAN;
                            repairmenScanTask =
                                repairman.ScanAsync(
                                                    AngleUtils.AngleDegree(miner.X, miner.Y, capturedBase.X,
                                                                           capturedBase.Y), 10);
                            taskToWait.Add(repairmenScanTask);
                        } else {

                            repairmenSubState = SubState.DRIVE;
                            repairmenDriveTask = robotDriveAround(repairman);


                            taskToWait.Add(repairmenDriveTask);
                        }
                        break;

                    case RepairmenState.GO_TO_BASE:
                        repairmenSubState = SubState.DRIVE;
                        repairmenDriveTask = robotDriveToBase(repairman);

                        taskToWait.Add(repairmenDriveTask);
                        break;

                    case RepairmenState.REPAIR:
                        repairmenRepairTask =
                            repairman.RepairAsync((int)EuclideanSpaceUtils.Distance(repairman.X, repairman.Y, miner.X,
                                                                               miner.Y) + 1);
                        taskToWait.Add(repairmenRepairTask);
                        break;
                }

                Task.WaitAll(taskToWait.ToArray());
                taskToWait.Clear();

                switch (minetState) {
                    case MinerState.DETONATE:
                        break;
                    case MinerState.PUT_MINE:
                        break;
                    case MinerState.GO_TO_REPAIRMAN:
                        break;
                    case MinerState.GO_TO_BASE:
                        break;
                    case MinerState.GO_AROUND:
                        if (minerSubState == SubState.SCAN) {
                            if (minerScanTask != null) scan1 = minerScanTask.Result;
                        }
                        break;
                }

                switch (repairmenState) {
                    case RepairmenState.REPAIR:
                        miner.previousHitpoints = miner.HitPoints;
                        break;
                    case RepairmenState.GO_TO_BASE:
                        break;
                    case RepairmenState.GO_AROUND:
                        if (repairmenSubState == SubState.SCAN) {
                            if (repairmenScanTask != null) scan2 = repairmenScanTask.Result;
                        }
                        break;
                }

            }
        }
    }
}
