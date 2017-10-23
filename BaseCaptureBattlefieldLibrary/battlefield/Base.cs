using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseLibrary;
using BaseLibrary.protocol;
using BaseLibrary.utils;

namespace BaseCapcureBattlefieldLibrary.battlefield {
    public class Base {
        public readonly double X;
        public readonly double Y;

        public readonly int MAX_PROGRESS;

        public readonly string TYPE_NAME;

        public int Progress { get; set; }
        public int TeamId { get; set; }

        public int ProgressTeamId { get; set; }

        public Base(double x, double y, int MAX_PROGRESS) {
            X = x;
            Y = y;
            this.MAX_PROGRESS = MAX_PROGRESS;
            TYPE_NAME = this.GetType().ToString();
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
                int hashCode = X.GetHashCode();
                hashCode = (hashCode * 397) ^ Y.GetHashCode();
                hashCode = (hashCode * 397) ^ MAX_PROGRESS;
                return hashCode;
            }
        }
    }
}
