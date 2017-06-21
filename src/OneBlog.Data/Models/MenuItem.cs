using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace One.Data.Models
{
    public class MenuItem
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public bool IsActive { get; set; }

        public List<MenuItem> Menus { get; set; } = new List<MenuItem>();

        public List<RouteData> Route { get; set; }
    }

    public class RouteData
    {
        public string Action { get; set; }

        public string Controller { get; set; }
    }
}
