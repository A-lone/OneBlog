using System.Collections.Generic;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace System.Web.Mvc
{
    public static class RadioButtonListExtensions
    {
        public static string RadioButtonList(this HtmlHelper helper, string name, IEnumerable<string> items)
        {
            var selectList = new SelectList(items);
            return helper.RadioButtonList(name, selectList, htmlAttributes: null);
        }

        public static string RadioButtonList(this HtmlHelper helper, string name, IEnumerable<SelectListItem> items, object htmlAttributes = null)
        {
            return helper.RadioButtonList(name, items, new RouteValueDictionary(htmlAttributes));
        }

        public static string RadioButtonList(this HtmlHelper helper, string name, IEnumerable<SelectListItem> items, IDictionary<string, object> htmlAttributes = null)
        {
            TagBuilder tableTag = new TagBuilder("table");
            tableTag.AddCssClass("radio-main");

            var trTag = new TagBuilder("tr");
            foreach (var item in items)
            {
                var tdTag = new TagBuilder("td");
                var rbValue = item.Value ?? item.Text;
                var rbName = name + "_" + rbValue;
                var radioTag = helper.RadioButton(rbName, rbValue, item.Selected, new { name = name });

                var labelTag = new TagBuilder("label");
                labelTag.MergeAttribute("for", rbName);
                labelTag.MergeAttribute("id", rbName + "_label");
                labelTag.MergeAttributes(htmlAttributes);
                labelTag.InnerHtml = rbValue;

                tdTag.InnerHtml = radioTag.ToString() + labelTag.ToString();

                trTag.InnerHtml += tdTag.ToString();
            }
            tableTag.InnerHtml = trTag.ToString();

            return tableTag.ToString();
        }
    }

}