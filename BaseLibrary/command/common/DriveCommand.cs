using System;
using System.Collections.Generic;
using BaseLibrary.protocol;
using BaseLibrary.visitors;

namespace BaseLibrary.command.common {
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
        public double POWER { get; private set; }
        public double ANGLE { get; private set; }

        public DriveCommand(double power, double angle) {
            power = Math.Max(0, power);
            power = Math.Min(100.0, power);
            POWER = power;
            ANGLE = angle;
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
