using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Utac.Rnc2.BusinessObjects;
//using Utac.Rnc2.Repository.Nhibernate;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Fabriques;
using Utac.Rnc2.UI.Web.Helpers;

namespace Utac.Rnc2.UI.Web.Controllers
{
    public class EmailsController : RestrictedAcessController
    {

        private readonly IrTemplateMailService _rTemplateMailService;

        /* Constructeur par défaut */
        public EmailsController() : this(NhibernateServiceFactory.getInstance().getrTemplateMailService(true))
        {
        }

        public EmailsController(IrTemplateMailService rTemplateMailService)
        {
            this._rTemplateMailService = rTemplateMailService;

        }

        public ActionResult Index()
        {
            try
            {
                IList<rTemplateMail> liste = this._rTemplateMailService.FindAll();
                return View(liste);
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

        public ActionResult New()
        {
            try
            {
                rTemplateMail template = new rTemplateMail();
                return View(template);
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult New(FormCollection formulaire)
        {
            try
            {
                rTemplateMail template = new rTemplateMail();
                template.BodyTemplate = formulaire["BodyTemplate"];
                template.NomTemplate = formulaire["NomTemplate"];
                template.SujetTemplate = formulaire["SujetTemplate"];
                validateTemplate(template);
                if (ModelState.IsValid)
                {
                    template = this._rTemplateMailService.ajouter(template);
                    return RedirectToAction("Edit", new { id = template.Id });
                }
                return View(template);
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

        private void validateTemplate(rTemplateMail template)
        {
            if (string.IsNullOrWhiteSpace(template.NomTemplate))
            {
                ModelState.AddModelError("NomTemplate", "Le nom du template doit être saisi");
            }

            if (string.IsNullOrWhiteSpace(template.BodyTemplate))
            {
                ModelState.AddModelError("BodyTemplate", "Le corps du template doit être saisi");
            }

            if (string.IsNullOrWhiteSpace(template.SujetTemplate))
            {
                ModelState.AddModelError("SujetTemplate", "Le sujet du template doit être saisi");
            }

        }

        public ActionResult Edit(int id)
        {
            try
            {
                rTemplateMail template = this._rTemplateMailService.FindBy(id);
                return View(template);
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(rTemplateMail template)
        {
            try
            {
                validateTemplate(template);
                if (ModelState.IsValid)
                {
                    this._rTemplateMailService.modifier(template);
                    return RedirectToAction("Edit", new { id = template.Id });
                }
                return View(template);
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

    }
}
