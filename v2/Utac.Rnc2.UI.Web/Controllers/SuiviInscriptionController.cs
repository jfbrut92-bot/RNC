using System.Collections.Generic;
using System.Web.Mvc;
using Utac.Rnc2.BusinessObjects;
//using Utac.Rnc2.Repository.Nhibernate;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Fabriques;

namespace Utac.Rnc2.UI.Web.Controllers
{
    public class SuiviInscriptionController : RestrictedAcessController
    {

        private readonly IrUtilisateurService _utilisateurService;

        public SuiviInscriptionController(IrUtilisateurService service)
        {
            this._utilisateurService = service;
        }

        public SuiviInscriptionController() : this(NhibernateServiceFactory.getInstance().getrUtilisateurService(false, false))
        {
        }

        //
        // GET: /GestionOTC/

        public ActionResult Index()
        {
            IEnumerable<rUtilisateur> liste = this._utilisateurService.FindAllTemporaire();
            return View(liste);
        }

    }
}
