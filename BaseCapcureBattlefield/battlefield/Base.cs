using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseLibrary.utils;
using BaseLibrary.utils.protocolV1_0Utils;

namespace BaseCapcureBattlefield.battlefield {
    internal class Base : InnerSerializerV1_0 {
        public const string COMMAND_NAME = "BASE";

        public readonly double X;
        public readonly double Y;

        public int Progress { get; set; }
        public int TeamId { get; set; }

        public readonly string TYPE_NAME;

        public Base(double x, double y) {
            TYPE_NAME = GetType().ToString();
            X = x;
            Y = y;
        }

        public string Serialize(Deep deep) {
            return ProtocolV1_0Utils.SerializeParams(COMMAND_NAME, deep, X, Y, Progress, TeamId);
        }
    }
}
