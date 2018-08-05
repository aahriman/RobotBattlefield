using System;

namespace BaseLibrary.utils {
    public static class StringUtils {
        /// <summary>
        /// Set <code>rest</code> to string between <code>start</code> and <code>end</code> in <code>str</code>. <code>Str</code> have to start with string <code>start</code> and end with string <code>end</code>.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="rest"></param>
        /// <returns>true - if in <code>rest</code> is valid data, false - otherwise</returns>
        public static bool GetRestOfString(string str, string start, string end, out string rest) {
            if(str.StartsWith(start) && str.EndsWith(end)){
				rest = str.Substring(start.Length, str.Length - end.Length - start.Length);
                return true;
            }else{
                rest = "";
                return false;
            }
        }

        /// <summary>
        /// Truncate <code>str</code> by <code>start</code> and <code>end</code> and then it split by separator.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="rest"></param>
        /// <param name="separator"></param>
        /// <returns>true - if in <code>rest</code> is valid data, false - otherwise</returns>
        public static bool GetRestOfStringSplit(string str, string start, string end, out string [] rest, params char[] separator) {
            if (GetRestOfString(str, start, end, out string restStr)) {
				rest = restStr.Split(separator, StringSplitOptions.None);
                return true;
            } else {
                rest = null;
                return false;
            }
        }

        /// <summary>
        /// Truncate <code>str</code> by <code>start</code> and <code>end</code> and then it split by separator.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="rest"></param>
        /// <param name="separator"></param>
        /// <returns>true - if in <code>rest</code> is valid data, false - otherwise</returns>
        public static bool GetRestOfStringSplit(string str, string start, string end, out string[] rest, params string[] separator) {
		    if (GetRestOfString(str, start, end, out string restStr)) {
				rest = restStr.Split(separator, StringSplitOptions.None);
				return true;
			} else {
				rest = null;
				return false;
			}
		}
    }
}
