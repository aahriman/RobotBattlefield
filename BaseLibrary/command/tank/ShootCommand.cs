using System;
using System.Collections.Generic;
using BaseLibrary.protocol;

namespace BaseLibrary.command.tank {
    public class ShootCommand : ATankCommand {

        private static readonly List<ISubCommandFactory> SUB_COMMAND_FACTORIES = new List<ISubCommandFactory>();

        public static int RegisterSubCommandFactory(ISubCommandFactory subCommandFactory) {
            int position = SUB_COMMAND_FACTORIES.Count;
            SUB_COMMAND_FACTORIES.Add(subCommandFactory);
            return position;
        }

        private double _angle;

        /// <summary>
        /// In witch direction want to shot.
        /// </summary>
        public double ANGLE {
            get {
                if (pending)
                    throw new NotSupportedException("Cannot access to property of pending request.");
                return _angle;
            }
            private set => _angle = value;
        }

        private double _range;
        /// <summary>
        /// How far from want to shot.
        /// </summary>
        public double RANGE {
            get {
                if (pending)
                    throw new NotSupportedException("Cannot access to property of pending request.");
                return _range;
            }
            private set => _range = value;
        }

        public ShootCommand(double range, double angle)
            : base() {
                ANGLE = angle;
                RANGE = range;
            pending = false;
        }
    }
}
