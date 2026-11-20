using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Fabriques;
using Utac.Rnc2.Infrastructure.Search;
using Utac.Rnc2.Infrastructure.Utilitaires;
using Utac.Rnc2.MVC.Extensions;
using Utac.Rnc2.MVC.Extensions.mail;
using Utac.Rnc2.MVC.Extensions.validator;
using Utac.Rnc2.UI.Web.Helpers;
using Utac.Rnc2.UI.Web.Models;

namespace Utac.Rnc2.UI.Web.Controllers
{
    [LoggerFilter()]
    public class HomeController : Controller
    {
        private readonly IrMessageUtilisateurService _messageService;
        private readonly IrUtilisateurService _utilisateurService;
        private readonly IpControleurEnCourService _controleurService;
        private readonly IpCentreEncourService _centreService;
        private readonly IrReseauService _reseauService;
        private readonly IpInfoRdvService _pInfoRdvService;

        public HomeController(IrMessageUtilisateurService service, IrUtilisateurService utilisateurService, IpControleurEnCourService controleurService, IpCentreEncourService centreService, IrReseauService reseauService, IpInfoRdvService pInfoRdvService)
        {
            this._messageService = service;
            this._utilisateurService = utilisateurService;
            this._controleurService = controleurService;
            this._centreService = centreService;
            this._reseauService = reseauService;
            this._pInfoRdvService = pInfoRdvService;
        }

        public HomeController() : this(
            NhibernateServiceFactory.getInstance().getrMessageUtilisateurService(false),
            NhibernateServiceFactory.getInstance().getrUtilisateurService(false, false),
            NhibernateServiceFactory.getInstance().getpControleurEnCourService(false),
            NhibernateServiceFactory.getInstance().getpCentreEncourService(false, true),
            NhibernateServiceFactory.getInstance().getrReseauService(false),
            NhibernateServiceFactory.getInstance().getpInfoRdvService(true)
            )
        {
        }

