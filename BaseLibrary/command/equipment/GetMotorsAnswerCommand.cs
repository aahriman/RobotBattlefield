using System;
using System.Collections.Generic;
using BaseLibrary.equip;

namespace BaseLibrary.command.equipment {
    public class GetMotorsAnswerCommand : AEquipmentCommand {

        private static readonly List<ISubCommandFactory> SUB_COMMAND_FACTORIES = new List<ISubCommandFactory>();

        public static int RegisterSubCommandFactory(ISubCommandFactory subCommandFactory) {
            int position = SUB_COMMAND_FACTORIES.Count;
            SUB_COMMAND_FACTORIES.Add(subCommandFactory);
            return position;
        }

        /// <summary>
        /// Available motors to buy.
        /// </summary>
        /// <seealso cref="Motor"/>
        public Motor[] MOTORS { get; private set; }

		public GetMotorsAnswerCommand(Motor[] motors)
			: base() {
				MOTORS = motors;
		}


    }
}
