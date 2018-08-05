using System;
using System.Collections.Generic;

namespace BaseLibrary.communication.command.common {
    /// <summary>
    /// Answer for drive command.
    /// </summary>
    public class DriveAnswerCommand : ACommonCommand {
        private static readonly List<ISubCommandFactory> SUB_COMMAND_FACTORIES = new List<ISubCommandFactory>();

        public static int RegisterSubCommandFactory(ISubCommandFactory subCommandFactory) {
            int position = SUB_COMMAND_FACTORIES.Count;
            SUB_COMMAND_FACTORIES.Add(subCommandFactory);
            return position;
        }

        private bool _success;

        /// <summary>
        /// True if robot change direction.
        /// </summary>
        public bool SUCCESS {
            get {
                if (pending)
                    throw new NotSupportedException("Cannot access to property of pending request.");
                return _success;
            }
            private set => _success = value;
        }

        public DriveAnswerCommand() {}

        public DriveAnswerCommand(bool success) {
            SUCCESS = success;
            pending = false;
        }

        public void FillData(DriveAnswerCommand source) {
            SUCCESS = source.SUCCESS;
            pending = false;
        }
    }
}
