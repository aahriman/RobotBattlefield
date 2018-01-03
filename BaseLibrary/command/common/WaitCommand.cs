using System.Collections.Generic;

namespace BaseLibrary.command.common {

    /// <summary>
    /// Wait one turn or to until HitPoint > 0 or to the end of turn
    /// </summary>
    public class WaitCommand : ACommonCommand {

        private static readonly List<ISubCommandFactory> SUB_COMMAND_FACTORIES = new List<ISubCommandFactory>();

        public static int RegisterSubCommandFactory(ISubCommandFactory subCommandFactory) {
            int position = SUB_COMMAND_FACTORIES.Count;
            SUB_COMMAND_FACTORIES.Add(subCommandFactory);
            return position;
        }

        public WaitCommand() : base() {
            pending = false;
        }
    }
}
