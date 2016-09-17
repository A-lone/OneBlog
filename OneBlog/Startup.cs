using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OneBlog.Startup))]
namespace OneBlog
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
