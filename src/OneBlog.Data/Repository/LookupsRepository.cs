using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using OneBlog.Data.Contracts;
using OneBlog.Data.Models;
using System.Collections.Generic;

namespace OneBlog.Data
{
    /// <summary>
    /// Lookups repository
    /// </summary>
    public class LookupsRepository : ILookupsRepository
    {
        private Lookups lookups = new Lookups();

        private UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext _ctx;

        public LookupsRepository(UserManager<ApplicationUser> userManager, ApplicationDbContext ctx)
        {
            _ctx = ctx;
            _userManager = userManager;
        }

        /// <summary>
        /// List of available cultures
        /// </summary>
        /// <returns>List of cultures</returns>
        public Lookups GetLookups()
        {
            LoadCultures();

            LoadSelfRegisterRoles();

            LoadPages();

            LoadAuthors();

            LoadCategories();

            LoadThemes();

            LoadEditorOptions();

            return lookups;
        }

        void LoadCultures()
        {
            var items = new List<SelectOption>();

            items.Add(new SelectOption { OptionName = "Auto", OptionValue = "Auto" });
            items.Add(new SelectOption { OptionName = "English", OptionValue = "en" });
            items.Add(new SelectOption { OptionName = "中文(中国)", OptionValue = "zh-CN", IsSelected = false });
            items.Add(new SelectOption { OptionName = "Auto", OptionValue = "Auto", IsSelected = false });

            lookups.Cultures = items;
        }

        void LoadSelfRegisterRoles()
        {
            var roles = new List<SelectOption>();
            //foreach (var role in Roles.GetAllRoles().Where(r => !r.Equals(BlogConfig.AnonymousRole, StringComparison.OrdinalIgnoreCase)))
            //{
            //    roles.Add(new SelectOption { OptionName = role, OptionValue = role });
            //}
            lookups.SelfRegisterRoles = roles;
        }

        void LoadAuthors()
        {
            var items = new List<SelectOption>();

            //if (!Security.IsAuthorizedTo(Rights.EditOtherUsersPosts))
            //{
            //    items.Add(new SelectOption { OptionName = Security.CurrentUser.Identity.Name, OptionValue = Security.CurrentUser.Identity.Name });
            //}
            //else
            //{
            //    IEnumerable<MembershipUser> users = Membership.GetAllUsers()
            //    .Cast<MembershipUser>()
            //    .ToList()
            //    .OrderBy(a => a.UserName);

            //    foreach (MembershipUser user in users)
            //    {
            //        items.Add(new SelectOption { OptionName = user.UserName, OptionValue = user.UserName });
            //    }
            //}

            var task = _userManager.GetUsersInRoleAsync("Administrator");
            var administrators = task.Result;
            Author author = new Author();
            bool isSelected = false;

            foreach (var item in administrators)
            {
                author.Id = item.Id;
                author.DisplayName = item.DisplayName;
                author.Signature = item.Signature;
                author.Name = item.UserName;
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(author);
                items.Add(new SelectOption { OptionName = item.DisplayName, OptionValue = author.Id, IsSelected = isSelected });
            }
            lookups.AuthorList = items;
        }

        void LoadPages()
        {
            var pages = new List<SelectOption>();
            //foreach (var page in Page.Pages)
            //{
            //    if (!page.IsDeleted)
            //    {
            //        pages.Add(new SelectOption { OptionName = page.Title, OptionValue = page.Id.ToString() });
            //    }
            //}
            lookups.PageList = pages;
        }

        void LoadCategories()
        {
            var cats = new List<SelectOption>();
            foreach (var cat in _ctx.Categories)
            {
                cats.Add(new SelectOption { OptionName = cat.Title, OptionValue = cat.Id.ToString() });
            }
            lookups.CategoryList = cats;
        }

        void LoadThemes()
        {
            var items = new List<SelectOption>();
            //var packages = Packaging.FileSystem.LoadThemes();

            //if (packages == null)
            //{
            //    return;
            //}

            //foreach (var pkg in packages)
            //{
            //    items.Add(new SelectOption { OptionName = pkg.Title, OptionValue = pkg.Id.ToString() });
            //}
            lookups.InstalledThemes = items;
        }

        void LoadEditorOptions()
        {

            lookups.PostOptions = new EditorOptions
            {
                OptionType = "Post",
                ShowSlug = true,
                ShowDescription = true,
                ShowCustomFields = true,
                ShowAuthors = true
            };

            lookups.PageOptions = new EditorOptions
            {
                OptionType = "Page",
                ShowSlug = true,
                ShowDescription = true,
                ShowCustomFields = true
            };
        }

        /// <summary>
        /// Editor options
        /// </summary>
        /// <param name="options">Options</param>
        public void SaveEditorOptions(EditorOptions options)
        {
            //var bs = BlogSettings.Instance;
            //if (options.OptionType == "Post")
            //{
            //    bs.PostOptionsSlug = options.ShowSlug;
            //    bs.PostOptionsDescription = options.ShowDescription;
            //    bs.PostOptionsCustomFields = options.ShowCustomFields;
            //}
            //if (options.OptionType == "Page")
            //{
            //    bs.PageOptionsSlug = options.ShowSlug;
            //    bs.PageOptionsDescription = options.ShowDescription;
            //    bs.PageOptionsCustomFields = options.ShowCustomFields;
            //}
            //bs.Save();
        }
    }
}
