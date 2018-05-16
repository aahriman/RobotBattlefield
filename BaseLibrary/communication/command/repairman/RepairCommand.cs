using System;
using System.Collections.Generic;

namespace BaseLibrary.communication.command.repairman {
    public class RepairCommand : ARepairmanCommand {

        private static readonly List<ISubCommandFactory> SUB_COMMAND_FACTORIES = new List<ISubCommandFactory>();

        public static int RegisterSubCommandFactory(ISubCommandFactory subCommandFactory) {
            int position = SUB_COMMAND_FACTORIES.Count;
            SUB_COMMAND_FACTORIES.Add(subCommandFactory);
            return position;
        }

        private double _maxDistance;
        /// <summary>
        /// Distance how far repair.
        /// </summary>
        public double MAX_DISTANCE {
            get {
                if (pending)
                    throw new NotSupportedException("Cannot access to property of pending request.");
                return _maxDistance;
            }
            private set => _maxDistance = value;
        }

        public RepairCommand() : this(10000) {
        }

        public RepairCommand(double maxDistance) {
            this.MAX_DISTANCE = maxDistance;
            pending = false;
        }
    }
}
