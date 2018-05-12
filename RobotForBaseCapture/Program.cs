using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BaseCapcureBattlefieldLibrary.battlefield;
using BaseLibrary.communication.command.common;
using BaseLibrary.communication.command.miner;
using BaseLibrary.communication.command.repairman;
using BaseLibrary.utils;
using BaseLibrary.utils.euclidianSpaceStruct;
using ClientLibrary.robot;

namespace MinerForBaseCapture {
    class Program {

        static String TEAM_ID = Guid.NewGuid().ToString();
        private static Base[] bases;

        private static ScanAnswerCommand scan1;
        private static ScanAnswerCommand scan2;

        static Base capturedBase = null;

        static MyMineLayer mineLayer = new MyMineLayer("Miner", TEAM_ID);
        static Repairman repairman = new Repairman("Repairman", TEAM_ID);

        class MyMineLayer : MineLayer {

            public MyMineLayer(String name, String teamName) : base(name, teamName) { }

            public int previousHitpoints = 0;

            protected override void ProcessState(RobotStateCommand state) {
                base.ProcessState(state);
                bases = BaseCapture.GetBases(state);

                foreach (var @base in bases) {
                    if (@base.TeamId != mineLayer.TEAM_ID) {
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

        private static DriveAnswerCommand robotDriveToBase(ClientRobot robot) {
            double angle = AngleUtils.AngleDegree(robot.X, robot.Y, capturedBase.X, capturedBase.Y);

            if (robot.Power > robot.Motor.ROTATE_IN && Math.Abs(angle - robot.AngleDrive) > 1) {
                return robot.Drive(robot.AngleDrive, robot.Motor.ROTATE_IN);
            } else {
                return robot.Drive(angle, 100);
            }
        }

        private static DriveAnswerCommand robotDriveAround(ClientRobot robot) {
            if (robot.Power > robot.Motor.ROTATE_IN) {
                return robot.Drive(robot.AngleDrive, robot.Motor.ROTATE_IN);
            } else {
                if (0 <= robot.AngleDrive && robot.AngleDrive < 90) {
                    return robot.Drive(180, 100);
                } else if (90 <= robot.AngleDrive && robot.AngleDrive < 180) {
                    return robot.Drive(270, 100);
                } else if (180 <= robot.AngleDrive && robot.AngleDrive < 270) {
                    return robot.Drive(0, 100);
                } else if (270 <= robot.AngleDrive && robot.AngleDrive < 360) {
                    return robot.Drive(90, 100);
                }
            }
            return null;
        }

        private static bool ScanSeeEnemy(ScanAnswerCommand scan) {
            return scan != null && scan.ENEMY_ID != mineLayer.ID && scan.ENEMY_ID != repairman.ID;
        }

        private static RepairmenState getRepairmenState() {
            if (
                EuclideanSpaceUtils.Distance(new Point(repairman.X, repairman.Y),
                    new Point(capturedBase.X, capturedBase.Y)) >
                BaseCapture.BASE_SIZE / 2.0) {
                return RepairmenState.GO_TO_BASE;
            } else if (mineLayer.HitPoints < 100 &&
                       EuclideanSpaceUtils.Distance(repairman.X, repairman.Y, mineLayer.X, mineLayer.Y) <
                       repairman.REPAIR_TOOL.ZONES[0].DISTANCE) {
                return RepairmenState.REPAIR;
            } else {
                return RepairmenState.GO_AROUND;
            }
        }

        private static MinerState getMinerState() {
            if (EuclideanSpaceUtils.Distance(new Point(mineLayer.X, mineLayer.Y), new Point(capturedBase.X, capturedBase.Y)) >
                BaseCapture.BASE_SIZE) {
                return MinerState.GO_TO_BASE;
            } else if (mineLayer.PutMines < mineLayer.MINE_GUN.MAX_MINES) {
                return MinerState.PUT_MINE;
            } else if (mineLayer.HitPoints < mineLayer.previousHitpoints) {
                return MinerState.GO_TO_REPAIRMAN;
            } else if (ScanSeeEnemy(scan1) || ScanSeeEnemy(scan2)) {
                return MinerState.DETONATE;
            } else {
                return MinerState.GO_AROUND;
            }
        }


        public static void Main(string[] args) {
            ClientRobot.Connect(args);
            while (true) {

                RepairmenState repairmenState = getRepairmenState();
                MinerState minerState = getMinerState();


                switch (minerState) {
                    case MinerState.DETONATE:
                        if (mineLayer.PutMinesList.Count > 0) {
                            mineLayer.DetonateMine(mineLayer.PutMinesList[0].ID);
                        }
                        break;
                    case MinerState.PUT_MINE:
                        mineLayer.PutMine();
                        break;
                    case MinerState.GO_TO_REPAIRMAN:
                        mineLayer.Drive(AngleUtils.AngleDegree(mineLayer.X, mineLayer.Y, repairman.X, repairman.Y),
                            mineLayer.Motor.ROTATE_IN);
                        break;
                    case MinerState.GO_TO_BASE:
                        robotDriveToBase(mineLayer);
                        break;
                    case MinerState.GO_AROUND:
                        if (EuclideanSpaceUtils.Distance(mineLayer.X, mineLayer.Y, capturedBase.X, capturedBase.Y) < BaseCapture.BASE_SIZE * 3.0 / 4.0) {
                            scan1 = mineLayer.Scan(AngleUtils.AngleDegree(mineLayer.X, mineLayer.Y, capturedBase.X, capturedBase.Y), 10);
                        } else {
                            robotDriveAround(mineLayer);
                        }
                        break;
                }

                switch (repairmenState) {
                    case RepairmenState.GO_AROUND:
                        if (EuclideanSpaceUtils.Distance(mineLayer.X, mineLayer.Y, capturedBase.X, capturedBase.Y) <
                            BaseCapture.BASE_SIZE * 3.0 / 4.0) {
                            scan2 = repairman.Scan(AngleUtils.AngleDegree(mineLayer.X, mineLayer.Y, capturedBase.X, capturedBase.Y), 10);
                        } else {
                            robotDriveAround(repairman);
                        }
                        break;

                    case RepairmenState.GO_TO_BASE:
                        robotDriveToBase(repairman);
                        break;

                    case RepairmenState.REPAIR:
                        repairman.Repair((int) EuclideanSpaceUtils.Distance(repairman.X, repairman.Y, mineLayer.X, mineLayer.Y) + 1);
                        break;
                }

                if (repairmenState == RepairmenState.REPAIR) {
                    mineLayer.previousHitpoints = mineLayer.HitPoints;
                }
            }

        }
    }
}
