using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Microsoft.AspNetCore.Mvc
{
    /// <summary>Represents an attribute that is used to indicate that an action method should be called only as a child action.</summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class ChildActionOnlyAttribute : ActionFilterAttribute, IAuthorizationFilter
    {
        /// <summary>Called when authorization is required.</summary>
        /// <param name="filterContext">An object that encapsulates the information that is required in order to authorize access to the child action.</param>

        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

        }
    }
}
