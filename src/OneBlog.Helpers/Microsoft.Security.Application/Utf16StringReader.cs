using System;
namespace Microsoft.Security.Application
{
	internal struct Utf16StringReader
	{
		private const char LeadingSurrogateStart = '\ud800';
		private const char TrailingSurrogateStart = '\udc00';
		private const int UnicodeReplacementCharacterCodePoint = 65533;
		private readonly string input;
		private int currentOffset;
		public Utf16StringReader(string input)
		{
			this.input = input;
			this.currentOffset = 0;
		}
		public int ReadNextScalarValue()
		{
			if (this.currentOffset >= this.input.Length)
			{
				return -1;
			}
			char c = this.input[this.currentOffset++];
			int num = (int)c;
			if (char.IsHighSurrogate(c) && this.currentOffset < this.input.Length)
			{
				char c2 = this.input[this.currentOffset];
				if (char.IsLowSurrogate(c2))
				{
					this.currentOffset++;
					num = Utf16StringReader.ConvertToUtf32(c, c2);
				}
			}
			if (Utf16StringReader.IsValidUnicodeScalarValue(num))
			{
				return num;
			}
			return 65533;
		}
		private static int ConvertToUtf32(char leadingSurrogate, char trailingSurrogate)
		{
			return (int)((leadingSurrogate - '\ud800') * 'Ѐ' + (trailingSurrogate - '\udc00')) + 65536;
		}
		private static bool IsValidUnicodeScalarValue(int codePoint)
		{
			return (0 <= codePoint && codePoint <= 55295) || (57344 <= codePoint && codePoint <= 1114111);
		}
	}
}
