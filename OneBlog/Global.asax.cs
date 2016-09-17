using OneBlog.Core;
using OneBlog.Core.Data;
using OneBlog.Core.Data.Contracts;
using OneBlog.ViewEngine;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace OneBlog
{
    public class MvcApplication : System.Web.HttpApplication
    {
        //http://stevemichelotti.com/mvc-radiobuttonlist-html-helper/
        protected void Application_Start()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new CustomViewEngine(BlogSettings.Instance.Theme));

            WebUtils.LoadExtensions();

            AreaRegistration.RegisterAllAreas();
            RegisterDiContainer();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
   

        }

        static void RegisterDiContainer()
        {
            var container = new SimpleInjector.Container();

            container.Register<ISettingsRepository, SettingsRepository>(Lifestyle.Transient);
            container.Register<IPostRepository, PostRepository>(Lifestyle.Transient);
            container.Register<IPageRepository, PageRepository>(Lifestyle.Transient);
            container.Register<IBlogRepository, BlogRepository>(Lifestyle.Transient);
            container.Register<IStatsRepository, StatsRepository>(Lifestyle.Transient);
            container.Register<IPackageRepository, PackageRepository>(Lifestyle.Transient);
            container.Register<ILookupsRepository, LookupsRepository>(Lifestyle.Transient);
            container.Register<ICommentsRepository, CommentsRepository>(Lifestyle.Transient);
            container.Register<ITrashRepository, TrashRepository>(Lifestyle.Transient);
            container.Register<ITagRepository, TagRepository>(Lifestyle.Transient);
            container.Register<ICategoryRepository, CategoryRepository>(Lifestyle.Transient);
            container.Register<ICustomFieldRepository, CustomFieldRepository>(Lifestyle.Transient);
            container.Register<IUsersRepository, UsersRepository>(Lifestyle.Transient);
            container.Register<IRolesRepository, RolesRepository>(Lifestyle.Transient);
            container.Register<IFileManagerRepository, FileManagerRepository>(Lifestyle.Transient);
            container.Register<ICommentFilterRepository, CommentFilterRepository>(Lifestyle.Transient);
            container.Register<IDashboardRepository, DashboardRepository>(Lifestyle.Transient);
            container.Register<IWidgetsRepository, WidgetsRepository>(Lifestyle.Transient);

            container.Verify();

            GlobalConfiguration.Configuration.DependencyResolver =
                new SimpleInjectorWebApiDependencyResolver(container);
        }
    }
}
