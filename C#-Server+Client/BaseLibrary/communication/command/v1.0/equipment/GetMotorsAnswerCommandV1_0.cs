using BaseLibrary.communication.command.equipment;
using BaseLibrary.communication.protocol;
using BaseLibrary.utils;

namespace BaseLibrary.communication.command.v1._0.equipment{
    public class GetMotorsAnswerCommandV1_0 : GetMotorsAnswerCommand, ACommand.Sendable {
		private const string NAME = "MOTORS_ANSWER";
        public static readonly IFactory<ACommand.Sendable, ACommand> FACTORY = new CommandFactory();
        private sealed class CommandFactory : ACommandFactory {
            public override bool IsDeserializeable(string s) {
                s = s.Trim();
				string[] rest;
                if (ProtocolV1_0Utils.GetParams(s, NAME, ProtocolV1_0Utils.DEFAULT, out rest)) {
					if (rest.Length == 1 && ProtocolV1_0Utils.Deserialize(rest[0], out rest, ProtocolV1_0Utils.DEFAULT.NEXT)) { 
						MotorV1_0[] motors = new MotorV1_0[rest.Length];
						for (int i = 0; i < motors.Length; i++) {
							if (!MotorV1_0.Deserialize(rest[i], ProtocolV1_0Utils.DEFAULT.NEXT.NEXT, out motors[i])) {
								return false;
							}
						}
						cache.Cached(s, new GetMotorsAnswerCommandV1_0(motors));
						return true;
					}
                }
                return false;
            }

            public override bool IsTransferable(ACommand c) {
                if (c is GetMotorsAnswerCommand) {
                    var c2 = (GetMotorsAnswerCommand)c;
					MotorV1_0[] motors = new MotorV1_0[c2.MOTORS.Length];
					for (int i = 0; i < motors.Length; i++) {
						motors[i] = new MotorV1_0(c2.MOTORS[i]);
					}
					cache.Cached(c, new GetMotorsAnswerCommandV1_0(motors));
                    return true;
                }
                return false;
            }
        }
        public GetMotorsAnswerCommandV1_0(MotorV1_0[] motors) : base(motors) { }

        public string Serialize() {
			return ProtocolV1_0Utils.SerializeParams(NAME, ProtocolV1_0Utils.DEFAULT, new object[]{MOTORS});
        }
	}

}