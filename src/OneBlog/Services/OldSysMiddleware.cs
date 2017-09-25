using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace OneBlog.Services
{
    public class OldSysMiddleware
    {
        private RequestDelegate _next;
        private ILogger<ActiveUsersMiddleware> _logger;

        public OldSysMiddleware(RequestDelegate next, ILogger<ActiveUsersMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                if (context.Request.Host.Host == "chenrensong.com" || context.Request.Host.Host == "www.chenrensong.com")
                {
                    context.Response.OnStarting((state) =>
                    {
                        context.Response.Redirect("http://www.huafenfei.com/author/de3d4cee-3bc4-465e-8b62-8038fed682b8");
                        return Task.FromResult(0);
                    }, null);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to store active user");
            }
            await _next.Invoke(context);

        }


    }
}
