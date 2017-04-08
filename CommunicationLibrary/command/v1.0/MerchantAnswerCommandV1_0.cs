using System;
using CommunicationLibrary.utils;

namespace CommunicationLibrary.command.v1._0 {
	internal class MerchantAnswerCommandV1_0 : MerchantAnswerCommand, ACommand.Sendable {
		public static readonly ICommandFactory FACTORY = new CommandFactory();
        private sealed class CommandFactory : ACommandFactory {
            public override Boolean IsDeserializable(String s) {
                s = s.Trim();
                string[] rest;
                if (StringUtils.GetRestOfStringSplited(s, "MERCHANT_ASNWER(", ")", out rest, ';')) {
                    if (rest.Length != 3) {
                        return false;
                    }
                    int motorId, gunId, armorId;
                    if (int.TryParse(rest[0], out motorId) &&
                       int.TryParse(rest[1], out gunId) &&
                       int.TryParse(rest[2], out armorId) ) {
                           cache.Cached(s, new MerchantAnswerCommandV1_0(motorId, gunId, armorId));
                        return true;
                    }
                }
                return false;
            }

            public override bool IsTransferable(ACommand c) {
                if (c is MerchantAnswerCommand) {
                    var c2 = (MerchantAnswerCommand)c;
                    cache.Cached(c, new MerchantAnswerCommandV1_0(c2.MOTOR_ID_BOUGHT, c2.GUN_ID_BOUGHT, c2.ARMOR_ID_BOUGHT));
                    return true;
                }
                return false;
            }
        }


        public MerchantAnswerCommandV1_0(int motorIdBought, int gunIdBought, int armorIdBought) : base(motorIdBought, gunIdBought, armorIdBought) { }

        public string serialize() {
            return String.Format("MERCHANT_ASNWER({0};{1};{2})", MOTOR_ID_BOUGHT, GUN_ID_BOUGHT, ARMOR_ID_BOUGHT);
        }
    }
}
