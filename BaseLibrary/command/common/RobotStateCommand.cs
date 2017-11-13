using System;
using System.Collections.Generic;
using BaseLibrary.visitors;

namespace BaseLibrary.command.common {
    public class RobotStateCommand : ACommonCommand{

        protected static readonly List<ISubCommandFactory> SUB_COMMAND_FACTORIES = new List<ISubCommandFactory>();

        public static int RegisterSubCommandFactory(ISubCommandFactory subCommandFactory) {
            int position = SUB_COMMAND_FACTORIES.Count;
            SUB_COMMAND_FACTORIES.Add(subCommandFactory);
            return position;
        }

        /// <summary>
        /// X-coordinate of robot position.
        /// </summary>
        public double X { get; private set; }

        /// <summary>
        /// Y-coordinate of robot position.
        /// </summary>
        public double Y { get; private set; }
        /// <summary>
        /// Heal of robot.
        /// </summary>
        public int HIT_POINTS { get; private set; }
        /// <summary>
        /// Actual robot power.
        /// </summary>
        public double POWER { get; private set; }

        /// <summary>
        /// Actual turn.
        /// </summary>
        public int TURN { get; private set; }

        /// <summary>
        /// Max turns in lap.
        /// </summary>
        public int MAX_TURN { get; private set; }

        /// <summary>
        /// How many robot are life. 
        /// </summary>
        public int COUNT_OF_LIFE_ROBOTS { get; private set; }

        /// <summary>
        /// Ids of lives robot.
        /// </summary>
        public int[] ARRAY_IDS_OF_LIFE_ROBOTS { get; private set; }

        public EndLapCommand END_LAP_COMMAND { get; private set; }

        public RobotStateCommand(double x, double y, int hitPoints, double power, int turn, int maxTurn, int countOfLefeRobots, int[] arrayIdsOfLifeRobots, EndLapCommand endLapCommand) {
            X = x;
            Y = y;
            HIT_POINTS = hitPoints;
            POWER = power;
            TURN = turn;
            MAX_TURN = maxTurn;
            COUNT_OF_LIFE_ROBOTS = countOfLefeRobots;
            ARRAY_IDS_OF_LIFE_ROBOTS = arrayIdsOfLifeRobots;
            END_LAP_COMMAND = endLapCommand;
            MORE = new object[SUB_COMMAND_FACTORIES.Count];
        }

        public sealed override void accept(ICommandVisitor accepter) {
            accepter.visit(this);
        }

        public sealed override Output accept<Output>(ICommandVisitor<Output> accepter) {
            return accepter.visit(this);
        }

        public sealed override Output accept<Output, Input>(ICommandVisitor<Output, Input> accepter, Input input) {
            return accepter.visit(this, input);
        }

    }
}
