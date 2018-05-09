using System;
using System.Collections.Generic;
using BaseLibrary.communication.protocol;

namespace BaseLibrary.communication.command.common {
    public class DriveCommand : ACommonCommand {

        private static readonly List<ISubCommandFactory> SUB_COMMAND_FACTORIES = new List<ISubCommandFactory>();

        public static int RegisterSubCommandFactory(ISubCommandFactory subCommandFactory) {
            int position = SUB_COMMAND_FACTORIES.Count;
            SUB_COMMAND_FACTORIES.Add(subCommandFactory);
            return position;
        }

        public static DriveCommand GetInstance(ProtocolDouble speed, ProtocolDouble angle) {
            return new DriveCommand(speed, angle);
        }

        private double _power;
        /// <summary>
        /// How fast robot wants to go.
        /// </summary>
        public double POWER {
            get {
                if (pending)
                    throw new NotSupportedException("Cannot access to property of pending request.");
                return _power;
            }
            private set => _power = value;
        }


        private double _angle;
        /// <summary>
        /// In witch direction robot wants to go.
        /// </summary>
        public double ANGLE {
            get {
                if (pending)
                    throw new NotSupportedException("Cannot access to property of pending request.");
                return _angle;
            }
            private set => _angle = value;
        }

        public DriveCommand(double power, double angle) {
            power = Math.Max(0, power);
            power = Math.Min(100.0, power);
            POWER = power;
            ANGLE = angle;
            pending = false;
        }
    }
}
