using System;
using System.Collections.Generic;
using BaseLibrary;
using BaseLibrary.command;
using BaseLibrary.command.equipment;
using BaseLibrary.command.miner;
using BaseLibrary.command.repairman;
using BaseLibrary.command.tank;
using BaseLibrary.equip;
using BaseLibrary.utils;
using BaseLibrary.visitors;
using BattlefieldLibrary.battlefield.robot;
using ViewerLibrary;
using BattlefieldRobot = BattlefieldLibrary.battlefield.robot.BattlefieldRobot;

namespace BattlefieldLibrary.battlefield {
    public partial class Battlefield {
        protected abstract class ArenaCommandVisitor : ICommandVisitor<NullType, robot.BattlefieldRobot>, ITankCommandVisitor<NullType, robot.BattlefieldRobot>, IRepairmanCommandVisitor<NullType, robot.BattlefieldRobot>, IMinerCommandVisitor<NullType, robot.BattlefieldRobot> {
            protected readonly Battlefield BATTLEFIELD;


            protected ArenaCommandVisitor(Battlefield battlefield) {
                this.BATTLEFIELD = battlefield;
            }

            public virtual NullType visit(GetArmorsCommand visitor, BattlefieldRobot input) {
                throw new Exception("Unsupported command " + visitor.GetType().Name + ". Arena is in " + BATTLEFIELD._battlefieldState + ".");
            }

