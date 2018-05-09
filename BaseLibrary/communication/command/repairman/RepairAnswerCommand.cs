using System;
using System.Collections.Generic;

namespace BaseLibrary.communication.command.repairman {
    public class RepairAnswerCommand : ARepairmanCommand {

        private static readonly List<ISubCommandFactory> SUB_COMMAND_FACTORIES = new List<ISubCommandFactory>();

        public static int RegisterSubCommandFactory(ISubCommandFactory subCommandFactory) {
            int position = SUB_COMMAND_FACTORIES.Count;
            SUB_COMMAND_FACTORIES.Add(subCommandFactory);
            return position;
        }

        private bool _success;
        /// <summary>
        /// <list type="bullet">
        ///   <item>
        ///      <description>true - robot use repair tool to repair robots in range.</description>
        ///   </item>
        ///   <item>
        ///      <description>false - robot do not use repair tool to repair. Robot use repair tool too many times.</description>
        ///   </item>
        /// </list>
        /// </summary>
        public bool SUCCESS {
            get {
                if (pending)
                    throw new NotSupportedException("Cannot access to property of pending request.");
                return _success;
            } private set => _success = value; }

        /// <summary>
        /// Using for create pending command.
        /// </summary>
        public RepairAnswerCommand() { }

        /// <summary>
        /// Using for create command fulled with data.
        /// </summary>
        public RepairAnswerCommand(bool success) {
            SUCCESS = success;
            pending = false;
        }

        /// <summary>
        /// Fill command by data from another command (use full for filling pending command by command with data).
        /// </summary>
        /// <param name="source"></param>
        public void FillData(RepairAnswerCommand source) {
            SUCCESS = source.SUCCESS;
            pending = false;
        }
    }
}
