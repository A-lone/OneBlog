using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using One.Data.Contracts;
using One.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace One.Areas.Admin.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class RolesController : Controller
    {
        readonly IRolesRepository repository;

        public RolesController(IRolesRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public IEnumerable<RoleItem> Get(int take = 10, int skip = 0, string filter = "", string order = "")
        {
            return repository.Find(take, skip, filter, order);
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var result = repository.FindById(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        public IActionResult Post([FromBody]RoleItem role)
        {
            var result = repository.Add(role);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPut]
        public IActionResult Put([FromBody]List<RoleItem> items)
        {
            if (items == null || items.Count == 0)
            {
                return NotFound();
            }

            foreach (var item in items)
            {
                repository.Remove(item.RoleName);
            }
            return Ok();
        }

        public IActionResult Delete(string id)
        {
            repository.Remove(id);
            return Ok();
        }

        [HttpPut]
        public IActionResult ProcessChecked([FromBody]List<RoleItem> items)
        {
            if (items == null || items.Count == 0)
            {
                return NotFound();
            }

            var action = ControllerContext.RouteData.Values["id"].ToString().ToLowerInvariant();

            if (action.ToLower() == "delete")
            {
                foreach (var item in items)
                {
                    if (item.IsChecked)
                    {
                        repository.Remove(item.RoleName);
                    }
                }
            }
            return Ok();
        }

  

        [HttpGet]
        [Route("getuserroles/{id?}")]
        public IActionResult GetUserRoles(string id)
        {
            var result = repository.GetUserRoles(id);
            return Ok(result);
        }


        //[HttpGet]
        //public IActionResult GetRights(string id)
        //{
        //    var result = repository.GetRoleRights(id);
        //    return Ok(result);
        //}

        //[HttpPut]
        //public IActionResult SaveRights([FromBody]List<Group> rights, string id)
        //{
        //    repository.SaveRights(rights, id);

        //    return Ok();
        //}
    }
}
