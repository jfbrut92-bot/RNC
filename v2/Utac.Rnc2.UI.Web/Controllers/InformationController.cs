using System;
using System.Web.Mvc;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Fabriques;
using Utac.Rnc2.Infrastructure.Utilitaires;
using Utac.Rnc2.MVC.Extensions.mail;
using Utac.Rnc2.MVC.Extensions.validator;
using Utac.Rnc2.UI.Web.Helpers;
using Utac.Rnc2.UI.Web.Models;

namespace Utac.Rnc2.UI.Web.Controllers
{
    [Authorize()]
    public class InformationController : RestrictedAcessController
    {

        private readonly IrUtilisateurService _service;

        public InformationController(IrUtilisateurService service)
        {
            this._service = service;
        }

        public InformationController()
            : this(NhibernateServiceFactory.getInstance().getrUtilisateurService(true, true))
        {

        }

        //
        // GET: /Information/
        public ActionResult Index()
        {
            /*EG le 20/01/2012 : force = true pour éviter le bug suivant :Initializing[Utac.Rnc2.BusinessObjects.rPrefecture#45]-Could not initialize proxy - no Session. 
            en cas de changement de mot de passe*/
            InformationPersonelle info = InformationHelper.getInformationPersonnelle(true);
            return View(info);
        }

        //
        // GET: /RenewPassword0/
        //public ActionResult RenewPassword0()
        //{
        //    return View();
        //}

        //
        // GET: /RenewPassword1/
        //public ActionResult RenewPassword1()
        //{
        //    InformationPersonelle info = InformationHelper.getInformationPersonnelle(false);
        //    return View(info);
        //}

        //
        // POST: /RenewPassword1/
        [HttpPost]
        //public ActionResult RenewPassword1(InformationPersonelle form)
        //{
        //    InformationPersonelle info = InformationHelper.getInformationPersonnelle(false);
        //    //On récupère le user
        //    //rUtilisateur u = this._controleurStatistiqueService.FindBy(info.Id);

        //    this.validateUserInfo(form, this.ModelState);
        //    if (ModelState.IsValid)
        //    {
        //        //Mise a jour de l'email
        //        if (!info.Email.Equals(form.Email))
        //        {
        //            /*
        //            u.Email = form.Email;
        //            this._controleurStatistiqueService.Save(u);
        //            this._controleurStatistiqueService.getUnitOfWork().Commit();
        //             */
        //            info.Email = form.Email; //ple 18/11/2011 Sauvegarde du changement d'email dans la variable de session
        //        }

        //        return RedirectToAction("RenewPassword2");
        //    }

        //    return View();
        //}

        //
        // GET: /RenewPassword2/
        //public ActionResult RenewPassword2()
        //{
        //    return View();
        //}

        //
        // POST: /RenewPassword2/
        //[HttpPost]
        //public ActionResult RenewPassword2(string motPasse)
        //{
        //    try
        //    {
        //        string erreur = "";
        //        InformationPersonelle info = InformationHelper.getInformationPersonnelle(false);
        //        //On récupère le user
        //        rUtilisateur u = this._service.FindBy(info.Id);

        //        //Vérification motPasse
        //        if (u.MotDePasse.Equals(motPasse))
        //        {
        //            //Mise a jour du mot de passe
        //            string newPassword = PasswordGenerator.getInstance().GenerateForDesizedSize();
        //            //EG le 03/12/2012 : mot de passe en minuscule
        //            newPassword = newPassword.ToLower();

        //            u.MotDePasse = newPassword;
        //            u.Email = info.Email; //ple 18/11/2011 Sauvegarde du changement d'email éventuel
        //            this._service.Save(u);
        //            this._service.getUnitOfWork().Commit();
        //            InformationHelper.initInformationPersonnelle(); //ple 18/11/2011 Réinitialisation de la variable de session

                    //Envoi de mail
                    /* EG commenté le 11/01/2012 pour nouvelle méthode envoi de mail par template
                    string message = string.Format("un nouveau mot de passe a été généré à votre demande : {0}", newPassword);
                    string objet = "Rnc2 : Changement de votre mot de passe d'accès à Rnc2";
                    if (Mailer.Send(objet,message, u.Email))
                    */
        //            rUtilisateur declencheur = InformationHelper.getInformationPersonnelle(false).Utilisateur;
        //            string logoPath = Server.MapPath("~/Ressources/logoOTC.gif");
        //            MailTemplateSender sender = new MailTemplateSender(declencheur, u, rTemplateMail.TemplateEnum.NOUVEAU_PASSWORD, logoPath);
        //            TempData["Message"] = "Mot de passe changé avec succès...";
        //            TempData["MessageType"] = "Success";

        //            if (sender.Send())
        //            {
        //                TempData["Message"] += "email envoyé";
        //            }
        //            else
        //            {
        //                TempData["Message"] += "MAIS ECHEC ENVOI MAIL !";
        //            }
        //            //Redirection vers la page par défaut
        //            return RedirectToAction("Index");

        //        }
        //        else
        //            erreur = "Mot de passe incorrect! : Veuillez saisir le mot de passe que vous utilisez actuellement lors de votre connexion.";

        //        //Redirection vers la page d'Index avec notification du changement de mot de passe
        //        ViewData["erreur"] = erreur;
        //        return View();
        //    }
        //    catch (Exception e)
        //    {
        //        return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
        //    }
        //}

        private void validateUserInfo(InformationPersonelle form, ModelStateDictionary ModelState)
        {

            if (string.IsNullOrWhiteSpace(form.Email))
            {
                ModelState.AddModelError("Email", "L'email est obligatoire");
            }
            else
            {
                if (!Validator.IsValidEmail(form.Email))
                {
                    ModelState.AddModelError("Email", "L'email n'est pas valide");
                }
            }

        }

        public ActionResult Support()
        {

            InformationPersonelle info = InformationHelper.getInformationPersonnelle(true);
            return View(info);
        }

    }
}
