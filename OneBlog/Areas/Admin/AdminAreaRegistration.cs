using System.Web.Mvc;

namespace OneBlog.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{action}/{id}",
                new { action = "Index", Controller = "Admin", id = UrlParameter.Optional }
            );
        }
    }
}