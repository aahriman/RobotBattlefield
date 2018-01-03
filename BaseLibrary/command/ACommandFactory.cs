﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseLibrary.protocol;

namespace BaseLibrary.command {
    /// <summary>
    /// Factory for command
    /// </summary>
    public abstract class ACommandFactory : AFactory<ACommand.Sendable, ACommand> {
        /// <summary>
        /// Try to Transferable and if it is possible store serialization of transfer object like string.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public override bool IsSerializable(ACommand c) {
            if (IsTransferable(c)) {
                cacheForSerialize.Cached(c, Transfer(c).Serialize());
                return true;
            }
            return false;
        }
    }
}
