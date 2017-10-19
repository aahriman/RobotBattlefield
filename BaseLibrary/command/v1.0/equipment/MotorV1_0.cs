using System;
using BaseLibrary.equip;
using BaseLibrary.protocol;
using BaseLibrary.utils;
using BaseLibrary.utils.protocolV1_0Utils;

namespace BaseLibrary.command.v1._0.equipment {
    public class MotorV1_0 : Motor, InnerSerializerV1_0 {
		private const string NAME = "MOTOR";

		public MotorV1_0(double maxSpeed, double rotateIn, double speedUp, double speedDown, double speedUpTo, int cost, int id) :
			base(maxSpeed, rotateIn, speedUp, speedDown, speedUpTo, cost, id) { }

		public MotorV1_0(Motor m) :
			base(m.MAX_SPEED, m.ROTATE_IN, m.SPEED_UP, m.SPEED_DOWN, m.SPEED_UP_TO, m.COST, m.ID) { }

        public string Serialize(Deep deep) {
			return ProtocolV1_0Utils.SerializeParams(NAME, deep, (ProtocolDouble) MAX_SPEED, (ProtocolDouble) ROTATE_IN, (ProtocolDouble) SPEED_UP, (ProtocolDouble) SPEED_DOWN, (ProtocolDouble) SPEED_UP_TO, COST, ID);
        }

		public static bool Deserialize(string orig, Deep deep, out MotorV1_0 deserialized) {
			string[] rest;
			if (ProtocolV1_0Utils.GetParams(orig, NAME, deep, out rest)) {
				if (rest.Length == 7) {
					ProtocolDouble[] paramsDouble;
					int[] paramsInt;
					if (Parser.TryParse(new int[] { 0, 1, 2, 3, 4 }, rest, out paramsDouble) &&
						Parser.TryParse(new int[] { 5, 6 }, rest, out paramsInt)) {
						deserialized = new MotorV1_0(paramsDouble[0], paramsDouble[1], paramsDouble[2], paramsDouble[3], paramsDouble[4], paramsInt[0], paramsInt[1]);
						return true;
					}
				}
			}
			deserialized = null;
			return false;
		}
	}
}
