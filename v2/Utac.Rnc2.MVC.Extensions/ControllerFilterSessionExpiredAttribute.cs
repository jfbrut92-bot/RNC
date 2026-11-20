using System;
using System.Web;
using System.Web.Mvc;

namespace Utac.Rnc2.MVC.Extensions
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ControllerFilterSessionExpiredAttribute : System.Web.Mvc.ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;

            // check if session is supported
            if (ctx.Session != null)
            {

                // check if a new session id was generated
                if (ctx.Session.IsNewSession)
                {

                    // If it says it is a new session, but an existing cookie exists, then it must
                    // have timed out
                    string sessionCookie = ctx.Request.Headers["Cookie"];
                    if ((null != sessionCookie) && (sessionCookie.IndexOf("ASP.NET_SessionId") >= 0))
                    {
                        string redirectTo = "~/Account/Logon";

                        if (!string.IsNullOrEmpty(ctx.Request.RawUrl))
                        {
                            //EG le 19/10/2011 , pour éviter de faire un return sur post de formulaire et session expirée
                            string returnUrl = "/Home";
                            if ("GET".Equals(ctx.Request.HttpMethod))
                            {
                                returnUrl = ctx.Request.RawUrl;
                            }
                            redirectTo = string.Format("~/Account/Logon?ReturnUrl={0}&Raison=Session_expire", HttpUtility.UrlEncode(returnUrl));
                        }
                        filterContext.Result = new RedirectResult(redirectTo);
                    }
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
