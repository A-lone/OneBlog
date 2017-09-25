using Microsoft.AspNetCore.Identity;
using OneBlog.Data;
using OneBlog.Data.Contracts;
using OneBlog.Helpers;
using OneBlog.Services;
using Qiniu.IO;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OneBlog.MetaWeblog
{
    public class WeblogProvider : IMetaWeblogProvider
    {
        private IPostsRepository _repo;
        private UserManager<ApplicationUser> _userMgr;
        private QiniuService _qiniuService;

        public WeblogProvider(UserManager<ApplicationUser> userMgr, IPostsRepository repo, QiniuService qiniuService)
        {
            _repo = repo;
            _userMgr = userMgr;
            _qiniuService = qiniuService;
        }


        public string AddPost(string blogid, string username, string password, Post post, bool publish)
        {
            EnsureUser(username, password).Wait();

            var newStory = new Posts();
            try
            {
                newStory.Title = post.title;
                newStory.Content = post.description;
                newStory.DatePublished = post.dateCreated == DateTime.MinValue ? DateTime.UtcNow : post.dateCreated;
                newStory.Tags = string.Join(",", post.categories);
                newStory.IsPublished = publish;
                newStory.Slug = newStory.GetStoryUrl();

                _repo.AddPost(newStory);
                _repo.SaveAll();
            }
            catch (Exception)
            {
                throw new MetaWeblogException("Failed to save the post.");
            }
            return newStory.Id.ToString();

        }

        public bool EditPost(string postid, string username, string password, Post post, bool publish)
        {
            EnsureUser(username, password).Wait();

            try
            {
                var story = _repo.GetPost(new Guid(postid));

                story.Title = post.title;
                story.Content = post.description;
                story.DatePublished = post.dateCreated == DateTime.MinValue ? DateTime.UtcNow : post.dateCreated;
                story.Tags = string.Join(",", post.categories);
                story.IsPublished = publish;
                story.Slug = story.GetStoryUrl();

                _repo.SaveAll();

                return true;
            }
            catch (Exception)
            {
                throw new MetaWeblogException("Failed to save the post.");
            }
        }

        public Post GetPost(string postid, string username, string password)
        {
            EnsureUser(username, password).Wait();

            try
            {
                var story = _repo.GetPost(new Guid(postid));
                var newPost = new Post()
                {
                    title = story.Title,
                    description = story.Content,
                    dateCreated = story.DatePublished,
                    categories = story.Tags.Split(','),
                    postid = story.Id,
                    userid = "test",
                    wp_slug = story.GetStoryUrl()
                };

                return newPost;
            }
            catch (Exception)
            {
                throw new MetaWeblogException("Failed to get the post.");
            }
        }

        private string GetFileName(string ext = "")
        {
            var random = new Random(Guid.NewGuid().GetHashCode());

            return string.Format("{0}{1}{2}", DateTime.Now.ToString("yyyy/MM/dd/HHmmss"), random.Next(100000, 1000000), ext);
        }

        public MediaObjectInfo NewMediaObject(string blogid, string username, string password, MediaObject mediaObject)
        {
            EnsureUser(username, password).Wait();

            var filenameonly = mediaObject.name.Substring(mediaObject.name.LastIndexOf('/') + 1);
            var ext = Path.GetExtension(filenameonly);

            var target = new IOClient();

            var key = GetFileName(ext);
            MediaObjectInfo objectInfo = new MediaObjectInfo();

            var bits = Convert.FromBase64String(mediaObject.bits);

            objectInfo.url = _qiniuService.Upload(key, bits).Result;
            return objectInfo;
        }

        public CategoryInfo[] GetCategories(string blogid, string username, string password)
        {
            EnsureUser(username, password).Wait();

            return _repo.GetCategories()
              .Select(c => new CategoryInfo()
              {
                  categoryid = c,
                  title = c,
                  description = c,
                  htmlUrl = string.Concat("/tags/", c),
                  rssUrl = ""
              }).ToArray();

        }

        public Post[] GetRecentPosts(string blogid, string username, string password, int numberOfPosts)
        {
            EnsureUser(username, password).Wait();

            return _repo.GetPosts(numberOfPosts).Posts.Select(s => new Post()
            {
                title = s.Title,
                description = s.Content,
                categories = s.Tags.Select(m => m.TagName).ToArray(),
                dateCreated = s.DatePublished,
                postid = s.Id,
                permalink = s.Slug,
                wp_slug = s.Slug
            }).ToArray();
        }

        public bool DeletePost(string key, string postid, string username, string password, bool publish)
        {
            EnsureUser(username, password).Wait();

            try
            {
                var result = _repo.DeletePost(new Guid(postid));
                _repo.SaveAll();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public BlogInfo[] GetUsersBlogs(string key, string username, string password)
        {
            var task = EnsureUser(username, password);
            var user = task.Result;
            var blog = new BlogInfo()
            {
                blogid = user.Id,
                blogName = user.DisplayName,
                url = "/"
            };

            return new BlogInfo[] { blog };
        }

        public UserInfo GetUserInfo(string key, string username, string password)
        {
            var task = EnsureUser(username, password);
            var user = task.Result;
            return new UserInfo()
            {
                email = user.Email,
                nickname = user.DisplayName,
                userid = user.Id,
                url = "/"
            };
        }

        private async Task<ApplicationUser> EnsureUser(string username, string password)
        {
            var user = await _userMgr.FindByNameAsync(username);
            if (user != null)
            {
                if (await _userMgr.CheckPasswordAsync(user, password))
                {
                    return user;
                }
            }
            throw new MetaWeblogException("Authentication failed.");
        }

        private void EnsureDirectory(DirectoryInfo dir)
        {
            if (dir.Parent != null)
            {
                EnsureDirectory(dir.Parent);
            }

            if (!dir.Exists)
            {
                dir.Create();
            }
        }

        public int AddCategory(string key, string username, string password, NewCategory category)
        {
            EnsureUser(username, password).Wait();

            // We don't store these, just query them from the list of stories so don't do anything
            return 1;
        }
    }
}