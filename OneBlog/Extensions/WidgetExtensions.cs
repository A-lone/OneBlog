using OneBlog.Core;
using OneBlog.Core.DataStore;
using System;
using System.Text;
using System.Web.Mvc;
using System.Xml;

namespace System.Web.Mvc
{
    public static class WidgetExtensions
    {
        private static string zoneName = "be_WIDGET_ZONE";

        #region Properties

        /// <summary>
        ///     Gets or sets the name of the data-container used by this instance
        /// </summary>
        public static string ZoneName
        {
            get
            {
                return zoneName;
            }

            set
            {
                zoneName = WebUtils.RemoveIllegalCharacters(value);
            }
        }

        private static XmlDocument XmlDocument
        {
            get
            {
                // look up the document by zone name
                return Blog.CurrentInstance.Cache[ZoneName] == null ? null : (XmlDocument)Blog.CurrentInstance.Cache[ZoneName];
            }
        }

        #endregion
        public static IHtmlString RenderWidget(this HtmlHelper helper)
        {
            if (XmlDocument == null)
            {
                var doc = RetrieveXml(ZoneName);
                if (doc != null)
                {
                    Blog.CurrentInstance.Cache[ZoneName] = doc;
                }
            }
            XmlNodeList zone = null;
            if (XmlDocument != null)
            {
                zone = XmlDocument.SelectNodes("//widget");
            }
            StringBuilder builder = new StringBuilder();
            if (zone != null)
            {
                builder.Append(string.Format("<div id=\"widgetzone_{0}\" class=\"widgetzone\">", zoneName));
                foreach (XmlNode widget in zone)
                {
                    var fileName = string.Format("{0}Custom/Widgets/{1}/widget.cshtml",
                        WebUtils.ApplicationRelativeWebRoot,
                        widget.InnerText);
                    try
                    {
                        var model = new { Id = widget.Attributes["id"].Value, Name = widget.InnerText, Title = widget.Attributes["title"].Value };
                        builder.Append(RazorExtensions.ParseRazor(fileName, model));
                    }
                    catch (Exception ex)
                    {
                        builder.Append(string.Format("<p style=\"color:red\">Widget {0} not found, check log for details.<p>", widget.InnerText));
                        WebUtils.Log("WidgetZone.OnLoad", ex);
                    }
                }
                builder.Append("</div>");
            }
            return MvcHtmlString.Create(builder.ToString());
        }



        private static XmlDocument RetrieveXml(string zoneName)
        {
            var ws = new WidgetSettings(zoneName) { SettingsBehavior = new XmlDocumentBehavior() };
            var doc = (XmlDocument)ws.GetSettings();
            return doc;
        }
    }
}