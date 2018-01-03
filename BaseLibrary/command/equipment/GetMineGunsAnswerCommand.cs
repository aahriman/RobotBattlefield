using System.Collections.Generic;
using BaseLibrary.equip;

namespace BaseLibrary.command.equipment {
    public class GetMineGunsAnswerCommand : AEquipmentCommand {

        private static readonly List<ISubCommandFactory> SUB_COMMAND_FACTORIES = new List<ISubCommandFactory>();

        public static int RegisterSubCommandFactory(ISubCommandFactory subCommandFactory) {
            int position = SUB_COMMAND_FACTORIES.Count;
            SUB_COMMAND_FACTORIES.Add(subCommandFactory);
            return position;
        }

        /// <summary>
        /// Available mine guns to buy.
        /// </summary>
        /// <seealso cref="MineGun"/>
        public MineGun[] MINE_GUNS { get; private set; }

        public GetMineGunsAnswerCommand(MineGun[] mineGuns)
            : base() {
            MINE_GUNS = mineGuns;
        }
    }
}
