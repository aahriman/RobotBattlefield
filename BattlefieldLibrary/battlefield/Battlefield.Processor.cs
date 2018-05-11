using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Threading.Tasks;
using BaseLibrary;
using BaseLibrary.communication.command;
using BaseLibrary.communication.command.common;
using BaseLibrary.communication.command.equipment;
using BaseLibrary.communication.command.handshake;
using BaseLibrary.communication.command.miner;
using BaseLibrary.communication.command.repairman;
using BaseLibrary.communication.command.tank;
using BaseLibrary.equipment;
using BaseLibrary.utils;
using BaseLibrary.utils.euclidianSpaceStruct;
using BattlefieldLibrary.battlefield.robot;
using ViewerLibrary;
using BattlefieldRobot = BattlefieldLibrary.battlefield.robot.BattlefieldRobot;

namespace BattlefieldLibrary.battlefield {
    public partial class Battlefield {
        public const int RELOAD_TIME = 20;

        private void addProcess() {
            addGetCommandProcesses();
            addFightProcesses();
            addTankProcesses();
            addMineLayerProcesses();
            addRepairmanProcesses();
            addMerchantProcesses();
        }

        private void addGetCommandProcesses() {
            commandProcessorBeforeInitRobot.RegisterProcess<GetArmorsCommand>((command, networkStreamAndBattlefield) => networkStreamAndBattlefield.BATTLEFIELD._battlefieldState == BattlefieldState.GETTING_EQUIPMENT ? new GetArmorsAnswerCommand(networkStreamAndBattlefield.BATTLEFIELD.Armors) : (ACommand) new ErrorCommand("Armors can be ask only in " + BattlefieldState.GETTING_EQUIPMENT + "."));
            commandProcessorBeforeInitRobot.RegisterProcess<GetMotorsCommand>((command, networkStreamAndBattlefield) => networkStreamAndBattlefield.BATTLEFIELD._battlefieldState == BattlefieldState.GETTING_EQUIPMENT ? new GetMotorsAnswerCommand(networkStreamAndBattlefield.BATTLEFIELD.Motors) : (ACommand) new ErrorCommand("Motors can be ask only in " + BattlefieldState.GETTING_EQUIPMENT + "."));
            commandProcessorBeforeInitRobot.RegisterProcess<GetGunsCommand>((command, networkStreamAndBattlefield) => networkStreamAndBattlefield.BATTLEFIELD._battlefieldState == BattlefieldState.GETTING_EQUIPMENT ? new GetGunsAnswerCommand(networkStreamAndBattlefield.BATTLEFIELD.Guns) : (ACommand) new ErrorCommand("Guns can be ask only in " + BattlefieldState.GETTING_EQUIPMENT + "."));
            commandProcessorBeforeInitRobot.RegisterProcess<GetRepairToolsCommand>((command, networkStreamAndBattlefield) => networkStreamAndBattlefield.BATTLEFIELD._battlefieldState == BattlefieldState.GETTING_EQUIPMENT ? new GetRepairToolsAnswerCommand(networkStreamAndBattlefield.BATTLEFIELD.RepairTools) : (ACommand) new ErrorCommand("Repair tools can be ask only in " + BattlefieldState.GETTING_EQUIPMENT + "."));
            commandProcessorBeforeInitRobot.RegisterProcess<GetMineGunsCommand>((command, networkStreamAndBattlefield) => networkStreamAndBattlefield.BATTLEFIELD._battlefieldState == BattlefieldState.GETTING_EQUIPMENT ? new GetMineGunsAnswerCommand(networkStreamAndBattlefield.BATTLEFIELD.MineGuns) : (ACommand) new ErrorCommand("Mine gun can be ask only in " + BattlefieldState.GETTING_EQUIPMENT + "."));
            commandProcessorBeforeInitRobot.RegisterProcess<InitCommand>(initProcess);
        }


