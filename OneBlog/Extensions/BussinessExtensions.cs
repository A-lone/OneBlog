using OneBlog.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Routing;

namespace System.Web.Mvc
{
    public static class BussinessExtensions
    {
        static string GetHash(string value)
        {
            MD5 algorithm = MD5.Create();
            byte[] data = algorithm.ComputeHash(Encoding.UTF8.GetBytes(value));
            string md5 = "";
            for (int i = 0; i < data.Length; i++)
            {
                md5 += data[i].ToString("x2").ToUpperInvariant();
            }
            return md5;
        }

        public  static string Gravatar(this HtmlHelper helper, string email)
        {
            //var hash = FormsAuthentication.HashPasswordForStoringInConfigFile(email.ToLowerInvariant().Trim(), "MD5");
            var hash = GetHash(email.ToLowerInvariant().Trim());

            if (hash != null)
                hash = hash.ToLowerInvariant();

            var gravatar = string.Format("http://www.gravatar.com/avatar/{0}.jpg?d=", hash);

            switch (BlogSettings.Instance.Avatar)
            {
                case "identicon":
                    return gravatar + "identicon";
                case "wavatar":
                    return gravatar + "wavatar";
                case "retro":
                    return gravatar + "retro";
                case "mm":
                    return gravatar + "mm";
                case "blank":
                    return gravatar + "blank";
                case "monsterid":
                    return gravatar + "monsterid";
                default:
                    return "";
            }
        }

        public static MvcHtmlString Pager(this HtmlHelper helper, int currentPage, int pageSize, int totalItemCount, object routeValues, string actionOveride = null, string controllerOveride = null)
        {
            // how many pages to display in each page group const  	
            var pageCount = (int)Math.Ceiling(totalItemCount / (double)pageSize);

            if (pageCount <= 0)
            {
                return null;
            }

            // cleanup any out bounds page number passed  	
            currentPage = Math.Max(currentPage, 1);
            currentPage = Math.Min(currentPage, pageCount);


        

         

            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext, helper.RouteCollection);
            var containerdiv = new TagBuilder("nav");
            containerdiv.AddCssClass("pagination");
            containerdiv.MergeAttribute("role", "pagination");

            var actionName = !string.IsNullOrEmpty(actionOveride) ? actionOveride : helper.ViewContext.RouteData.GetRequiredString("action");
            var controllerName = !string.IsNullOrEmpty(controllerOveride) ? controllerOveride : helper.ViewContext.RouteData.GetRequiredString("controller");

            // if we are past the first page  	
            if (currentPage > 1)
            {
                var previous = new TagBuilder("a");
                previous.SetInnerText("← Newer Posts");
                previous.AddCssClass("newer-posts");
                var routingValues = new RouteValueDictionary(routeValues) { { "p", currentPage - 1 } };
                foreach (var item in helper.ViewContext.RouteData.Values)
                {
                    if (item.Key == "controller" || item.Key == "action")
                    {
                        break;
                    }
                    routingValues.Add(item.Key, item.Value);
                }
                previous.MergeAttribute("href", urlHelper.Action(actionName, controllerName, routingValues));
                containerdiv.InnerHtml += previous;
            }

            var span = new TagBuilder("span");
            span.AddCssClass("page-number");
            span.SetInnerText(string.Format("Page {0} of {1}", currentPage, pageCount));
            containerdiv.InnerHtml += span;


            // if we still have pages left to show  	
            if (currentPage < pageCount)
            {
                var next = new TagBuilder("a");
                next.SetInnerText("Older Posts →");
                next.AddCssClass("older-posts");
                var routingValues = new RouteValueDictionary(routeValues) { { "p", currentPage + 1 } };
                foreach (var item in helper.ViewContext.RouteData.Values)
                {
                    if (item.Key == "controller" || item.Key == "action")
                    {
                        break;
                    }
                    routingValues.Add(item.Key, item.Value);

                }
                next.MergeAttribute("href", urlHelper.Action(actionName, controllerName, routingValues));
                containerdiv.InnerHtml += next.ToString();
            }

            return MvcHtmlString.Create(containerdiv.ToString());
        }

    }
}