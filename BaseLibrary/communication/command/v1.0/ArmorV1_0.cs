using System;
using BaseLibrary.equip;
using BaseLibrary.utils;
using BaseLibrary.utils.protocolV1_0Utils;

namespace BaseLibrary.command.v1._0 {
	public class ArmorV1_0 : Armor, InnerSerializerV1_0 {
        private const string NAME = "MOTOR";

		public ArmorV1_0(int maxHp, int cost, int id) :
			base(maxHp, cost, id) { }

		public ArmorV1_0(Armor a) :
			base(a.MAX_HP, a.COST, a.ID) { }

        public string Serialize(Deep deep) {
			return ProtocolV1_0Utils.SerializeParams(NAME, deep, MAX_HP, COST, ID);
        }

		public static bool Deserialize(String orig, Deep deep, out ArmorV1_0 deserialized) {
			string[] rest;
			if (ProtocolV1_0Utils.GetParams(orig, NAME, deep, out rest)) {
				if (rest.Length == 3) {
					int[] paramsInt;
					if (Parser.TryParse(rest, out paramsInt)) {
						deserialized = new ArmorV1_0(paramsInt[0], paramsInt[1], paramsInt[2]);
						return true;
					}
				}
			}
			deserialized = null;
			return false;
		}
    }
}
