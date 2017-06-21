using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace One.Helpers
{
    public static class BotHelper
    {
        public static bool UserIsBot()
        {
            var context = AspNetCoreHelper.HttpContext;
            if (context == null)
            {
                throw new ApplicationException("UserIsBot() get HttpContext is null");
            }
            var userAgent = context.Request.Headers["User-Agent"].FirstOrDefault();
            if (userAgent != null)
            {
                userAgent = userAgent.ToLower();
                var botKeywords = new List<string> { "bot", "spider", "google", "yahoo", "search", "crawl", "slurp", "msn", "teoma", "ask.com", "bing", "accoona" };
                return botKeywords.Any(userAgent.Contains);
            }
            return true;
        }
    }
}
