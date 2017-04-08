using System;

namespace ViewerLibrary.serializers {
    public abstract class ASerializer {
        public abstract String Serialize(Turn t);
        public abstract Turn Deserialize(String s);
        public abstract string SerializerName();

        public String Config() {
            return String.Format("{{'serializer':'{0}'}}", this.SerializerName());
        }

        public static ASerializer getSerializer(String serializerConfig) {
            // TODO
            return new JSONSerializer();
        }
    }
}
