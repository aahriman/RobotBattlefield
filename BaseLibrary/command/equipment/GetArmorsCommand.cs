using System.Collections.Generic;

namespace BaseLibrary.command.equipment {
    public class GetArmorsCommand : AEquipmentCommand {

        private static readonly List<ISubCommandFactory> SUB_COMMAND_FACTORIES = new List<ISubCommandFactory>();

        public static int RegisterSubCommandFactory(ISubCommandFactory subCommandFactory) {
            int position = SUB_COMMAND_FACTORIES.Count;
            SUB_COMMAND_FACTORIES.Add(subCommandFactory);
            return position;
        }

        public GetArmorsCommand() : base() { }
    }
}
