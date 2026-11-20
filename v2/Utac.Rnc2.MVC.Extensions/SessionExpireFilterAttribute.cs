using System.Web;
using System.Web.Mvc;

namespace Utac.Rnc2.MVC.Extensions
{

    public class SessionExpireFilterAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;

            if (!string.IsNullOrEmpty(ctx.Request.CurrentExecutionFilePathExtension))
            {
                if (!ctx.Request.CurrentExecutionFilePathExtension.Contains("aspx"))
                {
                    return;
                }
            }

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
                            redirectTo = string.Format("~/Account/Logon?ReturnUrl={0}&test=2", HttpUtility.UrlEncode(ctx.Request.RawUrl));
                        }
                        filterContext.Result = new RedirectResult(redirectTo);
                    }
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}