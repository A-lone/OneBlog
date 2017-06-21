using System;
namespace Microsoft.Security.Application
{
	[Obsolete("This class has been deprecated. Please use Microsoft.Security.Application.Encoder instead.")]
	public static class AntiXss
	{
		[Obsolete("This method has been deprecated. Please use Encoder.HtmlEncode() instead.")]
		public static string HtmlEncode(string input)
		{
			return Encoder.HtmlEncode(input);
		}
		[Obsolete("This method has been deprecated. Please use Encoder.HtmlAttributeEncode() instead.")]
		public static string HtmlAttributeEncode(string input)
		{
			return Encoder.HtmlAttributeEncode(input);
		}
		[Obsolete("This method has been deprecated. Please use Encoder.UrlEncode() instead.")]
		public static string UrlEncode(string input)
		{
			return Encoder.UrlEncode(input);
		}
		[Obsolete("This method has been deprecated. Please use Encoder.UrlEncode() instead.")]
		public static string UrlEncode(string input, int codepage)
		{
			return Encoder.UrlEncode(input, codepage);
		}
		[Obsolete("This method has been deprecated. Please use Encoder.XmlEncode() instead.")]
		public static string XmlEncode(string input)
		{
			return Encoder.XmlEncode(input);
		}
		[Obsolete("This method has been deprecated. Please use Encoder.XmlAttributeEncode() instead.")]
		public static string XmlAttributeEncode(string input)
		{
			return Encoder.XmlAttributeEncode(input);
		}
		[Obsolete("This method has been deprecated. Please use Encoder.JavaScriptEncode() instead.")]
		public static string JavaScriptEncode(string input)
		{
			return Encoder.JavaScriptEncode(input);
		}
		[Obsolete("This method has been deprecated. Please use Encoder.JavaScriptEncode() instead.")]
		public static string JavaScriptEncode(string input, bool flagforQuote)
		{
			return Encoder.JavaScriptEncode(input, flagforQuote);
		}
		[Obsolete("This method has been deprecated. Please use Encoder.VisualBasicScriptEncode() instead.")]
		public static string VisualBasicScriptEncode(string input)
		{
			return Encoder.VisualBasicScriptEncode(input);
		}
	}
}
