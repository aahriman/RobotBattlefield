using BaseLibrary.command;
using BaseLibrary.equip;

namespace ClientLibrary.robot {
    public class Repairman : ClientRobot {
        public RepairTool REPAIR_TOOL { get; private set; }

        public Repairman() : base() { }
        public Repairman(bool processStateAfterEveryCommand, bool processMerchant) : base(processStateAfterEveryCommand, processMerchant) { }

        public override RobotType GetRobotType() {
            return RobotType.REPAIRMAN;
        }

        protected override void setClassEquip(int id) {
            REPAIR_TOOL = REPAIR_TOOLS_BY_ID[id];
        }

        protected override ClassEquipment getClassEquip() {
            return REPAIR_TOOL;
        }
    }
}
