using System.Collections.Generic;
using System.Linq;
using BaseLibrary.protocol;

namespace BaseLibrary.utils {
	public static class Parser {
		public static bool TryParse(IEnumerable<string> sources, out int[] param) {
			param = new int[sources.Count()];
			int i = 0;
			foreach (var source in sources) {
				if (!int.TryParse(source, out param[i++])) {
					return false;
				}
			}
			return true;
		}

		public static bool TryParse(IEnumerable<string> sources, out double[] param) {
			param = new double[sources.Count()];
			int i = 0;
			foreach (var source in sources) {
				if (!double.TryParse(source, out param[i++])) {
					return false;
				}
			}
			return true;
		}

		public static bool TryParse(IEnumerable<string> sources, out ProtocolDouble[] param) {
			param = new ProtocolDouble[sources.Count()];
			int i = 0;
			foreach (var source in sources) {
				if (!ProtocolDouble.TryParse(source, out param[i++])) {
					return false;
				}
			}
			return true;
		}

		public static bool TryParse(int[] positions, IList<string> sources, out int[] param) {
			param = new int[positions.Length];
			
			for (int i = 0; i < positions.Length; i++) {
				if (!int.TryParse(sources[positions[i]], out param[i])) {
					return false;
				}
			}
			return true;
		}

		public static bool TryParse(int[] positions, IList<string> sources, out double[] param) {
			param = new double[positions.Length];

			for (int i = 0; i < positions.Length; i++) {
				if (!double.TryParse(sources[positions[i]], out param[i])) {
					return false;
				}
			}
			return true;
		}

		public static bool TryParse(int[] positions, IList<string> sources, out ProtocolDouble[] param) {
			param = new ProtocolDouble[positions.Length];

			for (int i = 0; i < positions.Length; i++) {
				if (!ProtocolDouble.TryParse(sources[positions[i]], out param[i])) {
					return false;
				}
			}
			return true;
		}

		public static bool TryParse(int[] positions, string[] sources, out int[] param) {
			param = new int[positions.Length];

			for (int i = 0; i < positions.Length; i++) {
				if (!int.TryParse(sources[positions[i]], out param[i])) {
					return false;
				}
			}
			return true;
		}

		public static bool TryParse(int[] positions, string[] sources, out double[] param) {
			param = new double[positions.Length];

			for (int i = 0; i < positions.Length; i++) {
				if (!double.TryParse(sources[positions[i]], out param[i])) {
					return false;
				}
			}
			return true;
		}

		public static bool TryParse(int[] positions, string[] sources, out ProtocolDouble[] param) {
			param = new ProtocolDouble[positions.Length];

			for (int i = 0; i < positions.Length; i++) {
				if (!ProtocolDouble.TryParse(sources[positions[i]], out param[i])) {
					return false;
				}
			}
			return true;
		}
	}
}
