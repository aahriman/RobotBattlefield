namespace BaseLibrary.protocol {
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class ProtocolDescription : System.Attribute {
        public readonly string NAME;

        public ProtocolDescription(string name) {
            this.NAME = name;
        }
    }
}
