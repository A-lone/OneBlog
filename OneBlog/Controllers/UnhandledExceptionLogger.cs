using System;
using System.Web.Http.ExceptionHandling;
using OneBlog.Core;

namespace OneBlog.NET.AppCode.Api
{
    public class UnhandledExceptionLogger : ExceptionLogger
    {
        public override void Log(ExceptionLoggerContext context)
        {
            if (context.Exception is UnauthorizedAccessException) { return; }

            CoreUtils.Log("{0} {1}: {2}", context.Request.Method, context.Request.RequestUri, context.Exception.Message);
        }
    }
}