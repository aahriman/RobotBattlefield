using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlagCapcureBattlefield.battlefield {
    public class Flag {
        public int RobotId;
        public readonly int FROM_FLAGPLACE_ID;

        public Flag(int FROM_FLAGPLACE_ID) {
            this.FROM_FLAGPLACE_ID = FROM_FLAGPLACE_ID;
        }
    }
}
