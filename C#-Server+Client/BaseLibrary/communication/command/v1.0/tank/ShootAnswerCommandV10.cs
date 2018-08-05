using BaseLibrary.communication.command.tank;
using BaseLibrary.communication.protocol;
using BaseLibrary.utils;

namespace BaseLibrary.communication.command.v1._0.tank {
    public class ShootAnswerCommandV10 : ShootAnswerCommand, ACommand.Sendable {
	    private const string NAME = "SHOT_ANSWER";

        public static readonly IFactory<ACommand.Sendable, ACommand> FACTORY = new CommandFactory();
        private sealed class CommandFactory : ACommandFactory {

			public override bool IsDeserializeable(string s) {
				string rest;
				if (ProtocolV1_0Utils.GetParams(s, NAME, out rest)) {
					if (rest.Equals("1") || rest.Equals("0")) {
						cache.Cached(s, new ShootAnswerCommandV10(rest.Equals("1")));
						return true;
					}
				}
				return false;
			}

            public override bool IsTransferable(ACommand c) {
				ShootAnswerCommand command = c as ShootAnswerCommand;
	            if (command != null) {
					cache.Cached(c, new ShootAnswerCommandV10(command.SUCCESS));
		            return true;
	            }
                return false;
            }
        }

        public ShootAnswerCommandV10(bool success)
            : base(success) { }

        public string Serialize() {
           return ProtocolV1_0Utils.SerializeParams(NAME, (SUCCESS? 1 : 0));
        }
    }
}
