using System;
using BaseLibrary.command.common;
using BaseLibrary.protocol;
using BaseLibrary.utils;

namespace BaseLibrary.command.v1._0 {
	internal class MerchantAnswerCommandV1_0 : MerchantAnswerCommand, ACommand.Sendable {
        public static readonly IFactory<ACommand.Sendable, ACommand> FACTORY = new CommandFactory();
        private sealed class CommandFactory : ACommandFactory {
            public override Boolean IsDeserializable(String s) {
                s = s.Trim();
                string[] rest;
                if (StringUtils.GetRestOfStringSplited(s, "MERCHANT_ASNWER(", ")", out rest, ';')) {
                    if (rest.Length != 3) {
                        return false;
                    }
                    int motorId, classEquipmentId, armorId;
                    if (int.TryParse(rest[0], out motorId) &&
                       int.TryParse(rest[1], out classEquipmentId) &&
                       int.TryParse(rest[2], out armorId) ) {
                           cache.Cached(s, new MerchantAnswerCommandV1_0(motorId, armorId, classEquipmentId));
                        return true;
                    }
                }
                return false;
            }

            public override bool IsTransferable(ACommand c) {
                if (c is MerchantAnswerCommand) {
                    var c2 = (MerchantAnswerCommand)c;
                    cache.Cached(c, new MerchantAnswerCommandV1_0(c2.MOTOR_ID_BOUGHT, c2.ARMOR_ID_BOUGHT, c2.CLASS_EQUIPMENT_ID_BOUGHT));
                    return true;
                }
                return false;
            }
        }


        public MerchantAnswerCommandV1_0(int motorIdBought, int armorIdBought, int classEquipmentIdBought) : base(motorIdBought, armorIdBought, classEquipmentIdBought) { }

        public string Serialize() {
            return String.Format("MERCHANT_ASNWER({0};{1};{2})", MOTOR_ID_BOUGHT, ARMOR_ID_BOUGHT, CLASS_EQUIPMENT_ID_BOUGHT);
        }
    }
}
