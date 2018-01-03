using System;
using System.Collections.Generic;
using BaseLibrary.command.common;

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


        public string NAME { get; private set; }
        public string TEAM_NAME { get; private set; }
        public RobotType ROBOT_TYPE { get; private set; }


        public InitCommand(string name) : this(name, null) {}

        public InitCommand(string name, string teamName) : this(name, teamName, RobotType.TANK) { }

        public InitCommand(string name, string teamName, RobotType robotType) {
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

    }

    public enum RobotType {
        REPAIRMAN, TANK, MINE_LAYER, NONE // none is not defined type
    }

}
