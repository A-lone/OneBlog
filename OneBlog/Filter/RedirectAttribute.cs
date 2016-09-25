using System.Text.RegularExpressions;

namespace System.Web.Mvc
{
    public class RedirectAttribute : ActionFilterAttribute
    {
        private const string MAIN_HOST = "chenrensong.com";

        private static readonly Regex wwwRegex = new Regex(@"www\.(?<mainDomain>.*)",
        RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            string hostName = filterContext.HttpContext.Request.Headers["x-forwarded-host"];
            hostName = string.IsNullOrEmpty(hostName) ? filterContext.HttpContext.Request.Url.Host : hostName;
            hostName = hostName.ToLower();
            var IsDomain = hostName.Contains(MAIN_HOST);
            UriBuilder builder = null;
            if (!IsDomain)
            {
                if (hostName != "localhost" && hostName != "127.0.0.1")
                {
                    builder = new UriBuilder(filterContext.HttpContext.Request.Url)
                    {
                        Host = MAIN_HOST
                    };
                }
            }
            else
            {
                Match match = wwwRegex.Match(hostName);
                if (match.Success)
                {
                    string mainDomain = match.Groups["mainDomain"].Value;
                    builder = new UriBuilder(filterContext.HttpContext.Request.Url)
                    {
                        Host = mainDomain
                    };
                }
            }

            if (builder != null)
            {
                string redirectUrl = builder.Uri.ToString();
                filterContext.HttpContext.Response.Clear();
                filterContext.HttpContext.Response.StatusCode = 301;
                filterContext.HttpContext.Response.StatusDescription = "Moved Permanently";
                filterContext.HttpContext.Response.AddHeader("Location", redirectUrl);
                filterContext.HttpContext.Response.End();
            }

        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            //filterContext.HttpContext.Response.Write("Action执行之后" + Message + "<br />");
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
            //filterContext.HttpContext.Response.Write("返回Result之前" + Message + "<br />");
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
            //filterContext.HttpContext.Response.Write("返回Result之后" + Message + "<br />");
        }
    }
}