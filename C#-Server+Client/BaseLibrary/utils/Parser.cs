using System.Collections.Generic;
using System.Linq;
using BaseLibrary.communication.protocol;

namespace BaseLibrary.utils {
	public static class Parser {

        /// <summary>
        /// Try to convert every element in source to int. If one of them cannot be converted return false. Otherwise it return true and in param is every element converted. 
        /// </summary>
        /// <param name="sources"></param>
        /// <param name="param"></param>
        /// <returns></returns>
		public static bool TryParse(ICollection<string> sources, out int[] param) {
			param = new int[sources.Count];
			int i = 0;
			foreach (var source in sources) {
				if (!int.TryParse(source, out param[i++])) {
					return false;
				}
			}
			return true;
		}

	    /// <summary>
	    /// Try to convert every element in source to double. If one of them cannot be converted return false. Otherwise it return true and in param is every element converted. 
	    /// </summary>
	    /// <param name="sources"></param>
	    /// <param name="param"></param>
	    /// <returns></returns>
		public static bool TryParse(ICollection<string> sources, out double[] param) {
			param = new double[sources.Count];

            int i = 0;
			foreach (var source in sources) {
				if (!double.TryParse(source, out param[i++])) {
					return false;
				}
			}
			return true;
		}

	    /// <summary>
	    /// Try to convert every element in source to ProtocolDouble. If one of them cannot be converted return false. Otherwise it return true and in param is every element converted. 
	    /// </summary>
	    /// <param name="sources"></param>
	    /// <param name="param"></param>
	    /// <returns></returns>
		public static bool TryParse(ICollection<string> sources, out ProtocolDouble[] param) {
			param = new ProtocolDouble[sources.Count];
			int i = 0;
			foreach (var source in sources) {
				if (!ProtocolDouble.TryParse(source, out param[i++])) {
					return false;
				}
			}
			return true;
		}

	    /// <summary>
	    /// Try to convert every element at position defined in positions array to int. If one of them cannot be converted return false. Otherwise it return true and in param is every element converted. 
	    /// </summary>
	    /// <param name="positions"></param>
	    /// <param name="sources"></param>
	    /// <param name="param"></param>
	    /// <returns></returns>
		public static bool TryParse(int[] positions, IList<string> sources, out int[] param) {
			param = new int[positions.Length];
			
			for (int i = 0; i < positions.Length; i++) {
				if (!int.TryParse(sources[positions[i]], out param[i])) {
					return false;
				}
			}
			return true;
		}

	    /// <summary>
	    /// Try to convert every element at position defined in positions array to double. If one of them cannot be converted return false. Otherwise it return true and in param is every element converted. 
	    /// </summary>
	    /// <param name="positions"></param>
	    /// <param name="sources"></param>
	    /// <param name="param"></param>
	    /// <returns></returns>
		public static bool TryParse(int[] positions, IList<string> sources, out double[] param) {
			param = new double[positions.Length];

			for (int i = 0; i < positions.Length; i++) {
				if (!double.TryParse(sources[positions[i]], out param[i])) {
					return false;
				}
			}
			return true;
		}

	    /// <summary>
	    /// Try to convert every element at position defined in positions array to ProtocolDouble. If one of them cannot be converted return false. Otherwise it return true and in param is every element converted. 
	    /// </summary>
	    /// <param name="positions"></param>
	    /// <param name="sources"></param>
	    /// <param name="param"></param>
	    /// <returns></returns>
		public static bool TryParse(int[] positions, IList<string> sources, out ProtocolDouble[] param) {
			param = new ProtocolDouble[positions.Length];

			for (int i = 0; i < positions.Length; i++) {
				if (!ProtocolDouble.TryParse(sources[positions[i]], out param[i])) {
					return false;
				}
			}
			return true;
		}

	    /// <summary>
	    /// Try to convert every element at position defined in positions array to int. If one of them cannot be converted return false. Otherwise it return true and in param is every element converted. 
	    /// </summary>
	    /// <param name="positions"></param>
	    /// <param name="sources"></param>
	    /// <param name="param"></param>
	    /// <returns></returns>
        public static bool TryParse(int[] positions, string[] sources, out int[] param) {
			param = new int[positions.Length];

			for (int i = 0; i < positions.Length; i++) {
				if (!int.TryParse(sources[positions[i]], out param[i])) {
					return false;
				}
			}
			return true;
		}

	    /// <summary>
	    /// Try to convert every element at position defined in positions array to double. If one of them cannot be converted return false. Otherwise it return true and in param is every element converted. 
	    /// </summary>
	    /// <param name="positions"></param>
	    /// <param name="sources"></param>
	    /// <param name="param"></param>
	    /// <returns></returns>
        public static bool TryParse(int[] positions, string[] sources, out double[] param) {
			param = new double[positions.Length];

			for (int i = 0; i < positions.Length; i++) {
				if (!double.TryParse(sources[positions[i]], out param[i])) {
					return false;
				}
			}
			return true;
		}

	    /// <summary>
	    /// Try to convert every element at position defined in positions array to ProtocolDouble. If one of them cannot be converted return false. Otherwise it return true and in param is every element converted. 
	    /// </summary>
	    /// <param name="positions"></param>
	    /// <param name="sources"></param>
	    /// <param name="param"></param>
	    /// <returns></returns>
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
