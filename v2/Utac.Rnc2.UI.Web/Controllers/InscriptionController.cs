using System;
using System.Web;
using System.Web.Mvc;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Fabriques;
using Utac.Rnc2.Infrastructure.Utilitaires;
using Utac.Rnc2.MVC.Extensions.mail;
using Utac.Rnc2.MVC.Extensions.validator;
using Utac.Rnc2.UI.Web.Helpers;

namespace Utac.Rnc2.UI.Web.Controllers
{
    public class InscriptionController : RestrictedAcessController
    {

        private readonly IrUtilisateurService _utilisateurService;

        public InscriptionController(IrUtilisateurService utilisateurService)
        {
            this._utilisateurService = utilisateurService;
        }

        public InscriptionController() : this(NhibernateServiceFactory.getInstance().getrUtilisateurService(true, true))
        {
        }

        private bool isLoginTemporaire()
        {
            HttpCookie cookie = this.Request.Cookies["logOnType"];
            if (cookie != null)
            {
                if (rUtilisateur.LogonType.TEMPORAIRE.ToString().Equals(cookie.Value))
                {
                    return true;
                }
            }

            return false;
        }
        //
        // GET: /Inscription/

        public ActionResult Index()
        {
            try
            {
                if (!this.User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("TempLogOn", "Account");
                }
                //EG le 09/11/2011, connecté mais login temporaire ?
                if (!isLoginTemporaire())
                {
                    Exception e = new Exception("Vous n'êtes pas autorisé....");
                    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                }

                //etape 0/4
                return View();
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // Gestion des inscriptions des centres nouvellement notifiés
        // GET: /Inscription/Etape1
        public ActionResult Etape1()
        {
            try
            {
                if (!this.User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("TempLogOn", "Account");
                }
                //EG le 09/11/2011, connecté mais login temporaire ?
                if (!isLoginTemporaire())
                {
                    Exception e = new Exception("Vous n'êtes pas autorisé....");
                    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                }

                //Afichage de l'assistant
                //EG le 16/11/2011 , pour afficher les données déjà saisies en cas d'inscription multiple tentive par exemple
                rUtilisateur u = this._utilisateurService.FindBy(this.User.Identity.Name);
                return View(u);

            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // POST: /Inscription/Etape1
        [HttpPost]
        public ActionResult Etape1(rUtilisateur form)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(form.Nom))
                {
                    this.ModelState.AddModelError("Nom", "Le nom est obligatoire");
                }

                if (string.IsNullOrWhiteSpace(form.Prenom))
                {
                    this.ModelState.AddModelError("Prenom", "Le prénom est obligatoire");
                }

                if (this.ModelState.IsValid)
                {
                    //On sauvegarde les données saisies
                    this._utilisateurService.updateIdentite(this.User.Identity.Name, form.Nom.ToUpper().Trim(), form.Prenom.Trim(), this.User.Identity.Name);
                    return RedirectToAction("Etape2");
                }
                else
                {
                    return View(form);
                }
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // GET: /Inscription/Etape2
        public ActionResult Etape2()
        {
            try
            {
                if (!this.User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("TempLogOn", "Account");
                }
                //EG le 09/11/2011, connecté mais login temporaire ?
                if (!isLoginTemporaire())
                {
                    Exception e = new Exception("Vous n'êtes pas autorisé....");
                    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                }
                //Afichage de l'assistant
                //EG le 16/11/2011 , pour afficher les données déjà saisies en cas d'inscription multiple tentive par exemple
                rUtilisateur u = this._utilisateurService.FindBy(this.User.Identity.Name);
                return View(u);

            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // POST: /Inscription/Etape2
        [HttpPost]
        public ActionResult Etape2(rUtilisateur form)
        {
            try
            {
                if (string.IsNullOrEmpty(form.Email))
                {
                    this.ModelState.AddModelError("Email", "L'email est obligatoire.");
                }
                else
                {
                    if (!Validator.IsValidEmail(form.Email))
                    {
                        ModelState.AddModelError("Email", "L'email n'est pas valide.");
                    }
                }

                if (this.ModelState.IsValid)
                {
                    //On sauvegarde les données saisies
                    this._utilisateurService.updateEmail(this.User.Identity.Name, form.Email, this.User.Identity.Name);
                    return RedirectToAction("Etape3");
                }
                else
                    return View(form);
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // GET: /Inscription/Etape3
        public ActionResult Etape3()
        {
            try
            {
                if (!this.User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("TempLogOn", "Account");
                }
                //EG le 09/11/2011, connecté mais login temporaire ?
                if (!isLoginTemporaire())
                {
                    Exception e = new Exception("Vous n'êtes pas autorisé....");
                    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                }
                //Afichage de l'assistant//EG le 16/11/2011 , pour afficher les données déjà saisies en cas d'inscription multiple tentive par exemple
                rUtilisateur u = this._utilisateurService.FindBy(this.User.Identity.Name);
                return View(u);

            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // POST: /Inscription/Etape3
        [HttpPost]
        public ActionResult Etape3(rUtilisateur form)
        {
            try
            {
                if (string.IsNullOrEmpty(form.NumTel))
                {
                    this.ModelState.AddModelError("Telephone", "Le telephone est obligatoire");
                }

                if (this.ModelState.IsValid)
                {
                    rUtilisateur u = this._utilisateurService.FindBy(this.User.Identity.Name);

                    //On sauvegarde les données saisies
                    this._utilisateurService.updateTelephone(this.User.Identity.Name, form.NumTel, this.User.Identity.Name);

                    //EG le 16/11/2011: on génère un nouveau mot de passe si non null
                    //On génère un nouveau mot de passe
                    string motPasse = null;

                    if (string.IsNullOrWhiteSpace(u.MotDePasse))
                    {
                        //On génère un nouveau mot de passe
                        motPasse = PasswordGenerator.getInstance().GenerateForDesizedSize();
                        //EG le 03/01/2012 : mot de passe en minuscule
                        motPasse = motPasse.ToLower();
                        this._utilisateurService.updateMotPasse(this.User.Identity.Name, motPasse, this.User.Identity.Name);
                    }
                    else
                    {
                        motPasse = u.MotDePasse;
                    }

                    //On rend le user actif
                    this._utilisateurService.setActif(this.User.Identity.Name, this.User.Identity.Name);

                    //Envoi d'un mail
                    /*string objet = "Votre inscription à RNC2";
                    string message = string.Format("Votre inscription à RNC2 a été prise en compte\n USER = {0} \nPWD = {1}" , this.User.Identity.Name,  motPasse);
                     */
                    rUtilisateur declencheur = InformationHelper.getInformationPersonnelle(false).Utilisateur;
                    string logoPath = Server.MapPath("~/Ressources/logoOTC.gif");
                    MailTemplateSender sender = new MailTemplateSender(declencheur, declencheur, rTemplateMail.TemplateEnum.INSCRIPTION_CENTRE_INDEPENDANT, logoPath);

                    if (sender.Send())
                    {
                        TempData["Message"] = "Inscription prise en compte ... email envoyé";
                        TempData["MessageType"] = "Success";
                        return RedirectToAction("Etape4");

                    }
                    else
                    {
                        TempData["Message"] = "Inscription prise en compte ...MAIS ECHEC ENVOI MAIL !";
                        TempData["MessageType"] = "Error";
                        return RedirectToAction("InscriptionKO");
                    }

                }
                else
                    return View(form);
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // GET: /Inscription/Etape4
        public ActionResult Etape4()
        {
            try
            {
                if (!this.User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("TempLogOn", "Account");
                }
                //EG le 09/11/2011, connecté mais login temporaire ?
                if (!isLoginTemporaire())
                {
                    Exception e = new Exception("Vous n'êtes pas autorisé....");
                    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                }

                //Afichage de l'assistant//EG le 16/11/2011 , pour afficher les données déjà saisies en cas d'inscription multiple tentive par exemple
                rUtilisateur u = this._utilisateurService.FindBy(this.User.Identity.Name);
                return View(u);

            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // GET: /Inscription/InscriptionKO
        public ActionResult InscriptionKO()
        {
            try
            {

                //Afichage de l'assistant//EG le 16/11/2011 , pour afficher les données déjà saisies en cas d'inscription multiple tentive par exemple
                rUtilisateur u = this._utilisateurService.FindBy(this.User.Identity.Name);
                return View(u);

            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

    }
}
