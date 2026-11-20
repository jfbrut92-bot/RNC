using System;
using System.Web.Mvc;

using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.UI.Web.Helpers;

namespace Utac.Rnc2.Pv.UI.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    if (User.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) || User.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) ||
                        User.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()) || User.IsInRole(rUtilisateur.Profil.DRIRE.ToString()) ||
                        User.IsInRole(rUtilisateur.Profil.DSCR.ToString()) || User.IsInRole(rUtilisateur.Profil.PIV.ToString()))
                    {
                        return View();
                    }
                    else
                    {
                        ViewData["title"] = "Index";
                        return View("~/Views/AccessDenied/Index.cshtml");
                    }
                }
                return Redirect("/Account/LogOn");
            }
            catch (Exception e)
            {
                //Gestion de l'erreur
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }
        // GET: /Home/Version/
        public ActionResult Version()
        {
            return View();
        }
    }
}
