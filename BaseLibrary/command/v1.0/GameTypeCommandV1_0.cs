using System;
using BaseLibrary.command.common;
using BaseLibrary.command.handshake;
using BaseLibrary.protocol;
using BaseLibrary.utils;

namespace BaseLibrary.command.v1._0 {
    public class GameTypeCommandV1_0 : GameTypeCommand, ACommand.Sendable {
        public const string NAME = "GAME_TYPE";

        public static readonly IFactory<ACommand.Sendable, ACommand> FACTORY = new CommandFactory();
        private sealed class CommandFactory : ACommandFactory {
            public override bool IsDeserializable(string s) {
                string[] rest;
                if (ProtocolV1_0Utils.GetParams(s, NAME, out rest)) {
                    int param1; GameType param2;
                    if (rest.Length == 2 && int.TryParse(rest[0], out param1) && Enum.TryParse(rest[1], true, out param2)) {
                        cache.Cached(s, new GameTypeCommandV1_0(param1, param2));
                        return true;
                    }
                }
                return false;
            }

            public override bool IsTransferable(ACommand c) {
                if (c is GameTypeCommand) {
                    var c1 = (GameTypeCommand) c;
                    cache.Cached(c, new GameTypeCommandV1_0(c1.ROBOTS_IN_ONE_TEAM, c1.GAME_TYPE));
                    return true;
                }
                return false;
            }
        }

        public GameTypeCommandV1_0(int ROBOTS_IN_ONE_TEAM, GameType GAME_TYPE) : base(ROBOTS_IN_ONE_TEAM, GAME_TYPE) {}

        public string Serialize() {
            return ProtocolV1_0Utils.SerializeParams(NAME, ROBOTS_IN_ONE_TEAM, GAME_TYPE);
        }
    }
}
