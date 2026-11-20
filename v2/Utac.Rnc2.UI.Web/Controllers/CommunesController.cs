using System;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Utac.Rnc2.Contrats.Services;

namespace Utac.Rnc2.UI.Web.Controllers
{
    public class CommunesController : Controller
    {

        private readonly ICommuneInseeService _communeService;

        public CommunesController(ICommuneInseeService service)
        {
            this._communeService = service;
        }

        public CommunesController()
            : this(Utac.Rnc2.Fabriques.NhibernateServiceFactory.getInstance().getCommuneInseeService(false))
        {

        }

        public JsonResult Liste(string codePostal, string ville)
        {
            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            Array liste;
            if (!string.IsNullOrWhiteSpace(codePostal))
            {
                liste = this._communeService.findByCodePostalArray(codePostal);
            }
            else
            {
                liste = this._communeService.findByVilleArray(ville);
            }
            string strJSON = oSerializer.Serialize(liste);
            return Json(liste, "application/json", JsonRequestBehavior.AllowGet);
        }

    }
}
