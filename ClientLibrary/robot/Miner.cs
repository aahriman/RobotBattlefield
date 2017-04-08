using BaseLibrary.command;
using BaseLibrary.equip;

namespace ClientLibrary.robot {
    class Miner : ClientRobot {
        public MineGun MINE_GUN { get; private set; }

        public Miner() : base() {}
        public Miner(bool processStateAfterEveryCommand, bool processMerchant) : base(processStateAfterEveryCommand, processMerchant) { }

        public override RobotType GetRobotType() {
            return RobotType.MINER;
        }

        protected override void setClassEquip(int id) {
            MINE_GUN = MINE_GUNS_BY_ID[id];
        }

        protected override ClassEquipment getClassEquip() {
            return MINE_GUN;
        }
    }
}
