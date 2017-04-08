using System.Collections.Generic;
using BaseLibrary.visitors;

namespace BaseLibrary.command {
    public class DriveCommand : ACommand {

        private static readonly List<ISubCommandFactory> SUB_COMMAND_FACTORIES = new List<ISubCommandFactory>();

        public static int RegisterSubCommandFactory(ISubCommandFactory subCommandFactory) {
            int position = SUB_COMMAND_FACTORIES.Count;
            SUB_COMMAND_FACTORIES.Add(subCommandFactory);
            return position;
        }

        public static DriveCommand GetInstance(ProtocolDouble speed, ProtocolDouble angle) {
            return new DriveCommand(speed, angle);
        }
        public ProtocolDouble POWER { get; private set; }
        public ProtocolDouble ANGLE { get; private set; }

        public DriveCommand(ProtocolDouble speed, ProtocolDouble angle) {
            POWER = speed;
            ANGLE = angle;
        }

        public sealed override void accept(ICommandVisitor accepter) {
            accepter.visit(this);
        }

        public sealed override Output accept<Output>(ICommandVisitor<Output> accepter) {
            return accepter.visit(this);
        }

        public sealed override Output accept<Output, Input>(ICommandVisitor<Output, Input> accepter, params Input[] inputs) {
            return accepter.visit(this, inputs);
        }
    }
}
