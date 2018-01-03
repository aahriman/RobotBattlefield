using System;
using System.Collections.Generic;
using BaseLibrary.battlefield;

namespace BaseLibrary.command.common {
    /// <summary>
    /// Command at the end of lap. This is sub command to RobotStateCommand
    /// </summary>
    public class EndLapCommand : ACommonCommand {

        private static readonly List<ISubCommandFactory> SUB_COMMAND_FACTORIES = new List<ISubCommandFactory>();

        public static int RegisterSubCommandFactory(ISubCommandFactory subCommandFactory) {
            int position = SUB_COMMAND_FACTORIES.Count;
            SUB_COMMAND_FACTORIES.Add(subCommandFactory);
            return position;
        }

        private LapState _state;

        /// <summary>
        /// Why lap end.
        /// </summary>
        public LapState STATE {
            get {
                if (pending)
                    throw new NotSupportedException("Cannot access to property of pending request.");
                return _state;
            }
            private set => _state = value;
        }

        private int _gold;
        /// <summary>
        /// How many gold robot has.
        /// </summary>
        public int GOLD {
            get {
                if (pending)
                    throw new NotSupportedException("Cannot access to property of pending request.");
                return _gold;
            }
            private set => _gold = value;
        }

        private int _score;
        /// <summary>
        /// What robot score is.
        /// </summary>
        public int SCORE {
            get {
                if (pending)
                    throw new NotSupportedException("Cannot access to property of pending request.");
                return _score;
            }
            private set => _score = value;
        }

        public EndLapCommand(LapState state, int gold, int score) {
            STATE = state;
            GOLD = gold;
            SCORE = score;
            pending = false;
        }
    }
}
