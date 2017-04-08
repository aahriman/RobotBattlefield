﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using BaseLibrary.utils.protocolV1_0Utils;

namespace BaseLibrary.utils {
	namespace protocolV1_0Utils {
		/// <summary>
		/// Should implement method with following definition
		/// <code>static bool Deserialize(String orig, ProtocolV1_0Utils.Deep deep, out object deserialized)</code>
		/// </summary>
		public interface InnerSerializerV1_0 {
			string Serialize(Deep deep);
		}

		public class Deep {
            public static readonly Deep _8 = new Deep('*', null, _7);
            public static readonly Deep _7 = new Deep('$', _8, _6);
            public static readonly Deep _6 = new Deep('@', _7, _5);
            public static readonly Deep _5 = new Deep('#', _6, _4);
			public static readonly Deep _4 = new Deep('!', _5, _3);
			public static readonly Deep _3 = new Deep('\\', _4, _2);
			public static readonly Deep _2 = new Deep('/', _3, _1);
			public static readonly Deep _1 = new Deep('|', _2, _0);
			public static readonly Deep _0 = new Deep(';', _1, null);

		    public readonly Deep PREV;
		    public readonly Deep NEXT;
		    public readonly char SEPARATOR;
		    public readonly string SEPARATOR_STRING;

			private Deep(char separator, Deep next, Deep prev) {
				SEPARATOR = separator;
				SEPARATOR_STRING = "" + separator;
				NEXT = next;
			    PREV = prev;
			}

		}
	}

	public static class ProtocolV1_0Utils {

		public static Deep DEFAULT = Deep._0;
        public static Deep LAST = Deep._7;

		public const string DICTIONARY_KEY_VALUE_SEPARATOR = "=>";

	    public static String ConvertToDeeper(String serializedString, Deep toDeep) {
	        StringBuilder sb = new StringBuilder();
	        char[] serializedCharacter = serializedString.ToCharArray();

            for (int i = 0; i < serializedCharacter.Length; i++) {
                Deep replaceDeep = toDeep;
                Deep replacedDeep = DEFAULT;
                while (replaceDeep != null) {

                    if (serializedCharacter[i] == replacedDeep.SEPARATOR) {
                        sb.Append(replaceDeep.SEPARATOR);
                        goto nextCharacter;
                    }

                    replaceDeep = replaceDeep.NEXT;
                    replacedDeep = replacedDeep.NEXT;
                }
                sb.Append(serializedCharacter[i]);
                nextCharacter:
                ;
            }
	        return sb.ToString();
	    }

	    public static String ConvertToShallowly(String serializedString) {
            StringBuilder sb = new StringBuilder();
            char[] serializedCharacter = serializedString.ToCharArray();

            for (int i = 0; i < serializedCharacter.Length; i++) {
                Deep replacedDeep = LAST;
                while (replacedDeep != DEFAULT) {
                    if (serializedCharacter[i] == replacedDeep.SEPARATOR) {
                        sb.Append(replacedDeep.PREV.SEPARATOR);
                        goto nextCharacter;
                    }
                    replacedDeep = replacedDeep.PREV;
                }
                sb.Append(serializedCharacter[i]);
                nextCharacter:
                ;
            }
            return sb.ToString();
        }

        // =========== PARAMS ========
        public static String SerializeParams(String commandName, params object[] param) {
			return SerializeParams(commandName, DEFAULT, param);
		}

		public static String SerializeParams(String commandName, Deep deep, params object[] param) {
			StringBuilder s = new StringBuilder();
			s.Append(commandName);
			s.Append("(");
			for (int i = 0; i < param.Length; i++) {
				if(i > 0){
					s.Append(deep.SEPARATOR);
				}
				object o = param[i];
				s.Append(serialize(o, deep));
			}
			return s.Append(")").ToString();
		}

		public static bool GetParams(String orig, String commandName, out string rest) {
			return StringUtils.GetRestOfString(orig, commandName + "(", ")", out rest);
		}

        public static bool GetParams(String orig, String commandName, out string[] rest) {
			return GetParams(orig, commandName, DEFAULT, out rest);
		}

		public static bool GetParams(String orig, String commandName, Deep deep, out string[] rest) {
			return StringUtils.GetRestOfStringSplited(orig, commandName + "(", ")", out rest, deep.SEPARATOR);
		}

		// =========== ARRAY ========
		public static String SerializeArray(IEnumerable enumerable, Deep deep) {
			StringBuilder s = new StringBuilder();
			s.Append("[");
			foreach (var item in enumerable) {
				InnerSerializerV1_0 innerSerializable = item as InnerSerializerV1_0;
				if (innerSerializable != null) {
					s.Append(innerSerializable.Serialize(deep.NEXT));
				} else {
					s.Append(item);
				}
				s.Append(deep.SEPARATOR);
			}
			if (s.Length > 1) {
				s.Remove(s.Length - 1, 1); // remove last separator
			}
			return s.Append("]").ToString();
		}

