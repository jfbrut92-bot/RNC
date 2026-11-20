using System.Web.Mvc;
using log4net;

namespace Utac.Rnc2.MVC.Extensions
{

    public class LoggerFilter : ActionFilterAttribute
    {
        private readonly ILog log = LogManager.GetLogger(typeof(LoggerFilter));

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string actionName = filterContext.ActionDescriptor.ActionName;
            string rawUrl = string.Empty;
            string ip = string.Empty;
            string identityName = "?";
            string httpMethod = string.Empty;

            if (filterContext.HttpContext != null)
            {
                if (filterContext.HttpContext.Request != null)
                {
                    rawUrl = filterContext.HttpContext.Request.RawUrl;
                    httpMethod = filterContext.HttpContext.Request.HttpMethod;
                    ip = filterContext.HttpContext.Request.UserHostAddress;
                }

                if (filterContext.HttpContext.User != null && filterContext.HttpContext.User.Identity != null)
                {
                    identityName = filterContext.HttpContext.User.Identity.Name;
                }

            }

            log.Info(
                      string.Format("OnActionExecuting CONTROLEUR:{0}, ACTION:{1}, URL:{2}, METHOD:{3} Idendity:{4} IP:{5}",
                                                  controllerName,
                                                  actionName,
                                                  rawUrl,
                                                  httpMethod,
                                                  identityName,
                                                  ip

                      )
            );
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            //string controllerName = filterContext.Controller.ControllerContext.HttpContext
            //string actionName = filterContext.ActionDescriptor.ActionName;
            string rawUrl = string.Empty;
            string ip = string.Empty;
            string identityName = "?";

            if (filterContext.HttpContext != null)
            {
                if (filterContext.HttpContext.Request != null)
                {
                    rawUrl = filterContext.HttpContext.Request.RawUrl;
                    ip = filterContext.HttpContext.Request.UserHostAddress;
                }

                if (filterContext.HttpContext.User != null && filterContext.HttpContext.User.Identity != null)
                {
                    identityName = filterContext.HttpContext.User.Identity.Name;
                }

            }

            log.Info(
                      string.Format("OnResultExecuted DE URL:{0}, Idendity:{1}, IP:{2}",
                                                  rawUrl,
                                                  identityName,
                                                  ip

                      )
            );
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string actionName = filterContext.ActionDescriptor.ActionName;
            string rawUrl = string.Empty;
            string ip = string.Empty;
            string identityName = "?";

            if (filterContext.HttpContext != null)
            {
                if (filterContext.HttpContext.Request != null)
                {
                    rawUrl = filterContext.HttpContext.Request.RawUrl;
                    ip = filterContext.HttpContext.Request.UserHostAddress;
                }

                if (filterContext.HttpContext.User != null && filterContext.HttpContext.User.Identity != null)
                {
                    identityName = filterContext.HttpContext.User.Identity.Name;
                }

            }

            log.Info(
                      string.Format("OnActionExecuted CONTROLEUR:{0}, ACTION:{1}, URL:{2}, Idendity:{3}, IP:{4}",
                                                  controllerName,
                                                  actionName,
                                                  rawUrl,
                                                  identityName,
                                                  ip

                      )
            );
        }
    }
}
