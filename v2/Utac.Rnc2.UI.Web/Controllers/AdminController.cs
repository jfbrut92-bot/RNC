using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Services;
//using Utac.Rnc2.Repository.Nhibernate;
using Utac.Rnc2.Fabriques;
using Utac.Rnc2.UI.Web.Helpers;

namespace Utac.Rnc2.UI.Web.Controllers
{
    public class AdminController : RestrictedAcessController
    {
        private readonly IrMessageUtilisateurService _messageService;
        private readonly IParametreService _parametreService;

        public AdminController(IrMessageUtilisateurService service, IParametreService parametreService)
        {
            this._messageService = service;
            this._parametreService = parametreService;
        }

        public AdminController() : this(
            NhibernateServiceFactory.getInstance().getrMessageUtilisateurService(false),
            NhibernateServiceFactory.getInstance().getParametreService(true)
            )
        {
        }

        //
        // GET: /Admin/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Message/

        public ActionResult Message()
        {
            rMessageUtilisateur message = new rMessageUtilisateur();
            List<rMessageUtilisateur> result = this._messageService.FindAll();
            if (result.Count > 0)
                message = this._messageService.FindAll().ElementAt<rMessageUtilisateur>(0);

            return View(message);
        }

        //
        // POST: /Message/
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Message(FormCollection form)
        {
            rMessageUtilisateur message = new rMessageUtilisateur();
            List<rMessageUtilisateur> result = this._messageService.FindAll();
            if (result.Count > 0)
                message = this._messageService.FindAll().ElementAt<rMessageUtilisateur>(0);

            message.TxtMessageBienvenue = form["TxtMessageBienvenue"];
            this._messageService.Save(message);
            return RedirectToAction("Message");
        }

        //EG le 10/10/2011 : Liste des paramètres
        // GET: /Admin/ListeParametres

        public ActionResult ListeParametres()
        {

            try
            {
                IList<Parametre> liste = this._parametreService.FindAll();

                return View(liste);

            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

        // GET: /Admin/CreateParametre
        public ActionResult CreateParametre()
        {

            try
            {
                return View(new Parametre());

            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

        // POST: /Admin/CreateParametre
        [HttpPost]
        public ActionResult CreateParametre(Parametre p)
        {

            try
            {
                validateParametre(p);
                if (ModelState.IsValid)
                {
                    this._parametreService.Save(p);
                }
                return View(p);

            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

        // GET: /Admin/EditParametre
        public ActionResult EditParametre(int id)
        {

            try
            {
                Parametre p = this._parametreService.FindBy(id);
                return View(p);

            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

        // POST: /Admin/EditParametre
        [HttpPost]
        public ActionResult EditParametre(int id, Parametre p)
        {

            try
            {
                validateParametre(p);
                if (ModelState.IsValid)
                {
                    this._parametreService.modifyParametre(id, p);
                }
                return View(p);

            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

        // GET: /Admin/ConfirmerDeleteParametre
        public ActionResult ConfirmerDeleteParametre(int id)
        {

            try
            {
                Parametre p = this._parametreService.FindBy(id);
                return View(p);

            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

        // POST: /Admin/DeleteParametre
        [HttpPost]
        public ActionResult DeleteParametre(int id)
        {

            try
            {
                this._parametreService.suppression(id);

                return RedirectToAction("ListeParametres");

            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

        /// <summary>
        /// Permet de valider les données saisies pour le paramètre en entrée
        /// </summary>
        /// <param name="p"></param>

        private void validateParametre(Parametre p)
        {
            if (string.IsNullOrEmpty(p.Nom))
            {
                ModelState.AddModelError("Nom", "Le nom du paramètre est obligatoire");
            }

            if (string.IsNullOrEmpty(p.Valeur))
            {
                ModelState.AddModelError("Valeur", "La valeur du paramètre est obligatoire");
            }

        }

    }
}