		public static String Serialize(IEnumerable enumerable, Deep deep) {
			StringBuilder s = new StringBuilder();
			s.Append("[");
			foreach (var item in enumerable) {
				s.Append(serialize(item, deep));
				s.Append(deep.SEPARATOR);
			}
			if (s.Length > 1) { 
				s.Remove(s.Length - 1, 1); // remove last separator
			}
			return s.Append("]").ToString();
		}

		public static bool Deserialize(String s, out string[] array, Deep deep) {
			return StringUtils.GetRestOfStringSplited(s, "[", "]", out array, deep.SEPARATOR);
		}

		public static bool Deserialize(String s, out Double[] array, Deep deep) {
			string[] rest;
			if (StringUtils.GetRestOfStringSplited(s, "[", "]", out rest, deep.SEPARATOR)) {
				array = new Double[rest.Length];
				for (int i = 0; i < rest.Length; i++) {
					if (!Double.TryParse(rest[i], out array[i])) {
						return false;
					}
				}
				return true;
			} else {
				array = null;
				return false;
			}
		}

		public static bool Deserialize(String s, out int[] array, Deep deep) {
			string[] rest;
			if (StringUtils.GetRestOfStringSplited(s, "[", "]", out rest, deep.SEPARATOR)) {
				array = new int[rest.Length];
				for (int i = 0; i < rest.Length; i++) {
					if (!int.TryParse(rest[i], out array[i])) {
						return false;
					}
				}
				return true;
			} else {
				array = null;
				return false;
			}
		}

		// =========== DICTIONARY ========
		public static String Serialize(IDictionary dict, Deep deep) {
			List<string> serializeList = new List<string>();
			foreach (var key in dict.Keys) {
				serializeList.Add(serialize(key, deep) + DICTIONARY_KEY_VALUE_SEPARATOR + serialize(dict[key], deep));
			}
			return SerializeArray(serializeList, deep);
		}

		private static bool preDeserializeDict(string s, out KeyValuePair<string, string>[] dict, Deep deep) {
			String[] rest;
			if (Deserialize(s, out rest, deep)) {
				dict = new KeyValuePair<string, string>[rest.Length];
				string[] keyValue;
				for (int i = 0; i < dict.Length; i++) {
					if (StringUtils.GetRestOfStringSplited(rest[i], String.Empty, String.Empty, out keyValue, DICTIONARY_KEY_VALUE_SEPARATOR)) {
						if (keyValue.Length == 2) {
							dict[i] = new KeyValuePair<string, string>(keyValue[0], keyValue[1]);
						} else {
							throw new ArgumentException(String.Format("S containt illegal dictionary pair '{0}'", rest[i]));
						}
					}
				}
				return true;
			} else {
				dict = null;
				return false;
			}
		}

		public static bool Deserialize(string s, IDictionary<int, int> dict, Deep deep) {
			KeyValuePair<string, string>[] keyValues;
			if (preDeserializeDict(s, out keyValues, deep)) {
				foreach (var keyValue in keyValues) {
					int key;
					int value;
					if (int.TryParse(keyValue.Key, out key) &&
						int.TryParse(keyValue.Value, out value)) {
						dict.Add(key, value);
					} else {
						throw new ArgumentException(String.Format("String '{0}' constaint illegal values for dict<{1},{2}>.", keyValue, dict.GetType().GenericTypeArguments[0].Name, dict.GetType().GenericTypeArguments[1].Name));
					}
				}
				return true;
			} else {
				dict = null;
				return false;
			}
		}

		public static bool Deserialize(string s, IDictionary<int, double> dict, Deep deep) {
			KeyValuePair<string, string>[] keyValues;
			if (preDeserializeDict(s, out keyValues, deep)) {
				foreach (var keyValue in keyValues) {
					int key;
					double value;
					if (int.TryParse(keyValue.Key, out key) &&
						double.TryParse(keyValue.Value, out value)) {
						dict.Add(key, value);
					} else {
						throw new ArgumentException(String.Format("String '{0}' constaint illegal values for dict<{1},{2}>.", keyValue, dict.GetType().GenericTypeArguments[0].Name, dict.GetType().GenericTypeArguments[1].Name));
					}
				}
				return true;
			} else {
				dict = null;
				return false;
			}
		}


		public static bool Deserialize(string s, IDictionary<double, int> dict, Deep deep) {
			KeyValuePair<string, string>[] keyValues;
			if (preDeserializeDict(s, out keyValues, deep)) {
				foreach (var keyValue in keyValues) {
					double key;
					int value;
					if (double.TryParse(keyValue.Key, out key) &&
						int.TryParse(keyValue.Value, out value)) {
						dict.Add(key, value);
					} else {
						throw new ArgumentException(String.Format("String '{0}' constaint illegal values for dict<{1},{2}>.", keyValue, dict.GetType().GenericTypeArguments[0].Name, dict.GetType().GenericTypeArguments[1].Name));
					}
				}
				return true;
			} else {
				dict = null;
				return false;
			}
		}

