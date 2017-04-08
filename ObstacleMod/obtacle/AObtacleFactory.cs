using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseLibrary.command;
using BaseLibrary.protocol;

namespace ObstacleMod.obtacle {
    public abstract class AObtacleFactory : AFactory<IObtacle, IObtacle> {
        protected Factories<IObtacle, IObtacle> comandsFactory = new Factories<IObtacle, IObtacle>();
    }
}
