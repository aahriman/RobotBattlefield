using BaseLibrary.equipment;
using BaseLibrary.utils;
using BaseLibrary.utils.protocolV1_0Utils;

namespace BaseLibrary.communication.command.v1._0.equipment {
	public class ArmorV1_0 : Armor, InnerSerializerV1_0 {
        private const string NAME = "ARMOR";

		public ArmorV1_0(int id, int cost, int maxHp) :
			base(id, cost, maxHp) { }

		public ArmorV1_0(Armor a) :
			base(a.ID, a.COST, a.MAX_HP) { }

        public string Serialize(Deep deep) {
			return ProtocolV1_0Utils.SerializeParams(NAME, deep, ID, COST, MAX_HP);
        }

		public static bool Deserialize(string orig, Deep deep, out ArmorV1_0 deserialized) {
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
