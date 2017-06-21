using OneBlog.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneBlog.Data.Contracts
{
    /// <summary>
    /// Lookups repository
    /// </summary>
    public interface ILookupsRepository
    {
        /// <summary>
        /// Get lookups
        /// </summary>
        /// <returns>Lookups</returns>
        Lookups GetLookups();

        /// <summary>
        /// Editor options
        /// </summary>
        /// <param name="options">Options</param>
        void SaveEditorOptions(EditorOptions options);
    }
}
