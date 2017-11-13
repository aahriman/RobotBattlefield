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
                
        static MyMineLayer _mineLayer = new MyMineLayer("mines", TEAM_ID);
        static Repairman repairman = new Repairman("repairman", TEAM_ID);

        class MyMineLayer : MineLayer {

            public MyMineLayer(String name, String teamName) : base(name, teamName) {}

            public int previousHitpoints = 0;

            protected override void ProcessState(RobotStateCommand state) {
                base.ProcessState(state);
                bases = BaseCapture.GetBases(state);

                foreach (var @base in bases) {
                    if (@base.TeamId != _mineLayer.TEAM_ID) {
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

        private static DriveAnswerCommand robotDriveToBase(ClientRobot robot) {
            double angle = AngleUtils.AngleDegree(robot.X, robot.Y, capturedBase.X, capturedBase.Y);

            if (robot.Power > robot.Motor.ROTATE_IN && Math.Abs(angle - robot.AngleDrive) > 1){
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
            return scan != null && scan.ENEMY_ID != _mineLayer.ID && scan.ENEMY_ID != repairman.ID;
        }

        private static RepairmenState getRepairmenState() {
            if (
                EuclideanSpaceUtils.Distance(new Point(repairman.X, repairman.Y),
                                             new Point(capturedBase.X, capturedBase.Y)) >
                BaseCapture.BASE_SIZE / 2.0) {
                return RepairmenState.GO_TO_BASE;
            } else if (_mineLayer.HitPoints < 100 &&
                       EuclideanSpaceUtils.Distance(repairman.X, repairman.Y, _mineLayer.X, _mineLayer.Y) <
                       repairman.REPAIR_TOOL.ZONES[0].DISTANCE) {
                return RepairmenState.REPAIR;
            } else {
                return RepairmenState.GO_AROUND;
            }
        }

        private static MinerState getMinerState() {
            if (EuclideanSpaceUtils.Distance(new Point(_mineLayer.X, _mineLayer.Y), new Point(capturedBase.X, capturedBase.Y)) >
                BaseCapture.BASE_SIZE) {
                return MinerState.GO_TO_BASE;
            } else if (_mineLayer.PuttedMines < _mineLayer.MINE_GUN.MAX_MINES) {
                return MinerState.PUT_MINE;
            } else if (_mineLayer.HitPoints < _mineLayer.previousHitpoints) {
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
                MinerState minetState = getMinerState();

                SubState minerSubState = default(SubState);
                SubState repairmenSubState = default(SubState);

                DriveAnswerCommand minerDrive = null;
                PutMineAnswerCommand minerPut = null;
                DetonateMineAnswerCommand minerDetonate = null;
                ScanAnswerCommand minerScan = null;
                
                switch (minetState) {
                    case MinerState.DETONATE:
                        minerDetonate = _mineLayer.DetonateMine(_mineLayer.PuttedMinesList[0].ID);
                        
                        break;
                    case MinerState.PUT_MINE:
                        minerPut = _mineLayer.PutMine();
                        break;
                    case MinerState.GO_TO_REPAIRMAN:
                        minerDrive =
                            _mineLayer.Drive(AngleUtils.AngleDegree(_mineLayer.X, _mineLayer.Y, repairman.X, repairman.Y),
                                             _mineLayer.Motor.ROTATE_IN);
                        break;
                    case MinerState.GO_TO_BASE:
                        minerDrive = robotDriveToBase(_mineLayer);
                        break;
                    case MinerState.GO_AROUND:
                        if (EuclideanSpaceUtils.Distance(_mineLayer.X, _mineLayer.Y, capturedBase.X, capturedBase.Y) < BaseCapture.BASE_SIZE  * 3.0 / 4.0) {
                            minerSubState = SubState.SCAN;
                            minerScan = _mineLayer.Scan(AngleUtils.AngleDegree(_mineLayer.X, _mineLayer.Y, capturedBase.X, capturedBase.Y), 10);
                        } else {

                            minerSubState = SubState.DRIVE;
                            minerDrive = robotDriveAround(_mineLayer);
                        }
                        break;
                }

                DriveAnswerCommand repairmenDrive = null;
                ScanAnswerCommand repairmenScan = null;
                RepairAnswerCommand repairmenRepair = null;

                switch (repairmenState) {
                    case RepairmenState.GO_AROUND:
                        if (EuclideanSpaceUtils.Distance(_mineLayer.X, _mineLayer.Y, capturedBase.X, capturedBase.Y) <
                            BaseCapture.BASE_SIZE * 3.0 / 4.0) {
                            repairmenSubState = SubState.SCAN;
                            repairmenScan =
                                repairman.Scan(
                                                    AngleUtils.AngleDegree(_mineLayer.X, _mineLayer.Y, capturedBase.X,
                                                                           capturedBase.Y), 10);
                        } else {

                            repairmenSubState = SubState.DRIVE;
                            repairmenDrive = robotDriveAround(repairman);
                        }
                        break;

                    case RepairmenState.GO_TO_BASE:
                        repairmenSubState = SubState.DRIVE;
                        repairmenDrive = robotDriveToBase(repairman);
                        break;

                    case RepairmenState.REPAIR:
                        repairmenRepair =
                            repairman.Repair((int)EuclideanSpaceUtils.Distance(repairman.X, repairman.Y, _mineLayer.X,
                                                                               _mineLayer.Y) + 1);
                        break;
                }
                

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
                            if (minerScan != null) scan1 = minerScan;
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                switch (repairmenState) {
                    case RepairmenState.REPAIR:
                        _mineLayer.previousHitpoints = _mineLayer.HitPoints;
                        break;
                    case RepairmenState.GO_TO_BASE:
                        break;
                    case RepairmenState.GO_AROUND:
                        if (repairmenSubState == SubState.SCAN) {
                            if (repairmenScan != null) scan2 = repairmenScan;
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

            }
        }
    }
}
