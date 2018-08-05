using System;
using System.Collections.Generic;
using BaseLibrary.equipment;

namespace BaseLibrary.communication.command.equipment {
    public class GetArmorsAnswerCommand : AEquipmentCommand {

        private static readonly List<ISubCommandFactory> SUB_COMMAND_FACTORIES = new List<ISubCommandFactory>();

        public static int RegisterSubCommandFactory(ISubCommandFactory subCommandFactory) {
            int position = SUB_COMMAND_FACTORIES.Count;
            SUB_COMMAND_FACTORIES.Add(subCommandFactory);
            return position;
        }

        private Armor[] _armors;

        /// <summary>
        /// Available armors to buy.
        /// </summary>
        public Armor[] ARMORS {
            get {
                if (pending)
                    throw new NotSupportedException("Cannot access to property of pending request.");
                return _armors;
            }
            private set => _armors = value;
        }


        public GetArmorsAnswerCommand(Armor[] armors)
            : base() {
            pending = false;
            ARMORS = new Armor[armors.Length];
            for (int i = 0; i < armors.Length; i++) {
                ARMORS[i] = armors[i];
            }
        }
    }
}
