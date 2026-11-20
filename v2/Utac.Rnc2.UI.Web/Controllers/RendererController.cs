using System;
using System.IO;
using System.Web.Mvc;

namespace Utac.Rnc2.UI.Web.Controllers
{
    public abstract class RendererController : RestrictedAcessController
    {
        protected string RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return string.Join(Environment.NewLine, sw.GetStringBuilder().ToString()
                    .Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries));
            }
        }

        protected string RenderViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindView(ControllerContext, viewName, null);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return string.Join(Environment.NewLine, sw.GetStringBuilder().ToString()
                    .Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries));
            }
        }

        protected string RenderViewToString(object model) => RenderViewToString(null, model);
    }
}
