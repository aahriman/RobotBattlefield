using System;
using BaseLibrary.command.equipment;
using BaseLibrary.protocol;
using BaseLibrary.utils;

namespace BaseLibrary.command.v1._0.equipment {
    class GetRepairToolAnswerCommandV1_0 : GetRepairToolAnswerCommand, ACommand.Sendable{
        private const string NAME = "REPAIR_TOOL_ANSWER";
        public static readonly IFactory<ACommand.Sendable, ACommand> FACTORY = new CommandFactory();
        private sealed class CommandFactory : ACommandFactory {
            internal CommandFactory() : base() { }

            public override bool IsDeserializable(string s) {
                s = s.Trim();
                string[] rest;
                if (ProtocolV1_0Utils.GetParams(s, NAME, ProtocolV1_0Utils.DEFAULT, out rest)) {
                    if (rest.Length == 1 && ProtocolV1_0Utils.Deserialize(rest[0], out rest, ProtocolV1_0Utils.DEFAULT.NEXT)) {
                        RepairToolV1_0[] repairTools = new RepairToolV1_0[rest.Length];
                        for (int i = 0; i < repairTools.Length; i++) {
                            if (!RepairToolV1_0.Deserialize(rest[i], ProtocolV1_0Utils.DEFAULT.NEXT.NEXT, out repairTools[i])) {
                                return false;
                            }
                        }
                        cache.Cached(s, new GetRepairToolAnswerCommandV1_0(repairTools));
                        return true;
                    }
                }
                return false;
            }

            public override bool IsTransferable(ACommand c) {
                var c2 = c as GetRepairToolAnswerCommand;
                if (c2 != null){
                    cache.Cached(c, new GetRepairToolAnswerCommandV1_0(RepairToolV1_0.Convert(c2.REPAIR_TOOLS)));
                    return true;
                }
                return false;
            }
        }
        public GetRepairToolAnswerCommandV1_0(RepairToolV1_0[] repairTool) : base(repairTool) { }

        public string Serialize() {
            return ProtocolV1_0Utils.SerializeParams(NAME, ProtocolV1_0Utils.DEFAULT, new object[]{ REPAIR_TOOLS});
        }
    }
}
