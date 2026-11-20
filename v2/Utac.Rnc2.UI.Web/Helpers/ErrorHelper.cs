using System;
using System.Web;
using System.Web.Mvc;
using log4net;

namespace Utac.Rnc2.UI.Web.Helpers
{
    public class ErrorHelper
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ErrorHelper));

        public static ActionResult HandleError(Exception e, ControllerContext context, string errorViewName)
        {
            string controllerName = context.RouteData.GetRequiredString("controller");
            string actionName = context.RouteData.GetRequiredString("action");
            string loginName;
            try
            {
                loginName = context.HttpContext.User.Identity.Name;
            }
            catch (Exception ex)
            {
                loginName = "ANONYMOUS ?";
            }

            string stackTrace = e.StackTrace != null ? e.StackTrace.ToString() : "";

            //EG le 11/01/2012 on loggue ici
            log.Error(
                        string.Format("CONTROLLER = {0} , ACTION = {1} , LOGIN = {2} , ERREUR = {3}\r\nINNER EXCEPTION = {4}\r\nSTACK = {5}",
                                      controllerName, actionName, loginName, e.Message, e.InnerException, stackTrace)
                     );
            //EG le 09/01/2012 : on remonte les erreurs HTTP 404
            if (e is HttpException)
            {
                throw e;
            }

            HandleErrorInfo errorInfo = new HandleErrorInfo(e, controllerName, actionName);
            context.Controller.ViewData.Model = errorInfo;

            ViewResult r = new ViewResult();
            r.ViewData.Model = errorInfo;
            r.ViewName = errorViewName;

            return r;

        }

        public static ActionResult InvaliAccessError(ControllerContext context)
        {
            Exception e = new Exception("Vous n'êtes pas autorisé à exécuter cette action");
            string errorViewName = "Error";
            return HandleError(e, context, errorViewName);

        }

    }
}