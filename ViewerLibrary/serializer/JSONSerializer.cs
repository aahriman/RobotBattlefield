using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BaseLibrary.utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ViewerLibrary.serializer {
    /// <summary>
    /// Serialize from Turn object to JSON format and de-serialize from JSON format to Turn object.
    /// </summary>
    public sealed class JSONSerializer {
        private int row = 1;

        public string Serialize(Turn t) {
            return JsonConvert.SerializeObject(t);
        }

        public Turn Deserialize(string s) {
            row++;
            try {
                Turn t = JsonConvert.DeserializeObject<Turn>(s);
                JObject jObject = JObject.Parse(s);
                JArray moreArray = (JArray) jObject["MORE"];
                object[][] deserializeMore = (object[][])ModUtils.DeserializeMoreObjects(moreArray);
                for (int i = 0; i < deserializeMore.Length; i++) {
                    t.MORE[i] = deserializeMore[i];
                }
                return t;

            } catch {
                Console.Error.WriteLine("At row {0} is wrong format.", row);
                return null;
            }
            
        }

        public static Type GetType(string typeName) {
            Type type = Type.GetType(typeName);
            if (type != null)
                return type;
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies()) {
                type = asm.GetType(typeName);
                if (type != null)
                    return type;
            }
            return null;
        }
    }
}