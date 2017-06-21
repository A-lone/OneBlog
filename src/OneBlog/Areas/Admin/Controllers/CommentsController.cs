using Microsoft.AspNetCore.Mvc;
using OneBlog.Data.Contracts;
using OneBlog.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneBlog.Areas.Admin.Controllers
{
    [Route("api/[controller]")]
    public class CommentsController : Controller
    {
        readonly ICommentsRepository repository;

        public CommentsController(ICommentsRepository repository)
        {
            this.repository = repository;
        }
        [HttpGet]
        public CommentsResult Get()
        {
            return repository.Get();
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var result = repository.FindById(id);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
        [HttpPost]
        public IActionResult Post([FromBody]CommentDetail item)
        {
            var result = repository.Add(item);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
        [HttpPut]
        public IActionResult Put([FromBody]CommentItem item)
        {
            repository.Update(item, "update");
            return Ok();
        }

        public IActionResult Delete(string id)
        {
            Guid gId;
            if (Guid.TryParse(id, out gId))
            {
                repository.Remove(gId);
            }
            return Ok();
        }

        [HttpPut]
        [Route("processchecked/{id?}")]
        public IActionResult ProcessChecked([FromBody]List<CommentItem> items, string id)
        {
            if (items == null || items.Count == 0)
            {
                return BadRequest();
            }
            var action = id.ToString().ToLowerInvariant();// ControllerContext.RouteData.Values["id"].ToString().ToLowerInvariant();

            foreach (var item in items)
            {
                if (item.IsChecked)
                {
                    if (action == "delete")
                    {
                        repository.Remove(item.Id);
                    }
                    else
                    {
                        repository.Update(item, action);
                    }
                }
            }

            return Ok();
        }

        [HttpPut]
        public IActionResult DeleteAll([FromBody]CommentItem item)
        {
            var action = ControllerContext.RouteData.Values["id"].ToString().ToLowerInvariant();

            if (action.ToLower() == "pending" || action.ToLower() == "spam")
            {
                repository.DeleteAll(action);
            }
            return Ok();
        }
    }
}
