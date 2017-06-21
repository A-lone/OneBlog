using Microsoft.Exchange.Data.TextConverters;
using System;
using System.IO;
namespace Microsoft.Security.Application
{
    public static class Sanitizer
    {
        public static string GetSafeHtml(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }
            string result;
            using (TextReader textReader = new StringReader(input))
            {
                using (TextWriter textWriter = new StringWriter())
                {
                    HtmlToHtml htmlToHtml = new HtmlToHtml
                    {
                        FilterHtml = true,
                        OutputHtmlFragment = false,
                        NormalizeHtml = true
                    };
                    htmlToHtml.Convert(textReader, textWriter);
                    result = ((textWriter.ToString().Length != 0) ? textWriter.ToString() : string.Empty);
                }
            }
            return result;
        }
        public static string GetSafeHtmlFragment(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }
            string result;
            using (TextReader textReader = new StringReader(input))
            {
                using (TextWriter textWriter = new StringWriter())
                {
                    HtmlToHtml htmlToHtml = new HtmlToHtml
                    {
                        FilterHtml = true,
                        OutputHtmlFragment = true,
                        NormalizeHtml = true
                    };
                    htmlToHtml.Convert(textReader, textWriter);
                    if (textWriter.ToString().Length == 0)
                    {
                        result = string.Empty;
                    }
                    else
                    {
                        string text = textWriter.ToString();
                        if (text.Substring(0, 5).ToLower() == "<div>")
                        {
                            text = text.Substring(5);
                            text = text.Substring(0, text.Length - 8);
                        }
                        result = text;
                    }
                }
            }
            return result;
        }
        public static void GetSafeHtml(TextReader sourceReader, TextWriter destinationWriter)
        {
            HtmlToHtml htmlToHtml = new HtmlToHtml
            {
                FilterHtml = true,
                OutputHtmlFragment = false,
                NormalizeHtml = true
            };
            htmlToHtml.Convert(sourceReader, destinationWriter);
        }
        public static void GetSafeHtml(TextReader sourceReader, Stream destinationStream)
        {
            HtmlToHtml htmlToHtml = new HtmlToHtml
            {
                FilterHtml = true,
                OutputHtmlFragment = false,
                NormalizeHtml = true
            };
            htmlToHtml.Convert(sourceReader, destinationStream);
        }
        public static void GetSafeHtmlFragment(TextReader sourceReader, TextWriter destinationWriter)
        {
            HtmlToHtml htmlToHtml = new HtmlToHtml
            {
                FilterHtml = true,
                OutputHtmlFragment = true,
                NormalizeHtml = true
            };
            htmlToHtml.Convert(sourceReader, destinationWriter);
        }
        public static void GetSafeHtmlFragment(TextReader sourceReader, Stream destinationStream)
        {
            HtmlToHtml htmlToHtml = new HtmlToHtml
            {
                FilterHtml = true,
                OutputHtmlFragment = true,
                NormalizeHtml = true
            };
            htmlToHtml.Convert(sourceReader, destinationStream);
        }
    }
}
