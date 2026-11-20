using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.BusinessObjects.SharedModel;
//using Utac.Rnc2.Repository.Nhibernate;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Fabriques;
using Utac.Rnc2.Infrastructure.Queries;
using Utac.Rnc2.Infrastructure.Search;
using Utac.Rnc2.Infrastructure.Utilitaires;
using Utac.Rnc2.MVC.Extensions.mail;
using Utac.Rnc2.MVC.Extensions.validator;
using Utac.Rnc2.UI.Web.Helpers;
using Utac.Rnc2.UI.Web.Models;

namespace Utac.Rnc2.UI.Web.Controllers
{
    public class UtilisateursController : RestrictedAcessController
    {

        private readonly IrReseauService _reseauService;
        private readonly IrUtilisateurService _utilisateurService;
        private readonly IrRegionService _regionService;
        private readonly IrProfilService _profilService;
        private readonly IrProfil2Service _sousProfilService;
        private readonly IrPrefectureService _prefectureService;
        private readonly IpCentreEncourService _pCentreEncourService;
        private readonly IrReseauAppartenanceService _reseauAppartenanceService;

        //TODO a mettre en config
        private const int NB_LINE_PER_PAGE = 30;

        /* Constructeur par défaut */
        public UtilisateursController() :
            this(
                  NhibernateServiceFactory.getInstance().getrUtilisateurService(true, true),
                  NhibernateServiceFactory.getInstance().getrRegionService(false),
                  NhibernateServiceFactory.getInstance().getrReseauService(false),
                  NhibernateServiceFactory.getInstance().getrProfilService(false),
                  NhibernateServiceFactory.getInstance().getrProfil2Service(false),
                  NhibernateServiceFactory.getInstance().getrPrefectureService(false),
                  NhibernateServiceFactory.getInstance().getpCentreEncourService(false, true),
                  NhibernateServiceFactory.getInstance().getrReseauAppartenanceService(false)
            )
        {
        }

        public UtilisateursController(
            IrUtilisateurService rUtilisateurService,
            IrRegionService rRegionService,
            IrReseauService rReseauService,
            IrProfilService rProfilService,
            IrProfil2Service rProfil2Service,
            IrPrefectureService rPrefectureService,
            IpCentreEncourService pCentreEncourService,
            IrReseauAppartenanceService rReseauAppartenanceService

            )
        {
            this._utilisateurService = rUtilisateurService;
            this._regionService = rRegionService;
            this._reseauService = rReseauService;
            this._profilService = rProfilService;
            this._sousProfilService = rProfil2Service;
            this._prefectureService = rPrefectureService;
            this._pCentreEncourService = pCentreEncourService;
            this._reseauAppartenanceService = rReseauAppartenanceService;

        }

        //
        // GET: /Utilisateurs/

        public ActionResult Index()
        {
            return this.RedirectToAction("Liste");
        }

        //
        // GET: /Utilisateurs/Liste

