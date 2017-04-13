using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseLibrary.protocol;

namespace BaseLibrary.command {
    public abstract class ACommandFactory : AFactory<ACommand.Sendable, ACommand> {
        public override bool IsSerializeable(ACommand c) {
            if (IsTransferable(c)) {
                cacheForSerialize.Cached(c, Transfer(c).Serialize());
                return true;
            }
            return false;
        }
    }
}
