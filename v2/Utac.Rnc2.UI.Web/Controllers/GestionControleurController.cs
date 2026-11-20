using System.Web.Mvc;
//using Utac.Rnc2.Repository.Nhibernate;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Fabriques;

namespace Utac.Rnc2.UI.Web.Controllers
{
    public class GestionControleurController : Controller
    {
        //
        // GET: /GestionControleur/

        private readonly IpControleurEnCourService _controleurTechniqueService;
        //private IUnitOfWork _unitOfWork = new NhibernateUnitOfWork();

        public GestionControleurController()
        {
            this._controleurTechniqueService = NhibernateServiceFactory.getInstance().getpControleurEnCourService(false);

        }

        public GestionControleurController(IpControleurEnCourService controleurTechniqueService)
        {
            this._controleurTechniqueService = controleurTechniqueService;
        }

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /GestionControleur/Edit

        public ActionResult Edit(int id)
        {
            pContoleurEncour c = this._controleurTechniqueService.FindBy(id);

            return View(c);
        }

        //
        // POST: /GestionControleur/Edit
        [HttpPost]
        public ActionResult Edit(pContoleurEncour cForm)
        {
            pContoleurEncour cInitial = this._controleurTechniqueService.FindBy(cForm.Id);
            //cInitial.Nom = cForm.Nom;
            //            this._controleurTechniqueService.Save(cInitial);
            //this._controleurTechniqueService.SaveWithHisto(cInitial,cForm);
            return RedirectToAction("Edit", new { id = cForm.Id });
        }

    }
}
