using System;
using BaseLibrary.equip;
using BaseLibrary.utils;
using BaseLibrary.utils.protocolV1_0Utils;

namespace BaseLibrary.command.v1._0.equipment {
    public class MineGunV1_0 : MineGun, InnerSerializerV1_0 {
        private const string NAME = "MINE_GUN";

        public MineGunV1_0(int ID, int COST, int maxMines, Zone[] ZONES) : base(ID, COST, maxMines, ZONES) { }
        public MineGunV1_0(MineGun mineGun) :
			base(mineGun.ID, mineGun.COST, mineGun.MAX_MINES, ZoneV1_0.Convert(mineGun.ZONES)) { }

        public string Serialize(Deep deep) {
            return ProtocolV1_0Utils.SerializeParams(NAME, deep, ID, COST, MAX_MINES, ZONES);
        }

        public static bool Deserialize(string orig, Deep deep, out MineGunV1_0 deserialized) {
            string[] rest;
            if (ProtocolV1_0Utils.GetParams(orig, NAME, deep, out rest)) {
                if (rest.Length == 4) {
                    int[] paramsInt;
                    string[] zonesString;
                    if (Parser.TryParse(new int[] { 0, 1, 2 }, rest, out paramsInt) &&
                        ProtocolV1_0Utils.Deserialize(rest[3], out zonesString, deep.NEXT)) {
                        ZoneV1_0[] zones = new ZoneV1_0[zonesString.Length];
                        for (int i = 0; i < zones.Length; i++) {
                            if (!ZoneV1_0.Deserialize(zonesString[i], deep.NEXT.NEXT, out zones[i])) {
                                deserialized = null;
                                return false;
                            }
                        }
                        deserialized = new MineGunV1_0(paramsInt[0], paramsInt[1], paramsInt[2], zones);
                        return true;
                    }
                }
            }
            deserialized = null;
            return false;
        }

        public static MineGunV1_0[] Convert(MineGun[] repairTools) {
            MineGunV1_0[] output = new MineGunV1_0[repairTools.Length];
            for (int i = 0; i < repairTools.Length; i++) {
                output[i] = new MineGunV1_0(repairTools[i]);
            }
            return output;
        }
    }
}
