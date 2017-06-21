using System;
using System.IO;
using System.Text;
namespace Microsoft.Security.Application
{
    public class AntiXssEncoder 
	{
		protected virtual void HtmlEncode(string value, TextWriter output)
		{
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			output.Write(Encoder.HtmlEncode(value));
		}
		protected virtual void HtmlAttributeEncode(string value, TextWriter output)
		{
			if (output == null)
			{
				throw new ArgumentNullException("output");
			}
			output.Write(Encoder.HtmlAttributeEncode(value));
		}
		protected virtual byte[] UrlEncode(byte[] bytes, int offset, int count)
		{
			if (count == 0)
			{
				return null;
			}
			if (bytes == null || bytes.Length == 0)
			{
				throw new ArgumentNullException("bytes");
			}
			if (offset < 0 || offset > bytes.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (count < 0 || offset + count > bytes.Length)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			string @string = Encoding.UTF8.GetString(bytes, offset, count);
			string s = Encoder.UrlEncode(@string, Encoding.UTF8);
			return Encoding.UTF8.GetBytes(s);
		}
		protected virtual string UrlPathEncode(string value)
		{
			return Encoder.UrlPathEncode(value);
		}
	}
}
