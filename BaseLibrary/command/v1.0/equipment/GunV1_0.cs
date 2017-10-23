using System;
using BaseLibrary.equip;
using BaseLibrary.utils;
using BaseLibrary.utils.protocolV1_0Utils;

namespace BaseLibrary.command.v1._0.equipment {
	public class GunV1_0 : Gun, InnerSerializerV1_0 {
		private const string NAME = "GUN";

		public GunV1_0(int id, int cost, int maxBullets, int maxRange, double shotSpeed, ZoneV1_0[] zones) :
			base(id, cost, maxBullets, maxRange, shotSpeed, zones) { }
		
		public GunV1_0(Gun GUN) :
			base(GUN.ID, GUN.COST, GUN.MAX_BULLETS, GUN.MAX_RANGE, GUN.SHOT_SPEED, ZoneV1_0.Convert(GUN.ZONES)) { }

		public string Serialize(Deep deep) {
			return ProtocolV1_0Utils.SerializeParams(NAME, deep, ID, COST, MAX_BULLETS, MAX_RANGE, SHOT_SPEED, ZONES);
		}

		public static bool Deserialize(string orig, Deep deep, out GunV1_0 deserialized) {
			string[] rest;
			if (ProtocolV1_0Utils.GetParams(orig, NAME, deep, out rest)) {
				if (rest.Length == 6) {
					int[] paramsInt;
					string[] zonesString;
					if (Parser.TryParse(new int[] { 0, 1, 2, 3, 4 }, rest, out paramsInt) &&
						ProtocolV1_0Utils.Deserialize(rest[5], out zonesString, deep.NEXT)) {
						ZoneV1_0[] zones = new ZoneV1_0[zonesString.Length];
						for (int i = 0; i < zones.Length; i++) {
							if (!ZoneV1_0.Deserialize(zonesString[i], deep.NEXT.NEXT, out zones[i])) {
								deserialized = null;
								return false;
							}
						}
						deserialized = new GunV1_0(paramsInt[0], paramsInt[1], paramsInt[2], paramsInt[3], paramsInt[4], zones);
						return true;
					}
				}
			}
			deserialized = null;
			return false;
		}
	}
}
