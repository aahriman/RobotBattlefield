using System;
using CommunicationLibrary.equip;
using CommunicationLibrary.utils;
using CommunicationLibrary.utils.protocolV1_0Utils;

namespace CommunicationLibrary.command.v1._0 {
	public class GunV1_0 : Gun, InnerSerializerV1_0 {
		public class ZoneV1_0 : Zone, InnerSerializerV1_0 {
			private const string NAME = "ZONE";

			public ZoneV1_0(int distance, int damage) : base(distance, damage) { }

			public ZoneV1_0(Zone zone) : base(zone.DISTANCE, zone.DAMAGE) { }

			public string Serialize(Deep deep) {
				return ProtocolV1_0Utils.SerializeParams(NAME, deep, DISTANCE, DAMAGE);
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
		}
		private const string NAME = "GUN";

		public GunV1_0(int id, int cost, int maxBullets, int maxRange, double shotSpeed, ZoneV1_0[] zones) :
			base(id, cost, maxBullets, maxRange, shotSpeed, zones) { }
		
		public GunV1_0(Gun GUN) :
			base(GUN.ID, GUN.COST, GUN.MAX_BULLETS, GUN.MAX_RANGE, GUN.SHOT_SPEED, getZones(GUN.ZONES)) { }

		public string Serialize(Deep deep) {
			return ProtocolV1_0Utils.SerializeParams(NAME, deep, ID, COST, MAX_BULLETS, MAX_RANGE, SHOT_SPEED, ZONES);
		}

		public static bool Deserialize(String orig, Deep deep, out GunV1_0 deserialized) {
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

		private static ZoneV1_0[] getZones(Zone[] zones) {
			ZoneV1_0[] zonesV1_0 = new ZoneV1_0[zones.Length];
			for(int i = 0; i < zones.Length; i++){
				zonesV1_0[i] = new ZoneV1_0(zones[i]);
			}
			return zonesV1_0;
		}
	}
}
