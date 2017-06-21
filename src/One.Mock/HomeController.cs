using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace One.Mock
{
    [Route("Home")]
    public class HomeController : Controller
    {
        [HttpGet]
        public int Get()
        {
            return 0;
        }
    }
}
