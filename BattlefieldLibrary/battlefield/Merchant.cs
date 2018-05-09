using System;
using System.Collections.Generic;
using BaseLibrary.communication.command.common;
using BaseLibrary.equipment;
using BattlefieldLibrary.battlefield.robot;

namespace BattlefieldLibrary.battlefield {
    public class Merchant {
	    private readonly Dictionary<int, Motor> motorsById = new Dictionary<int, Motor>();
	    private readonly Dictionary<int, Armor> armorsById = new Dictionary<int, Armor>();
	    private readonly Dictionary<int, Gun> gunsById = new Dictionary<int, Gun>();
        private readonly Dictionary<int, RepairTool> repairToolsById = new Dictionary<int, RepairTool>();
        private readonly Dictionary<int, MineGun> mineGunsById = new Dictionary<int, MineGun>();

        public Merchant(Motor[] motors, Armor[] armors, Gun[] guns, RepairTool[] repairTools, MineGun[] mineGuns) {
            foreach (var m in motors) {
                motorsById.Add(m.ID, m);
            }

            foreach (var a in armors) {
                armorsById.Add(a.ID, a);
            }

            foreach (var g in guns) {
                gunsById.Add(g.ID, g);
            }

            foreach (var repairTool in repairTools) {
                repairToolsById.Add(repairTool.ID, repairTool);
            }

            foreach (var mineGun in mineGuns) {
                mineGunsById.Add(mineGun.ID, mineGun);
            }
        }

        /// <summary>
        /// Add gold beside robots score. Robots gold is increased by bonus[i], where i is robot position sort by score.
        /// </summary>
        /// <param name="robots"></param>
        /// <param name="bonus"></param>
        public void CalcGold(List<BattlefieldRobot> robots, int[] bonus) {
            SortedDictionary<int, BattlefieldRobot> robotsByScore = new SortedDictionary<int, BattlefieldRobot>();
            foreach (BattlefieldRobot r in robots) {
                robotsByScore.Add(r.Score, r);
            }
            int i = 0;
            foreach (var pair in robotsByScore) {
                if (i < bonus.Length) {
                    pair.Value.Gold += bonus[i++];
                } else {
                    break;
                }
            }

            foreach (BattlefieldRobot r in robots) {
                r.Gold += r.Score - r.OldScore;
                r.OldScore = r.Score;
            }
        }

        /// <summary>
        /// It buy and repair hit-points for robot r. Robot can only buy equipment witch is cheaper then robot gold.
        /// </summary>
        /// <param name="r"></param>
        /// <param name="motorId"></param>
        /// <param name="armorId"></param>
        /// <param name="classEquipmentID"></param>
        /// <param name="hp"></param>
        /// <returns></returns>
        public MerchantAnswerCommand Buy(BattlefieldRobot r, int motorId, int armorId, int classEquipmentID, int hp) {
            repairHp(r, r.Armor.MAX_HP/10);


           {
                if (armorsById.TryGetValue(classEquipmentID, out Armor wantedBuy) && !wantedBuy.Equals(r.Armor)) {
                    if (r.Gold >= wantedBuy.COST) {
                        r.Gold -= wantedBuy.COST;
                        r.Armor = wantedBuy;
                        repairHp(r, r.Armor.MAX_HP);
                    }
                }
            }

            if (hp - r.HitPoints > 0) {
                int possibleToRepairHP = Math.Min(hp - r.HitPoints, r.Gold*4);
                r.Gold -= (possibleToRepairHP+3)/4;
                repairHp(r, possibleToRepairHP);
            }

	        {
	            if (motorsById.TryGetValue(motorId, out Motor wantedBuy) && !wantedBuy.Equals(r.Motor)) {
			        if (r.Gold >= wantedBuy.COST) {
				        r.Gold -= wantedBuy.COST;
				        r.Motor = wantedBuy;
			        }
		        }
	        }

            IClassEquipment classEquipment = buyClassEquipment(r, classEquipmentID);

	        return new MerchantAnswerCommand(r.Motor.ID, r.Armor.ID, classEquipment.ID);
        }

        private IClassEquipment buyClassEquipment(BattlefieldRobot r, int classEquipmentId) {
            IClassEquipment classEquipment = null;
            {
                Tank tank = r as Tank;
                if (tank != null) {
                    classEquipment = tank.Gun;
                    if (gunsById.TryGetValue(classEquipmentId, out Gun wantedBuy) && !wantedBuy.Equals(classEquipment)) {
                        if (r.Gold >= wantedBuy.COST) {
                            r.Gold -= wantedBuy.COST;
                            tank.Gun = wantedBuy;
                        }
                    }
                }
            }

            {
                MineLayer mineLayer = r as MineLayer;
                if (mineLayer != null) {
                    classEquipment = mineLayer.MineGun;
                    if (mineGunsById.TryGetValue(classEquipmentId, out MineGun wantedBuy) && !wantedBuy.Equals(classEquipment)) {
                        if (r.Gold >= wantedBuy.COST) {
                            r.Gold -= wantedBuy.COST;
                            mineLayer.MineGun = wantedBuy;
                        }
                    }
                }
            }

            {
                Repairman repairman = r as Repairman;
                if (repairman != null) {
                    classEquipment = repairman.RepairTool;
                    if (repairToolsById.TryGetValue(classEquipmentId, out RepairTool wantedBuy) && !wantedBuy.Equals(classEquipment)) {
                        if (r.Gold >= wantedBuy.COST) {
                            r.Gold -= wantedBuy.COST;
                            repairman.RepairTool = wantedBuy;
                        }
                    }
                }
            }
            return classEquipment;
        }

        private static void repairHp(BattlefieldRobot r, int hp) {
            r.HitPoints += hp;
            r.HitPoints = Math.Min(r.HitPoints, r.Armor.MAX_HP);
        }
    }
}
