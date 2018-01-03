using System;
using System.Collections.Generic;
using BaseLibrary.protocol;

namespace BaseLibrary.command.common {
    public class ScanCommand : ACommonCommand {
        /// <summary>
        /// Max precision of scanner.
        /// </summary>
        /// <seealso cref="PRECISION"/>
        public const double MAX_SCAN_PRECISION = 10.0;

        private static readonly List<ISubCommandFactory> SUB_COMMAND_FACTORIES = new List<ISubCommandFactory>();

        public static int RegisterSubCommandFactory(ISubCommandFactory subCommandFactory) {
            int position = SUB_COMMAND_FACTORIES.Count;
            SUB_COMMAND_FACTORIES.Add(subCommandFactory);
            return position;
        }

        private double _precision;
        /// <summary>
        /// How wide is scan cone.
        /// </summary>
        public double PRECISION {
            get {
                if (pending)
                    throw new NotSupportedException("Cannot access to property of pending request.");
                return _precision;
            }
            private set => _precision = value;
        }


        private double _angle;
        /// <summary>
        /// Direction to scan (0 to right, 90 down, 180 left, 270 up).
        /// </summary>
        public double ANGLE {
            get {
                if (pending)
                    throw new NotSupportedException("Cannot access to property of pending request.");
                return _angle;
            }
            private set => _angle = value;
        }

        public ScanCommand(double precision, double angle) : base() {
            ANGLE = angle;
            precision = Math.Max(0, precision);
            precision = Math.Min(MAX_SCAN_PRECISION, precision);
            PRECISION = precision;
            pending = false;
        }
    }
}
