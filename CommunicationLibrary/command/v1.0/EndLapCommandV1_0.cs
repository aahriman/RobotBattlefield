using System;
using CommunicationLibrary.utils;

namespace CommunicationLibrary.command.v1._0 {
	internal class EndLapCommandV1_0 : EndLapCommand, ACommand.Sendable{
        public static readonly ICommandFactory FACTORY = new CommandFactory();
        private sealed class CommandFactory : ACommandFactory {
            internal CommandFactory() : base() { }

            public override Boolean IsDeserializable(String s) {
                s = s.Trim();
                string[] rest;
                if (StringUtils.GetRestOfStringSplited(s, "END_LAP(", ")", out rest, ';')) {
                    if (rest.Length != 2) {
                        return false;
                    }
                    int gold;
                    LapStates lapState;

                    if (rest[0].Equals(LapStates.NONE.ToString())) {
                        lapState = LapStates.NONE;
                    }else if(rest[0].Equals(LapStates.WIN.ToString())){
                        lapState = LapStates.WIN;
                    }else if(rest[0].Equals(LapStates.LAP_OUT.ToString())){
                        lapState = LapStates.LAP_OUT;
                    }else{
                        return false;
                    }
                    if (int.TryParse(rest[1], out gold)) {
                        cache.Cached(s, new EndLapCommandV1_0(lapState, gold));
                        return true;
                    }
                }
                return false;
            }

            public override bool IsTransferable(ACommand c) {
                if (c is EndLapCommand) {
                    var c2 = (EndLapCommand)c;
                    cache.Cached(c, new EndLapCommandV1_0(c2.STATE, c2.GOLD));
                    return true;
                }
                return false;
            }
        }

        public EndLapCommandV1_0(LapStates lapState, int gold) : base(lapState, gold) {}

        public string serialize() {
            return String.Format("END_LAP({0}; {1})", STATE, GOLD);
        }
    }
}
