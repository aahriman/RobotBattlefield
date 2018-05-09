using System.Collections.Generic;
using BaseLibrary.equipment;

namespace BaseLibrary.communication.command.equipment {
    public class GetGunsAnswerCommand : AEquipmentCommand {

        private static readonly List<ISubCommandFactory> SUB_COMMAND_FACTORIES = new List<ISubCommandFactory>();

        public static int RegisterSubCommandFactory(ISubCommandFactory subCommandFactory) {
            int position = SUB_COMMAND_FACTORIES.Count;
            SUB_COMMAND_FACTORIES.Add(subCommandFactory);
            return position;
        }

        /// <summary>
        /// Available guns to buy.
        /// </summary>
        /// <seealso cref="Gun"/>
        public Gun[] GUNS { get; private set; }

        public GetGunsAnswerCommand(Gun[] guns)
            : base() {
				GUNS = guns;
        }
    }
}
