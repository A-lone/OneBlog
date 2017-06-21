using System;
using System.Collections;
using System.Text;
namespace Microsoft.Security.Application
{
	internal static class HtmlParameterEncoder
	{
		private static readonly char[] QueryStringSpace = "%20".ToCharArray();
		private static readonly char[] FormStringSpace = "+".ToCharArray();
		private static Lazy<char[][]> characterValuesLazy = new Lazy<char[][]>(new Func<char[][]>(HtmlParameterEncoder.InitialiseSafeList));
		private static Lazy<char[][]> pathCharacterValuesLazy = new Lazy<char[][]>(new Func<char[][]>(HtmlParameterEncoder.InitialisePathSafeList));
		internal static string QueryStringParameterEncode(string s, Encoding encoding)
		{
			return HtmlParameterEncoder.FormQueryEncode(s, encoding, EncodingType.QueryString);
		}
		internal static string FormStringParameterEncode(string s, Encoding encoding)
		{
			return HtmlParameterEncoder.FormQueryEncode(s, encoding, EncodingType.HtmlForm);
		}
		internal static string UrlPathEncode(string s, Encoding encoding)
		{
			return HtmlParameterEncoder.FormQueryEncode(s, encoding, EncodingType.QueryString, HtmlParameterEncoder.pathCharacterValuesLazy);
		}
		private static string FormQueryEncode(string s, Encoding encoding, EncodingType encodingType)
		{
			return HtmlParameterEncoder.FormQueryEncode(s, encoding, encodingType, HtmlParameterEncoder.characterValuesLazy);
		}
		private static string FormQueryEncode(string s, Encoding encoding, EncodingType encodingType, Lazy<char[][]> characterValues)
		{
			if (string.IsNullOrEmpty(s))
			{
				return s;
			}
			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}
			byte[] bytes = encoding.GetBytes(s.ToCharArray());
			char[] array = new char[bytes.Length * 3];
			int length = 0;
			char[][] value = characterValues.Value;
			for (int i = 0; i < bytes.Length; i++)
			{
				byte b = bytes[i];
				if (b == 0 || b == 32 || (int)b > value.Length || value[(int)b] != null)
				{
					char[] array2;
					if (b == 32)
					{
						switch (encodingType)
						{
						case EncodingType.QueryString:
							array2 = HtmlParameterEncoder.QueryStringSpace;
							break;
						case EncodingType.HtmlForm:
							array2 = HtmlParameterEncoder.FormStringSpace;
							break;
						default:
							throw new ArgumentOutOfRangeException("encodingType");
						}
					}
					else
					{
						array2 = value[(int)b];
					}
					for (int j = 0; j < array2.Length; j++)
					{
						array[length++] = array2[j];
					}
				}
				else
				{
					array[length++] = (char)b;
				}
			}
			return new string(array, 0, length);
		}
		private static char[][] InitialiseSafeList()
		{
			char[][] result = SafeList.Generate(255, new SafeList.GenerateSafeValue(SafeList.PercentThenHexValueGenerator));
			SafeList.PunchSafeList(ref result, HtmlParameterEncoder.UrlParameterSafeList());
			return result;
		}
		private static IEnumerable UrlParameterSafeList()
		{
			yield return 45;
			yield return 46;
			for (int i = 48; i <= 57; i++)
			{
				yield return i;
			}
			for (int j = 65; j <= 90; j++)
			{
				yield return j;
			}
			yield return 95;
			for (int k = 97; k <= 122; k++)
			{
				yield return k;
			}
			yield return 126;
			yield break;
		}
		private static char[][] InitialisePathSafeList()
		{
			char[][] result = SafeList.Generate(255, new SafeList.GenerateSafeValue(SafeList.PercentThenHexValueGenerator));
			SafeList.PunchSafeList(ref result, HtmlParameterEncoder.UrlPathSafeList());
			return result;
		}
		private static IEnumerable UrlPathSafeList()
		{
			foreach (object current in HtmlParameterEncoder.UrlParameterSafeList())
			{
				yield return current;
			}
			yield return 35;
			yield return 37;
			yield return 47;
			yield return 92;
			yield return 40;
			yield return 41;
			yield break;
		}
	}
}
