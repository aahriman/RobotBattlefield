using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ViewerLibrary.serializer {
    public sealed class JSONSerializer {
        private int row = 1;

        public string Serialize(Turn t) {
            return JsonConvert.SerializeObject(t);
        }

        public Turn Deserialize(string s) {
            row++;
            try {
                Turn t = JsonConvert.DeserializeObject<Turn>(s);
                JObject deserializedObject = JObject.Parse(s);
                JArray moreArray = (JArray) deserializedObject["MORE"];
                for (int i = 0; i < moreArray.Count; i++) {
                    JArray singleMoreArray = (JArray) moreArray[i];
                    if (singleMoreArray.Count > 0) {
                        t.MORE[i] = (object[]) JsonConvert.DeserializeObject(singleMoreArray.ToString(),
                                                                             GetType(
                                                                                     singleMoreArray[0]["TYPE_NAME"]
                                                                                         .ToString()).MakeArrayType());
                    }


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