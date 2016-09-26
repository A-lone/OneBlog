using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace System.Web.Mvc
{
    public class GlobalHandleErrorAttribute : HandleErrorAttribute
    {
        /// <summary>
        /// 异常
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnException(ExceptionContext filterContext)
        {
            //使用log4net或其他记录错误消息
            Exception Error = filterContext.Exception;
            string Message = Error.Message;//错误信息
            string Url = HttpContext.Current.Request.RawUrl;//错误发生地址
            filterContext.ExceptionHandled = true;
            //filterContext.RequestContext.HttpContext.Response.Redirect("/Home/Error?code=500");
            filterContext.Result = new RedirectResult("/Home/Error?code=500");//跳转至错误提示页面
        }
    }
}