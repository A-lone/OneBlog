using System;
using System.Globalization;
using System.Text;
namespace Microsoft.Security.Application
{
	public static class Encoder
	{
		private const string VbScriptEmptyString = "\"\"";
		private const string JavaScriptEmptyString = "''";
		private static readonly char[][] SafeListCodes = Encoder.InitializeSafeList();
		public static string LdapFilterEncode(string input)
		{
			return LdapEncoder.FilterEncode(input);
		}
		public static string LdapDistinguishedNameEncode(string input)
		{
			return Encoder.LdapDistinguishedNameEncode(input, true, true);
		}
		public static string LdapDistinguishedNameEncode(string input, bool useInitialCharacterRules, bool useFinalCharacterRule)
		{
			return LdapEncoder.DistinguishedNameEncode(input, useInitialCharacterRules, useFinalCharacterRule);
		}
		[Obsolete("This method has been deprecated. Please use Encoder.LdapFilterEncode() instead.")]
		public static string LdapEncode(string input)
		{
			return Encoder.LdapFilterEncode(input);
		}
		public static string CssEncode(string input)
		{
			return CssEncoder.Encode(input);
		}
		public static string HtmlEncode(string input)
		{
			return Encoder.HtmlEncode(input, false);
		}
		public static string HtmlEncode(string input, bool useNamedEntities)
		{
			return UnicodeCharacterEncoder.HtmlEncode(input, useNamedEntities);
		}
		public static string HtmlAttributeEncode(string input)
		{
			return UnicodeCharacterEncoder.HtmlAttributeEncode(input);
		}
		public static string UrlEncode(string input)
		{
			return Encoder.UrlEncode(input, Encoding.UTF8);
		}
		public static string HtmlFormUrlEncode(string input)
		{
			return Encoder.HtmlFormUrlEncode(input, Encoding.UTF8);
		}
		public static string UrlEncode(string input, int codePage)
		{
			return Encoder.UrlEncode(input, Encoding.GetEncoding(codePage));
		}
		public static string HtmlFormUrlEncode(string input, int codePage)
		{
			return Encoder.HtmlFormUrlEncode(input, Encoding.GetEncoding(codePage));
		}
		public static string UrlEncode(string input, Encoding inputEncoding)
		{
			if (inputEncoding == null)
			{
				inputEncoding = Encoding.UTF8;
			}
			return HtmlParameterEncoder.QueryStringParameterEncode(input, inputEncoding);
		}
		public static string HtmlFormUrlEncode(string input, Encoding inputEncoding)
		{
			if (inputEncoding == null)
			{
				inputEncoding = Encoding.UTF8;
			}
			return HtmlParameterEncoder.FormStringParameterEncode(input, inputEncoding);
		}
		public static string UrlPathEncode(string input)
		{
			if (string.IsNullOrEmpty(input))
			{
				return input;
			}
			string str;
			string s;
			string str2;
			if (!UriUtil.TrySplitUriForPathEncode(input, out str, out s, out str2))
			{
				str = null;
				UriUtil.ExtractQueryAndFragment(input, out s, out str2);
			}
			return str + HtmlParameterEncoder.UrlPathEncode(s, Encoding.UTF8) + str2;
		}
		public static string XmlEncode(string input)
		{
			return UnicodeCharacterEncoder.XmlEncode(input);
		}
		public static string XmlAttributeEncode(string input)
		{
			return UnicodeCharacterEncoder.XmlAttributeEncode(input);
		}
		public static string JavaScriptEncode(string input)
		{
			return Encoder.JavaScriptEncode(input, true);
		}
		public static string JavaScriptEncode(string input, bool emitQuotes)
		{
			if (!string.IsNullOrEmpty(input))
			{
				int length = 0;
				int length2 = input.Length;
				char[] array = new char[length2 * 8];
				if (emitQuotes)
				{
					array[length++] = '\'';
				}
				for (int i = 0; i < length2; i++)
				{
					int num = (int)input[i];
					char c = input[i];
					if (Encoder.SafeListCodes[num] != null || num == 92 || (num >= 123 && num <= 127))
					{
						if (num >= 127)
						{
							array[length++] = '\\';
							array[length++] = 'u';
							int num2 = (int)c;
							string text = num2.ToString("x", CultureInfo.InvariantCulture).PadLeft(4, '0');
							array[length++] = text[0];
							array[length++] = text[1];
							array[length++] = text[2];
							array[length++] = text[3];
						}
						else
						{
							array[length++] = '\\';
							array[length++] = 'x';
							int num3 = (int)c;
							string text2 = num3.ToString("x", CultureInfo.InvariantCulture).PadLeft(2, '0');
							array[length++] = text2[0];
							array[length++] = text2[1];
						}
					}
					else
					{
						array[length++] = input[i];
					}
				}
				if (emitQuotes)
				{
					array[length++] = '\'';
				}
				return new string(array, 0, length);
			}
			if (!emitQuotes)
			{
				return string.Empty;
			}
			return "''";
		}
		public static string VisualBasicScriptEncode(string input)
		{
			if (string.IsNullOrEmpty(input))
			{
				return "\"\"";
			}
			int num = 0;
			int length = input.Length;
			char[] array = new char[length * 12];
			bool flag = false;
			for (int i = 0; i < length; i++)
			{
				int num2 = (int)input[i];
				char c = input[i];
				if (Encoder.SafeListCodes[num2] != null)
				{
					if (flag)
					{
						array[num++] = '"';
						flag = false;
					}
					string text = "&chrw(" + (uint)c + ")";
					string text2 = text;
					for (int j = 0; j < text2.Length; j++)
					{
						char c2 = text2[j];
						array[num++] = c2;
					}
				}
				else
				{
					if (!flag)
					{
						array[num++] = '&';
						array[num++] = '"';
						flag = true;
					}
					array[num++] = input[i];
				}
			}
			if (flag)
			{
				array[num++] = '"';
			}
			if (array.Length > 0 && array[0] == '&')
			{
				return new string(array, 1, num - 1);
			}
			return new string(array, 0, num);
		}
		private static char[][] InitializeSafeList()
		{
			char[][] array = new char[65536][];
			for (int i = 0; i < array.Length; i++)
			{
				if ((i >= 97 && i <= 122) || (i >= 65 && i <= 90) || (i >= 48 && i <= 57) || (i == 32 || i == 46 || i == 44 || i == 45 || i == 95 || (i >= 256 && i <= 591)) || (i >= 880 && i <= 2047) || (i >= 2304 && i <= 6319) || (i >= 6400 && i <= 6687) || (i >= 6912 && i <= 7039) || (i >= 7680 && i <= 8191) || (i >= 11264 && i <= 11743) || (i >= 12352 && i <= 12591) || (i >= 12688 && i <= 12735) || (i >= 12784 && i <= 12799) || (i >= 19968 && i <= 40899) || (i >= 40960 && i <= 42191) || (i >= 42784 && i <= 43055) || (i >= 43072 && i <= 43135) || (i >= 44032 && i <= 55215))
				{
					array[i] = null;
				}
				else
				{
					string text = i.ToString(CultureInfo.InvariantCulture);
					int length = text.Length;
					char[] array2 = new char[length];
					for (int j = 0; j < length; j++)
					{
						array2[j] = text[j];
					}
					array[i] = array2;
				}
			}
			return array;
		}
	}
}
