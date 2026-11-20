using System;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using Utac.Rnc2.Infrastructure.Utilitaires;

namespace Utac.Rnc2.Pv.UI.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Session_Start()
        {
            // Pour eviter le bug de ctx.Session.IsNewSession toujours false dans ControllerFilterSessionExpiredAttribute
            Session["Dummy"] = 1;

            //EG le 16/11/2011 , Le mot de passe généré ne doit contenir que des chiffres et des lettres minuscules.taille= 8 , cf rnc2.config
            string allowedCaracters = System.Configuration.ConfigurationManager.AppSettings["password_allowed_chars"];
            int passwordLength = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["password_length"]);
            PasswordGenerator.getInstance(allowedCaracters, passwordLength);
        }

        protected void Session_End()
        {
            // Pour eviter le bug de ctx.Session.IsNewSession toujours false dans  ControllerFilterSessionExpiredAttribute
            Session.Clear();
        }

        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            if (HttpContext.Current.User != null)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    if (HttpContext.Current.User.Identity is FormsIdentity)
                    {
                        FormsIdentity fid = (FormsIdentity)Context.User.Identity;
                        FormsAuthenticationTicket t = fid.Ticket;
                        string[] roles = t.UserData.Split(';');
                        Context.User = new GenericPrincipal(fid, roles);
                    }
                }
            }
        }
    }
}
