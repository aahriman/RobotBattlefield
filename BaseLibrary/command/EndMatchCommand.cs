using System.Collections.Generic;
using BaseLibrary.visitors;

namespace BaseLibrary.command {
    public class EndMatchCommand : ACommand {

        private static readonly List<ISubCommandFactory> SUB_COMMAND_FACTORIES = new List<ISubCommandFactory>();

        public static int RegisterSubCommandFactory(ISubCommandFactory subCommandFactory) {
            int position = SUB_COMMAND_FACTORIES.Count;
            SUB_COMMAND_FACTORIES.Add(subCommandFactory);
            return position;
        }

        public string FILE_URL { get; private set; }

        public EndMatchCommand(string fileUrl) {
            FILE_URL = fileUrl;
        }

        public override void accept(ICommandVisitor accepter) {
            accepter.visit(this);
        }

        public override Output accept<Output>(ICommandVisitor<Output> accepter) {
            return accepter.visit(this);
        }

        public override Output accept<Output, Input>(ICommandVisitor<Output, Input> accepter, params Input[] inputs) {
            return accepter.visit(this, inputs);
        }
    }
}
