using OneBlog.Core.Data.Models;
using OneBlog.Core.Data.ViewModels;

namespace OneBlog.Core.Data.Contracts
{
    /// <summary>
    /// Settings repository
    /// </summary>
    public interface ISettingsRepository
    {
        /// <summary>
        /// Get all settings
        /// </summary>
        /// <returns>Settings view model</returns>
        SettingsVM Get();
        /// <summary>
        /// Update settings
        /// </summary>
        /// <param name="item">Settings item</param>
        /// <returns>True if success</returns>
        bool Update(Settings item);
    }
}
