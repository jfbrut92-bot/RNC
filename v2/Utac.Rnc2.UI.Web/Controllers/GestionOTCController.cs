using System.Web.Mvc;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Fabriques;
using Utac.Rnc2.UI.Web.Helpers;
using Utac.Rnc2.UI.Web.Models;

namespace Utac.Rnc2.UI.Web.Controllers
{
    public class GestionOTCController : RestrictedAcessController
    {

        private readonly IrReseauService _reseauService;

        public GestionOTCController() : this(NhibernateServiceFactory.getInstance().getrReseauService(false))
        {

        }

        public GestionOTCController(IrReseauService reseauService)
        {
            this._reseauService = reseauService;
        }

        //
        // GET: /GestionOTC/

        public ActionResult Index()
        {
            if (this.User.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()))
            {
                GestionOTCModel model = new GestionOTCModel();
                model.ListeReseau = this._reseauService.FindAllReseauSansAWZ();
                return View(model);
            }
            else
            {
                return ErrorHelper.InvaliAccessError(this.ControllerContext);
            }
        }

    }
}
