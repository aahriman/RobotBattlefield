using System;
using BaseLibrary.command.common;
using BaseLibrary.protocol;
using BaseLibrary.utils;

namespace BaseLibrary.command.v1._0 {
	internal class MerchantCommandV1_0 : MerchantCommand, ACommand.Sendable{
        public static readonly IFactory<ACommand.Sendable, ACommand> FACTORY = new CommandFactory();
        private sealed class CommandFactory : ACommandFactory {
            public override Boolean IsDeserializable(String s) {
                s = s.Trim();
                string[] rest;
                if (StringUtils.GetRestOfStringSplited(s, "MERCHANT(", ")", out rest, ';')) {
                    if (rest.Length != 4) {
                        return false;
                    }
                    int motorId, gunId, armorId, repairHp;
                    if (int.TryParse(rest[0], out motorId) &&
                       int.TryParse(rest[1], out gunId) &&
                       int.TryParse(rest[2], out armorId) &&
                       int.TryParse(rest[3], out repairHp)) {
                       cache.Cached(s, new MerchantCommandV1_0(motorId, gunId, armorId, repairHp));
                       return true;
                    }
                }
                return false;
            }

            public override bool IsTransferable(ACommand c) {
                if (c is MerchantCommand) {
                    var c2 = (MerchantCommand)c;
                    cache.Cached(c, new MerchantCommandV1_0(c2.MOTOR_ID, c2.CLASS_EQUIPMENT_ID, c2.ARMOR_ID, c2.REPAIR_HP));
                    return true;
                }
                return false;
            }
        }

        public MerchantCommandV1_0(int motorId, int gunId, int armorId, int repairHp) : base(motorId, gunId, armorId, repairHp) { }

        public string Serialize() {
            return String.Format("MERCHANT({0};{1};{2};{3})", MOTOR_ID, CLASS_EQUIPMENT_ID, ARMOR_ID, REPAIR_HP);
        }
    }
}