        private HomeModel makeModel()
        {

            HomeModel model = new HomeModel();

            if (this.User.Identity.IsAuthenticated)
            {
                //EG le 18/02/2012...déplacé ici
                //On récupère le user de la base
                rUtilisateur utilisateur = this._utilisateurService.FindBy(this.User.Identity.Name);
                model.utilisateur = utilisateur;

                //EG le 17/11/2011 , ajout de la vérification du profil "CENTRE_INDEPENDANT"
                if (this.User.IsInRole(rUtilisateur.Profil.CENTRE_INDEPENDANT.ToString()))
                {
                    //on recupère la liste des controleurs d'un centre
                    if (utilisateur.MonCentreEstVL)
                    {
                        model.controleur = this._controleurService.findAllVL(utilisateur.MonCentre);
                    }
                    if (utilisateur.MonCentreEstPL)
                    {
                        model.controleur = this._controleurService.findAllPL(utilisateur.MonCentre);
                    }
                    if (utilisateur.MonCentreEstCL)
                    {
                        model.controleur = this._controleurService.findAllCL(utilisateur.MonCentre);
                    }
                }

                //EG le 18/11/2011
                if (this.User.IsInRole(rUtilisateur.Profil.RESEAU.ToString()))
                {
                    model.RechercheProfilReseauModel = new RechercheProfilReseauModel();
                    model.RechercheProfilReseauModel.Filtre = new FiltreRechercheProfilReseau();

                    Boolean bReseauVL = false;
                    Boolean bReseauCL = false;
                    Boolean bReseauPL = false;
                    foreach (var r in utilisateur.RReseaux)
                    {
                        if (r.TypeReseauVl && !bReseauVL)
                        {
                            bReseauVL = true;
                            model.ListeControleurAgreeCentreFermeVL = this._controleurService.FindAllAgreesCentreFermeReseau(rCategorieVehicule.CategorieEnum.VL, model.utilisateur.RReseaux);
                        }
                        if (r.TypeReseauCl && !bReseauCL)
                        {
                            bReseauCL = true;
                            model.ListeControleurAgreeCentreFermeCL = this._controleurService.FindAllAgreesCentreFermeReseau(rCategorieVehicule.CategorieEnum.CL, model.utilisateur.RReseaux);
                        }
                        if (r.TypeReseauPl && !bReseauPL)
                        {
                            bReseauPL = true;
                            model.ListeControleurAgreeCentreFermePL = this._controleurService.FindAllAgreesCentreFermeReseau(rCategorieVehicule.CategorieEnum.PL, model.utilisateur.RReseaux);
                        }

                    }

                    //EG le 06/12/2011
                    if (this.User.IsInRole(rUtilisateur.Profil.RESEAU_Administrateur.ToString()) || this.User.IsInRole(rUtilisateur.Profil.RESEAU_Gestionnaire.ToString()))
                    {
                        //Recherche des centres pour accuse réception
                        foreach (var r in utilisateur.RReseaux)
                        {
                            model.ListeCentreAccuseReception.Add(r, this._centreService.findAllCentrePourAccuseReception(r));
                        }

                    }
                }

                //EG le 17/01/2012 : page d'accueil des utilisateur de profil PREFECTURE
                if (this.User.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()))
                {
                    model.ListeAutresUtilisateurs = this._utilisateurService.FindAllUser(model.utilisateur.RPrefecture);
                    model.ListeControleurAgreeCentreFermeVL = this._controleurService.FindAllAgreesCentreFerme(rCategorieVehicule.CategorieEnum.VL, model.utilisateur.RPrefecture);
                    model.ListeControleurAgreeCentreFermePL = this._controleurService.FindAllAgreesCentreFerme(rCategorieVehicule.CategorieEnum.PL, model.utilisateur.RPrefecture);
                    model.ListeControleurAgreeCentreFermeCL = this._controleurService.FindAllAgreesCentreFerme(rCategorieVehicule.CategorieEnum.CL, model.utilisateur.RPrefecture);
                    model.ListeControleurAttenteAgrement = this._controleurService.FindAllDemandeEnCours(model.utilisateur.RPrefecture);
                    model.ListeCentreAttenteAgrement = this._centreService.FindAllDemandeEnCours(model.utilisateur.RPrefecture);
                    model.ListeControleurNouvellementAgree = this._controleurService.FindAllNouvellementAgree(model.utilisateur.RPrefecture);
                    model.ListeCentreNouvellementAgree = this._centreService.FindAllNouvellementAgree(model.utilisateur.RPrefecture);

                    model.ListeControleurAgreeSansCentreRattachementVL = this._controleurService.FindAllAgreesSansCentreRattachement(rCategorieVehicule.CategorieEnum.VL, model.utilisateur.RPrefecture);

                    model.ListeControleurAgreeSansCentreRattachementPL = this._controleurService.FindAllAgreesSansCentreRattachement(rCategorieVehicule.CategorieEnum.PL, model.utilisateur.RPrefecture);

                    model.ListeControleurAgreeSansCentreRattachementCL = this._controleurService.FindAllAgreesSansCentreRattachement(rCategorieVehicule.CategorieEnum.CL, model.utilisateur.RPrefecture);

                    model.ListePrestatairesEuropeensVL = this._controleurService.FindAllPrestatairesEuropeensVL(model.utilisateur.RPrefecture);
                    model.ListePrestatairesEuropeensPL = this._controleurService.FindAllPrestatairesEuropeensPL(model.utilisateur.RPrefecture);
                }

                //YM le 25/01/2012 : page d'accueil des utilisateurs de profil DRIRE
                if (this.User.IsInRole(rUtilisateur.Profil.DRIRE.ToString()))
                {
                    model.ListeAutresUtilisateurs = this._utilisateurService.FindAllUserRegionProfil(model.utilisateur.RRegion, model.utilisateur.RProfil, model.utilisateur.RProfil2);
                    model.ListeControleurAgreeCentreFermeVL = this._controleurService.FindAllAgreesCentreFermeRegion(rCategorieVehicule.CategorieEnum.VL, model.utilisateur.RRegion);
                    model.ListeControleurAgreeCentreFermePL = this._controleurService.FindAllAgreesCentreFermeRegion(rCategorieVehicule.CategorieEnum.PL, model.utilisateur.RRegion);
                    model.ListeControleurAgreeCentreFermeCL = this._controleurService.FindAllAgreesCentreFermeRegion(rCategorieVehicule.CategorieEnum.CL, model.utilisateur.RRegion);
                    DateTime datedeb = System.DateTime.Today;
                    int daydeb = datedeb.Day;
                    datedeb = System.DateTime.Today.AddDays(-daydeb);
                    datedeb = datedeb.AddMonths(-1);
                    string datedebmois = string.Format("{0000}", datedeb.Year) + '/' + string.Format("{00}", datedeb.Month) + '/' + string.Format("{00}", datedeb.Day);
                    model.ListeCentreNouvellementAgree = this._centreService.FindAllCentreNouvellementAgreeRegion(model.utilisateur.RRegion, datedebmois);
                    datedebmois = string.Format("{00}", datedeb.Day) + '/' + string.Format("{00}", datedeb.Month) + '/' + string.Format("{0000}", datedeb.Year);
                    model.datedeb = datedebmois;
                    model.ListeCentreAgreeSansControleur = this._centreService.FindAllCentreAgreeSansControleurRegion(model.utilisateur.RRegion);

                }
            }
            else
            {
                //EG le 18/02/2012...déplacé ici
                rMessageUtilisateur message = new rMessageUtilisateur();
                List<rMessageUtilisateur> result = this._messageService.FindAll();
                if (result.Count > 0)
                    //23/08/2021 PLE Deux appels de la methode de récupération du message d'acceuil ????
                    //message = this._messageService.FindAll().ElementAt<rMessageUtilisateur>(0);
                    message = result.ElementAt<rMessageUtilisateur>(0);
                model.message = message;

            }

