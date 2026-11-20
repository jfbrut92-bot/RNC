using System.Collections.Generic;
using System.Web.Mvc;
using Utac.Rnc2.BusinessObjects;
//using Utac.Rnc2.Repository.Nhibernate;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Fabriques;

namespace Utac.Rnc2.UI.Web.Controllers
{
    public class ExempleController : RestrictedAcessController
    {
        #region "Services" utilisé
        private readonly IrUtilisateurService _rUtilisateurService;
        #endregion

        #region les deux Constructeurs
        public ExempleController(IrUtilisateurService rUtilisateurService)
        {
            this._rUtilisateurService = rUtilisateurService;

        }

        public ExempleController()
            : this(NhibernateServiceFactory.getInstance().getrUtilisateurService(false, true))
        {

        }

        #endregion

        //
        // GET: /Exemple/

        public ActionResult Index()
        {
            List<rUtilisateur> liste = this._rUtilisateurService.FindAll();

            return View(liste);
        }

        //
        // GET: /Exemple/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Exemple/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Exemple/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Exemple/Edit/5

        public ActionResult Edit(string id)
        {
            rUtilisateur user = this._rUtilisateurService.FindBy(id);
            return View(user);
        }

        //
        // POST: /Exemple/Edit/5

        [HttpPost]
        public ActionResult Edit(string id, FormCollection collection)
        {
            try
            {
                rUtilisateur user = this._rUtilisateurService.FindBy(id);
                user.DateMaj = System.DateTime.Now;
                this._rUtilisateurService.Save(user);
                this._rUtilisateurService.getUnitOfWork().Commit(); //C'est juste pour le test , sinon pas de commit ici
                return RedirectToAction("Index");
            }
            catch
            {
                //Les erreurs sont traitées ici
                return View();
            }
        }

        //
        // GET: /Exemple/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Exemple/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
