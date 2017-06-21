//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.AspNetCore.Mvc.ViewEngines;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Threading.Tasks;

//namespace OneBlog.Helpers
//{
//    public static class ControllerExtensions
//    {

//        public static string RenderViewToString(this Controller controller, string viewName, object model = null)
//        {
//            controller.ViewData.Model = model;
//            using (var sw = new StringWriter())
//            {
//                var viewResult = ViewEngine.Engines.FindPartialView(controller.ControllerContext, viewName);
//                var viewContext = new ViewContext(controller.ControllerContext, viewResult.View,
//                                             controller.ViewData, controller.TempData, sw);
//                viewResult.View.Render(viewContext, sw);
//                viewResult.ViewEngine.ReleaseView(controller.ControllerContext, viewResult.View);
//                return sw.GetStringBuilder().ToString();
//            }
//        }

//    }
//}
