using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
    public class TagsController
    {
        private readonly ITagsRepository _tagsRepository;
        private readonly IHostingEnvironment _env;
        public TagsController(IHostingEnvironment env,ITagsRepository tagsRepository)
        {
            _tagsRepository = tagsRepository;
            _env = env;
        }

        [HttpGet]
        public IEnumerable<TagItem> Get(int take = 10, int skip = 0, string filter = "1 == 1", string order = "")
        {
            return _tagsRepository.Find(take, skip, filter, order);
        }
    }
}
