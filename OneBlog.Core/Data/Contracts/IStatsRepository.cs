using OneBlog.Core.Data.Models;

namespace OneBlog.Core.Data.Contracts
{
    /// <summary>
    /// Statistics info
    /// </summary>
    public interface IStatsRepository
    {
        /// <summary>
        /// Get stats info
        /// </summary>
        /// <returns>Stats counts</returns>
        Stats Get();
    }
}
