using System.Web.Mvc;

namespace Utac.Rnc2.UI.Web.Controllers
{
    public class AccessDeniedController : Controller
    {
        //
        // GET: /AccessDenied/

        public ActionResult Index()
        {
            //this.Response.StatusCode = 403;
            return View();
        }

    }
}
