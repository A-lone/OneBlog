using System;
using System.Collections;
using System.Text;
namespace Microsoft.Security.Application
{
	internal static class LdapEncoder
	{
		private static Lazy<char[][]> filterCharacterValuesLazy = new Lazy<char[][]>(new Func<char[][]>(LdapEncoder.InitialiseFilterSafeList));
		private static Lazy<char[][]> distinguishedNameCharacterValuesLazy = new Lazy<char[][]>(new Func<char[][]>(LdapEncoder.InitialiseDistinguishedNameSafeList));
		internal static string FilterEncode(string input)
		{
			if (string.IsNullOrEmpty(input))
			{
				return input;
			}
			char[][] value = LdapEncoder.filterCharacterValuesLazy.Value;
			byte[] bytes = Encoding.UTF8.GetBytes(input.ToCharArray());
			char[] array = new char[bytes.Length * 3];
			int length = 0;
			for (int i = 0; i < bytes.Length; i++)
			{
				byte b = bytes[i];
				if (value[(int)b] != null)
				{
					char[] array2 = value[(int)b];
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
		internal static string DistinguishedNameEncode(string input, bool useInitialCharacterRules, bool useFinalCharacterRule)
		{
			if (string.IsNullOrEmpty(input))
			{
				return input;
			}
			char[][] value = LdapEncoder.distinguishedNameCharacterValuesLazy.Value;
			byte[] bytes = Encoding.UTF8.GetBytes(input.ToCharArray());
			char[] array = new char[bytes.Length * 3];
			int length = 0;
			for (int i = 0; i < bytes.Length; i++)
			{
				byte b = bytes[i];
				if (i == 0 && b == 32 && useInitialCharacterRules)
				{
					array[length++] = '\\';
					array[length++] = ' ';
				}
				else
				{
					if (i == 0 && b == 35 && useInitialCharacterRules)
					{
						array[length++] = '\\';
						array[length++] = '#';
					}
					else
					{
						if (i == bytes.Length - 1 && b == 32 && useFinalCharacterRule)
						{
							array[length++] = '\\';
							array[length++] = ' ';
						}
						else
						{
							if (value[(int)b] != null)
							{
								char[] array2 = value[(int)b];
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
					}
				}
			}
			return new string(array, 0, length);
		}
		private static char[][] InitialiseFilterSafeList()
		{
			char[][] result = SafeList.Generate(255, new SafeList.GenerateSafeValue(SafeList.SlashThenHexValueGenerator));
			SafeList.PunchSafeList(ref result, LdapEncoder.FilterEncodingSafeList());
			return result;
		}
		private static IEnumerable FilterEncodingSafeList()
		{
			for (int i = 32; i <= 126; i++)
			{
				if (i != 40 && i != 41 && i != 42 && i != 47 && i != 92)
				{
					yield return i;
				}
			}
			yield break;
		}
		private static char[][] InitialiseDistinguishedNameSafeList()
		{
			char[][] result = SafeList.Generate(255, new SafeList.GenerateSafeValue(SafeList.HashThenHexValueGenerator));
			SafeList.PunchSafeList(ref result, LdapEncoder.DistinguishedNameSafeList());
			LdapEncoder.EscapeDistinguisedNameCharacter(ref result, ',');
			LdapEncoder.EscapeDistinguisedNameCharacter(ref result, '+');
			LdapEncoder.EscapeDistinguisedNameCharacter(ref result, '"');
			LdapEncoder.EscapeDistinguisedNameCharacter(ref result, '\\');
			LdapEncoder.EscapeDistinguisedNameCharacter(ref result, '<');
			LdapEncoder.EscapeDistinguisedNameCharacter(ref result, '>');
			LdapEncoder.EscapeDistinguisedNameCharacter(ref result, ';');
			return result;
		}
		private static IEnumerable DistinguishedNameSafeList()
		{
			for (int i = 32; i <= 126; i++)
			{
				if (i != 44 && i != 43 && i != 34 && i != 92 && i != 60 && i != 62 && i != 38 && i != 33 && i != 124 && i != 61 && i != 45 && i != 39 && i != 59)
				{
					yield return i;
				}
			}
			yield break;
		}
		private static void EscapeDistinguisedNameCharacter(ref char[][] safeList, char c)
		{
			safeList[(int)c] = new char[]
			{
				'\\',
				c
			};
		}
	}
}