            return model;
        }
        //
        // GET: /Home/
        public ActionResult Index()
        {
            try
            {
                HomeModel model = makeModel();
                return View(model);
            }
            catch (Exception e)
            {
                //Gestion de l'erreur
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        //
        // POST: /Home/ListeCentrePourReseau
        [HttpPost]
        public ActionResult ListeCentrePourReseau(HomeModel form)
        {
            try
            {
                Boolean estDepartementErrone = false;
                rUtilisateur user = this._utilisateurService.FindBy(this.User.Identity.Name);
                form.utilisateur = user;
                form.RechercheProfilReseauModel.ReseauCourant = this._reseauService.FindBy(form.RechercheProfilReseauModel.Filtre.CodeReseau);
                form.RechercheProfilReseauModel.ListeCentres = this._centreService.FindByFiltreProfilReseau(form.RechercheProfilReseauModel.Filtre, out estDepartementErrone);
                if (estDepartementErrone)
                {
                    ModelState.AddModelError("RechercheProfilReseauModel.Filtre.CodeDepartement", "Liste des centres : le numéro du département est erroné");
                }
                //PLE le 24/01/2024 Contrôle de l'autorisation du réseau.
                if (!GestionDroitsExecution.isUtilisateurAppartientReseau(this.User, form.RechercheProfilReseauModel.ReseauCourant))
                {
                    return ErrorHelper.InvaliAccessError(this.ControllerContext);
                }

                return View(form);
            }
            catch (Exception e)
            {
                //Gestion de l'erreur
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

        //
        // POST: /Home/ListeControleurPourReseau
        [HttpPost]
        public ActionResult ListeControleurPourReseau(HomeModel form)
        {

            //code département obligatoire pour la liste des contrôleurs
            if (String.IsNullOrWhiteSpace(form.RechercheProfilReseauModel.Filtre.CodeDepartement))
            {
                ModelState.AddModelError("RechercheProfilReseauModel.Filtre.CodeDepartement", "Liste des contrôleurs : le numéro du département est obligatoire");
            }

            try
            {
                rUtilisateur user = this._utilisateurService.FindBy(this.User.Identity.Name);
                form.utilisateur = user;
                form.RechercheProfilReseauModel.ReseauCourant = this._reseauService.FindBy(form.RechercheProfilReseauModel.Filtre.CodeReseau);
                if (ModelState.IsValid)
                {
                    Boolean estDepartementErrone = false;
                    form.RechercheProfilReseauModel.ListeControleurs = this._controleurService.FindByFiltreProfilReseau(form.RechercheProfilReseauModel.Filtre, out estDepartementErrone);
                    if (estDepartementErrone)
                    {
                        ModelState.AddModelError("RechercheProfilReseauModel.Filtre.CodeDepartement", "Liste des contrôleurs  : le numéro du département est erroné");
                    }
                }

                //PLE le 24/01/2024 Contrôle de l'autorisation du réseau.
                if (!GestionDroitsExecution.isUtilisateurAppartientReseau(this.User, form.RechercheProfilReseauModel.ReseauCourant))
                {
                    return ErrorHelper.InvaliAccessError(this.ControllerContext);
                }

                return View(form);
            }
            catch (Exception e)
            {
                //Gestion de l'erreur
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

        //
        // POST: /Home/AccuseReception
        [HttpPost]
        public ActionResult AccuseReception(AccuseReceptionModel form)
        {
            try
            {
                validateAccuseReception(form);
                if (ModelState.IsValid)
                {
                    pCentreEncour centre = this._centreService.FindBy(form.CentreId);
                    if (this._centreService.accuseReception(centre, form.CategorieVehicule, form.CodAgrementCentre, form.NumAccuseReception))
                    {
                        string message = "Vous avez déclaré avec succès la réception de la notification d'agrément de la Préfecture pour le centre agréé : {0}";
                        //"Pour ouvrir la fiche de cette installation cliquer {1} ";
                        string lien = string.Format("<a href='{0}'>ici</a>", Url.Action("Fiche", "Centre", new { id = form.CentreId }));

                        //TempData["Message"] = string.Format(message, form.CodAgrementCentre, new System.Web.HtmlString(lien));
                        TempData["Message"] = string.Format(message, form.CodAgrementCentre);
                        TempData["MessageType"] = "Success";

                    }
                    else
                    {
                        TempData["Message"] = "Numéro  d'agrément de l'installation ou numéro d'accusé reception incorrecte";
                        TempData["MessageType"] = "Error";

                    }

                    return RedirectToAction("Index");

                }
                HomeModel model = makeModel();
                return View("Index", model);
            }
            catch (Exception e)
            {
                //Gestion de l'erreur
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        private void validateAccuseReception(AccuseReceptionModel form)
        {
            if (string.IsNullOrWhiteSpace(form.NumAccuseReception))
            {
                this.ModelState.AddModelError("NumAccuseReception", "Le numéro d'accusé réception est obligatoire");
            }
            else
            {
                if (!Validator.IsValidNumeroAccuseReception(form.NumAccuseReception))
                {
                    this.ModelState.AddModelError("NumAccuseReception", "Le numéro d'accusé réception est composé de 4 caractères alphanumériques en minuscule");
                }
            }

            if (string.IsNullOrWhiteSpace(form.CodAgrementCentre))
            {
                this.ModelState.AddModelError("CodAgrementCentre", "L'agrément du centre est obligatoire");
            }

        }

        //EG le 14/12/2011
        public ActionResult CreateInfoRDVPL(string id)
        {
            try
            {
                rReseau reseau = this._reseauService.FindBy(id);
                if (
                    this.User.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) ||
                    this.User.IsInRole(rUtilisateur.Profil.RESEAU_Administrateur.ToString()) ||
                    this.User.IsInRole(rUtilisateur.Profil.RESEAU_Gestionnaire.ToString())
                    )
                {
                    if (reseau.PInfoRdvs.Count == 0)
                    {
                        reseau.PInfoRdvs.Add(new pInfoRdv());
                    }
                    return View(reseau);
                }
                else
                {
                    return ErrorHelper.InvaliAccessError(this.ControllerContext);
                }
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        [HttpPost]
        public ActionResult CreateInfoRDVPL(string id, rReseau model)
        {
            try
            {
                rReseau reseau = this._reseauService.FindBy(id);
                if (
                    this.User.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) ||
                    this.User.IsInRole(rUtilisateur.Profil.RESEAU_Administrateur.ToString()) ||
                    this.User.IsInRole(rUtilisateur.Profil.RESEAU_Gestionnaire.ToString())
                    )
                {
                    //todo validation
                    this._pInfoRdvService.nouveauRdv(model, model.PInfoRdvs[0], this.User.Identity.Name);
                    TempData["Message"] = "Informations de prise de rendez-vous enregistrées...";
                    TempData["MessageType"] = "Success";

                    return RedirectToAction("ModifierInfoRDVPL", new { id = id });
                }
                else
                {
                    return ErrorHelper.InvaliAccessError(this.ControllerContext);
                }
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        public ActionResult VoirInfoRDVPL(string id)
        {
            try
            {
                rReseau reseau = this._reseauService.FindBy(id);
                if (
                    this.User.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) ||
                    this.User.IsInRole(rUtilisateur.Profil.RESEAU_Administrateur.ToString()) ||
                    this.User.IsInRole(rUtilisateur.Profil.RESEAU_Gestionnaire.ToString())
                    )
                {
                    return View(reseau);
                }
                else
                {
                    return ErrorHelper.InvaliAccessError(this.ControllerContext);
                }
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        public ActionResult ModifierInfoRDVPL(string id)
        {
            try
            {
                rReseau reseau = this._reseauService.FindBy(id);
                if (
                    this.User.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) ||
                    this.User.IsInRole(rUtilisateur.Profil.RESEAU_Administrateur.ToString()) ||
                    this.User.IsInRole(rUtilisateur.Profil.RESEAU_Gestionnaire.ToString())
                    )
                {

                    return View(reseau);
                }
                else
                {
                    return ErrorHelper.InvaliAccessError(this.ControllerContext);
                }
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        [HttpPost]
        public ActionResult ModifierInfoRDVPL(string id, rReseau model)
        {
            try
            {
                rReseau reseau = this._reseauService.FindBy(id);
                if (
                    this.User.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) ||
                    this.User.IsInRole(rUtilisateur.Profil.RESEAU_Administrateur.ToString()) ||
                    this.User.IsInRole(rUtilisateur.Profil.RESEAU_Gestionnaire.ToString())
                    )
                {
                    //todo validation
                    this._pInfoRdvService.modifierInfoRdv(model, model.PInfoRdvs[0], this.User.Identity.Name);
                    TempData["Message"] = "Informations de prise de rendez-vous modifiées...";
                    TempData["MessageType"] = "Success";

                    return RedirectToAction("ModifierInfoRDVPL", new { id = id });
                }
                else
                {
                    return ErrorHelper.InvaliAccessError(this.ControllerContext);
                }
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        public ActionResult CustomError(int? statusCode)
        {

            try
            {
                ViewData["statusCode"] = statusCode;
                if (statusCode != null)
                    this.Response.StatusCode = (int)statusCode;
                return View();
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

        public ActionResult ModifierAdresseMail(string id)
        {
            //PLE le 05/11/2026 Suppression de la fonctionnalité de changement email depuis la page d'accueil du site
            return RedirectToAction("Index", "Home");

            //try
            //{
            //    ModificationCourrielUtilisateurModel m = new ModificationCourrielUtilisateurModel();
            //    m.Utilisateur = this._utilisateurService.FindBy(this.User.Identity.Name);
            //    return View(m);

            //}
            //catch (Exception e)
            //{
            //    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            //}
        }

        [HttpPost]
        public ActionResult ModifierAdresseMail(ModificationCourrielUtilisateurModel m)
        {
            //PLE le 05/11/2026 Suppression de la fonctionnalité de changement email depuis la page d'accueil du site
            return RedirectToAction("Index", "Home");

            //try
            //{
            //    validateModifierCourriel(m);
            //    if (ModelState.IsValid)
            //    {
            //        this._utilisateurService.updateEmail(this.User.Identity.Name, m.NouveauCourriel, this.User.Identity.Name);
            //        rUtilisateur declencheur = InformationHelper.getInformationPersonnelle(false).Utilisateur;
            //        string logoPath = Server.MapPath("~/Ressources/logoOTC.gif");
            //        MailTemplateSender sender = new MailTemplateSender(declencheur, declencheur, rTemplateMail.TemplateEnum.NOUVEAU_EMAIL, logoPath);
            //        TempData["Message"] = "Adresse email changée...";
            //        TempData["MessageType"] = "Success";

            //        if (sender.Send())
            //        {
            //            TempData["Message"] += "email envoyé";
            //        }
            //        else
            //        {
            //            TempData["Message"] += "MAIS ECHEC ENVOI MAIL !";
            //        }

            //        //On affiche l'écran de bienvenue
            //        return RedirectToAction("Index", "Home");
            //    }
            //    return View("ModifierAdresseMail", m);
            //}
            //catch (Exception e)
            //{
            //    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            //}
        }

        private void validateModifierCourriel(ModificationCourrielUtilisateurModel m)
        {
            if (m.NouveauCourriel == null)
                ModelState.AddModelError("NouveauCourriel", "La nouvelle adresse courriel doit être renseignée.");
            if (m.ConfirmationCourriel == null)
                ModelState.AddModelError("ConfirmationCourriel", "La confirmation de l'adresse courriel doit être renseignée.");
            if ((m.NouveauCourriel != m.ConfirmationCourriel) && (m.NouveauCourriel != null && m.ConfirmationCourriel != null))
                ModelState.AddModelError("NouveauCourriel", "Erreur de saisie de l'adresse courriel.");
        }

        ////
        //// POST: /Home/ListeUtilisateurDuReseau
        //[HttpPost]
        //public ActionResult ListeUtilisateurDuReseau(HomeModel form)
        //{
        //    try
        //    {
        //        rUtilisateur user = this._utilisateurService.FindBy(this.User.Identity.Name);

        //        //form.ListeUtilisateurDuReseau = this._utilisateurService.FindAllUserDuReseau(user);
        //        form.utilisateur = user;

        //        return View(form);
        //    }
        //    catch (Exception e)
        //    {
        //        //Gestion de l'erreur
        //        return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
        //    }

        //}

        ////
        //// POST: /Home/ListeCentreAgreeMois
        //[HttpPost]
        //public ActionResult ListeCentreAgreeMois(HomeModel form)
        //{
        //    try
        //    {
        //        form.ListeCentreNouvellementAgree = this._centreService.FindAllCentreAgreeMoisDernier(form.utilisateur);

        //        return View(form);
        //    }
        //    catch (Exception e)
        //    {
        //        //Gestion de l'erreur
        //        return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
        //    }

        //}

        //
        // GET: /Home/SendNewPassword/5?codeReseau=D
        public ActionResult SendNewPassword(string id)
        {
            try
            {
                //Retrieve du user
                rUtilisateur u = this._utilisateurService.FindBy(id);

                //Génération du mot de passe
                //todo paramétrer les caractères autorisés
                PasswordGenerator pg = PasswordGenerator.getInstance();
                pg.Exclusions = "<>";
                string motPasse = pg.GenerateForDesizedSize();
                //EG le 03/01/2012 : le mot de passe est en minuscule
                motPasse = motPasse.ToLower();

                //Envoi du mot de passe
                //todo paramétrer l'objet et le template du corps du message
                /*string objet = "Changement de mot de passe d'accès à RNC2";
                string message = string.Format("Un nouveau mot de passe a été généré \n USER = {0} \nPWD = {1}" , u.Id,  motPasse);
                if (Mailer.Send(objet, message, u.Email))
                {
                    //Mise à jour du mot de passe
                    this._utilisateurService.updateMotPasse(id, motPasse, this.User.Identity.Name);
              
                }*/
                //update du mot de passe
                this._utilisateurService.updateMotPasse(id, motPasse, this.User.Identity.Name);

                //Envoi de l'email
                rUtilisateur declencheur = InformationHelper.getInformationPersonnelle(false).Utilisateur;
                string logoPath = Server.MapPath("~/Ressources/logoOTC.gif");
                MailTemplateSender sender = new MailTemplateSender(declencheur, u, rTemplateMail.TemplateEnum.NOUVEAU_PASSWORD, logoPath);
                TempData["Message"] = "Mot de passe changé avec succès...";
                TempData["MessageType"] = "Success";

                if (sender.Send())
                {
                    TempData["Message"] += "email envoyé";
                }
                else
                {
                    TempData["Message"] += "MAIS ECHEC ENVOI MAIL !";
                }

                //return RedirectToAction("Edit", new { id = id });

                HomeModel model = makeModel();
                return View("Index", model);

            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

        // GET: /Home/Version/
        public ActionResult Version()
        {
            return View();
        }

        // GET: /Home/LieuPassageCT/
        public ActionResult LieuPassageCT()
        {
            return View();
        }

        // GET: /
        public ActionResult ReturnToListeCentrePourReseau(String ReturnLettreReseau, String ReturnCodDep)
        {
            HomeModel model = new HomeModel();
            model.RechercheProfilReseauModel = new RechercheProfilReseauModel();
            model.RechercheProfilReseauModel.Filtre = new FiltreRechercheProfilReseau();
            model.RechercheProfilReseauModel.Filtre.CodeDepartement = ReturnCodDep;
            model.RechercheProfilReseauModel.Filtre.CodeReseau = ReturnLettreReseau;
            return this.ListeCentrePourReseau(model);
        }
        // GET: /
        public ActionResult ReturnToListeControleurPourReseau(String ReturnLettreReseau, String ReturnCodDep)
        {
            HomeModel model = new HomeModel();
            model.RechercheProfilReseauModel = new RechercheProfilReseauModel();
            model.RechercheProfilReseauModel.Filtre = new FiltreRechercheProfilReseau();
            model.RechercheProfilReseauModel.Filtre.CodeDepartement = ReturnCodDep;
            model.RechercheProfilReseauModel.Filtre.CodeReseau = ReturnLettreReseau;
            return this.ListeControleurPourReseau(model);
        }
    }
}
