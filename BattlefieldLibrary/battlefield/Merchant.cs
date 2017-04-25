using System;
using System.Collections.Generic;
using BaseLibrary.command;
using BaseLibrary.command.common;
using BaseLibrary.equip;
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

        public void CalcGold(List<BattlefieldRobot> robots, int[] premies) {
            SortedDictionary<int, BattlefieldRobot> robotsByScore = new SortedDictionary<int, BattlefieldRobot>();
            foreach (BattlefieldRobot r in robots) {
                robotsByScore.Add(r.Score, r);
            }
            int i = 0;
            foreach (var pair in robotsByScore) {
                if (i < premies.Length) {
                    pair.Value.Gold += premies[i++];
                } else {
                    break;
                }
            }

            foreach (BattlefieldRobot r in robots) {
                r.Gold += r.Score - r.OldScore;
                r.OldScore = r.Score;
            }
        }

        public MerchantAnswerCommand Buy(BattlefieldRobot r, int motorId, int armorId, int classEquipmentID, int hp) {
            repairHp(r, r.Armor.MAX_HP/10);

            if (hp - r.HitPoints > 0) {
                int possibleToRapairHP = Math.Min(hp - r.HitPoints, r.Gold*4);
                r.Gold -= (possibleToRapairHP+3)/4;
                repairHp(r, possibleToRapairHP);
            }

	        {
		        Motor wantedBuy;
		        if (motorsById.TryGetValue(motorId, out wantedBuy) && !wantedBuy.Equals(r.Motor)) {
			        if (r.Gold >= wantedBuy.COST) {
				        r.Gold -= wantedBuy.COST;
				        r.Motor = wantedBuy;
			        }
		        }
	        }

            {
                Armor wantedBuy;
                if (armorsById.TryGetValue(classEquipmentID, out wantedBuy) && !wantedBuy.Equals(r.Armor)) {
                    if (r.Gold >= wantedBuy.COST) {
                        r.Gold -= wantedBuy.COST;
                        r.Armor = wantedBuy;
                        repairHp(r, r.Armor.MAX_HP);
                    }
                }
            }

            ClassEquipment classEquipment = buyClassEquipment(r, classEquipmentID);

	        return new MerchantAnswerCommand(r.Motor.ID, r.Armor.ID, classEquipment.ID);
        }

        private ClassEquipment buyClassEquipment(BattlefieldRobot r, int classEquipmentId) {
            ClassEquipment classEquipment = null;
            {
                Tank tank = r as Tank;
                if (tank != null) {
                    classEquipment = tank.Gun;
                    Gun wantedBuy;
                    if (gunsById.TryGetValue(classEquipmentId, out wantedBuy) && !wantedBuy.Equals(classEquipment)) {
                        if (r.Gold >= wantedBuy.COST) {
                            r.Gold -= wantedBuy.COST;
                            tank.Gun = wantedBuy;
                        }
                    }
                }
            }

            {
                Miner miner = r as Miner;
                if (miner != null) {
                    classEquipment = miner.MineGun;
                    MineGun wantedBuy;
                    if (mineGunsById.TryGetValue(classEquipmentId, out wantedBuy) && !wantedBuy.Equals(classEquipment)) {
                        if (r.Gold >= wantedBuy.COST) {
                            r.Gold -= wantedBuy.COST;
                            miner.MineGun = wantedBuy;
                        }
                    }
                }
            }

            {
                Repairman repairman = r as Repairman;
                if (repairman != null) {
                    classEquipment = repairman.RepairTool;
                    RepairTool wantedBuy;
                    if (repairToolsById.TryGetValue(classEquipmentId, out wantedBuy) && !wantedBuy.Equals(classEquipment)) {
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