        private ACommand initProcess(InitCommand command, NetworkStreamAndBattlefield networkStreamAndBattlefield) {
            Battlefield battlefield = networkStreamAndBattlefield.BATTLEFIELD;

            IClassEquipment classEquipment = null;

            // get teamId for this robot
            if (!battlefield.robotTeamIdByTeamName.TryGetValue(command.TEAM_NAME, out int teamId)) {
                lock (battlefield.robotsByTeamId) {
                    lock (battlefield.robotTeamIdByTeamName) {
                        if (battlefield.robotTeamIdByTeamName.Count < battlefield.TEAMS) {
                            teamId = battlefield.idForTeam++;
                            battlefield.robotsByTeamId[teamId] = new List<BattlefieldRobot>();
                            battlefield.robotTeamIdByTeamName.Add(command.TEAM_NAME, teamId);
                        } else {
                            return new ErrorCommand("Too many teams.");
                        }
                    }
                }
            } else{
                lock (battlefield.robotsByTeamId) {
                    if (battlefield.robotsByTeamId[teamId].Count >= battlefield.ROBOTS_IN_TEAM) {
                        return new ErrorCommand("Too many robots in one team.");
                    }
                }
            }

            BattlefieldRobot robot;
            // create robot
            switch (command.ROBOT_TYPE) {
                case RobotType.MINE_LAYER:
                    robot = new MineLayer(teamId, idForRobot++, networkStreamAndBattlefield.NETWORK_STREAM);
                    initRobot(robot);
                    classEquipment = ((MineLayer)robot).MineGun;
                    break;
                case RobotType.TANK:
                    robot = new Tank(teamId, idForRobot++, networkStreamAndBattlefield.NETWORK_STREAM);
                    initRobot(robot);
                    classEquipment = ((Tank)robot).Gun;
                    break;
                case RobotType.REPAIRMAN:
                    robot = new Repairman(teamId, idForRobot++, networkStreamAndBattlefield.NETWORK_STREAM);
                    initRobot(robot);
                    classEquipment = ((Repairman)robot).RepairTool;
                    break;
                default:
                    return new ErrorCommand("Unsupported RobotType (" + command.ROBOT_TYPE + ") support only" + RobotType.MINE_LAYER + ", " + RobotType.TANK + ", " + RobotType.REPAIRMAN);
            }
            

            lock (battlefield.robots) {
                battlefield.robots.Add(robot);
            }
            lock (battlefield.robotsById) {
                battlefield.robotsById[robot.ID] = robot;
            }
            lock (battlefield.robotsByTeamId) {
                battlefield.robotsByTeamId[teamId].Add(robot);
            }

            lock (battlefield.robotsByStream) {
                battlefield.robotsByStream.Add(networkStreamAndBattlefield.NETWORK_STREAM, robot);
            }
            robot.NAME = command.NAME;

            battlefieldTurn.AddRobot(new Robot(robot.TEAM_ID, robot.Score, robot.Gold, robot.HitPoints, robot.X, robot.Y,
                robot.AngleDrive, robot.NAME));
            turnDataModel?.Add(battlefieldTurn.ConvertToTurn(), false);

            return battlefield.AddToInitAnswerCommand(new InitAnswerCommand(battlefield.MAX_TURN, battlefield.lap, battlefield.MAX_LAP,
                robot.ID, robot.TEAM_ID, classEquipment.ID, robot.Armor.ID, robot.Motor.ID));
        }

        protected void initRobot(BattlefieldRobot robot) {

            robot.Motor = Motors[0];
            robot.Armor = Armors[0];
            robot.HitPoints = robot.Armor.MAX_HP;

            switch (robot.ROBOT_TYPE) {
                case RobotType.MINE_LAYER:
                    MineLayer mineLayer = robot as MineLayer;
                    if (mineLayer != null) {
                        mineLayer.MineGun = MineGuns[0];
                        mineLayer.MinesNow = 0;
                        mineLayer.MINES_BY_ID.Clear();
                    }
                    break;
                case RobotType.TANK:
                    Tank tank = robot as Tank;
                    if (tank != null) {
                        tank.Gun = Guns[0];
                        tank.GunsToLoad = 0;
                    }
                    break;
                case RobotType.REPAIRMAN:
                    Repairman repairman = robot as Repairman;
                    if (repairman != null) {
                        repairman.RepairTool = RepairTools[0];
                        repairman.RepairToolUsed = 0;
                    }
                    break;
                default:
                    throw new NotSupportedException("Unsupported type of robot.");
            }
            
            Point position = obstacleManager.StartRobotPosition(ARENA_MAX_SIZE, ARENA_MAX_SIZE);

            robot.X = position.X;
            robot.Y = position.Y;
        }


        private void addFightProcesses() {
            commandProcessor.RegisterProcess<DriveCommand>(driveProcess);
            commandProcessor.RegisterProcess<ScanCommand>(scanProcess);
            commandProcessor.RegisterProcess<WaitCommand>(waitProcess);
        }

