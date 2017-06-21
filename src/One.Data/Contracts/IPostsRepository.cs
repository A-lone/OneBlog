using One.Data.Models;
using System;
using System.Collections.Generic;

namespace One.Data.Contracts
{
    public interface IPostsRepository
    {

        Pager<PostItem> Find(int take = 10, int skip = 0, string filter = "", string order = "");
        /// <summary>
        /// 获取Post
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="page"></param>
        /// <param name="authorId"></param>
        /// <returns></returns>
        PostsResult GetPosts(int pageSize = 10, int page = 1, Guid? authorId = null);
        PostsResult GetPostsByTerm(string term, int pageSize, int page);
        PostsResult GetPostsByTag(string tag, int pageSize, int page);

        PostsResult GetPostsByCategory(Guid categoryId, int pageSize, int page);

        Posts GetPost(Guid id);

        long AddPostCount(Guid id);

        PostDetail FindById(Guid id);

        Posts GetPost(string slug);

        PostDetail Add(PostDetail post);

        PostDetail Update(PostDetail post);

        void AddPost(Posts story);

        void SaveAll();
        bool DeletePost(Guid postid);

        IEnumerable<string> GetCategories();

        string FixContent(string content);
    }
}