//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Razor.TagHelpers;
//using One.Helpers;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace One.TagHelpers
//{

//    [HtmlTargetElement("menus")]
//    public class MenuTagHelper : TagHelper
//    {
//        private readonly NavigationHelper _helper;

//        private readonly IHttpContextAccessor _contextAccessor;

//        public MenuTagHelper(NavigationHelper helper, IHttpContextAccessor contextAccessor)
//        {
//            _helper = helper;
//            _contextAccessor = contextAccessor;

//        }

//        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
//        {
//            foreach (var item in table)
//            {
//                var url = item.Value.Url.Replace("~/", _contextAccessor.HttpContext.Request.PathBase);
//                var match = _contextAccessor.HttpContext.Request.Path.Value.Equals("/" + url, StringComparison.OrdinalIgnoreCase);
//                if (match)
//                {
//                    item.Value.IsActive = true;
//                    break;
//                }
//            }
//            return base.ProcessAsync(context, output);
//        }
//    }
//}
