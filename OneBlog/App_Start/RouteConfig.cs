using OneBlog.Areas.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OneBlog
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            RouteTable.Routes.LowercaseUrls = true;
            RouteTable.Routes.AppendTrailingSlash = false;//自动加入斜杠
            routes.RouteExistingFiles = false;
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });

            routes.MapRoute(
                "Archive", // Route name
                "archive", // URL with parameters
                new { controller = "Home", action = "Archive" } // Parameter defaults
            );

            routes.MapRoute(
                "Contact", // Route name
                "contact", // URL with parameters
                new { controller = "Home", action = "Contact" } // Parameter defaults
            );

            routes.MapRoute(
                "Post", // Route name
                "post/{slug}", // URL with parameters
                new { controller = "Home", action = "Post" } // Parameter defaults
            );

            routes.MapRoute(
               "Post_Date_Old", // 旧版本兼容
               "post/{year}/{month}/{day}/{slug}.html", // URL with parameters
               new { controller = "Home", action = "Post", year = @"\d+", month = @"\d+", day = @"\d+" } // Parameter defaults
           );

            routes.MapRoute(
               "Post_Date", // Route name
               "post/{year}/{month}/{day}/{slug}", // URL with parameters
               new { controller = "Home", action = "Post", year = @"\d+", month = @"\d+", day = @"\d+" } // Parameter defaults
           );


            routes.MapRoute(
                "Rss", // Route name
                "rss", // URL with parameters
                new { controller = "Home", action = "Rss" } // Parameter defaults
            );

            routes.MapRoute(
                "Category", // Route name
                "category/{category}/{p}", // URL with parameters
                new { controller = "Home", action = "Category", p = UrlParameter.Optional } // Parameter defaults
            );



            routes.MapRoute(
                "Tag_Old", // Route name
                "tag/{tag}.html/{p}", // URL with parameters
                new { controller = "Home", action = "Tag", p = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                "Tag", // Route name
                "tag/{tag}/{p}", // URL with parameters
                new { controller = "Home", action = "Tag", p = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );



        }
    }
}
