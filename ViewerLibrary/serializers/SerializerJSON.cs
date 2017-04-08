using Newtonsoft.Json;

namespace ViewerLibrary.serializers {
    public sealed class JSONSerializer : ASerializer {
        private int row = 1;
        public override string Serialize(Turn t) {
            return JsonConvert.SerializeObject(t, Formatting.None);
        }

        public override Turn Deserialize(string s) {
            row++;
            return JsonConvert.DeserializeObject<Turn>(s);
        }

        public override string SerializerName() {
            return "json";
        }
    }
}
//891, 1465, 1504