            public virtual NullType visit(GetArmorsAnswerCommand visitor, BattlefieldRobot input) {
                throw new Exception("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + BATTLEFIELD._battlefieldState + ".");
            }

            public virtual NullType visit(GetMotorsCommand visitor, BattlefieldRobot input) {
                throw new Exception("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + BATTLEFIELD._battlefieldState + ".");
            }

            public virtual NullType visit(GetMotorsAnswerCommand visitor, BattlefieldRobot input) {
                throw new Exception("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + BATTLEFIELD._battlefieldState + ".");
            }

            public virtual NullType visit(InitCommand visitor, BattlefieldRobot input) {
                throw new Exception("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + BATTLEFIELD._battlefieldState + ".");
            }

            public virtual NullType visit(InitAnswerCommand visitor, BattlefieldRobot input) {
                throw new Exception("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + BATTLEFIELD._battlefieldState + ".");
            }

            public virtual NullType visit(DriveCommand visitor, BattlefieldRobot input) {
                throw new Exception("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + BATTLEFIELD._battlefieldState + ".");
            }

            public virtual NullType visit(DriveAnswerCommand visitor, BattlefieldRobot input) {
                throw new Exception("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + BATTLEFIELD._battlefieldState + ".");
            }

            public virtual NullType visit(RobotStateCommand visitor, BattlefieldRobot input) {
                throw new Exception("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + BATTLEFIELD._battlefieldState + ".");
            }

            public virtual NullType visit(ScanCommand visitor, BattlefieldRobot input) {
                throw new Exception("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + BATTLEFIELD._battlefieldState + ".");
            }

            public virtual NullType visit(ScanAnswerCommand visitor, BattlefieldRobot input) {
                throw new Exception("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + BATTLEFIELD._battlefieldState + ".");
            }

            public virtual NullType visit(WaitCommand visitor, BattlefieldRobot input) {
                throw new Exception("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + BATTLEFIELD._battlefieldState + ".");
            }

            public virtual NullType visit(EndLapCommand visitor, BattlefieldRobot input) {
                throw new Exception("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + BATTLEFIELD._battlefieldState + ".");
            }

            public virtual NullType visit(MerchantCommand visitor, BattlefieldRobot input) {
                throw new Exception("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + BATTLEFIELD._battlefieldState + ".");
            }

            public virtual NullType visit(MerchantAnswerCommand visitor, BattlefieldRobot input) {
                throw new Exception("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + BATTLEFIELD._battlefieldState + ".");
            }

            public virtual NullType visit(EndMatchCommand visitor, BattlefieldRobot input) {
                throw new Exception("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + BATTLEFIELD._battlefieldState + ".");
            }

            public virtual NullType visit(GameTypeCommand visitor, BattlefieldRobot input) {
                throw new Exception("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + BATTLEFIELD._battlefieldState + ".");
            }

            public virtual NullType visit(GetGunsCommand visitor, BattlefieldRobot input) {
                throw new Exception("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + BATTLEFIELD._battlefieldState + ".");
            }

            public virtual NullType visit(GetGunsAnswerCommand visitor, BattlefieldRobot input) {
                throw new Exception("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + BATTLEFIELD._battlefieldState + ".");
            }

            public virtual NullType visit(GetRepairToolCommand visitor, BattlefieldRobot input) {
                throw new Exception("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + BATTLEFIELD._battlefieldState + ".");
            }

            public virtual NullType visit(GetRepairToolAnswerCommand visitor, BattlefieldRobot input) {
                throw new Exception("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + BATTLEFIELD._battlefieldState + ".");
            }

            public virtual NullType visit(GetMineGunCommand visitor, BattlefieldRobot input) {
                throw new Exception("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + BATTLEFIELD._battlefieldState + ".");
            }

            public virtual NullType visit(GetMineGunAnswerCommand visitor, BattlefieldRobot input) {
                throw new Exception("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + BATTLEFIELD._battlefieldState + ".");
            }

            public virtual NullType visit(RepairCommand visitor, BattlefieldRobot input) {
                throw new Exception("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + BATTLEFIELD._battlefieldState + ".");
            }

            public virtual NullType visit(RepairAnswerCommand visitor, BattlefieldRobot input) {
                throw new Exception("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + BATTLEFIELD._battlefieldState + ".");
            }

            public virtual NullType visit(ShotCommand visitor, BattlefieldRobot input) {
                throw new Exception("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + BATTLEFIELD._battlefieldState + ".");
            }

            public virtual NullType visit(ShotAnswerCommand visitor, BattlefieldRobot input) {
                throw new Exception("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + BATTLEFIELD._battlefieldState + ".");
            }

            public virtual NullType visit(PutMineCommand visitor, BattlefieldRobot input) {
                throw new Exception("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + BATTLEFIELD._battlefieldState + ".");
            }

            public virtual NullType visit(PutMineAnswerCommand visitor, BattlefieldRobot input) {
                throw new Exception("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + BATTLEFIELD._battlefieldState + ".");
            }

            public virtual NullType visit(DetonateMineCommand visitor, BattlefieldRobot input) {
                throw new Exception("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + BATTLEFIELD._battlefieldState + ".");
            }

            public virtual NullType visit(DetonateMineAnswerCommand visitor, BattlefieldRobot input) {
                throw new Exception("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + BATTLEFIELD._battlefieldState + ".");
            }

            protected void sendCommandDontWait(ACommand c, BattlefieldRobot robot) {
                robot.SuperNetworkStream.SendCommandAsyncDontWait(c);
            }
        }

        protected class GetCommandVisitor : ArenaCommandVisitor {
            
            private int teamId = 1;

            public GetCommandVisitor(Battlefield battlefield) : base(battlefield) {}

            public override NullType visit(GetArmorsCommand visitor, BattlefieldRobot input) {
                GetArmorsAnswerCommand response = new GetArmorsAnswerCommand(BATTLEFIELD.Armors);
                sendCommandDontWait(response, input);
                return null;
            }

            public override NullType visit(GetMotorsCommand visitor, BattlefieldRobot input) {
                var response = new GetMotorsAnswerCommand(BATTLEFIELD.Motors);
                sendCommandDontWait(response, input);
                return null;
            }

            public override NullType visit(GetGunsCommand visitor, BattlefieldRobot input) {
                var response = new GetGunsAnswerCommand(BATTLEFIELD.Guns);
                sendCommandDontWait(response, input);
                return null;
            }

            public override NullType visit(GetRepairToolCommand element, BattlefieldRobot input) {
                var response = new GetRepairToolAnswerCommand(BATTLEFIELD.RepairTools);
                sendCommandDontWait(response, input);
                return null;
            }

            public override NullType visit(GetMineGunCommand element, BattlefieldRobot input) {
                var response = new GetMineGunAnswerCommand(BATTLEFIELD.MineGuns);
                sendCommandDontWait(response, input);
                return null;
            }

            public override NullType visit(InitCommand visitor, BattlefieldRobot input) {
                BattlefieldRobot r = input; {
                    r.NAME = visitor.NAME;
                    r.ROBOT_TYPE = visitor.ROBOT_TYPE;
                    BATTLEFIELD.robots.Remove(r);
                    ClassEquipment classEquipment = null;
                    switch (r.ROBOT_TYPE) {
                        case RobotType.MINER:
                            var miner = new Miner(r);
                            BATTLEFIELD.robots.Add(miner);
                            miner.MineGun = BATTLEFIELD.MineGuns[0];
                            classEquipment = miner.MineGun;
                            r = miner;
                            break;
                        case RobotType.TANK:
                            var tank = new Tank(r);
                            BATTLEFIELD.robots.Add(tank);
                            tank.Gun = BATTLEFIELD.Guns[0];
                            classEquipment = tank.Gun;
                            r = tank;
                            break;
                        case RobotType.REPAIRMAN:
                            var repairman = new Repairman(r);
                            BATTLEFIELD.robots.Add(repairman);
                            repairman.RepairTool = BATTLEFIELD.RepairTools[0];
                            classEquipment = repairman.RepairTool;
                            r = repairman;
                            break;
                    }

                    int teamId;
                    if (!BATTLEFIELD.robotTeamIdByTeamName.TryGetValue(visitor.TEAM_NAME, out teamId)) {
                        if (BATTLEFIELD.robotTeamIdByTeamName.Count <
                            BATTLEFIELD.MAX_ROBOTS / BATTLEFIELD.ROBOTS_IN_TEAM) {
                            teamId = this.teamId++;
                            BATTLEFIELD.robotsByTeamId[teamId] = new List<BattlefieldRobot> {r};
                            BATTLEFIELD.robotTeamIdByTeamName.Add(visitor.TEAM_NAME, teamId);
                        } else {
                            throw new Exception("Too many teams.");
                        }
                    } else {
                        if (BATTLEFIELD.robotsByTeamId[teamId].Count >= BATTLEFIELD.ROBOTS_IN_TEAM) {
                            throw new Exception("Too many robots in one team.");
                        } else {
                            BATTLEFIELD.robotsByTeamId[teamId].Add(r);
                        }
                    }
                    r.TEAM_ID = teamId;
                    ACommand command;
                    if (classEquipment == null) {
                        throw new Exception("Unsupported RobotType (" + r.ROBOT_TYPE + ") support only" + RobotType.MINER + ", " + RobotType.TANK + ", " + RobotType.REPAIRMAN);
                    } else {
                        command = BATTLEFIELD.AddToInitAnswereCommand(new InitAnswerCommand(BATTLEFIELD.MAX_TURN, BATTLEFIELD.lap, BATTLEFIELD.MAX_LAP,
                            r.ID, r.TEAM_ID, classEquipment.ID, r.Armor.ID));
                    }
                    sendCommandDontWait(command, r);
                }
                return null;
            }
        }

        protected abstract class FinghtCommandVisitor : ArenaCommandVisitor{

            protected FinghtCommandVisitor(Battlefield battlefield) : base(battlefield){}

            public override NullType visit(DriveCommand visitor, BattlefieldRobot input) {
                BattlefieldRobot r = input; {
                    if (r.HitPoints > 0) {
                        if (r.Power <= r.Motor.ROTATE_IN) {
                            r.AngleDrive = visitor.ANGLE;
                        }
                        r.WantedPower = Math.Min(visitor.POWER, 100);
                        r.WantedPower = Math.Max(r.WantedPower, 0);
                        DriveAnswerCommand command = DriveAnswerCommand.GetInstance(r.AngleDrive.DEquals(visitor.ANGLE));
                        sendCommandDontWait(command, r);
                    } else {
                        DriveAnswerCommand command = DriveAnswerCommand.GetInstance(r.AngleDrive.DEquals(visitor.ANGLE));
                        sendCommandDontWait(command, r);
                    }
                }
                return null;
            }

            public override NullType visit(ScanCommand visitor, BattlefieldRobot input) {
                double minDistance = Battlefield.ARENA_MAX_SIZE*10;
                BattlefieldRobot r = input; {
                    BattlefieldRobot minTarget = r;
                    foreach (BattlefieldRobot target in BATTLEFIELD.robots) {
                        if (r != target && target.HitPoints > 0 && BATTLEFIELD.obtacleManager.CanScan(BATTLEFIELD.turn, r.X, r.Y, target.X, target.Y)) {
                            double distance = Math.Sqrt(Math.Pow(r.X - target.X, 2) + Math.Pow(r.Y - target.Y, 2));
                            if (distance < minDistance) {
                                double degree = AngleUtils.ToDegree(BATTLEFIELD.computeAngle(r.X, r.Y, target.X, target.Y));
                                if (Math.Abs(degree - visitor.ANGLE) <= visitor.PRECISION) {
                                    minDistance = distance;
                                    minTarget = target;
                                }
                            }
                        }
                    }
                    ScanAnswerCommand command = ScanAnswerCommand.GetInstance((ProtocolDouble)minDistance, minTarget.ID);
                    sendCommandDontWait(command, r);
                    BATTLEFIELD.battlefieldTurn.AddScan(new Scan(visitor.ANGLE, visitor.PRECISION, minDistance, r.X, r.Y));
                }
                return null;
            }

            public override NullType visit(WaitCommand visitor, BattlefieldRobot input) {
                if (input.HitPoints <= 0) {
                    BATTLEFIELD.robotsWaitingForReborn.Add(input);
                    List<BattlefieldRobot> respawnedRobots;
                    int respawnTurn = BATTLEFIELD.turn + BATTLEFIELD.RESPAWN_TIMEOUT;
                    if (!BATTLEFIELD.respawnRobotAtTurn.TryGetValue(respawnTurn, out respawnedRobots)) {
                        respawnedRobots = new List<BattlefieldRobot>();
                        BATTLEFIELD.respawnRobotAtTurn.Add(respawnTurn, respawnedRobots);
                    }
                    respawnedRobots.Add(input);
                }
                return null;
            }
        }

        protected class TankFightCommandVisitor : FinghtCommandVisitor {

            public TankFightCommandVisitor(Battlefield battlefield) : base(battlefield) {}

            public override NullType visit(ShotCommand visitor, BattlefieldRobot input) {
                BattlefieldRobot r = input; {
                    Tank tank = (Tank) r; // this CommandVisitor is only for robots witch is Tank and have RobotType RobotType.TANK, so if this conversion fail, it is error in Arena
                    if (tank.BulletsNow < tank.Gun.MAX_BULLETS && tank.HitPoints > 0) {
                        double range = visitor.RANGE;
                        if (range > tank.Gun.MAX_RANGE) {
                            range = tank.Gun.MAX_RANGE;
                        }
                        double toX = range * Math.Cos(AngleUtils.ToRads(visitor.ANGLE)) + tank.X;
                        double toY = range * Math.Sin(AngleUtils.ToRads(visitor.ANGLE)) + tank.Y;
                        BATTLEFIELD.obtacleManager.ShotChange(BATTLEFIELD.turn, tank.X, tank.Y, ref toX, ref toY);
                        int toLap = (int)Math.Ceiling(range / tank.Gun.SHOT_SPEED) + BATTLEFIELD.turn;
                        List<Bullet> bulletList;
                        if (!BATTLEFIELD.heapBullet.TryGetValue(toLap, out bulletList)) {
                            bulletList = new List<Bullet>();
                            BATTLEFIELD.heapBullet.Add(toLap, bulletList);
                        }
                        bulletList.Add(new Bullet(BATTLEFIELD.turn, toLap, toX, toY, tank));
                        tank.BulletsNow++;
                        ShotAnswerCommand command = new ShotAnswerCommand(true);
                        sendCommandDontWait(command, r);
                    } else {
                        ShotAnswerCommand command = new ShotAnswerCommand(false);
                        sendCommandDontWait(command, r);
                    }
                }
                return null;
            }
        }

        protected class MinerFightCommandVisitor : FinghtCommandVisitor {
            public MinerFightCommandVisitor(Battlefield battlefield) : base(battlefield) {}

            public override NullType visit(PutMineCommand visitor, BattlefieldRobot input) {
                BattlefieldRobot r = input; {
                    Miner miner = (Miner) r; // this CommandVisitor is only for robots witch is Miner and have RobotType RobotType.MINER, so if this conversion fail, it is error in Arena
                    if (miner.MinesNow < miner.MineGun.MAX_MINES && miner.HitPoints > 0) {
                        int id = PutMineAnswerCommand.FALSE_MINE_ID + 1;
                        while (miner.MINES_BY_ID.ContainsKey(id)) {
                            id++;
                        }
                        miner.MINES_BY_ID.Add(id, new Mine(id, miner.X, miner.Y, miner));
                        PutMineAnswerCommand command = new PutMineAnswerCommand(true, id);
                        sendCommandDontWait(command, r);
                    } else {
                        PutMineAnswerCommand command = new PutMineAnswerCommand(false, PutMineAnswerCommand.FALSE_MINE_ID);
                        sendCommandDontWait(command, r);
                    }
                }
                return null;
            }

            public override NullType visit(DetonateMineCommand visitor, BattlefieldRobot input) {
                BattlefieldRobot r = input; {
                    Miner miner = (Miner) r; // this CommandVisitor is only for robots witch is Miner and have RobotType RobotType.MINER, so if this conversion fail, it is error in Arena
                    Mine mine;
                    if (miner.MINES_BY_ID.TryGetValue(visitor.MINE_ID, out mine) && miner.HitPoints > 0) {
                        miner.MINES_BY_ID.Remove(visitor.MINE_ID);
                        BATTLEFIELD.detonatedMines.Add(mine);
                        DetonateMineAnswerCommand command = new DetonateMineAnswerCommand(true);
                        sendCommandDontWait(command, r);
                    } else {
                        DetonateMineAnswerCommand command = new DetonateMineAnswerCommand(false);
                        sendCommandDontWait(command, r);
                    }
                }
                return null;
            }
        }

        protected class RepairmanFightCommandVisitor : FinghtCommandVisitor {
            public RepairmanFightCommandVisitor(Battlefield battlefield) : base(battlefield) { }

            public override NullType visit(RepairCommand visitor, BattlefieldRobot input) {
                BattlefieldRobot r = input; {
                    Repairman repairman = (Repairman) r; // this CommandVisitor is only for robots witch is Repairman and have RobotType RobotType.REPAIRMAN, so if this conversion fail, it is error in Arena
                    if (repairman.RepairToolUsed < repairman.RepairTool.MAX_USAGES && repairman.HitPoints > 0) {
                        foreach (BattlefieldRobot robot in BATTLEFIELD.robots) {
                            if (robot.HitPoints > 0) {
                                double distance = EuclideanSpaceUtils.Distance(robot.GetPosition(),
                                                                               repairman.GetPosition());
                                if (distance < visitor.MAX_DISTANCE) {
                                    Zone zone = Zone.GetZoneByDistance(repairman.RepairTool.ZONES, distance);
                                    r.HitPoints += zone.EFFECT;
                                    if (repairman != r) {
                                        repairman.Score += zone.EFFECT;
                                    }
                                }
                            }
                        }
                        repairman.RepairToolUsed++;
                        RepairAnswerCommand command = new RepairAnswerCommand(true);
                        sendCommandDontWait(command, r);
                    } else {
                        RepairAnswerCommand command = new RepairAnswerCommand(false);
                        sendCommandDontWait(command, r);
                    }
                }
                return null;
            }
        }

        protected class MerchantCommandVisitor : ArenaCommandVisitor {

            public MerchantCommandVisitor(Battlefield battlefield) : base(battlefield) {}

            public override NullType visit(MerchantCommand visitor, BattlefieldRobot input) {
                BattlefieldRobot r = input; {
                    MerchantAnswerCommand command = BATTLEFIELD.merchant.Buy(r, visitor.MOTOR_ID, visitor.ARMOR_ID, visitor.CLASS_EQUIPMENT_ID, visitor.REPAIR_HP);
                    sendCommandDontWait(command, r);
                }
                return null;
            }
        }
    }
}
