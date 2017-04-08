using System;

namespace CommunicationLibrary.utils {
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

        public static bool GetRestOfStringSplited(string str, string start, string end, out string [] rest, params char[] separator) {
            string restStr;
            if (GetRestOfString(str, start, end, out restStr)) {
				rest = restStr.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                return true;
            } else {
                rest = null;
                return false;
            }
        }

		public static bool GetRestOfStringSplited(string str, string start, string end, out string[] rest, params string[] separator) {
			string restStr;
			if (GetRestOfString(str, start, end, out restStr)) {
				rest = restStr.Split(separator, StringSplitOptions.RemoveEmptyEntries);
				return true;
			} else {
				rest = null;
				return false;
			}
		}
    }
}
