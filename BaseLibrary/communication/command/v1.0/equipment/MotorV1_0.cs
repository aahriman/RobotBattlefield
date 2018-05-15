using BaseLibrary.communication.protocol;
using BaseLibrary.communication.protocol.protocolV1_0Utils;
using BaseLibrary.equipment;
using BaseLibrary.utils;

namespace BaseLibrary.communication.command.v1._0.equipment {
    public class MotorV1_0 : Motor, InnerSerializerV1_0 {
		private const string NAME = "MOTOR";

		public MotorV1_0(double maxSpeed, double rotateIn, double acceleration, double deceleration, double maxInitialPower, int cost, int id) :
			base(maxSpeed, rotateIn, acceleration, deceleration, maxInitialPower, cost, id) { }

		public MotorV1_0(Motor m) :
			base(m.MAX_SPEED, m.ROTATE_IN, m.ACCELERATION, m.DECELERATION, m.MAX_INITIAL_POWER, m.COST, m.ID) { }

        public string Serialize(Deep deep) {
			return ProtocolV1_0Utils.SerializeParams(NAME, deep, (ProtocolDouble) MAX_SPEED, (ProtocolDouble) ROTATE_IN, (ProtocolDouble) ACCELERATION, (ProtocolDouble) DECELERATION, (ProtocolDouble) MAX_INITIAL_POWER, COST, ID);
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
