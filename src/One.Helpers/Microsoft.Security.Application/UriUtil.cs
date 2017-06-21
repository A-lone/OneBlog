using System;
namespace Microsoft.Security.Application
{
	internal static class UriUtil
	{
		private static readonly char[] QueryFragmentSeparators = new char[]
		{
			'?',
			'#'
		};
		internal static void ExtractQueryAndFragment(string input, out string path, out string queryAndFragment)
		{
			int num = input.IndexOfAny(UriUtil.QueryFragmentSeparators);
			if (num != -1)
			{
				path = input.Substring(0, num);
				queryAndFragment = input.Substring(num);
				return;
			}
			path = input;
			queryAndFragment = null;
		}
		internal static bool IsSafeScheme(string url)
		{
			return url.IndexOf(":", StringComparison.Ordinal) == -1 || url.StartsWith("http:", StringComparison.OrdinalIgnoreCase) || url.StartsWith("https:", StringComparison.OrdinalIgnoreCase) || url.StartsWith("ftp:", StringComparison.OrdinalIgnoreCase) || url.StartsWith("file:", StringComparison.OrdinalIgnoreCase) || url.StartsWith("news:", StringComparison.OrdinalIgnoreCase);
		}
		internal static bool TrySplitUriForPathEncode(string input, out string schemeAndAuthority, out string path, out string queryAndFragment)
		{
			string text;
			UriUtil.ExtractQueryAndFragment(input, out text, out queryAndFragment);
			Uri uri;
			if (UriUtil.IsSafeScheme(text) && Uri.TryCreate(text, UriKind.Absolute, out uri))
			{
				string authority = uri.Authority;
				if (!string.IsNullOrEmpty(authority))
				{
					int num = text.IndexOf(authority, StringComparison.Ordinal);
					if (num != -1)
					{
						int num2 = num + authority.Length;
						schemeAndAuthority = text.Substring(0, num2);
						path = text.Substring(num2);
						return true;
					}
				}
			}
			schemeAndAuthority = null;
			path = null;
			queryAndFragment = null;
			return false;
		}
	}
}
