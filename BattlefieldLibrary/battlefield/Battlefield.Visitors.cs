using System;
using System.Collections.Generic;
using BaseLibrary;
using BaseLibrary.command;
using BaseLibrary.command.common;
using BaseLibrary.command.equipment;
using BaseLibrary.command.handshake;
using BaseLibrary.command.miner;
using BaseLibrary.command.repairman;
using BaseLibrary.command.tank;
using BaseLibrary.equip;
using BaseLibrary.protocol;
using BaseLibrary.utils;
using BaseLibrary.visitors;
using BattlefieldLibrary.battlefield.robot;
using ViewerLibrary;
using BattlefieldRobot = BattlefieldLibrary.battlefield.robot.BattlefieldRobot;

namespace BattlefieldLibrary.battlefield {
    public partial class Battlefield {
        protected abstract class ArenaVisitor : ICommandVisitor<ACommand, BattlefieldRobot>, ITankVisitor<ACommand, BattlefieldRobot>, IRepairmanVisitor<ACommand, BattlefieldRobot>, IMinerVisitor<ACommand, BattlefieldRobot> {
            protected readonly Battlefield BATTLEFIELD;


            protected ArenaVisitor(Battlefield battlefield) {
                this.BATTLEFIELD = battlefield;
            }

            public virtual ACommand visit(GetArmorsCommand visitor, BattlefieldRobot input) {
                return new ErrorCommand("Unsupported command " + visitor.GetType().Name + ". Arena is in " + BATTLEFIELD._battlefieldState + ".");
            }

