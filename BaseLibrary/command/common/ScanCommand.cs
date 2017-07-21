using System;
using System.Collections.Generic;
using BaseLibrary.protocol;
using BaseLibrary.visitors;

namespace BaseLibrary.command.common {
    public class ScanCommand : ACommonCommand {
        private static readonly double MAX_SCAN_PRECISION = 10.0;

        private static readonly List<ISubCommandFactory> SUB_COMMAND_FACTORIES = new List<ISubCommandFactory>();

        public static int RegisterSubCommandFactory(ISubCommandFactory subCommandFactory) {
            int position = SUB_COMMAND_FACTORIES.Count;
            SUB_COMMAND_FACTORIES.Add(subCommandFactory);
            return position;
        }

        public double PRECISION { get; private set; }
        public double ANGLE {get; private set;}

        public ScanCommand(double precision, double angle) : base() {
            ANGLE = angle;
            precision = Math.Max(0, precision);
            precision = Math.Min(MAX_SCAN_PRECISION, precision);
            PRECISION = precision;
        }

        public sealed override void accept(ICommandVisitor accepter) {
            accepter.visit(this);
        }

        public sealed override Output accept<Output>(ICommandVisitor<Output> accepter) {
            return accepter.visit(this);
        }

        public sealed override Output accept<Output, Input>(ICommandVisitor<Output, Input> accepter, Input input) {
            return accepter.visit(this, input);
        }
    }
}
