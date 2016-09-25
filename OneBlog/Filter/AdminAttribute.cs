using OneBlog.Core;
using System.Web.Routing;

namespace System.Web.Mvc
{
    public class AdminAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!OneBlog.Core.Security.IsAuthorizedTo(Rights.AccessAdminPages))
            {
                string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                string actionName = filterContext.ActionDescriptor.ActionName;
                Uri referrer = filterContext.RequestContext.HttpContext.Request.UrlReferrer;
                bool isFromLoginPage = referrer != null && referrer.LocalPath.IndexOf("/Account/Login", StringComparison.OrdinalIgnoreCase) != -1;
                if (isFromLoginPage)
                {
                    filterContext.HttpContext.Response.Redirect(WebUtils.RelativeWebRoot);
                }
                else
                {
                    filterContext.HttpContext.Response.Redirect(string.Format("{0}Account/Login?ReturnURL={1}", WebUtils.RelativeWebRoot, HttpUtility.UrlPathEncode(filterContext.RequestContext.HttpContext.Request.RawUrl)));
                }
            }
        }

    }
}