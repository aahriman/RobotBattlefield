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
            protected readonly Battlefield battlefield;


            protected ArenaCommandVisitor(Battlefield battlefield) {
                this.battlefield = battlefield;
            }

            public virtual NullType visit(GetArmorsCommand visitor, params BattlefieldRobot[] inputs) {
                var response = new ErrorCommand("Unsupported command " + visitor.GetType().Name + ". Arena is in " + battlefield._battlefieldState + ".");
                sendCommandDontWait(response, inputs);
                return null;
            }

            public virtual NullType visit(GetArmorsAnswerCommand visitor, params BattlefieldRobot[] inputs) {
                var response = new ErrorCommand("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + battlefield._battlefieldState + ".");
                sendCommandDontWait(response, inputs);
                return null;
            }

            public virtual NullType visit(GetMotorsCommand visitor, params BattlefieldRobot[] inputs) {
                var response = new ErrorCommand("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + battlefield._battlefieldState + ".");
                sendCommandDontWait(response, inputs);
                return null;
            }

            public virtual NullType visit(GetMotorsAnswerCommand visitor, params BattlefieldRobot[] inputs) {
                var response = new ErrorCommand("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + battlefield._battlefieldState + ".");
                sendCommandDontWait(response, inputs);
                return null;
            }

            public virtual NullType visit(InitCommand visitor, params BattlefieldRobot[] inputs) {
                var response = new ErrorCommand("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + battlefield._battlefieldState + ".");
                sendCommandDontWait(response, inputs);
                return null;
            }

            public virtual NullType visit(InitAnswerCommand visitor, params BattlefieldRobot[] inputs) {
                var response = new ErrorCommand("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + battlefield._battlefieldState + ".");
                sendCommandDontWait(response, inputs);
                return null;
            }

            public virtual NullType visit(DriveCommand visitor, params BattlefieldRobot[] inputs) {
                var response = new ErrorCommand("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + battlefield._battlefieldState + ".");
                sendCommandDontWait(response, inputs);
                return null;
            }

            public virtual NullType visit(DriveAnswerCommand visitor, params BattlefieldRobot[] inputs) {
                var response = new ErrorCommand("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + battlefield._battlefieldState + ".");
                sendCommandDontWait(response, inputs);
                return null;
            }

            public virtual NullType visit(RobotStateCommand visitor, params BattlefieldRobot[] inputs) {
                var response = new ErrorCommand("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + battlefield._battlefieldState + ".");
                sendCommandDontWait(response, inputs);
                return null;
            }

            public virtual NullType visit(ScanCommand visitor, params BattlefieldRobot[] inputs) {
                var response = new ErrorCommand("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + battlefield._battlefieldState + ".");
                sendCommandDontWait(response, inputs);
                return null;
            }

            public virtual NullType visit(ScanAnswerCommand visitor, params BattlefieldRobot[] inputs) {
                var response = new ErrorCommand("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + battlefield._battlefieldState + ".");
                sendCommandDontWait(response, inputs);
                return null;
            }

            public virtual NullType visit(WaitCommand visitor, params BattlefieldRobot[] inputs) {
                var response = new ErrorCommand("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + battlefield._battlefieldState + ".");
                sendCommandDontWait(response, inputs);
                return null;
            }

            public virtual NullType visit(EndLapCommand visitor, params BattlefieldRobot[] inputs) {
                var response = new ErrorCommand("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + battlefield._battlefieldState + ".");
                sendCommandDontWait(response, inputs);
                return null;
            }

            public virtual NullType visit(MerchantCommand visitor, params BattlefieldRobot[] inputs) {
                var response = new ErrorCommand("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + battlefield._battlefieldState + ".");
                sendCommandDontWait(response, inputs);
                return null;
            }

            public virtual NullType visit(MerchantAnswerCommand visitor, params BattlefieldRobot[] inputs) {
                var response = new ErrorCommand("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + battlefield._battlefieldState + ".");
                sendCommandDontWait(response, inputs);
                return null;
            }

            public virtual NullType visit(EndMatchCommand visitor, params BattlefieldRobot[] inputs) {
                var response = new ErrorCommand("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + battlefield._battlefieldState + ".");
                sendCommandDontWait(response, inputs);
                return null;
            }

            public virtual NullType visit(GameTypeCommand visitor, params BattlefieldRobot[] inputs) {
                var response = new ErrorCommand("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + battlefield._battlefieldState + ".");
                sendCommandDontWait(response, inputs);
                return null;
            }

            public virtual NullType visit(GetGunsCommand visitor, params BattlefieldRobot[] inputs) {
                var response = new ErrorCommand("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + battlefield._battlefieldState + ".");
                sendCommandDontWait(response, inputs);
                return null;
            }

            public virtual NullType visit(GetGunsAnswerCommand visitor, params BattlefieldRobot[] inputs) {
                var response = new ErrorCommand("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + battlefield._battlefieldState + ".");
                sendCommandDontWait(response, inputs);
                return null;
            }

            public virtual NullType visit(GetRepairToolCommand visitor, params BattlefieldRobot[] inputs) {
                var response = new ErrorCommand("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + battlefield._battlefieldState + ".");
                sendCommandDontWait(response, inputs);
                return null;
            }

            public virtual NullType visit(GetRepairToolAnswerCommand visitor, params BattlefieldRobot[] inputs) {
                var response = new ErrorCommand("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + battlefield._battlefieldState + ".");
                sendCommandDontWait(response, inputs);
                return null;
            }

            public virtual NullType visit(GetMineGunCommand visitor, params BattlefieldRobot[] inputs) {
                var response = new ErrorCommand("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + battlefield._battlefieldState + ".");
                sendCommandDontWait(response, inputs);
                return null;
            }

            public virtual NullType visit(GetMineGunAnswerCommand visitor, params BattlefieldRobot[] inputs) {
                var response = new ErrorCommand("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + battlefield._battlefieldState + ".");
                sendCommandDontWait(response, inputs);
                return null;
            }

            public virtual NullType visit(RepairCommand visitor, params BattlefieldRobot[] inputs) {
                var response = new ErrorCommand("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + battlefield._battlefieldState + ".");
                sendCommandDontWait(response, inputs);
                return null;
            }

            public virtual NullType visit(RepairAnswerCommand visitor, params BattlefieldRobot[] inputs) {
                var response = new ErrorCommand("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + battlefield._battlefieldState + ".");
                sendCommandDontWait(response, inputs);
                return null;
            }

            public virtual NullType visit(ShotCommand visitor, params BattlefieldRobot[] inputs) {
                var response = new ErrorCommand("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + battlefield._battlefieldState + ".");
                sendCommandDontWait(response, inputs);
                return null;
            }

            public virtual NullType visit(ShotAnswerCommand visitor, params BattlefieldRobot[] inputs) {
                var response = new ErrorCommand("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + battlefield._battlefieldState + ".");
                sendCommandDontWait(response, inputs);
                return null;
            }

            public virtual NullType visit(PutMineCommand visitor, params BattlefieldRobot[] inputs) {
                var response = new ErrorCommand("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + battlefield._battlefieldState + ".");
                sendCommandDontWait(response, inputs);
                return null;
            }

            public virtual NullType visit(PutMineAnswerCommand visitor, params BattlefieldRobot[] inputs) {
                var response = new ErrorCommand("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + battlefield._battlefieldState + ".");
                sendCommandDontWait(response, inputs);
                return null;
            }

            public virtual NullType visit(DetonateMineCommand visitor, params BattlefieldRobot[] inputs) {
                var response = new ErrorCommand("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + battlefield._battlefieldState + ".");
                sendCommandDontWait(response, inputs);
                return null;
            }

            public virtual NullType visit(DetonateMineAnswerCommand visitor, params BattlefieldRobot[] inputs) {
                var response = new ErrorCommand("Unsupported command " + visitor.GetType().Name+ ". Arena is in " + battlefield._battlefieldState + ".");
                sendCommandDontWait(response, inputs);
                return null;
            }

            protected void sendCommandDontWait(ACommand c, params BattlefieldRobot[] robots) {
                foreach (BattlefieldRobot r in robots) {
                    r.SuperNetworkStream.SendCommandAsyncDontWait(c);
                }
            }
        }

        protected class GetCommandVisitor : ArenaCommandVisitor {
            
            public GetCommandVisitor(Battlefield battlefield) : base(battlefield) {}

            public override NullType visit(GetArmorsCommand visitor, params BattlefieldRobot[] inputs) {
                GetArmorsAnswerCommand response = new GetArmorsAnswerCommand(battlefield.Armors);
                sendCommandDontWait(response, inputs);
                return null;
            }

            public override NullType visit(GetMotorsCommand visitor, params BattlefieldRobot[] inputs) {
                var response = new GetMotorsAnswerCommand(battlefield.Motors);
                sendCommandDontWait(response, inputs);
                return null;
            }

            public override NullType visit(GetGunsCommand visitor, params BattlefieldRobot[] inputs) {
                var response = new GetGunsAnswerCommand(battlefield.Guns);
                sendCommandDontWait(response, inputs);
                return null;
            }

            public override NullType visit(GetRepairToolCommand element, params BattlefieldRobot[] inputs) {
                var response = new GetRepairToolAnswerCommand(battlefield.RepairTools);
                sendCommandDontWait(response, inputs);
                return null;
            }

            public override NullType visit(GetMineGunCommand element, params BattlefieldRobot[] inputs) {
                var response = new GetMineGunAnswerCommand(battlefield.MineGuns);
                sendCommandDontWait(response, inputs);
                return null;
            }

            public override NullType visit(InitCommand visitor, params BattlefieldRobot[] inputs) {
                Dictionary<int, int> motorIdForRobots = new Dictionary<int, int>();
                foreach (BattlefieldRobot r in battlefield.robots) {
                    motorIdForRobots[r.ID] = r.Motor.ID;
                }
                foreach (BattlefieldRobot r in inputs) {
                    r.NAME = visitor.NAME;
                    r.ROBOT_TYPE = visitor.ROBOT_TYPE;
                    battlefield.robots.Remove(r);
                    ClassEquipment classEquipment = null;
                    switch (r.ROBOT_TYPE) {
                        case RobotType.MINER:
                            var miner = new Miner(r);
                            battlefield.robots.Add(miner);
                            miner.MineGun = battlefield.MineGuns[0];
                            classEquipment = miner.MineGun;
                            break;
                        case RobotType.TANK:
                            var tank = new Tank(r);
                            battlefield.robots.Add(tank);
                            tank.Gun = battlefield.Guns[0];
                            classEquipment = tank.Gun;
                            break;
                        case RobotType.REPAIRMAN:
                            var repairman = new Repairman(r);
                            battlefield.robots.Add(repairman);
                            repairman.RepairTool = battlefield.RepairTools[0];
                            classEquipment = repairman.RepairTool;
                            break;
                    }

                    ACommand command;
                    if (classEquipment == null) {
                        command = new ErrorCommand("Unsupported RobotType (" + r.ROBOT_TYPE + ") support only" + RobotType.MINER + ", " + RobotType.TANK + ", " + RobotType.REPAIRMAN);
                    } else {
                        command = battlefield.AddToInitAnswereCommand(new InitAnswerCommand(battlefield.MAX_TURN, battlefield.lap, battlefield.MAX_LAP,
                            r.ID, motorIdForRobots, classEquipment.ID, r.Armor.ID));
                    }
                    sendCommandDontWait(command, r);
                }
                return null;
            }
        }

        protected abstract class FinghtCommandVisitor : ArenaCommandVisitor{

            protected FinghtCommandVisitor(Battlefield battlefield) : base(battlefield){}

            public override NullType visit(DriveCommand visitor, params BattlefieldRobot[] inputs) {
                foreach (BattlefieldRobot r in inputs) {
                    if (r.Power <= r.Motor.ROTATE_IN) {
                        r.AngleDrive = visitor.ANGLE;
                    }
                    r.WantedPower = Math.Min(visitor.POWER, 100);
                    r.WantedPower = Math.Max(r.WantedPower, 0);
                    DriveAnswerCommand command = DriveAnswerCommand.GetInstance(r.AngleDrive == visitor.ANGLE);
                    sendCommandDontWait(command, r);
                }
                return null;
            }

            public override NullType visit(ScanCommand visitor, params BattlefieldRobot[] inputs) {
                double minDistance = Battlefield.ARENA_MAX_SIZE*10;
                foreach (BattlefieldRobot r in inputs) {
                    BattlefieldRobot minTarget = r;
                    foreach (BattlefieldRobot target in battlefield.robots) {
                        if (r != target && target.HitPoints > 0 && battlefield.obtacleManager.CanScan(battlefield.turn, r.X, r.Y, target.X, target.Y)) {
                            double distance = Math.Sqrt(Math.Pow(r.X - target.X, 2) + Math.Pow(r.Y - target.Y, 2));
                            if (distance < minDistance) {
                                double degree = AngleUtils.ToDegree(battlefield.computeAngle(r.X, r.Y, target.X, target.Y));
                                if (Math.Abs(degree - visitor.ANGLE) <= visitor.PRECISION) {
                                    minDistance = distance;
                                    minTarget = target;
                                }
                            }
                        }
                    }
                    ScanAnswerCommand command = ScanAnswerCommand.GetInstance((ProtocolDouble)minDistance, minTarget.ID);
                    sendCommandDontWait(command, r);
                    battlefield.battlefieldTurn.AddScan(new Scan(visitor.ANGLE, visitor.PRECISION, minDistance, r.X, r.Y));
                }
                return null;
            }

            public override NullType visit(WaitCommand visitor, params BattlefieldRobot[] inputs) {
                // nothing to do
                return null;
            }
        }

        protected class TankFightCommandVisitor : FinghtCommandVisitor {

            public TankFightCommandVisitor(Battlefield battlefield) : base(battlefield) {}

            public override NullType visit(ShotCommand visitor, params BattlefieldRobot[] inputs) {
                foreach (BattlefieldRobot r in inputs) {
                    Tank tank = (Tank) r; // this CommandVisitor is only for robots witch is Tank and have RobotType RobotType.TANK, so if this conversion fail, it is error in Arena
                    if (tank.BulletsNow < tank.Gun.MAX_BULLETS && tank.HitPoints > 0) {
                        double range = visitor.RANGE;
                        if (range > tank.Gun.MAX_RANGE) {
                            range = tank.Gun.MAX_RANGE;
                        }
                        double toX = range * Math.Cos(AngleUtils.ToRads(visitor.ANGLE)) + tank.X;
                        double toY = range * Math.Sin(AngleUtils.ToRads(visitor.ANGLE)) + tank.Y;
                        battlefield.obtacleManager.ShotChange(battlefield.turn, tank.X, tank.Y, ref toX, ref toY);
                        int toLap = (int)Math.Ceiling(range / tank.Gun.SHOT_SPEED) + battlefield.turn;
                        List<Bullet> bulletList;
                        if (!battlefield.heapBullet.TryGetValue(toLap, out bulletList)) {
                            bulletList = new List<Bullet>();
                            battlefield.heapBullet.Add(toLap, bulletList);
                        }
                        bulletList.Add(new Bullet(battlefield.turn, toLap, toX, toY, tank));
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

            public override NullType visit(PutMineCommand visitor, params BattlefieldRobot[] inputs) {
                foreach (var r in inputs) {
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

            public override NullType visit(DetonateMineCommand visitor, params BattlefieldRobot[] inputs) {
                foreach (var r in inputs) {
                    Miner miner = (Miner) r; // this CommandVisitor is only for robots witch is Miner and have RobotType RobotType.MINER, so if this conversion fail, it is error in Arena
                    Mine mine;
                    if (miner.MINES_BY_ID.TryGetValue(visitor.MINE_ID, out mine) && miner.HitPoints > 0) {
                        miner.MINES_BY_ID.Remove(visitor.MINE_ID);
                        battlefield.detonatedMines.Add(mine);
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

            public override NullType visit(RepairCommand visitor, params BattlefieldRobot[] inputs) {
                foreach (BattlefieldRobot r in inputs) {
                    Repairman repairman = (Repairman) r; // this CommandVisitor is only for robots witch is Repairman and have RobotType RobotType.REPAIRMAN, so if this conversion fail, it is error in Arena
                    if (repairman.RepairToolUsed < repairman.RepairTool.MAX_USAGES && repairman.HitPoints > 0) {
                        double maxDistance = visitor.MAX_DISTANCE;

                        foreach (var robot in battlefield.robots) {
                            double distance = Math.Sqrt(Math.Pow(robot.X - repairman.X, 2) + Math.Pow(robot.Y - repairman.Y, 2));
                            if (distance < maxDistance) {
                                foreach (Zone zone in repairman.RepairTool.ZONES) {
                                    if (zone.DISTANCE > distance) {
                                        r.HitPoints += zone.EFFECT;
                                        if (repairman != r) {
                                            repairman.Score += zone.EFFECT;
                                        }
                                        break;
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

            public override NullType visit(MerchantCommand visitor, params BattlefieldRobot[] inputs) {
                foreach (BattlefieldRobot r in inputs) {
                    MerchantAnswerCommand command = battlefield.merchant.Buy(r, visitor.MOTOR_ID, visitor.ARMOR_ID, visitor.CLASS_EQUIPMENT_ID, visitor.REPAIR_HP);
                    sendCommandDontWait(command, r);
                }
                return null;
            }
        }
    }
}
