using OneBlog.Core;
using System.Web;
using System.Web.Optimization;

namespace OneBlog
{
    public class BundleConfig
    {
        // 有关绑定的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            // 生产准备时，请使用 http://modernizr.com 上的生成工具来仅选择所需的测试。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(
           new StyleBundle("~/Content/admincss")
           .Include("~/Content/bootstrap.min.css")
           .Include("~/Content/toastr.css")
           .Include("~/Content/font-awesome.min.css")
           .Include("~/Content/star-rating.css")
           );

            bundles.Add(
   new StyleBundle("~/Content/standard")
   .Include("~/Custom/Themes/Standard/src/css/animate.min.css")
   .Include("~/Custom/Themes/Standard/src/css/style.css")
   );



            bundles.Add(
              new ScriptBundle("~/scripts/blogadmin")
              .Include("~/Scripts/jquery-{version}.js")
              .Include("~/Scripts/jquery.form.js")
              .Include("~/Scripts/jquery.validate.js")
              .Include("~/Scripts/jquery-ui.js")
              .Include("~/Scripts/toastr.js")
              .Include("~/Scripts/bootstrap.js")
              .Include("~/Scripts/moment.js")
              .Include("~/Scripts/Q.js")
              .Include("~/Scripts/angular.min.js")
              .Include("~/Scripts/angular-route.min.js")
              .Include("~/Scripts/angular-sanitize.min.js")

              .Include("~/areas/admin/app/app.js")
              .Include("~/areas/admin/app/listpager.js")
              .Include("~/areas/admin/app/grid-helpers.js")
              .Include("~/areas/admin/app/data-service.js")
              .Include("~/areas/admin/app/editor/filemanagerController.js")
              .Include("~/areas/admin/app/common.js")

              .Include("~/areas/admin/app/dashboard/dashboardController.js")

              .Include("~/areas/admin/app/content/blogs/blogController.js")
              .Include("~/areas/admin/app/content/posts/postController.js")
              .Include("~/areas/admin/app/content/pages/pageController.js")
              .Include("~/areas/admin/app/content/tags/tagController.js")
              .Include("~/areas/admin/app/content/categories/categoryController.js")
              .Include("~/areas/admin/app/content/comments/commentController.js")
              .Include("~/areas/admin/app/content/comments/commentFilters.js")

              .Include("~/areas/admin/app/custom/plugins/pluginController.js")
              .Include("~/areas/admin/app/custom/themes/themeController.js")
              .Include("~/areas/admin/app/custom/widgets/widgetController.js")
              .Include("~/areas/admin/app/custom/widgets/widgetGalleryController.js")

              .Include("~/areas/admin/app/security/users/userController.js")
              .Include("~/areas/admin/app/security/roles/roleController.js")
              .Include("~/areas/admin/app/security/profile/profileController.js")

              .Include("~/areas/admin/app/settings/settingController.js")
              .Include("~/areas/admin/app/settings/tools/toolController.js")
              .Include("~/areas/admin/app/settings/controls/blogrollController.js")
              .Include("~/areas/admin/app/settings/controls/pingController.js")
              );

            bundles.Add(new ScriptBundle("~/bundles/mdd").Include(
           "~/Scripts/mkd/Markdown*"));

            bundles.Add(
                new ScriptBundle("~/scripts/wysiwyg")
                .Include("~/scripts/jquery-{version}.js")
                .Include("~/scripts/jquery.form.js")
                .Include("~/scripts/jquery.validate.js")
                .Include("~/scripts/toastr.js")
                .Include("~/scripts/Q.js")
                .Include("~/Scripts/angular.min.js")
                .Include("~/Scripts/angular-route.min.js")
                .Include("~/Scripts/angular-sanitize.min.js")
                .Include("~/scripts/bootstrap.js")
                .Include("~/scripts/textext.js")
                .Include("~/scripts/moment.js")
                .Include("~/areas/admin/app/app.js")
                .Include("~/areas/admin/app/grid-helpers.js")
                .Include("~/areas/admin/app/editor/editor-helpers.js")
                .Include("~/areas/admin/app/editor/posteditorController.js")
                .Include("~/areas/admin/app/editor/pageeditorController.js")

                .Include("~/areas/admin/app/common.js")
                .Include("~/areas/admin/app/data-service.js")
                );

            if (BlogConfig.DefaultEditor == "~/areas/admin/editors/tinymce/editor.cshtml")
            {
                bundles.GetBundleFor("~/scripts/wysiwyg").Include("~/areas/admin/app/editor/filemanagerController.tinymce.js");
            }
            else
            {
                bundles.GetBundleFor("~/scripts/wysiwyg").Include("~/areas/admin/app/editor/filemanagerController.js");
            }

            if (BlogConfig.DefaultEditor == "~/areas/admin/editors/bootstrap-wysiwyg/editor.cshtml")
            {
                bundles.GetBundleFor("~/scripts/wysiwyg").Include("~/areas/admin/editors/bootstrap-wysiwyg/jquery.hotkeys.js");
                bundles.GetBundleFor("~/scripts/wysiwyg").Include("~/areas/admin/editors/bootstrap-wysiwyg/bootstrap-wysiwyg.js");
                bundles.GetBundleFor("~/scripts/wysiwyg").Include("~/areas/admin/editors/bootstrap-wysiwyg/editor.js");
            }

            if (BlogConfig.DefaultEditor == "~/areas/admin/editors/tinymce/editor.cshtml")
            {
                // tinymce plugings won't load when compressed. added in post/page editors instead.

                bundles.GetBundleFor("~/scripts/wysiwyg").Include("~/areas/admin/editors/tinymce/tinymce.min.js");
                bundles.GetBundleFor("~/scripts/wysiwyg").Include("~/areas/admin/editors/tinymce/editor.js");
            }
            else if (BlogConfig.DefaultEditor == "~/areas/admin/editors/markdown/editor.cshtml")
            {
                bundles.GetBundleFor("~/scripts/wysiwyg").Include("~/Scripts/mkd/Markdown*");
                bundles.GetBundleFor("~/scripts/wysiwyg").Include("~/areas/admin/editors/markdown/editor.js");
            }
            else
            {
                bundles.GetBundleFor("~/scripts/wysiwyg").Include("~/areas/admin/editors/summernote/summernote.js");
                // change language here if needed
                //bundles.GetBundleFor("~/scripts/wysiwyg").Include("~/areas/admin/editors/summernote/lang/summernote-ru-RU.js");
                bundles.GetBundleFor("~/scripts/wysiwyg").Include("~/areas/admin/editors/summernote/editor.js");
            }
        }
    }
}
