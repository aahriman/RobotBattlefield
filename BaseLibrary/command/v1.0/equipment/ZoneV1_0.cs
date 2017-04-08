using System;
using BaseLibrary.equip;
using BaseLibrary.utils;
using BaseLibrary.utils.protocolV1_0Utils;

namespace BaseLibrary.command.v1._0.equipment {
    public class ZoneV1_0 : Zone, InnerSerializerV1_0 {
        private const string NAME = "ZONE";

        public ZoneV1_0(int distance, int effect) : base(distance, effect) { }

        public ZoneV1_0(Zone zone) : base(zone.DISTANCE, zone.EFFECT) { }

        public string Serialize(Deep deep) {
            return ProtocolV1_0Utils.SerializeParams(NAME, deep, DISTANCE, EFFECT);
        }

        public static bool Deserialize(String orig, Deep deep, out ZoneV1_0 deserialized) {
            string[] rest;
            int[] param;
            if (ProtocolV1_0Utils.GetParams(orig, NAME, deep, out rest)) {
                if (rest.Length == 2 && Parser.TryParse(rest, out param)) {
                    deserialized = new ZoneV1_0(param[0], param[1]);
                    return true;
                }
            }
            deserialized = null;
            return false;
        }

        public static ZoneV1_0[] Convert(Zone[] zones) {
            ZoneV1_0[] zonesV1_0 = new ZoneV1_0[zones.Length];
            for (int i = 0; i < zones.Length; i++) {
                zonesV1_0[i] = new ZoneV1_0(zones[i]);
            }
            return zonesV1_0;
        }
    }
}
