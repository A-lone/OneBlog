using System;
using System.Text;
namespace Microsoft.Security.Application
{
	internal static class EncoderUtil
	{
		internal static StringBuilder GetOutputStringBuilder(int inputLength, int worstCaseOutputCharsPerInputChar)
		{
			int capacity;
			if (inputLength >= 16384)
			{
				capacity = inputLength;
			}
			else
			{
				long val = (long)inputLength * (long)worstCaseOutputCharsPerInputChar;
				capacity = (int)Math.Min(16384L, val);
			}
			return new StringBuilder(capacity);
		}
	}
}
