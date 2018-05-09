using System;
using BaseLibrary.utils;

namespace BaseLibrary.command.v1._0 {
    public class ShotAnswerCommandV1_0 : ShotAnswerCommand, ACommand.Sendable {
	    private const string NAME = "SHOT_ANSWER";

        public static readonly ICommandFactory FACTORY = new ShotAnswerCommandFactory();
        private sealed class ShotAnswerCommandFactory : ACommandFactory {

			public override Boolean IsDeserializable(String s) {
				string rest;
				if (ProtocolV1_0Utils.GetParams(s, NAME, out rest)) {
					if (rest.Equals("1") || rest.Equals("0")) {
						cache.Cached(s, new DriveAnswerCommandV1_0(rest.Equals("1")));
						return true;
					}
				}
				return false;
			}

            public override bool IsTransferable(ACommand c) {
				ShotAnswerCommand command = c as ShotAnswerCommand;
	            if (command != null) {
					cache.Cached(c, new ShotAnswerCommandV1_0(command.SUCCESS));
		            return true;
	            }
                return false;
            }
        }

        public ShotAnswerCommandV1_0(bool success)
            : base(success) { }

        public string serialize() {
           return ProtocolV1_0Utils.SerializeParams(NAME, (SUCCESS? 1 : 0));
        }
    }
}