        public ActionResult Liste()
        {
            try
            {
                UtilisateurListeModel model = new UtilisateurListeModel();

                //1 Existence d'un Filtre ?
                UtilisateurFiltre filtre = (UtilisateurFiltre)this.Session["filtre_utilisateur"];

                //2 Recherche si Filtre
                rUtilisateur u = this._utilisateurService.FindBy(this.User.Identity.Name);

                if (filtre != null)
                {
                    model.filtre = filtre;
                    if (this.User.IsInRole(rUtilisateur.Profil.RESEAU_Administrateur.ToString()))
                    {
                        if (string.IsNullOrEmpty(filtre.CodeReseau))
                        {
                            filtre.CodeReseaux = new List<string>();
                            foreach (rReseau r in u.RReseaux)
                            {
                                filtre.CodeReseaux.Add(r.Id);
                            }
                        }
                    }
                    model.liste = this._utilisateurService.findByFilter(filtre);
                    model.profilFiltre = this._profilService.FindBy(filtre.ProfilId);
                }

                model.listeRegion = this._regionService.FindAll();
                model.listeRegion.Insert(0, new rRegion() { Id = -1, LibelleRegion = "" });

                if (this.User.IsInRole(rUtilisateur.Profil.RESEAU_Administrateur.ToString()))
                {
                    model.listeProfil = new List<rProfil>();
                    model.listeProfil.Add(u.RProfil);
                    model.listeReseau = new List<rReseau>();
                    foreach (rReseau r in u.RReseaux)
                    {
                        model.listeReseau.Add(r);
                    }
                }
                else
                {
                    model.listeProfil = this._profilService.FindAll();
                    model.listeReseau = this._reseauService.FindAllReseauSansAWZ();

                }
                model.listeReseau.Insert(0, new rReseau() { Id = "" });

                return View(model);

            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        //
        // GET: /Utilisateurs/DeleteFilter
        public ActionResult DeleteFilter()
        {
            try
            {
                this.Session["filtre_utilisateur"] = null;

                return RedirectToAction("Liste");

            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        //
        // POST: /Utilisateurs/AppliquerFiltre
        [HttpPost]

        public ActionResult AppliquerFiltre([Bind(Include = "Filtre")] UtilisateurListeModel form)
        {
            try
            {
                this.Session["filtre_utilisateur"] = form.filtre;

                return RedirectToAction("Liste");

            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        //
        // GET: /Utilisateurs/Details/5

        public ActionResult Details(string id)
        {
            try
            {

                return View();

            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        //
        // GET: /Utilisateurs/Ajouter
        public ActionResult Ajouter(int profilId, string codeDepartement, int? idRegion, string typeCentre)
        {
            try
            {
                AjouterUtilisateurModel model = new AjouterUtilisateurModel();
                model.Utilisateur = new rUtilisateur();
                model.Utilisateur.Actif = true;
                model.Utilisateur.RProfil = this._profilService.FindBy(profilId);
                model.ListeSousProfils = this._sousProfilService.findAllByProfil(model.Utilisateur.RProfil);
                //EG le 27/01/2012, le profile Reseau ne peut pas habiliter un utilisateur de profil RESEAU_Administrateur
                if (this.User.IsInRole("RESEAU"))
                {
                    rProfil2 profil = this._sousProfilService.FindByLibelle(rUtilisateur.Profil.RESEAU_Administrateur.ToString());
                    model.ListeSousProfils.Remove(profil);
                }
                model.ListePrefectures = null;
                model.TypeCentre = typeCentre;

                if (model.Utilisateur.RProfil.Id == 2) //2011-11-07 PLE Le code département est rensigné uniquement pour le profil PREFECTURE
                {
                    if (!string.IsNullOrEmpty(codeDepartement))
                    {
                        model.ListePrefectures = this._prefectureService.FindAll();
                        model.Utilisateur.RPrefecture = this._prefectureService.findByCodeDepartement(codeDepartement);
                    }
                }

                if (model.Utilisateur.RProfil.Id == 3) //2011-11-07 PLE Le région est renseigné uniquement pour le profil DRIRE                
                {
                    if (idRegion != null)
                    {
                        model.ListeRegions = this._regionService.FindAll();
                        if (idRegion != 1)
                        {
                            model.Utilisateur.RRegion = this._regionService.FindBy(idRegion);
                        }
                    }
                }

                if (profilId == 5)
                {
                    //2011-11-04 PLE L'adminitrateur RESEAU ne peux attribuer que ses réseaux de rattachement
                    if (this.User.IsInRole(rUtilisateur.Profil.RESEAU_Administrateur.ToString()))
                    {
                        rUtilisateur u = this._utilisateurService.FindBy(this.User.Identity.Name);
                        model.ListeReseaux = u.RReseaux;
                    }
                    else
                    {
                        model.ListeReseaux = this._reseauService.FindAllReseauSansA();
                    }
                }

                return View(model);
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        //
        // POST: /Utilisateurs/Ajouter
        [HttpPost]
        public ActionResult Ajouter(int profilId, AjouterUtilisateurModel form)
        {
            try
            {

                this.validateUserInfo(form, this.ModelState);
                if (ModelState.IsValid)
                {
                    rUtilisateur newUser = this._utilisateurService.addUser(form, this.User.Identity.Name);
                    //EG le 10/01/2012 envoi du mail template
                    rUtilisateur declencheur = InformationHelper.getInformationPersonnelle(false).Utilisateur;
                    string logoPath = Server.MapPath("~/Ressources/logoOTC.gif");
                    MailTemplateSender sender = new MailTemplateSender(declencheur, newUser, rTemplateMail.TemplateEnum.NOUVEL_UTILISATEUR, logoPath);
                    TempData["Message"] = "Nouvel utilisateur créé...";
                    TempData["MessageType"] = "Success";

                    if (sender.Send())
                    {
                        TempData["Message"] += "email envoyé";
                    }
                    else
                    {
                        TempData["Message"] += "MAIS ECHEC ENVOI MAIL !";
                    }
                    //24/10/2017 PLE si le profil est "PIV" ou "OTC_ADMINISTRATION" alors synchroniser avec la base requisition
                    if (newUser.RProfil.Id.Equals(rProfil.CodeProfil.PIV.GetHashCode()) || (newUser.RProfil.Id.Equals(rProfil.CodeProfil.OTC.GetHashCode()) && (newUser.RProfil2.Id.Equals(rProfil2.CodeProfil2.OTC_ADMINISTRATION.GetHashCode()))))
                    {
                        if (this._utilisateurService.SynchroniseUserWithDatabaseRequisition(newUser))
                        {
                            TempData["Message"] += "... et mise à jour de la base réquisition on-line.";
                        }
                        else
                        {
                            TempData["MessageType"] = "Error";
                            TempData["Message"] += "... MAIS ECHEC DE LA MISE A JOUR DE LA BASE REQUISITION ON-LINE !";
                        }
                    }

                    return RedirectToAction("Edit", new { id = newUser.Id });
                }
                else
                {
                    form.ListeSousProfils = this._sousProfilService.findAllByProfil(form.Utilisateur.RProfil);
                    if (form.Utilisateur.RPrefecture != null)
                    {
                        form.ListePrefectures = this._prefectureService.FindAll();
                        form.Utilisateur.RPrefecture = this._prefectureService.FindBy(form.Utilisateur.RPrefecture.Id);
                    }

                    if (form.Utilisateur.RRegion != null)
                    {
                        form.ListeRegions = this._regionService.FindAll();
                        form.Utilisateur.RRegion = this._regionService.FindBy(form.Utilisateur.RRegion.Id);
                    }

                    if (form.Utilisateur.RProfil.Id == 5)
                    {
                        {
                            //2011-11-04 PLE L'adminitrateur RESEAU ne peux attribuer que ses réseaux de rattachement
                            if (this.User.IsInRole(rUtilisateur.Profil.RESEAU_Administrateur.ToString()))
                            {
                                rUtilisateur u = this._utilisateurService.FindBy(this.User.Identity.Name);
                                form.ListeReseaux = u.RReseaux;
                            }
                            else
                            {
                                form.ListeReseaux = this._reseauService.FindAllReseauSansA();
                            }
                        }
                    }

                    return View(form);
                }

            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        //private 

        //
        // GET: /Utilisateurs/Create

        public ActionResult Create()
        {
            List<rProfil> profilesUtilisateur = this._profilService.FindAll();
            ViewData["profilesUtilisateur"] = profilesUtilisateur;

            return View();
        }

        //
        // POST: /Utilisateurs/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        //
        // GET: /Utilisateurs/Edit/5

        public ActionResult Edit(string id, string errorMsg, string technicalErrorMsg)
        {
            try
            {
                UtilisateurEditModel model = new UtilisateurEditModel();
                //EG le 16/11/2011 pour gérér le cas du delete avec erreur
                model.Message = errorMsg;

                //EG le 17/11/2011  pour gérér le cas du delete avec erreur et affichage erreur technique
                if (!string.IsNullOrWhiteSpace(technicalErrorMsg) && this.User.IsInRole(rUtilisateur.Profil.OTC.ToString()))
                {
                    model.MessageTechnique = technicalErrorMsg;
                }

                rUtilisateur u = this._utilisateurService.FindBy(id);

                model.Utilisateur = u;
                model.ListeSousProfil = this._sousProfilService.findAllByProfil(u.RProfil);
                //EG le 27/01/2012, le profile Reseau ne peut pas habiliter un utilisateur de profil RESEAU_Administrateur
                if (this.User.IsInRole("RESEAU"))
                {
                    rProfil2 profil = this._sousProfilService.FindByLibelle(rUtilisateur.Profil.RESEAU_Administrateur.ToString());
                    model.ListeSousProfil.Remove(profil);
                }
                model.ListeRegion = this._regionService.FindAll();

                //2011-11-04 PLE L'adminitrateur RESEAU ne peux attribuer que ses réseaux de rattachement
                //2011-11-09 EG on passe par des enum
                if (this.User.IsInRole(rUtilisateur.Profil.RESEAU_Administrateur.ToString()))
                {
                    rUtilisateur uConnecte = this._utilisateurService.FindBy(this.User.Identity.Name);
                    model.ListeReseau = uConnecte.RReseaux;
                }
                else
                {
                    model.ListeReseau = this._reseauService.FindAllReseauSansA();
                }

                return View(model);

            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

        //
        // POST: /Utilisateurs/Edit/5

        [HttpPost]
        public ActionResult Edit(string id, UtilisateurEditModel form)
        {
            try
            {
                this.validateUserInfo(form, this.ModelState);
                if (ModelState.IsValid)
                {
                    this._utilisateurService.updateUser(id, form.Utilisateur, this.User.Identity.Name);
                    //24/10/2017 PLE si le profil est "PIV" alors synchroniser avec la base requisition
                    if (form.Utilisateur.RProfil.Id.Equals(rProfil.CodeProfil.PIV.GetHashCode()) || (form.Utilisateur.RProfil.Id.Equals(rProfil.CodeProfil.OTC.GetHashCode()) && form.Utilisateur.RProfil2.Id.Equals(rProfil2.CodeProfil2.OTC_ADMINISTRATION.GetHashCode())))
                    {
                        if (this._utilisateurService.SynchroniseUserWithDatabaseRequisition(this._utilisateurService.FindBy(id)))
                        {
                            TempData["MessageType"] = "Success";
                            TempData["Message"] += "Mise à jour de la base réquisition on-line.";
                        }
                        else
                        {
                            TempData["MessageType"] = "Error";
                            TempData["Message"] += "ECHEC DE LA MISE A JOUR DE LA BASE REQUISITION ON-LINE !";
                        }
                    }
                    return RedirectToAction("Edit", new { id = id });
                }
                else
                {
                    rUtilisateur u = this._utilisateurService.FindBy(id);
                    form.Utilisateur = u;
                    form.ListeSousProfil = this._sousProfilService.findAllByProfil(u.RProfil);
                    //EG le 27/01/2012, le profile Reseau ne peut pas habiliter un utilisateur de profil RESEAU_Administrateur
                    if (this.User.IsInRole("RESEAU"))
                    {
                        rProfil2 profil = this._sousProfilService.FindByLibelle(rUtilisateur.Profil.RESEAU_Administrateur.ToString());
                        form.ListeSousProfil.Remove(profil);
                    }
                    form.ListeRegion = this._regionService.FindAll();

                    //2011-11-04 PLE L'adminitrateur RESEAU ne peux attribuer que ses réseaux de rattachement
                    if (this.User.IsInRole(rUtilisateur.Profil.RESEAU_Administrateur.ToString()))
                    {
                        rUtilisateur uConnecte = this._utilisateurService.FindBy(this.User.Identity.Name);
                        form.ListeReseau = uConnecte.RReseaux;
                    }
                    else
                    {
                        form.ListeReseau = this._reseauService.FindAll();
                    }

                    return View(form);
                }
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        //
        // GET: /Utilisateurs/RemoveAppartenance/5?codeReseau=D
        public ActionResult RemoveAppartenance(string id, string codeReseau)
        {
            try
            {
                this._utilisateurService.supprimeAppartenance(id, codeReseau, this.User.Identity.Name);
                return RedirectToAction("Edit", new { id = id });

            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

        //
        // GET: /Utilisateurs/AddAppartenance/5?codeReseau=D
        public ActionResult AddAppartenance(string id, string codeReseau)
        {
            try
            {

                this._utilisateurService.ajouteAppartenance(id, codeReseau, this.User.Identity.Name);
                return RedirectToAction("Edit", new { id = id });
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

        //
        // GET: /Utilisateurs/SendNewPassword/5?codeReseau=D
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

                //24/10/2017 PLE si le profil est "PIV" ou "OTC_ADMINISTRATION" alors synchroniser avec la base requisition
                if (u.RProfil.Id.Equals(rProfil.CodeProfil.PIV.GetHashCode()) || (u.RProfil.Id.Equals(rProfil.CodeProfil.OTC.GetHashCode()) && u.RProfil2.Id.Equals(rProfil2.CodeProfil2.OTC_ADMINISTRATION.GetHashCode())))
                {
                    if (this._utilisateurService.SynchroniseUserWithDatabaseRequisition(u))
                    {
                        TempData["Message"] += "... et mise à jour de la base réquisition on-line.";
                    }
                    else
                    {
                        TempData["MessageType"] = "Error";
                        TempData["Message"] += "... MAIS ECHEC DE LA MISE A JOUR DE LA BASE REQUISITION ON-LINE !";
                    }
                }

                return RedirectToAction("Edit", new { id = id });

            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

        //GET: /utilisateurs/EstValideCentre/?codAgremCentre=S091Z120&typeCentre=VL
        public JsonResult EstValideCentre(string codAgremCentre, rCategorieVehicule.CategorieEnum categorieVehicule)
        {
            try
            {

                RechercheCentreAgreeResultat result = this._pCentreEncourService.estValide(codAgremCentre, categorieVehicule, true);
                return Json(new { ok = result.EstRechercheValide, raison = result.Resultat.ToString() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(e);
            }

        }

        //
        // GET: /Utilisateurs/Delete/5

        public ActionResult Delete(string id)
        {
            try
            {
                rUtilisateur u = this._utilisateurService.FindBy(id);

                return View(u);
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        //
        // POST: /Utilisateurs/Delete/5

        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            try
            {
                rUtilisateur u = this._utilisateurService.FindBy(id);
                this._utilisateurService.supprimerUtilisateur(u);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                //if (e is NHibernate.StaleStateException || e is Sybase.Data.AseClient.AseException)//todo vérifier que c'est une erreur de cascade delete child ?
                //{
                //EG le 09/01/2012 , correction suite proposition PB 
                //return RedirectToAction("Edit", new { id = id , errorMsg = "Impossible de supprimer l'utilisateur" , technicalErrorMsg = e.Message });
                return RedirectToAction("Edit", new { id = id, errorMsg = "Impossible de supprimer un utilisateur qui a saisi des informations sur RNC2", technicalErrorMsg = e.Message });
                //}
                //return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }


        //
        // POST : /Utilisateurs/Search
        public ActionResult Search(FormCollection formulaireWeb)
        {

            Utac.Rnc2.Infrastructure.Search.rUtilisateurSearchObject requete = new Utac.Rnc2.Infrastructure.Search.rUtilisateurSearchObject();
            string codeDepartement = formulaireWeb.Get("code_departement");
            string codeRegion = formulaireWeb.Get("region");
            string codeReseau = formulaireWeb.Get("code_reseau");
            string categorieVehicule = formulaireWeb.Get("categorie_vehicule");
            string profil = formulaireWeb.Get("profil");
            string nomUtilisateur = formulaireWeb.Get("nom_utilisateur");

            if (!string.IsNullOrEmpty(codeDepartement))
                requete.IdDepartement = Convert.ToInt32(codeDepartement);

            if (!string.IsNullOrEmpty(codeRegion))
                requete.IdRegion = Convert.ToInt32(codeRegion);

            if (!string.IsNullOrEmpty(codeReseau))
                requete.CodeReseau = codeReseau;

            if (!string.IsNullOrEmpty(categorieVehicule))
                requete.CategorieVehicule = categorieVehicule;

            if (!string.IsNullOrEmpty(profil))
                requete.IdProfil = Convert.ToInt32(profil);

            if (!string.IsNullOrEmpty(nomUtilisateur))
                requete.Nom = nomUtilisateur;

            long nbUsers;
            List<rUtilisateur> utilisateurs = this._utilisateurService.LoadUtilisateurBy(requete, 1, NB_LINE_PER_PAGE, out nbUsers);
            var pagedUtilisateur = new MvcContrib.Pagination.CustomPagination<rUtilisateur>(utilisateurs, 1, NB_LINE_PER_PAGE, Convert.ToInt32(nbUsers));

            //On place le résultat dans le context
            this.HttpContext.Session["utilisateurs"] = pagedUtilisateur;

            //On place la requete dans le context
            this.HttpContext.Session["search"] = requete;

            return RedirectToAction("Show");

        }

        //
        // GET: /Utilisateurs/FilterByProfil
        public ActionResult FilterByProfil(int profilId)
        {
            try
            {
                Query q = new Query();
                q.Criteria.Add(new Criteria("RProfil.Id", profilId, CriteriaOperator.EQUALS));

                List<rUtilisateur> utilisateurs = this._utilisateurService.FindBy(q);

                //Les donnees de la vue
                //Liste des régions
                IList<rRegion> regions = this._regionService.FindAll();
                this.ViewData["regions"] = regions;

                //Liste des réseaux
                IList<rReseau> reseaux = this._reseauService.FindAll();
                this.ViewData["reseaux"] = reseaux;

                //Liste des profils utilisateurs
                IList<rProfil> profils = this._profilService.FindAll();
                this.ViewData["profils"] = profils;

                return View("Show", utilisateurs);

            }
            catch (Exception e)
            {
                ViewData["erreur"] = e.Message;
                return View();
            }
        }

        private void validateUserInfo(UtilisateurEditModel form, ModelStateDictionary ModelState)
        {
            if (string.IsNullOrWhiteSpace(form.Utilisateur.Nom))
            {
                ModelState.AddModelError("Utilisateur.Nom", "Le nom est obligatoire");
            }

            if (string.IsNullOrWhiteSpace(form.Utilisateur.Prenom))
            {
                ModelState.AddModelError("Utilisateur.Prenom", "Le prenom est obligatoire");
            }

            if (string.IsNullOrWhiteSpace(form.Utilisateur.Email))
            {
                ModelState.AddModelError("Utilisateur.Email", "L'email est obligatoire");
            }
            else
            {
                if (!Validator.IsValidEmail(form.Utilisateur.Email))
                {
                    ModelState.AddModelError("Utilisateur.Email", "L'email n'est pas valide");
                }
            }

        }

        //validation du formulaire pour la création d'un nouvel utilisateur
        private void validateUserInfo(AjouterUtilisateurModel form, ModelStateDictionary ModelState)
        {
            if (string.IsNullOrWhiteSpace(form.Utilisateur.Nom))
            {
                ModelState.AddModelError("Utilisateur.Nom", "Le nom est obligatoire");
            }

            if (string.IsNullOrWhiteSpace(form.Utilisateur.Prenom))
            {
                ModelState.AddModelError("Utilisateur.Prenom", "Le prénom est obligatoire");
            }

            if (string.IsNullOrWhiteSpace(form.Utilisateur.Email))
            {
                ModelState.AddModelError("Utilisateur.Email", "L'email est obligatoire");
            }
            else
            {
                if (!Validator.IsValidEmail(form.Utilisateur.Email))
                {
                    ModelState.AddModelError("Utilisateur.Email", "L'email n'est pas valide");
                }
            }

            if (string.IsNullOrWhiteSpace(form.Utilisateur.Id))
            {
                ModelState.AddModelError("Utilisateur.Id", "L'identifiant est obligatoire");
            }
            else
            {
                if (!Validator.IsValidIdentifiantUtilisateur(form.Utilisateur.Id))
                {
                    ModelState.AddModelError("Utilisateur.Id", "Présence de caractères spéciaux ou majuscules non autorisée.");
                }
            }

            //2011-11-04 PLE un utilisateur de profil RESEAU doit avoir au moins un réseau de rattachement
            if (form.Utilisateur.RProfil.Id == 5)
            {
                if ((form.ReseauxAppartenance == null) || form.ReseauxAppartenance.Length == 0)
                {
                    ModelState.AddModelError("Utilisateur.RReseaux", "L'appartenance à au moins un réseau est obligatoire.");
                }
            }

            //2011-11-07 PLE Si mot de passe saisie alors contrôle validité
            if (!string.IsNullOrWhiteSpace(form.Utilisateur.MotDePasse))
            {
                if (!Validator.IsValidMotDePasseUtilisateur(form.Utilisateur.MotDePasse))
                {
                    ModelState.AddModelError("Utilisateur.MotDePasse", "Présence de caractères spéciaux non autorisée (blancs ...).");
                }
            }
        }

        //
        // GET: /Utilisateurs/SendNewPassword/5?codeReseau=D
        public ActionResult SynchroniseAllUsersWithDatabaseRequisition(int profilId)
        {
            if (User.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()))
            {
                rUtilisateurSearchObject search = new rUtilisateurSearchObject();
                search.IdProfil = profilId;

                /* Seul les profil admin OTC sont synchronisés avec réquisition */
                if (search.IdProfil.Equals(rProfil.CodeProfil.OTC.GetHashCode()))
                {
                    search.IdProfil2 = rProfil2.CodeProfil2.OTC_ADMINISTRATION.GetHashCode();
                }
                if (search.IdProfil.Equals(rProfil.CodeProfil.PIV.GetHashCode()) || search.IdProfil.Equals(rProfil.CodeProfil.OTC.GetHashCode()))
                {
                    if (this._utilisateurService.SynchroniseAllUsersWithDatabaseRequisition(search))
                    {
                        TempData["MessageType"] = "Success";
                        TempData["Message"] += "Succés de la mise à jour de la base réquisition on-line.";
                    }
                    else
                    {
                        TempData["MessageType"] = "Error";
                        TempData["Message"] += "ECHEC DE LA MISE A JOUR DE LA BASE REQUISITION ON-LINE !";
                    }
                }
                else
                {
                    TempData["MessageType"] = "Error";
                    TempData["Message"] += "ECHEC DE LA MISE A JOUR DE LA BASE REQUISITION ON-LINE : utilisateurs non autorisés";
                }
                return this.RedirectToAction("Liste");
            }

            return ErrorHelper.InvaliAccessError(this.ControllerContext);
        }

    }
}
