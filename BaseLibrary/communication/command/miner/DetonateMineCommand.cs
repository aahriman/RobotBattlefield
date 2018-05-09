using System;
using System.Collections.Generic;

namespace BaseLibrary.communication.command.miner {
    public class DetonateMineCommand : AMinerCommand {

        private static readonly List<ISubCommandFactory> SUB_COMMAND_FACTORIES = new List<ISubCommandFactory>();

        public static int RegisterSubCommandFactory(ISubCommandFactory subCommandFactory) {
            int position = SUB_COMMAND_FACTORIES.Count;
            SUB_COMMAND_FACTORIES.Add(subCommandFactory);
            return position;
        }

        private int _mineID;
        /// <summary>
        /// ID of mine which want to detonate.
        /// </summary>
        public int MINE_ID {
            get {
                if (pending)
                    throw new NotSupportedException("Cannot access to property of pending request.");
                return _mineID;
            } private set => _mineID = value; }

        public DetonateMineCommand(int mineId) {
            MINE_ID = mineId;
        }
    }
}
