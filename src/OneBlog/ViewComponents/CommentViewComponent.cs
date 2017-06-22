using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OneBlog.Data;
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
        private readonly UserManager<ApplicationUser> _userManager;

        public CommentViewComponent(UserManager<ApplicationUser> userManager, IPostsRepository postRepository, ICommentsRepository commentsRepository)
        {
            _userManager = userManager;
            _postRepository = postRepository;
            _commentsRepository = commentsRepository;
        }


        public IViewComponentResult Invoke(Guid id)
        {
            var model = new CommentViewModel();
            var currentUser = _userManager.GetUserAsync(HttpContext.User).Result;
            if (currentUser != null)
            {
                model.UserName = currentUser.DisplayName;
                model.Email = currentUser.Email;
            }
            model.PostId = id;
            model.Comments = _commentsRepository.FindByPostId(id);
            return View(model);
        }
    }
}
