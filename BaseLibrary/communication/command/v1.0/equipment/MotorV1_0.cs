using BaseLibrary.communication.protocol;
using BaseLibrary.communication.protocol.protocolV1_0Utils;
using BaseLibrary.equipment;
using BaseLibrary.utils;

namespace BaseLibrary.communication.command.v1._0.equipment {
    public class MotorV1_0 : Motor, InnerSerializerV1_0 {
		private const string NAME = "MOTOR";

		public MotorV1_0(int id, int cost, double maxSpeed, double rotateIn, double acceleration, double deceleration, double maxInitialPower) :
			base(id, cost, maxSpeed, rotateIn, acceleration, deceleration, maxInitialPower) { }

		public MotorV1_0(Motor m) :
			base(m.ID, m.COST, m.MAX_SPEED, m.ROTATE_IN, m.ACCELERATION, m.DECELERATION, m.MAX_INITIAL_POWER) { }

        public string Serialize(Deep deep) {
			return ProtocolV1_0Utils.SerializeParams(NAME, deep, ID, COST, (ProtocolDouble) MAX_SPEED, (ProtocolDouble) ROTATE_IN, (ProtocolDouble) ACCELERATION, (ProtocolDouble) DECELERATION, (ProtocolDouble) MAX_INITIAL_POWER);
        }

		public static bool Deserialize(string orig, Deep deep, out MotorV1_0 deserialized) {
			string[] rest;
			if (ProtocolV1_0Utils.GetParams(orig, NAME, deep, out rest)) {
				if (rest.Length == 7) {
					ProtocolDouble[] paramsDouble;
					int[] paramsInt;
					if (Parser.TryParse(new int[] { 2, 3, 4, 5, 6 }, rest, out paramsDouble) &&
						Parser.TryParse(new int[] { 0, 1 }, rest, out paramsInt)) {
						deserialized = new MotorV1_0(paramsInt[0], paramsInt[1], paramsDouble[0], paramsDouble[1], paramsDouble[2], paramsDouble[3], paramsDouble[4]);
						return true;
					}
				}
			}
			deserialized = null;
			return false;
		}
	}
}
