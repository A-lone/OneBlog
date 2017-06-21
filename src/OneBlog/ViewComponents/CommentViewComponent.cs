using Microsoft.AspNetCore.Mvc;
using OneBlog.Data.Contracts;
using OneBlog.Data.Models;
using OneBlog.Models.CommentViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneBlog.ViewComponents
{
    public class CommentViewComponent : ViewComponent
    {
        private readonly IPostsRepository _postRepository;
        private readonly ICommentsRepository _commentsRepository;

        public CommentViewComponent(IPostsRepository postRepository, ICommentsRepository commentsRepository)
        {
            _postRepository = postRepository;
            _commentsRepository = commentsRepository;
        }


        public IViewComponentResult Invoke(Guid id)
        {
            var model = new CommentViewModel();
            model.PostId = id;
            model.Comments = _commentsRepository.FindByPostId(id);
            return View(model);
        }
    }
}
