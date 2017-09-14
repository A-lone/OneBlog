using OneBlog.Data.Contracts;
using OneBlog.Data.Models.ManageViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OneBlog.Data.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        private ApplicationDbContext _ctx;

        public DashboardRepository(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        /// <summary>
        /// Get all dashboard items
        /// </summary>
        /// <returns>Dashboard view model</returns>
        public DashboardViewModel Get()
        {
            return new DashboardViewModel(_ctx);
        }
    }
}
