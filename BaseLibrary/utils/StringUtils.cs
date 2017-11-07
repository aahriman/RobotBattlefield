using System;

namespace BaseLibrary.utils {
    public static class StringUtils {
        public static bool GetRestOfString(string str, string start, string end, out string rest) {
            if(str.StartsWith(start) && str.EndsWith(end)){
				rest = str.Substring(start.Length, str.Length - end.Length - start.Length);
                return true;
            }else{
                rest = "";
                return false;
            }
        }

        public static bool GetRestOfStringSplitted(string str, string start, string end, out string [] rest, params char[] separator) {
            if (GetRestOfString(str, start, end, out string restStr)) {
				rest = restStr.Split(separator, StringSplitOptions.None);
                return true;
            } else {
                rest = null;
                return false;
            }
        }

		public static bool GetRestOfStringSplitted(string str, string start, string end, out string[] rest, params string[] separator) {
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
