using System;
using System.Collections.Generic;

namespace BaseLibrary.communication.command.common {
    public class ScanAnswerCommand : ACommonCommand{

        private static readonly List<ISubCommandFactory> SUB_COMMAND_FACTORIES = new List<ISubCommandFactory>();

        public static int RegisterSubCommandFactory(ISubCommandFactory subCommandFactory) {
            int position = SUB_COMMAND_FACTORIES.Count;
            SUB_COMMAND_FACTORIES.Add(subCommandFactory);
            return position;
        }

        private double _range;
        /// <summary>
        /// Distance to robot which was scanned.
        /// </summary>
        public double RANGE {
            get {
                if (pending)
                    throw new NotSupportedException("Cannot access to property of pending request.");
                return _range;
            }
            private set => _range = value;
        }

        private int _enemyId;
        /// <summary>
        /// Robot's which was scanned id.
        /// </summary>
        public int ENEMY_ID {
            get {
                if (pending)
                    throw new NotSupportedException("Cannot access to property of pending request.");
                return _enemyId;
            }
            private set => _enemyId = value;
        }

        public ScanAnswerCommand() { }

        public ScanAnswerCommand(double range, int enemyID)
            : base() {
                ENEMY_ID = enemyID;
                RANGE = range;
            pending = false;
        }

        public void FillData(ScanAnswerCommand source) {
            ENEMY_ID = source.ENEMY_ID;
            RANGE = source.RANGE;
            pending = false;
        }
   }
}
