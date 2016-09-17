using System;
using System.IO;

namespace System.Web.Mvc
{
    public static class MarkdownDeepExtensions
    {
        public static void RenderMarkdownFile(this HtmlHelper helper, string filename)
        {
            // Load source text
            var text = File.ReadAllText(helper.ViewContext.HttpContext.Server.MapPath(filename));

            // Setup processor
            var md = new MarkdownDeep.Markdown
            {
                SafeMode = false,
                ExtraMode = true,
                AutoHeadingIDs = true,
                MarkdownInHtml = true,
                NewWindowForExternalLinks = true
            };

            // Write it
            helper.ViewContext.HttpContext.Response.Write(md.Transform(text));
        }


        public static IHtmlString RenderMarkdown(this HtmlHelper helper, string text)
        {
            var md = new MarkdownDeep.Markdown
            {
                SafeMode = false,
                ExtraMode = true,
                AutoHeadingIDs = true,
                MarkdownInHtml = true,
                NewWindowForExternalLinks = true
            };
            var markdown = md.Transform(text);
            return helper.Raw(markdown);
        }

    }
}