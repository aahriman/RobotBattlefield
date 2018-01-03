using System;
using System.Collections.Generic;

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

        private string _fileUrl;
        /// <summary>
        /// Where can be downloaded file with information about battle.
        /// </summary>
        public string FILE_URL {
            get {
                if (pending)
                    throw new NotSupportedException("Cannot access to property of pending request.");
                return _fileUrl;
            }
            private set => _fileUrl = value;
        }

        public EndMatchCommand(string fileUrl) {
            FILE_URL = fileUrl;
            pending = false;
        }
    }
}