		public static bool Deserialize(string s, IDictionary<double, double> dict, Deep deep) {
			KeyValuePair<string, string>[] keyValues;
			if (preDeserializeDict(s, out keyValues, deep)) {
				foreach (var keyValue in keyValues) {
					double key;
					double value;
					if (double.TryParse(keyValue.Key, out key) &&
						double.TryParse(keyValue.Value, out value)) {
						dict.Add(key, value);
					} else {
						throw new ArgumentException(String.Format("String '{0}' constaint illegal values for dict<{1},{2}>.", keyValue, dict.GetType().GenericTypeArguments[0].Name, dict.GetType().GenericTypeArguments[1].Name));
					}
				}
				return true;
			} else {
				dict = null;
				return false;
			}
		}

		public static bool Deserialize(string s, IDictionary<int, string> dict, Deep deep) {
			KeyValuePair<string, string>[] keyValues;
			if (preDeserializeDict(s, out keyValues, deep)) {
				foreach (var keyValue in keyValues) {
					int key;
					if (int.TryParse(keyValue.Key, out key)) {
						dict.Add(key, keyValue.Value);
					} else {
						throw new ArgumentException(String.Format("String '{0}' constaint illegal values for dict<{1},{2}>.", keyValue, dict.GetType().GenericTypeArguments[0].Name, dict.GetType().GenericTypeArguments[1].Name));
					}
				}
				return true;
			} else {
				dict = null;
				return false;
			}
		}

		public static bool Deserialize(string s, IDictionary<double, string> dict, Deep deep) {
			KeyValuePair<string, string>[] keyValues;
			if (preDeserializeDict(s, out keyValues, deep)) {
				foreach (var keyValue in keyValues) {
					double key;
					if (double.TryParse(keyValue.Key, out key)) {
						dict.Add(key, keyValue.Value);
					} else {
						throw new ArgumentException(String.Format("String '{0}' constaint illegal values for dict<{1},{2}>.", keyValue, dict.GetType().GenericTypeArguments[0].Name, dict.GetType().GenericTypeArguments[1].Name));
					}
				}
				return true;
			} else {
				dict = null;
				return false;
			}
		}

		public static bool Deserialize(string s, IDictionary<string, int> dict, Deep deep) {
			KeyValuePair<string, string>[] keyValues;
			if (preDeserializeDict(s, out keyValues, deep)) {
				foreach (var keyValue in keyValues) {
					int value;
					if (int.TryParse(keyValue.Value, out value)) {
						dict.Add(keyValue.Key, value);
					} else {
						throw new ArgumentException(String.Format("String '{0}' constaint illegal values for dict<{1},{2}>.", keyValue, dict.GetType().GenericTypeArguments[0].Name, dict.GetType().GenericTypeArguments[1].Name));
					}
				}
				return true;
			} else {
				dict = null;
				return false;
			}
		}

		public static bool Deserialize(string s, IDictionary<string, double> dict, Deep deep) {
			KeyValuePair<string, string>[] keyValues;
			if (preDeserializeDict(s, out keyValues, deep)) {
				foreach (var keyValue in keyValues) {
					double value;
					if (double.TryParse(keyValue.Value, out value)) {
						dict.Add(keyValue.Key, value);
					} else {
						throw new ArgumentException(String.Format("String '{0}' constaint illegal values for dict<{1},{2}>.", keyValue, dict.GetType().GenericTypeArguments[0].Name, dict.GetType().GenericTypeArguments[1].Name));
					}
				}
				return true;
			} else {
				dict = null;
				return false;
			}
		}
		public static bool Deserialize(string s, IDictionary<string, string> dict, Deep deep) {
			KeyValuePair<string, string>[] keyValues;
			if (preDeserializeDict(s, out keyValues, deep)) {
				foreach (var keyValue in keyValues) {
					dict.Add(keyValue);
				}
				return true;
			} else {
				dict = null;
				return false;
			}
		}

		private static String serialize(object o, Deep d) {
			IDictionary dictionary = o as IDictionary;
			IEnumerable enumerable = o as IEnumerable;
            String asString = o as String;
            InnerSerializerV1_0 innerSerializable = o as InnerSerializerV1_0;
			if (innerSerializable != null) {
				return innerSerializable.Serialize(d.NEXT);
			} else if (dictionary != null) {
				return Serialize(dictionary, d.NEXT);
			} else if (enumerable != null) {
			    if (asString != null) {
			        return asString;
			    } else {
			        return Serialize(enumerable, d.NEXT);
			    }
			} else {
			    if (o == null) {
			        return String.Empty;
			    }
				return o.ToString();
			}
		}
	}
}