        private ACommand driveProcess(DriveCommand command, RobotAndBattlefield robotAndBattlefield) {
            BattlefieldRobot robot = robotAndBattlefield.ROBOT;
            if (robot.HitPoints > 0) {
                if (robot.Power <= robot.Motor.ROTATE_IN) {
                    robot.AngleDrive = command.ANGLE;
                }
                robot.WantedPower = Math.Min(command.POWER, 100);
                robot.WantedPower = Math.Max(robot.WantedPower, 0);
                return new DriveAnswerCommand(robot.AngleDrive.DEquals(command.ANGLE));
            } else {
                return new DriveAnswerCommand(robot.AngleDrive.DEquals(command.ANGLE));
            }

        }

        public ACommand scanProcess(ScanCommand command, RobotAndBattlefield robotAndBattlefield) {
            double minDistance = Battlefield.ARENA_MAX_SIZE * 10;
            BattlefieldRobot robot = robotAndBattlefield.ROBOT;
            Battlefield battlefield = robotAndBattlefield.BATTLEFIELD;

            BattlefieldRobot minTarget = robot;
            if (robot.HitPoints > 0) {
                foreach (BattlefieldRobot target in battlefield.robots) {
                    if (robot.ID != target.ID && target.HitPoints > 0 && battlefield.obstacleManager.CanScan(battlefield.Turn, robot.X, robot.Y, target.X, target.Y)) {
                        double distance = EuclideanSpaceUtils.Distance(robot.X, robot.Y, target.X, target.Y);
                        if (distance < minDistance) {
                            double degree = AngleUtils.NormalizeDegree(AngleUtils.AngleDegree(robot.X, robot.Y, target.X, target.Y));
                            if (Math.Abs(degree - command.ANGLE) <= command.PRECISION) {
                                minDistance = distance;
                                minTarget = target;
                            }
                        }
                    }
                }

                battlefield.battlefieldTurn.AddScan(new Scan(command.ANGLE, command.PRECISION, minDistance, robot.X, robot.Y));
            }
            return new ScanAnswerCommand(minDistance, minTarget.ID);

        }

        public ACommand waitProcess(WaitCommand command, RobotAndBattlefield robotAndBattlefield) {
            return null;
        }

        private void addTankProcesses() {
            commandProcessor.RegisterProcess<ShootCommand>(shootProcess);
        }

        private ACommand shootProcess(ShootCommand command, RobotAndBattlefield robotAndBattlefield) {
            BattlefieldRobot robot = robotAndBattlefield.ROBOT;
            Battlefield battlefield = robotAndBattlefield.BATTLEFIELD;

            Tank tank = robot as Tank;
            if (tank == null) {
                return new ErrorCommand(robot.ROBOT_TYPE + " cannot use shot command.");
            }
            
            if (tank.GunsToLoad < tank.Gun.BARREL_NUMBER && tank.HitPoints > 0) {
                double range = command.RANGE;
                if (range > tank.Gun.MAX_RANGE) {
                    range = tank.Gun.MAX_RANGE;
                }
                double toX = range * Math.Cos(AngleUtils.ToRads(command.ANGLE)) + tank.X;
                double toY = range * Math.Sin(AngleUtils.ToRads(command.ANGLE)) + tank.Y;
                battlefield.obstacleManager.ShotChange(battlefield.Turn, tank.X, tank.Y, ref toX, ref toY);
                int toLap = (int) Math.Ceiling(range / tank.Gun.SHOT_SPEED) + battlefield.Turn;
                if (!battlefield.heapBullet.TryGetValue(toLap, out List<Bullet> bulletList)) {
                    bulletList = new List<Bullet>();
                    battlefield.heapBullet.Add(toLap, bulletList);
                }
                bulletList.Add(new Bullet(battlefield.Turn, toLap, toX, toY, tank));
                int loadAtTurn = battlefield.Turn + RELOAD_TIME;
                if (!battlefield.gunLoaded.TryGetValue(loadAtTurn, out List<Tank> list)) {
                    list = new List<Tank>();
                    battlefield.gunLoaded.Add(loadAtTurn, list);
                }
                list.Add(tank);
                tank.GunsToLoad++;
                return new ShootAnswerCommand(true);
            } else {
                return new ShootAnswerCommand(false);
            }
        }



        private void addMineLayerProcesses() {
            commandProcessor.RegisterProcess<PutMineCommand>(putMineProcess);
            commandProcessor.RegisterProcess<DetonateMineCommand>(detonateMineProcess);
        }

