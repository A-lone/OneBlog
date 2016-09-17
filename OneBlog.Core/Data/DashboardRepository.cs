using OneBlog.Core.Data.ViewModels;
using OneBlog.Core.Data.Contracts;

namespace OneBlog.Core.Data
{
    /// <summary>
    /// Dashboard repository
    /// </summary>
    public class DashboardRepository : IDashboardRepository
    {
        /// <summary>
        /// Get all dashboard items
        /// </summary>
        /// <returns>Dashboard view model</returns>
        public DashboardVM Get()
        {
            if (!Security.IsAuthorizedTo(Rights.ViewDashboard))
                throw new System.UnauthorizedAccessException();

            return new DashboardVM();
        }
    }
}
