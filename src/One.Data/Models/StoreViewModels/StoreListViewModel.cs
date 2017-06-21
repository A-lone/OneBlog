using One.Data;
using One.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace One.Models.StoreViewModels
{
    public class StoreListViewModel
    {
        public IList<StoreCategories> StoreCategories { get; set; }

        public StoreAppResult AppResult { get; set; }

    }
}
