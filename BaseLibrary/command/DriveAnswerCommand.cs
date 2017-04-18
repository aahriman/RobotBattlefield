using System.Collections.Generic;
using BaseLibrary.visitors;

namespace BaseLibrary.command {
    public class DriveAnswerCommand : ACommand {
        private static readonly List<ISubCommandFactory> SUB_COMMAND_FACTORIES = new List<ISubCommandFactory>();

        public static int RegisterSubCommandFactory(ISubCommandFactory subCommandFactory) {
            int position = SUB_COMMAND_FACTORIES.Count;
            SUB_COMMAND_FACTORIES.Add(subCommandFactory);
            return position;
        }


        public static DriveAnswerCommand GetInstance(bool succes) {
            return new DriveAnswerCommand(succes);
        }

        public bool SUCCES { get; private set; }

        public DriveAnswerCommand(bool succes) {
            SUCCES = succes;
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
