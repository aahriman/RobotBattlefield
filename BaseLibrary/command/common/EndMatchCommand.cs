using System.Collections.Generic;
using BaseLibrary.visitors;

namespace BaseLibrary.command.common {
    /// <summary>
    /// Command at the end of match.
    /// </summary>
    public class EndMatchCommand : ACommonCommand {

        private static readonly List<ISubCommandFactory> SUB_COMMAND_FACTORIES = new List<ISubCommandFactory>();

        public static int RegisterSubCommandFactory(ISubCommandFactory subCommandFactory) {
            int position = SUB_COMMAND_FACTORIES.Count;
            SUB_COMMAND_FACTORIES.Add(subCommandFactory);
            return position;
        }

        /// <summary>
        /// Where can be downloaded file with information about battle.
        /// </summary>
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

        public override Output accept<Output, Input>(ICommandVisitor<Output, Input> accepter, Input input) {
            return accepter.visit(this, input);
        }
    }
}
