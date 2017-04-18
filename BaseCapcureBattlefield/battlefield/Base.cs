using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseLibrary;
using BaseLibrary.utils;
using BaseLibrary.utils.protocolV1_0Utils;

namespace BaseCapcureBattlefield.battlefield {
    public class Base {
        public readonly ProtocolDouble X;
        public readonly ProtocolDouble Y;

        public readonly int MAX_PROGRESS;

        public int Progress { get; set; }
        public int TeamId { get; set; }

        public readonly string TYPE_NAME;

        public Base(ProtocolDouble x, ProtocolDouble y, int maxProgress) {
            TYPE_NAME = GetType().ToString();
            X = x;
            Y = y;
            MAX_PROGRESS = maxProgress;
        }

        protected bool Equals(Base other) {
            return Equals(X, other.X) && Equals(Y, other.Y) && MAX_PROGRESS == other.MAX_PROGRESS;
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Base) obj);
        }

        public override int GetHashCode() {
            unchecked {
                int hashCode = (X != null ? X.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Y != null ? Y.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ MAX_PROGRESS;
                return hashCode;
            }
        }
    }
}
