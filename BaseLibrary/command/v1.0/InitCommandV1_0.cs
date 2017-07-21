using System;
using BaseLibrary.command.common;
using BaseLibrary.command.handshake;
using BaseLibrary.protocol;
using BaseLibrary.utils;

namespace BaseLibrary.command.v1._0 {
	public class InitCommandV1_0 : InitCommand, ACommand.Sendable {
	    private const String COMMAND_NAME = "INIT";

        public static readonly IFactory<ACommand.Sendable, ACommand> FACTORY = new CommandFactory();
        private sealed class CommandFactory : ACommandFactory {
            internal CommandFactory() : base() { }

            public override Boolean IsDeserializable(String s) {
                String[] rest;
                if (ProtocolV1_0Utils.GetParams(s, COMMAND_NAME, out rest) && rest.Length == 3) {
                    RobotType robotType;
                    if (Enum.TryParse(rest[2], true, out robotType)) {
                        InitCommandV1_0 init = new InitCommandV1_0(rest[0], rest[1], robotType);
                        cache.Cached(s, init);
                        return true;
                    }
                }
                return false;
            }

            public override bool IsTransferable(ACommand c) {
                if (c is InitCommand) {
                    var c2 = (InitCommand)c;
                    cache.Cached(c, new InitCommandV1_0(c2.NAME, c2.TEAM_NAME, c2.ROBOT_TYPE));
                    return true;
                }
                return false;
            }
        }

		public InitCommandV1_0(String name, String teamName, RobotType robotType) : base(name, teamName, robotType) { }


        public string Serialize() {
            return ProtocolV1_0Utils.SerializeParams(COMMAND_NAME, NAME, TEAM_NAME, ROBOT_TYPE);
        }
    }
}