        private ACommand putMineProcess(PutMineCommand command, RobotAndBattlefield robotAndBattlefield) {
            BattlefieldRobot robot = robotAndBattlefield.ROBOT;

            MineLayer mineLayer = robot as MineLayer;
            if (mineLayer == null) {
                return new ErrorCommand(robot.ROBOT_TYPE + " cannot use put mine command.");
            }

            if (mineLayer.MinesNow < mineLayer.MineGun.MAX_MINES && mineLayer.HitPoints > 0) {
                int id = PutMineAnswerCommand.FALSE_MINE_ID + 1;
                while (mineLayer.MINES_BY_ID.ContainsKey(id)) {
                    id++;
                }
                mineLayer.MINES_BY_ID.Add(id, new Mine(id, mineLayer.X, mineLayer.Y, mineLayer));
                return new PutMineAnswerCommand(true, id);
            } else {
                return new PutMineAnswerCommand(false, PutMineAnswerCommand.FALSE_MINE_ID);
            }

        }

        private ACommand detonateMineProcess(DetonateMineCommand command, RobotAndBattlefield robotAndBattlefield) {
            BattlefieldRobot robot = robotAndBattlefield.ROBOT;
            Battlefield battlefield = robotAndBattlefield.BATTLEFIELD;
            MineLayer mineLayer = robot as MineLayer;
            if (mineLayer == null) {
                return new ErrorCommand(robot.ROBOT_TYPE + " cannot use detonate mine command.");
            }

            Mine mine;
            if (mineLayer.MINES_BY_ID.TryGetValue(command.MINE_ID, out mine) && mineLayer.HitPoints > 0) {
                mineLayer.MINES_BY_ID.Remove(command.MINE_ID);
                battlefield.detonatedMines.Add(mine);
                return new DetonateMineAnswerCommand(true);
            } else {
                return new DetonateMineAnswerCommand(false);
            }
        }

        private void addRepairmanProcesses() {
            commandProcessor.RegisterProcess<RepairCommand>(repairProcess);
        }

        private ACommand repairProcess(RepairCommand command, RobotAndBattlefield robotAndBattlefield) {

            Battlefield battlefield = robotAndBattlefield.BATTLEFIELD;
            Repairman repairman = robotAndBattlefield.ROBOT as Repairman;
            if (repairman == null) {
                return new ErrorCommand(robotAndBattlefield.ROBOT.ROBOT_TYPE + " cannot use repair command.");
            }

            if (repairman.RepairToolUsed < repairman.RepairTool.MAX_USAGES && repairman.HitPoints > 0) {
                foreach (BattlefieldRobot robot in battlefield.robots) {
                    if (robot.HitPoints > 0) {
                        double distance = EuclideanSpaceUtils.Distance(robot.Position,
                            repairman.Position);
                        if (distance < command.MAX_DISTANCE) {
                            Zone zone = Zone.GetZoneByDistance(repairman.RepairTool.ZONES, distance);
                            robot.HitPoints += zone.EFFECT;
                            robot.HitPoints = Math.Min(robot.HitPoints, robot.Armor.MAX_HP);
                            if (repairman != robot) {
                                repairman.Score += zone.EFFECT;
                            }
                        }

                        battlefield.battlefieldTurn.AddRepair(new ViewerLibrary.Repair(robot.X, robot.Y));
                    }
                }
                repairman.RepairToolUsed++;
                return new RepairAnswerCommand(true);
            } else {
                return new RepairAnswerCommand(false);
            }
        }

        private void addMerchantProcesses() {
            commandProcessor.RegisterProcess<MerchantCommand>(merchantProcess);
        }

        private ACommand merchantProcess(MerchantCommand visitor, RobotAndBattlefield robotAndBattlefield) {
            BattlefieldRobot robot = robotAndBattlefield.ROBOT;
            Battlefield battlefield = robotAndBattlefield.BATTLEFIELD;
            if (battlefield._battlefieldState == BattlefieldState.MERCHANT) {
                return battlefield.Merchant.Buy(robot, visitor.MOTOR_ID, visitor.ARMOR_ID, visitor.CLASS_EQUIPMENT_ID, visitor.REPAIR_HP);
            } else {
                return new ErrorCommand("Cannot use MerchantCommand in state " + battlefield._battlefieldState);
            }
        }

    }
}
