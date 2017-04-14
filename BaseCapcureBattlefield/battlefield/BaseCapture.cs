using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using BaseLibrary.battlefield;
using BaseLibrary.command;
using BaseLibrary.equip;
using BaseLibrary.protocol;
using BaseLibrary.utils;
using BattlefieldLibrary.battlefield;
using ServerLibrary.config;
using BattlefieldRobot = BattlefieldLibrary.battlefield.robot.BattlefieldRobot;


namespace BaseCapcureBattlefield.battlefield {
    [ModDescription()]
	public class BaseCapture : Battlefield {
	    private static readonly int POSITION_IN_BATTLE_TURN;
        private static readonly int POSITION_IN_ROBOT_STATE_COMMAND;

        private static readonly ISubCommandFactory SUB_COMMAND_FACTORY = new SubCommandFactory();
        private class SubCommandFactory : ISubCommandFactory {
            private const string COMMAND_BASE_NAME = "BASE";
            internal SubCommandFactory() {}
            public bool Deserialize(string s, object[] commandsMore) {
                String[] basesString;
                if (ProtocolV1_0Utils.Deserialize(s, out basesString, ProtocolV1_0Utils.DEFAULT.NEXT.NEXT)) {
                    
                        List<Base> bases = new List<Base>();
                        foreach (var baseString in basesString) {// parametrs are separated by DEFAULT.NEXT.NEXT.NEXT
                            String[] baseParam;

                            if (ProtocolV1_0Utils.GetParams(baseString, COMMAND_BASE_NAME,
                                                            ProtocolV1_0Utils.DEFAULT.NEXT.NEXT.NEXT, out baseParam)) {
                                if (baseParam.Length == 4) {
                                    int x, y, teamId, progress;
                                    if (int.TryParse(baseParam[0], out x) &&
                                        int.TryParse(baseParam[0], out y) &&
                                        int.TryParse(baseParam[0], out teamId) &&
                                        int.TryParse(baseParam[0], out progress)) {
                                        Base @base = new Base(x, y);
                                        @base.TeamId = teamId;
                                        @base.Progress = progress;

                                        bases.Add(@base);
                                    }
                                }
                            }
                        }
                        commandsMore[POSITION_IN_ROBOT_STATE_COMMAND] = bases.ToArray();
                }
                return false;
            }

            public bool Serialize(object singleMore, out string serializedSingleMore) {
                // this is use in RobotStateCommand (DEFAULT) in array (DEFAULT.NEXT) => separator for arrays items is DEAULT.NEXT.NEXT
                Base[] o = singleMore as Base[];
                if (o != null) {
                    StringBuilder sb = new StringBuilder();
                    if (0 < o.Length) {
                        sb.Append("[");
                    }
                    for (int i = 0; i < o.Length; i++) {
                        sb.Append(ProtocolV1_0Utils.ConvertToDeeper(ProtocolV1_0Utils.SerializeParams(COMMAND_BASE_NAME, o[i].X, o[i].Y, o[i].TeamId, o[i].Progress),
                                                          ProtocolV1_0Utils.DEFAULT.NEXT.NEXT.NEXT));
                        if (i + 1 < o.Length) {
                            sb.Append(ProtocolV1_0Utils.DEFAULT.NEXT.NEXT.SEPARATOR);
                        }
                    }
                    if (0 < o.Length) {
                        sb.Append("[");
                    }
                    serializedSingleMore = sb.ToString();
                    return true;
                }
                serializedSingleMore = null;
                return false;
            }
        }
        static BaseCapture() {
            POSITION_IN_BATTLE_TURN = BattlefieldTurn.RegisterMore();
            POSITION_IN_ROBOT_STATE_COMMAND = RobotStateCommand.RegisterSubCommandFactory(SUB_COMMAND_FACTORY);
        }

	    private Base[] bases;
		public BaseCapture(BattlefieldConfig battlefielConfig)  : base(battlefielConfig) {
            bases = convertFromMore(battlefielConfig.MORE);
		}

	    private Base[] convertFromMore(Object[] more) {
	        return (from m in more
	                where m is Base
	                select m as Base).ToArray();
	    }

        protected override RobotStateCommand AddToRobotStateCommand(RobotStateCommand robotStateCommand, BattlefieldRobot r) {
	        robotStateCommand.MORE[POSITION_IN_ROBOT_STATE_COMMAND] = bases;
            return robotStateCommand;

        }

	    protected override InitAnswerCommand AddToInitAnswereCommand(InitAnswerCommand initAnswerCommand) {
	        return initAnswerCommand;
	    }

	    protected override void afterProcessCommand() {
	        // do nothing
	    }

	    protected override void afterMovingAndDamaging() {
	        // vyhodnotit zabírání baze
	    }

        protected override LapState NewLapState() {
            // je jen jeden team|team zabral všechny baze
            if (turn > MAX_TURN) {
                return LapState.LAP_OUT;
            }

            return LapState.NONE;
        }
    }
}
