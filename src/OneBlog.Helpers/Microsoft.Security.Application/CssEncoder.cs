using System;
using System.Collections;
using System.Text;
namespace Microsoft.Security.Application
{
	internal static class CssEncoder
	{
		private static Lazy<char[][]> characterValuesLazy = new Lazy<char[][]>(new Func<char[][]>(CssEncoder.InitialiseSafeList));
		internal static string Encode(string input)
		{
			if (string.IsNullOrEmpty(input))
			{
				return input;
			}
			char[][] value = CssEncoder.characterValuesLazy.Value;
			StringBuilder outputStringBuilder = EncoderUtil.GetOutputStringBuilder(input.Length, 7);
			Utf16StringReader utf16StringReader = new Utf16StringReader(input);
			while (true)
			{
				int num = utf16StringReader.ReadNextScalarValue();
				if (num < 0)
				{
					break;
				}
				if (num >= value.Length)
				{
					char[] value2 = SafeList.SlashThenSixDigitHexValueGenerator(num);
					outputStringBuilder.Append(value2);
				}
				else
				{
					if (value[num] != null)
					{
						char[] value3 = value[num];
						outputStringBuilder.Append(value3);
					}
					else
					{
						outputStringBuilder.Append((char)num);
					}
				}
			}
			return outputStringBuilder.ToString();
		}
		private static char[][] InitialiseSafeList()
		{
			char[][] result = SafeList.Generate(255, new SafeList.GenerateSafeValue(SafeList.SlashThenSixDigitHexValueGenerator));
			SafeList.PunchSafeList(ref result, CssEncoder.CssSafeList());
			return result;
		}
		private static IEnumerable CssSafeList()
		{
			for (int i = 48; i <= 57; i++)
			{
				yield return i;
			}
			for (int j = 65; j <= 90; j++)
			{
				yield return j;
			}
			for (int k = 97; k <= 122; k++)
			{
				yield return k;
			}
			yield break;
		}
	}
}
