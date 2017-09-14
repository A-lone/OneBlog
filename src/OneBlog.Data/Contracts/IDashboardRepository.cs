using OneBlog.Data.Models.ManageViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OneBlog.Data.Contracts
{
    /// <summary>
    /// Dashboard repository
    /// </summary>
    public interface IDashboardRepository
    {
        /// <summary>
        /// Get all dashboard items
        /// </summary>
        /// <returns>Dashboard view model</returns>
        DashboardViewModel Get();
    }
}
