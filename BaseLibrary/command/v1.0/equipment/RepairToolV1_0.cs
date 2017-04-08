using System;
using BaseLibrary.equip;
using BaseLibrary.utils;
using BaseLibrary.utils.protocolV1_0Utils;

namespace BaseLibrary.command.v1._0.equipment {
    public class RepairToolV1_0 : RepairTool, InnerSerializerV1_0 {
        private const string NAME = "REPAIR_TOOL";

        public RepairToolV1_0(int ID, int COST, int maxUsages, Zone[] ZONES) : base(ID, COST, maxUsages, ZONES) {}
        public RepairToolV1_0(RepairTool repairTool) :
			base(repairTool.ID, repairTool.COST, repairTool.MAX_USAGES, ZoneV1_0.Convert(repairTool.ZONES)) { }

        public string Serialize(Deep deep) {
            return ProtocolV1_0Utils.SerializeParams(NAME, deep, ID, COST, MAX_USAGES, ZONES);
        }

        public static bool Deserialize(String orig, Deep deep, out RepairToolV1_0 deserialized) {
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
                        deserialized = new RepairToolV1_0(paramsInt[0], paramsInt[1], paramsInt[2], zones);
                        return true;
                    }
                }
            }
            deserialized = null;
            return false;
        }

        public static RepairToolV1_0[] Convert(RepairTool[] repairTools) {
            RepairToolV1_0[] output = new RepairToolV1_0[repairTools.Length];
            for (int i = 0; i < repairTools.Length; i++) {
                output[i] = new RepairToolV1_0(repairTools[i]);
            }
            return output;
        }
    }
}
