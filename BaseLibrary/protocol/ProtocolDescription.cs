namespace BaseLibrary.protocol {
    /// <summary>
    /// Attribute for protocol to automatic map implementation with name.
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class ProtocolDescription : System.Attribute {
        public readonly string NAME;

        public ProtocolDescription(string name) {
            this.NAME = name;
        }
    }
}
