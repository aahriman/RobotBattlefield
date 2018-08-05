using System.Collections.Generic;

namespace BaseLibrary.communication.command.tank {
    public class ShootAnswerCommand : ATankCommand {

        private static readonly List<ISubCommandFactory> SUB_COMMAND_FACTORIES = new List<ISubCommandFactory>();

        public static int RegisterSubCommandFactory(ISubCommandFactory subCommandFactory) {
            int position = SUB_COMMAND_FACTORIES.Count;
            SUB_COMMAND_FACTORIES.Add(subCommandFactory);
            return position;
        }

        /// <summary>
        /// <list type="bullet">
        /// <item>
        ///     <description>True if shoot release bullet.</description>
        /// </item>
        /// <item>
        ///     <description>False if shoot do not release bullet. No gun was loaded.</description>
        /// </item>
        /// </list>
        /// 
        /// </summary>
        public bool SUCCESS { get; private set; }

        /// <summary>
        /// Using for create pending command.
        /// </summary>
        public ShootAnswerCommand() { }

        /// <summary>
        /// Using for create command fulled with data.
        /// </summary>
        public ShootAnswerCommand(bool success)
            : base() {
            SUCCESS = success;
            pending = false;
        }

        /// <summary>
        /// Fill command by data from another command (use full for filling pending command by command with data).
        /// </summary>
        /// <param name="source"></param>
        public void FillData(ShootAnswerCommand source) {
            SUCCESS = source.SUCCESS;
            pending = false;
        }
    }
}
