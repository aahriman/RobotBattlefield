using System;

namespace BaseLibrary.protocol {
    public class ProtocolDouble {
        public static readonly ProtocolDouble ZERO = new ProtocolDouble(0.0);
        public static readonly ProtocolDouble _0 = ZERO;
        public static readonly ProtocolDouble _100 = new ProtocolDouble(100.0);

        public const double MULTIPLE = 1000.0; // it has to be power of ten

        public static bool TryParse(string s, out ProtocolDouble result) {
            int v;
            bool succes = int.TryParse(s, out v);
            result = v;
            return succes;
        }

        public static explicit operator ProtocolDouble(double d) {
            return new ProtocolDouble(d);
        }

        public static implicit operator double(ProtocolDouble pd) {
            return pd.value / MULTIPLE;
        }

        public static implicit operator ProtocolDouble(int i) {
            return new ProtocolDouble(i / MULTIPLE);
        }

        public int value { get; private set; }

        public ProtocolDouble(double d) {
            value = (int)(d * MULTIPLE);
        }

        public ProtocolDouble(ProtocolDouble d) {
            value = d.value;
        }

        public ProtocolDouble(int v) {
            value = (int) (v * MULTIPLE);
        }

        public string Serialize() {
            return value.ToString();
        }

        public override string ToString() {
            return (value).ToString();
        }
    }
}
