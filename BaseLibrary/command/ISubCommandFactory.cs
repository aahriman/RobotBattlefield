using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BaseLibrary.command {
    public interface ISubCommandFactory {

        bool Deserialize(String s, Object[] commandsMore);
        bool Serialize(Object singleMore, out String serializedSingleMore);
    }
}
