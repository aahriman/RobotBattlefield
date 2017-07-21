using System.Collections.Generic;
using BaseLibrary.protocol;
using BaseLibrary.visitors;

namespace BaseLibrary.command.tank {
    public class ShotCommand : ATankCommand {

        private static readonly List<ISubCommandFactory> SUB_COMMAND_FACTORIES = new List<ISubCommandFactory>();

        public static int RegisterSubCommandFactory(ISubCommandFactory subCommandFactory) {
            int position = SUB_COMMAND_FACTORIES.Count;
            SUB_COMMAND_FACTORIES.Add(subCommandFactory);
            return position;
        }

        public double ANGLE { get; private set; }
        public double RANGE { get; private set; }

        public ShotCommand(double range, double angle)
            : base() {
                ANGLE = angle;
                RANGE = range;
        }

        public sealed override void accept(ITankVisitor accepter) {
            accepter.visit(this);
        }

        public sealed override Output accept<Output>(ITankVisitor<Output> accepter) {
            return accepter.visit(this);
        }

        public sealed override Output accept<Output, Input>(ITankVisitor<Output, Input> accepter, Input input) {
            return accepter.visit(this, input);
        }
    }
}
