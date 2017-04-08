using System;
using BaseLibrary.utils;

namespace BaseLibrary.command.v1._0{
    public class GetMotorsAnwerCommandV1_0 : GetMotorsAnwerCommand, ACommand.Sendable {
		private const string NAME = "MOTOR_ANSER";
        public static readonly ICommandFactory FACTORY = new CommandFactory();
        private sealed class CommandFactory : ACommandFactory {
            public override Boolean IsDeserializable(String s) {
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
						cache.Cached(s, new GetMotorsAnwerCommandV1_0(motors));
						return true;
					}
                }
                return false;
            }

            public override bool IsTransferable(ACommand c) {
                if (c is GetMotorsAnwerCommand) {
                    var c2 = (GetMotorsAnwerCommand)c;
					MotorV1_0[] motors = new MotorV1_0[c2.MOTORS.Length];
					for (int i = 0; i < motors.Length; i++) {
						motors[i] = new MotorV1_0(c2.MOTORS[i]);
					}
					cache.Cached(c, new GetMotorsAnwerCommandV1_0(motors));
                    return true;
                }
                return false;
            }
        }
        public GetMotorsAnwerCommandV1_0(MotorV1_0[] motors) : base(motors) { }

        public string serialize() {
			return ProtocolV1_0Utils.SerializeParams(NAME, ProtocolV1_0Utils.DEFAULT, new object[]{MOTORS});
        }
	}

}