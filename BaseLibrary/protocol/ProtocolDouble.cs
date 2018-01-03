using System;

namespace BaseLibrary.protocol {
    /// <summary>
    /// Represent double witch is safe for send  by network. Its number is save in int type with 3 decimal places (that is mean that 1000 [in ProtocolDouble] is equals to 1 and 12 [in ProtocolDouble] is equals to 0.012)
    /// </summary>
    public class ProtocolDouble {
        public static readonly ProtocolDouble ZERO = new ProtocolDouble(0.0);
        public static readonly ProtocolDouble _0 = ZERO;
        public static readonly ProtocolDouble _100 = new ProtocolDouble(100.0);

        /// <summary>
        /// Quotient for multiply double.
        /// </summary>
        public const double MULTIPLE = 1000.0; // it has to be power of ten

        public static bool TryParse(string s, out ProtocolDouble result) {
            bool success = int.TryParse(s, out int v);
            result = new ProtocolDouble(v / MULTIPLE);
            return success;
        }

        public static explicit operator ProtocolDouble(double d) {
            return new ProtocolDouble(d);
        }

        public static implicit operator double(ProtocolDouble pd) {
            return pd.value / MULTIPLE;
        }

        public int value { get; private set; }

        public ProtocolDouble(double d) {
            value = (int)(d * MULTIPLE);
        }

        public ProtocolDouble(ProtocolDouble d) {
            value = d.value;
        }

        public string Serialize() {
            return value.ToString();
        }

        public override string ToString() {
            return (value).ToString();
        }
    }
}
