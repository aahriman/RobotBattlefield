using System;
using System.Collections.Generic;

namespace BaseLibrary.communication.command.common {
    public class RobotStateCommand : ACommonCommand{

        protected static readonly List<ISubCommandFactory> SUB_COMMAND_FACTORIES = new List<ISubCommandFactory>();

        public static int RegisterSubCommandFactory(ISubCommandFactory subCommandFactory) {
            lock (SUB_COMMAND_FACTORIES) {
                int position = SUB_COMMAND_FACTORIES.Count;
                if (SUB_COMMAND_FACTORIES.Contains(subCommandFactory)) {
                    position = SUB_COMMAND_FACTORIES.FindIndex(element => element.Equals(subCommandFactory));
                } else {
                    SUB_COMMAND_FACTORIES.Add(subCommandFactory);
                }
                return position;
            }
        }

        private double _x;
        /// <summary>
        /// X-coordinate of robot position.
        /// </summary>
        public double X {
            get {
                if (pending)
                    throw new NotSupportedException("Cannot access to property of pending request.");
                return _x;
            }
            private set => _x = value;
        }

        private double _y;
        /// <summary>
        /// Y-coordinate of robot position.
        /// </summary>
        public double Y {
            get {
                if (pending)
                    throw new NotSupportedException("Cannot access to property of pending request.");
                return _y;
            }
            private set => _y = value;
        }

        private int _hitPoints;
        /// <summary>
        /// Heal of robot.
        /// </summary>
        public int HIT_POINTS {
            get {
                if (pending)
                    throw new NotSupportedException("Cannot access to property of pending request.");
                return _hitPoints;
            }
            private set => _hitPoints = value;
        }

        private double _power;

        /// <summary>
        /// Actual robot power.
        /// </summary>
        public double POWER {
            get {
                if (pending)
                    throw new NotSupportedException("Cannot access to property of pending request.");
                return _power;
            }
            private set => _power = value;
        }

        private int _turn;
        /// <summary>
        /// Actual turn.
        /// </summary>
        public int TURN {
            get {
                if (pending)
                    throw new NotSupportedException("Cannot access to property of pending request.");
                return _turn;
            }
            private set => _turn = value;
        }

        private int _maxTurn;
        /// <summary>
        /// Max turns in lap.
        /// </summary>
        public int MAX_TURN {
            get {
                if (pending)
                    throw new NotSupportedException("Cannot access to property of pending request.");
                return _maxTurn;
            }
            private set => _maxTurn = value;
        }

        private int _countOfLifeRobots;
        /// <summary>
        /// How many robot are life. 
        /// </summary>
        public int COUNT_OF_LIFE_ROBOTS {
            get {
                if (pending)
                    throw new NotSupportedException("Cannot access to property of pending request.");
                return _countOfLifeRobots;
            }
            private set => _countOfLifeRobots = value;
        }

        private int[] _arrayIdsOfLifeRobots;
        /// <summary>
        /// Ids of living robot.
        /// </summary>
        public int[] ARRAY_IDS_OF_LIFE_ROBOTS {
            get {
                if (pending)
                    throw new NotSupportedException("Cannot access to property of pending request.");
                return _arrayIdsOfLifeRobots;
            }
            private set => _arrayIdsOfLifeRobots = value;
        }

        private EndLapCommand _endLapCommand;
        public EndLapCommand END_LAP_COMMAND {
            get {
                if (pending)
                    throw new NotSupportedException("Cannot access to property of pending request.");
                return _endLapCommand;
            }
            private set => _endLapCommand = value;
        }

        public RobotStateCommand(double x, double y, int hitPoints, double power, int turn, int maxTurn, int countOfLifeRobots, int[] arrayIdsOfLifeRobots, EndLapCommand endLapCommand) {
            X = x;
            Y = y;
            HIT_POINTS = hitPoints;
            POWER = power;
            TURN = turn;
            MAX_TURN = maxTurn;
            COUNT_OF_LIFE_ROBOTS = countOfLifeRobots;
            ARRAY_IDS_OF_LIFE_ROBOTS = arrayIdsOfLifeRobots;
            END_LAP_COMMAND = endLapCommand;
            MORE = new object[SUB_COMMAND_FACTORIES.Count];
            pending = false;
        }
    }
}
