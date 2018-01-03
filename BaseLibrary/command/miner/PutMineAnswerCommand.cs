using System;
using System.Collections.Generic;

namespace BaseLibrary.command.miner {
    public class PutMineAnswerCommand : AMinerCommand {

        private static readonly List<ISubCommandFactory> SUB_COMMAND_FACTORIES = new List<ISubCommandFactory>();

        public static int RegisterSubCommandFactory(ISubCommandFactory subCommandFactory) {
            int position = SUB_COMMAND_FACTORIES.Count;
            SUB_COMMAND_FACTORIES.Add(subCommandFactory);
            return position;
        }

        /// <summary>
        /// Mine id if SUCCESS is false.
        /// </summary>
        public const int FALSE_MINE_ID = 0;

        private bool _success;
        /// <summary>
        /// <list type="bullet">
        /// <item>
        ///     <description>true - mine was putted.</description>
        /// </item>
        /// <item>
        ///     <description>false - mine was not putted. Robot reach maximum of putted mines.</description>
        /// </item>
        /// </list>
        /// </summary>
        public bool SUCCESS {
            get {
                if (pending)
                    throw new NotSupportedException("Cannot access to property of pending request.");
                return _success;
            } private set =>_success = value; }

        private int _mineID;
        /// <summary>
        /// Mine id for putted mine.
        /// </summary>
        public int MINE_ID {
            get {
                if (pending)
                    throw new NotSupportedException("Cannot access to property of pending request.");
                return _mineID;
            } private set => _mineID = value; }

        /// <summary>
        /// Using for create pending command.
        /// </summary>
        public PutMineAnswerCommand() { }

        /// <summary>
        /// Using for create command fulled with data.
        /// </summary>
        public PutMineAnswerCommand(bool success, int mineId) {
            SUCCESS = success;
            MINE_ID = mineId;
            pending = false;
        }

        /// <summary>
        /// Fill command by data from another command (use full for filling pending command by command with data).
        /// </summary>
        /// <param name="source"></param>
        public void FillData(PutMineAnswerCommand source) {
            SUCCESS = source.SUCCESS;
            MINE_ID = source.MINE_ID;
            pending = false;
        }
    }
}
