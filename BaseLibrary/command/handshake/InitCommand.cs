using System;
using System.Collections.Generic;
using BaseLibrary.command.common;
using BaseLibrary.visitors;

namespace BaseLibrary.command.handshake {
    public class InitCommand : AHandShakeCommand {
        public static int NAME_MAX_LENGTH = 10;
        public static int TEAM_NAME_MAX_LENGTH = 40;

        private static readonly List<ISubCommandFactory> SUB_COMMAND_FACTORIES = new List<ISubCommandFactory>();

        public static int RegisterSubCommandFactory(ISubCommandFactory subCommandFactory) {
            int position = SUB_COMMAND_FACTORIES.Count;
            SUB_COMMAND_FACTORIES.Add(subCommandFactory);
            return position;
        }


        public String NAME { get; private set; }
        public String TEAM_NAME { get; private set; }
        public RobotType ROBOT_TYPE { get; private set; }


        public InitCommand(String name) : this(name, null) {}

        public InitCommand(String name, String teamName) : this(name, teamName, RobotType.TANK) { }

        public InitCommand(String name, String teamName, RobotType robotType) {
            if (name == null) {
                throw new ArgumentNullException(nameof(name));
            }

            if (teamName == null) {
                teamName = "";
            }

            NAME = name.Substring(0, Math.Min(name.Length, NAME_MAX_LENGTH));
            TEAM_NAME = teamName.Substring(0, Math.Min(teamName.Length, TEAM_NAME_MAX_LENGTH));
            ROBOT_TYPE = robotType;
        }

        public sealed override void accept(ICommandVisitor accepter) {
            accepter.visit(this);
        }

        public sealed override Output accept<Output>(ICommandVisitor<Output> accepter) {
            return accepter.visit(this);
        }

        public sealed override Output accept<Output, Input>(ICommandVisitor<Output, Input> accepter,
            params Input[] inputs) {
            return accepter.visit(this, inputs);
        }
    }

    public enum RobotType {
        REPAIRMAN, TANK, MINER, NONE // none is not defined type
    }

}