            public virtual ACommand visit(GetArmorsAnswerCommand visitor, BattlefieldRobot input) {
                return new ErrorCommand("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + BATTLEFIELD._battlefieldState + ".");
            }

            public virtual ACommand visit(GetMotorsCommand visitor, BattlefieldRobot input) {
                return new ErrorCommand("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + BATTLEFIELD._battlefieldState + ".");
            }

            public virtual ACommand visit(GetMotorsAnswerCommand visitor, BattlefieldRobot input) {
                return new ErrorCommand("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + BATTLEFIELD._battlefieldState + ".");
            }

            public virtual ACommand visit(InitCommand visitor, BattlefieldRobot input) {
                return new ErrorCommand("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + BATTLEFIELD._battlefieldState + ".");
            }

            public virtual ACommand visit(InitAnswerCommand visitor, BattlefieldRobot input) {
                return new ErrorCommand("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + BATTLEFIELD._battlefieldState + ".");
            }

            public virtual ACommand visit(DriveCommand visitor, BattlefieldRobot input) {
                return new ErrorCommand("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + BATTLEFIELD._battlefieldState + ".");
            }

            public virtual ACommand visit(DriveAnswerCommand visitor, BattlefieldRobot input) {
                return new ErrorCommand("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + BATTLEFIELD._battlefieldState + ".");
            }

            public virtual ACommand visit(RobotStateCommand visitor, BattlefieldRobot input) {
                return new ErrorCommand("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + BATTLEFIELD._battlefieldState + ".");
            }

            public virtual ACommand visit(ScanCommand visitor, BattlefieldRobot input) {
                return new ErrorCommand("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + BATTLEFIELD._battlefieldState + ".");
            }

            public virtual ACommand visit(ScanAnswerCommand visitor, BattlefieldRobot input) {
                return new ErrorCommand("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + BATTLEFIELD._battlefieldState + ".");
            }

            public virtual ACommand visit(WaitCommand visitor, BattlefieldRobot input) {
                return new ErrorCommand("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + BATTLEFIELD._battlefieldState + ".");
            }

            public virtual ACommand visit(EndLapCommand visitor, BattlefieldRobot input) {
                return new ErrorCommand("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + BATTLEFIELD._battlefieldState + ".");
            }

            public virtual ACommand visit(MerchantCommand visitor, BattlefieldRobot input) {
                return new ErrorCommand("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + BATTLEFIELD._battlefieldState + ".");
            }

            public virtual ACommand visit(MerchantAnswerCommand visitor, BattlefieldRobot input) {
                return new ErrorCommand("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + BATTLEFIELD._battlefieldState + ".");
            }

            public virtual ACommand visit(EndMatchCommand visitor, BattlefieldRobot input) {
                return new ErrorCommand("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + BATTLEFIELD._battlefieldState + ".");
            }

            public virtual ACommand visit(GameTypeCommand visitor, BattlefieldRobot input) {
                return new ErrorCommand("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + BATTLEFIELD._battlefieldState + ".");
            }

            public virtual ACommand visit(GetGunsCommand visitor, BattlefieldRobot input) {
                return new ErrorCommand("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + BATTLEFIELD._battlefieldState + ".");
            }

            public virtual ACommand visit(GetGunsAnswerCommand visitor, BattlefieldRobot input) {
                return new ErrorCommand("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + BATTLEFIELD._battlefieldState + ".");
            }

            public virtual ACommand visit(GetRepairToolCommand visitor, BattlefieldRobot input) {
                return new ErrorCommand("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + BATTLEFIELD._battlefieldState + ".");
            }

            public virtual ACommand visit(GetRepairToolAnswerCommand visitor, BattlefieldRobot input) {
                return new ErrorCommand("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + BATTLEFIELD._battlefieldState + ".");
            }

            public virtual ACommand visit(GetMineGunCommand visitor, BattlefieldRobot input) {
                return new ErrorCommand("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + BATTLEFIELD._battlefieldState + ".");
            }

            public virtual ACommand visit(GetMineGunAnswerCommand visitor, BattlefieldRobot input) {
                return new ErrorCommand("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + BATTLEFIELD._battlefieldState + ".");
            }

            public virtual ACommand visit(RepairCommand visitor, BattlefieldRobot input) {
                return new ErrorCommand("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + BATTLEFIELD._battlefieldState + ".");
            }

            public virtual ACommand visit(RepairAnswerCommand visitor, BattlefieldRobot input) {
                return new ErrorCommand("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + BATTLEFIELD._battlefieldState + ".");
            }

            public virtual ACommand visit(ShotCommand visitor, BattlefieldRobot input) {
                return new ErrorCommand("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + BATTLEFIELD._battlefieldState + ".");
            }

            public virtual ACommand visit(ShotAnswerCommand visitor, BattlefieldRobot input) {
                return new ErrorCommand("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + BATTLEFIELD._battlefieldState + ".");
            }

            public virtual ACommand visit(PutMineCommand visitor, BattlefieldRobot input) {
                return new ErrorCommand("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + BATTLEFIELD._battlefieldState + ".");
            }

            public virtual ACommand visit(PutMineAnswerCommand visitor, BattlefieldRobot input) {
                return new ErrorCommand("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + BATTLEFIELD._battlefieldState + ".");
            }

            public virtual ACommand visit(DetonateMineCommand visitor, BattlefieldRobot input) {
                return new ErrorCommand("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + BATTLEFIELD._battlefieldState + ".");
            }

            public virtual ACommand visit(DetonateMineAnswerCommand visitor, BattlefieldRobot input) {
                return new ErrorCommand("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + BATTLEFIELD._battlefieldState + ".");
            }
        }

        protected class GetVisitor : ArenaVisitor {
            
            private int teamId = 1;

            public GetVisitor(Battlefield battlefield) : base(battlefield) {}

            public override ACommand visit(GetArmorsCommand visitor, BattlefieldRobot input) {
                return new GetArmorsAnswerCommand(BATTLEFIELD.Armors);
            }

            public override ACommand visit(GetMotorsCommand visitor, BattlefieldRobot input) {
                return new GetMotorsAnswerCommand(BATTLEFIELD.Motors);
            }

            public override ACommand visit(GetGunsCommand visitor, BattlefieldRobot input) {
                return new GetGunsAnswerCommand(BATTLEFIELD.Guns);
            }

            public override ACommand visit(GetRepairToolCommand element, BattlefieldRobot input) {
                return new GetRepairToolAnswerCommand(BATTLEFIELD.RepairTools);
            }

            public override ACommand visit(GetMineGunCommand element, BattlefieldRobot input) {
                return new GetMineGunAnswerCommand(BATTLEFIELD.MineGuns);
            }

            public override ACommand visit(InitCommand visitor, BattlefieldRobot input) {
                BattlefieldRobot r = input; {
                    r.NAME = visitor.NAME;
                    r.ROBOT_TYPE = visitor.ROBOT_TYPE;
                    BATTLEFIELD.robots.Remove(r);
                    ClassEquipment classEquipment = null;
                    switch (r.ROBOT_TYPE) {
                        case RobotType.MINER:
                            var miner = new Miner(r);
                            miner.MineGun = BATTLEFIELD.MineGuns[0];
                            classEquipment = miner.MineGun;
                            r = miner;
                            BATTLEFIELD.robots.Add(miner);
                            BATTLEFIELD.robotsById[miner.ID] = miner;
                            break;
                        case RobotType.TANK:
                            var tank = new Tank(r);
                            tank.Gun = BATTLEFIELD.Guns[0];
                            classEquipment = tank.Gun;
                            r = tank;
                            BATTLEFIELD.robots.Add(tank);
                            BATTLEFIELD.robotsById[tank.ID] = tank;
                            break;
                        case RobotType.REPAIRMAN:
                            var repairman = new Repairman(r);
                            repairman.RepairTool = BATTLEFIELD.RepairTools[0];
                            classEquipment = repairman.RepairTool;
                            r = repairman;
                            BATTLEFIELD.robots.Add(repairman);
                            BATTLEFIELD.robotsById[repairman.ID] = repairman;
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
                            return new ErrorCommand("Too many teams.");
                        }
                    } else {
                        if (BATTLEFIELD.robotsByTeamId[teamId].Count >= BATTLEFIELD.ROBOTS_IN_TEAM) {
                            return new ErrorCommand("Too many robots in one team.");
                        } else {
                            BATTLEFIELD.robotsByTeamId[teamId].Add(r);
                        }
                    }
                    r.TEAM_ID = teamId;
                    if (classEquipment == null) {
                        return new ErrorCommand("Unsupported RobotType (" + r.ROBOT_TYPE + ") support only" + RobotType.MINER + ", " + RobotType.TANK + ", " + RobotType.REPAIRMAN);
                    } else {
                      return BATTLEFIELD.AddToInitAnswereCommand(new InitAnswerCommand(BATTLEFIELD.MAX_TURN, BATTLEFIELD.lap, BATTLEFIELD.MAX_LAP,
                            r.ID, r.TEAM_ID, classEquipment.ID, r.Armor.ID, r.Motor.ID));
                    }
                }
            }
        }

        protected abstract class FinghtVisitor : ArenaVisitor{

            protected FinghtVisitor(Battlefield battlefield) : base(battlefield){}

            public override ACommand visit(DriveCommand visitor, BattlefieldRobot input) {
                BattlefieldRobot r = input; {
                    if (r.HitPoints > 0) {
                        if (r.Power <= r.Motor.ROTATE_IN) {
                            r.AngleDrive = visitor.ANGLE;
                        }
                        r.WantedPower = Math.Min(visitor.POWER, 100);
                        r.WantedPower = Math.Max(r.WantedPower, 0);
                        return DriveAnswerCommand.GetInstance(r.AngleDrive.DEquals(visitor.ANGLE));
                    } else {
                        return DriveAnswerCommand.GetInstance(r.AngleDrive.DEquals(visitor.ANGLE));
                    }
                }
            }

            public override ACommand visit(ScanCommand visitor, BattlefieldRobot input) {
                double minDistance = Battlefield.ARENA_MAX_SIZE*10;
                BattlefieldRobot r = input; {
                    BattlefieldRobot minTarget = r;
                    if (r.HitPoints > 0) {
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
                    
                        BATTLEFIELD.battlefieldTurn.AddScan(new Scan(visitor.ANGLE, visitor.PRECISION, minDistance, r.X,
                                                                     r.Y));
                    }
                    return ScanAnswerCommand.GetInstance((ProtocolDouble)minDistance, minTarget.ID);
                }
            }

            public override ACommand visit(WaitCommand visitor, BattlefieldRobot input) {
                if (input.HitPoints <= 0) {
                    BATTLEFIELD.robotsWaitingForReborn.Add(input);
                }
                return null;
            }
        }

        protected class TankFightVisitor : FinghtVisitor {

            int LOAD_TIME = 20;

            public TankFightVisitor(Battlefield battlefield) : base(battlefield) {}

            public override ACommand visit(ShotCommand visitor, BattlefieldRobot input) {
                BattlefieldRobot r = input; {
                    Tank tank = (Tank) r; // this CommandVisitor is only for robots witch is Tank and have RobotType RobotType.TANK, so if this conversion fail, it is error in Arena
                    if (tank.GunsToLoad < tank.Gun.MAX_BULLETS && tank.HitPoints > 0) {
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
                        int loadAtTurn = BATTLEFIELD.turn + LOAD_TIME;
                        List<Tank> list;
                        if (!BATTLEFIELD.gunLoaded.TryGetValue(loadAtTurn, out list)) {
                            list = new List<Tank>();
                            BATTLEFIELD.gunLoaded.Add(loadAtTurn, list);
                        }
                        list.Add(tank);
                        tank.GunsToLoad++;
                        return new ShotAnswerCommand(true);
                    } else {
                        return new ShotAnswerCommand(false);
                    }
                }
            }
        }

        protected class MinerFightVisitor : FinghtVisitor {
            public MinerFightVisitor(Battlefield battlefield) : base(battlefield) {}

            public override ACommand visit(PutMineCommand visitor, BattlefieldRobot input) {
                BattlefieldRobot r = input; {
                    Miner miner = (Miner) r; // this CommandVisitor is only for robots witch is Miner and have RobotType RobotType.MINER, so if this conversion fail, it is error in Arena
                    if (miner.MinesNow < miner.MineGun.MAX_MINES && miner.HitPoints > 0) {
                        int id = PutMineAnswerCommand.FALSE_MINE_ID + 1;
                        while (miner.MINES_BY_ID.ContainsKey(id)) {
                            id++;
                        }
                        miner.MINES_BY_ID.Add(id, new Mine(id, miner.X, miner.Y, miner));
                        return new PutMineAnswerCommand(true, id);
                    } else {
                        return new PutMineAnswerCommand(false, PutMineAnswerCommand.FALSE_MINE_ID);
                    }
                }
            }

            public override ACommand visit(DetonateMineCommand visitor, BattlefieldRobot input) {
                BattlefieldRobot r = input; {
                    Miner miner = (Miner) r; // this CommandVisitor is only for robots witch is Miner and have RobotType RobotType.MINER, so if this conversion fail, it is error in Arena
                    Mine mine;
                    if (miner.MINES_BY_ID.TryGetValue(visitor.MINE_ID, out mine) && miner.HitPoints > 0) {
                        miner.MINES_BY_ID.Remove(visitor.MINE_ID);
                        BATTLEFIELD.detonatedMines.Add(mine);
                        return new DetonateMineAnswerCommand(true);
                    } else {
                        return new DetonateMineAnswerCommand(false);
                    }
                }
            }
        }

        protected class RepairmanFightVisitor : FinghtVisitor {
            public RepairmanFightVisitor(Battlefield battlefield) : base(battlefield) { }

            public override ACommand visit(RepairCommand visitor, BattlefieldRobot input) {
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
                                    r.HitPoints = Math.Min(r.HitPoints, r.Armor.MAX_HP);
                                    if (repairman != r) {
                                        repairman.Score += zone.EFFECT;
                                    }
                                }

                                BATTLEFIELD.battlefieldTurn.AddRepair(new ViewerLibrary.Repair(robot.X, robot.Y));
                            }
                        }
                        repairman.RepairToolUsed++;
                        return new RepairAnswerCommand(true);
                    } else {
                        return new RepairAnswerCommand(false);
                    }
                }
            }
        }

        protected class MerchantVisitor : ArenaVisitor {

            public MerchantVisitor(Battlefield battlefield) : base(battlefield) {}

            public override ACommand visit(MerchantCommand visitor, BattlefieldRobot input) {
                BattlefieldRobot r = input; 
                return BATTLEFIELD.merchant.Buy(r, visitor.MOTOR_ID, visitor.ARMOR_ID, visitor.CLASS_EQUIPMENT_ID, visitor.REPAIR_HP);
            }
        }
    }
}
