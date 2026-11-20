#pragma warning disable IDE0073 // L'en-tête du fichier est manquant ou ne se trouve pas en haut du fichier
using System;
#pragma warning restore IDE0073 // L'en-tête du fichier est manquant ou ne se trouve pas en haut du fichier
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.BusinessObjects.statistiques;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Fabriques;
using Utac.Rnc2.Infrastructure.Queries;
using Utac.Rnc2.UI.Web.DataSets;
using Utac.Rnc2.UI.Web.DataSets.Adapters;
using Utac.Rnc2.UI.Web.Helpers;
using Utac.Rnc2.UI.Web.Models;

namespace Utac.Rnc2.UI.Web.Controllers
{
    public class ControleurTechniqueController : RestrictedAcessController
    {
        private readonly IpControleurEnCourService _controleurTechniqueService;
        private readonly IpControleurEnCourService _controleurTechniqueServiceAutoCommit;
        private readonly IpCentreEncourService _centreService;
        private readonly IrTypeSanctionControleurService _rTypeSanctionControleurService;
        private readonly IrTypeOrganismeDemandeurService _rTypeOrganismeDemandeurService;
        private readonly IpSanctionControleurService _pSanctionControleurService;
        private readonly IpAmendeControleurService _pAmendeControleurService;
        private readonly IrHabilitationService _rHabilitationService;
        private readonly IpAuditControleurService _pAuditControleurService;
        private readonly IrCategorieVehiculeService _rCategorieVehiculeService;
        private readonly IpAuditControleurVLService _pAuditControleurVLService;
        private readonly IpAuditControleurCLService _pAuditControleurCLService;
        private readonly IpFormationControleurPLService _pFormationControleurPLService;
        private readonly IrTypeFormationService _rTypeFormationService;
        private readonly IpFormationControleurVLService _pFormationControleurVLService;
        private readonly IpFormationControleurCLService _pFormationControleurCLService;
        private readonly IpControleurHistoService _controleurHistoriqueService;
        private readonly IrUtilisateurService _utilisateurService;
        private readonly IpActiviteControleurVLService _pActiviteControleurVLService;
        //PLE - 25/03/2026 Ajout activité contrôleur CL
        private readonly IpActiviteControleurCLService _pActiviteControleurCLService;

        public ControleurTechniqueController() :
            this(
                  NhibernateServiceFactory.getInstance().getpControleurEnCourService(true),
                  NhibernateServiceFactory.getInstance().getpCentreEncourService(false, true),
                  NhibernateServiceFactory.getInstance().getpSanctionControleurService(true),
                  NhibernateServiceFactory.getInstance().getrTypeSanctionControleurService(false),
                  NhibernateServiceFactory.getInstance().getrTypeOrganismeDemandeurService(false),
                  NhibernateServiceFactory.getInstance().getpAmendeControleurService(true),
                  NhibernateServiceFactory.getInstance().getrHabilitationService(false),
                  NhibernateServiceFactory.getInstance().getpAuditControleurService(true),
                  NhibernateServiceFactory.getInstance().getrCategorieVehiculeService(false),
                  NhibernateServiceFactory.getInstance().getpAuditControleurVLService(true),
                  NhibernateServiceFactory.getInstance().getpAuditControleurCLService(true),
                  NhibernateServiceFactory.getInstance().getpFormationControleurPLService(true),
                  NhibernateServiceFactory.getInstance().getrTypeFormationService(true),
                  NhibernateServiceFactory.getInstance().getpFormationControleurVLService(true),
                  NhibernateServiceFactory.getInstance().getpFormationControleurCLService(true),
                  NhibernateServiceFactory.getInstance().getpControleurHistoService(true),
                  NhibernateServiceFactory.getInstance().getrUtilisateurService(true, false),
                  NhibernateServiceFactory.getInstance().getpActiviteControleurVLService(false),
                  //PLE - 25/03/2026 Ajout suivi activité contrôleur CL
                  NhibernateServiceFactory.getInstance().getpActiviteControleurCLService(false)
            )
        {
            _controleurTechniqueServiceAutoCommit = NhibernateServiceFactory.getInstance().getpControleurEnCourService(false);
        }

        public ControleurTechniqueController(
            IpControleurEnCourService controleurTechniqueService,
            IpCentreEncourService centreService,
            IpSanctionControleurService pSanctionControleurService,
            IrTypeSanctionControleurService rTypeSanctionControleurService,
            IrTypeOrganismeDemandeurService rTypeOrganismeDemandeurService,
            IpAmendeControleurService pAmendeControleurService,
            IrHabilitationService rHabilitationService,
            IpAuditControleurService pAuditControleurService,
            IrCategorieVehiculeService rCategorieVehiculeService,
            IpAuditControleurVLService pAuditControleurVLService,
            IpAuditControleurCLService pAuditControleurCLService,
            IpFormationControleurPLService pFormationControleurPLService,
            IrTypeFormationService rTypeFormationService,
            IpFormationControleurVLService pFormationControleurVLService,
            IpFormationControleurCLService pFormationControleurCLService,
            IpControleurHistoService pControleurHistoService,
            IrUtilisateurService utilisateurService,
            IpActiviteControleurVLService pActiviteControleurVLService,
            IpActiviteControleurCLService pActiviteControleurCLService)
        {
            this._controleurTechniqueService = controleurTechniqueService;
            this._centreService = centreService;
            this._pSanctionControleurService = pSanctionControleurService;
            this._rTypeSanctionControleurService = rTypeSanctionControleurService;
            this._rTypeOrganismeDemandeurService = rTypeOrganismeDemandeurService;
            this._pAmendeControleurService = pAmendeControleurService;
            this._rHabilitationService = rHabilitationService;
            this._pAuditControleurService = pAuditControleurService;
            this._rCategorieVehiculeService = rCategorieVehiculeService;
            this._pAuditControleurVLService = pAuditControleurVLService;
            this._pAuditControleurCLService = pAuditControleurCLService;
            this._pFormationControleurPLService = pFormationControleurPLService;
            this._rTypeFormationService = rTypeFormationService;
            this._pFormationControleurVLService = pFormationControleurVLService;
            this._pFormationControleurCLService = pFormationControleurCLService;
            this._controleurHistoriqueService = pControleurHistoService;
            this._utilisateurService = utilisateurService;
            this._pActiviteControleurVLService = pActiviteControleurVLService;
            this._pActiviteControleurCLService = pActiviteControleurCLService;
        }

        //
        // GET: /ControleurTechnique/Recherche
        // Affiche l'écran en mode recherche uniquement avec un formulaire vide

        public ActionResult Recherche()
        {
            RechercheControleurModel model = new RechercheControleurModel();
            return View(model);
        }

        //
        // POST: /ControleurTechnique/Recherche
        // Recherche les controleurs selon les paramètres du formulaire et affiche les résultats

        [HttpPost]
        public ActionResult Recherche(RechercheControleurModel form)
        {
            try
            {
                this.validateRechercheControleur(form);
                if (ModelState.IsValid)
                {
                    //1 Constitution de la requete pour le certificatService
                    Query q = getRechercheControleurQuery(form);

                    //Exécution de la recherche
                    //20/08/2021 PLE ajout d'un alias pour forcer fetch de l'entité "PSanctionControleurs"
                    form.Resultat = this._controleurTechniqueService.FindBy(q, "PSanctionControleurs");

                    //31/03/2014 PLE Evolution recherche éventuelle dans l'historique du contrôleur d'un ancien numéro d'agrément complet
                    if (form.Resultat.Count.Equals(0) && q.Criteria.Count.Equals(1) &&
                        (q.Criteria[0].PropertyName.Equals("CodAgremVl") || q.Criteria[0].PropertyName.Equals("CodAgremPl") || q.Criteria[0].PropertyName.Equals("CodAgremCl")) &&
                        q.Criteria[0].Value.ToString().Length.Equals(9))
                    {
                        //31/03/2014 PLE changement du critère Like xxx% en = valeur du numéro d'agrément exact
                        q.Criteria[0].Value = q.Criteria[0].Value.ToString().Substring(0, 8);
                        q.Criteria[0].CriteriaOperator = CriteriaOperator.EQUALS;

                        //31/12/2014 recherche dans l'historique du contrôleur.
                        IList<pControleurHisto> ResultatpControleurHisto = this._controleurHistoriqueService.FindBy(q);
                        if (ResultatpControleurHisto.Count != 0)
                        {
                            //31/12/2014 PLE ajout du résultat sur la recherche de l'ID
                            form.Resultat.Add(this._controleurTechniqueService.FindBy(ResultatpControleurHisto[0].IdControleurHisto));
                        }
                    }
                }
                return View(form);

            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }
        private void validateRechercheControleur(RechercheControleurModel form)
        {
            if (!string.IsNullOrEmpty(form.Nom) && form.Nom.Length < 3)
            {
                ModelState.AddModelError("Nom", "saisissez au moins les 3 premieres lettres");
            }
            if (!string.IsNullOrEmpty(form.CodeAgrementVL) && form.CodeAgrementVL.Length < 3)
            {
                ModelState.AddModelError("CodeAgrementVL", "saisissez au moins les 3 premiers caractères");
            }
            if (!string.IsNullOrEmpty(form.CodeAgrementPL) && form.CodeAgrementPL.Length < 3)
            {
                ModelState.AddModelError("CodeAgrementPL", "saisissez au moins les 3 premiers caractères");
            }
            if (!string.IsNullOrEmpty(form.CodeAgrementCL) && form.CodeAgrementCL.Length < 3)
            {
                ModelState.AddModelError("CodeAgrementCL", "saisissez au moins les 3 premiers caractères");
            }
            if (!string.IsNullOrEmpty(form.CodeAgrementCentreRattachementVL) && form.CodeAgrementCentreRattachementVL.Length < 4)
            {
                ModelState.AddModelError("CodeAgrementCentreRattachementVL", "saisissez au moins les 4 premiers caractères");
            }
            if (!string.IsNullOrEmpty(form.CodeAgrementCentreRattachementPL) && form.CodeAgrementCentreRattachementPL.Length < 4)
            {
                ModelState.AddModelError("CodeAgrementCentreRattachementPL", "saisissez au moins les 4 premiers caractères");
            }
            if (!string.IsNullOrEmpty(form.CodeAgrementCentreRattachementCL) && form.CodeAgrementCentreRattachementCL.Length < 4)
            {
                ModelState.AddModelError("CodeAgrementCentreRattachementCL", "saisissez au moins les 4 premiers caractères");
            }
            if (string.IsNullOrEmpty(form.Nom) && string.IsNullOrEmpty(form.CodeAgrementVL) && string.IsNullOrEmpty(form.CodeAgrementPL) && string.IsNullOrEmpty(form.CodeAgrementCL)
                && string.IsNullOrEmpty(form.CodeAgrementCentreRattachementVL) && string.IsNullOrEmpty(form.CodeAgrementCentreRattachementPL) && string.IsNullOrEmpty(form.CodeAgrementCentreRattachementCL))
            {
                ModelState.AddModelError("Resultat", "Saisissez au moins 1 critère de recherche ! (Nom ou N°agrément)");
            }

        }

        //
        // GET: /ControleurTechnique/Fiche/5
        public ActionResult Fiche(int id)
        {
            try
            {
                //1 retrieve de l'id
                pContoleurEncour c = this._controleurTechniqueService.FindBy(id);

                //Affichage de la vue
                return View(c);

            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

        //
        // GET: /ControleurTechnique/FicheHisto/5
        public ActionResult FicheHisto(int id, DateTime dateHisto)
        {
            try
            {
                //1 retrieve de l'id
                pControleurHisto h = this._controleurHistoriqueService.findByIdAndDate(id, dateHisto);
                pContoleurEncour c = this._controleurTechniqueService.FindBy(h.IdControleurHisto);
                if (GestionDroitsExecution.isRightActionForControleur(this.User, c, GestionDroitsExecution.ActionsFicheControleur.VOIR_HISTO))
                {
                    //Affichage de la vue
                    return View(h);
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

        //GET : /ControleurTechnique/ListeSanctions/613
        public ActionResult ListeSanctions(int id)
        {
            try
            {
                //1 on passe par le certificatService de la table principal
                pContoleurEncour c = this._controleurTechniqueService.FindBy(id);
                if (GestionDroitsExecution.isRightActionForControleur(this.User, c, GestionDroitsExecution.ActionsFicheControleur.VOIR_SANCT))
                {
                    if (c.PSanctionControleurs.Count == 0)
                    {
                        //pas de sanctions le mec a surement taper l'url ???
                        return this.RedirectToAction("Fiche", new { id = id });
                    }
                    else
                        return View(c);
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

        //GET : /ControleurTechnique/ListeHistoriques/613
        public ActionResult ListeHistoriques(int id)
        {
            try
            {
                //1 on passe par le certificatService de la table principal
                pContoleurEncour c = this._controleurTechniqueService.FindBy(id);
                if (GestionDroitsExecution.isRightActionForControleur(this.User, c, GestionDroitsExecution.ActionsFicheControleur.VOIR_HISTO))
                {
                    if (c.PControleurHistoriques.Count == 0)
                    {
                        //pas de sanctions le mec a surement taper l'url ???
                        return this.RedirectToAction("Fiche", new { id = id });
                    }
                    else
                        return View(c);
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

        //
        // GET: /ControleurTechnique/ModificationEtatCivil/313
        public ActionResult ModificationEtatCivil(int id)
        {
            try
            {
                //1 on retrieve l'id
                pContoleurEncour c = this._controleurTechniqueService.FindBy(id);
                if (GestionDroitsExecution.isRightActionForControleur(this.User, c, GestionDroitsExecution.ActionsFicheControleur.MODCO))
                {

                    return View(c);
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

        //
        // POST: /ControleurTechnique/ModificationEtatCivil
        [HttpPost]
        public ActionResult ModificationEtatCivil(int id, pContoleurEncour model)
        {
            try
            {
                validateEtatCivil(model);
                if (ModelState.IsValid)
                {
                    this._controleurTechniqueService.modifyEtatCivil(id, model, this.User.Identity.Name);
                    return this.RedirectToAction("Fiche", new { id = id });
                }
                return View(model);

            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        private void validateEtatCivil(pContoleurEncour model)
        {
            //Nom obligatoire
            if (string.IsNullOrEmpty(model.Nom))
            {
                ModelState.AddModelError("Nom", "Le nom est obligatoire");
            }

            //Prenom obligatoire
            if (string.IsNullOrEmpty(model.Prenom))
            {
                ModelState.AddModelError("Prenom", "Le prenom est obligatoire");
            }

        }

        // GET: /ControleurTechnique/ModificationRetourArriere/313
        public ActionResult ConfirmationModificationRetourArriere(int id)
        {
            try
            {
                pContoleurEncour c = this._controleurTechniqueService.FindBy(id);
                if (GestionDroitsExecution.isRightActionForControleur(this.User, c, GestionDroitsExecution.ActionsFicheControleur.RETOUR))
                {
                    return View(c);
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

        // POST: /ControleurTechnique/ModificationRetourArriere
        [HttpPost]
        public ActionResult ModificationRetourArriere(int id)
        {
            try
            {
                bool userMode = this.User.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) == false;
                this._controleurTechniqueService.retourArriere(id, userMode, this.User.Identity.Name, false);
                return RedirectToAction("Fiche", new { id = id });
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        //
        // GET: /ControleurTechnique/DemandeAgrement/313
        public ActionResult DemandeAgrement(int id, rCategorieVehicule.CategorieEnum categorieVehicule)
        {
            try
            {
                //1 on retrieve l'id
                pContoleurEncour c = this._controleurTechniqueService.FindBy(id);

                Dictionary<rCategorieVehicule.CategorieEnum, GestionDroitsExecution.ActionsFicheControleur> actions = new Dictionary<rCategorieVehicule.CategorieEnum, GestionDroitsExecution.ActionsFicheControleur>();
                actions.Add(rCategorieVehicule.CategorieEnum.VL, GestionDroitsExecution.ActionsFicheControleur.DEAVL);
                actions.Add(rCategorieVehicule.CategorieEnum.PL, GestionDroitsExecution.ActionsFicheControleur.DEAPL);
                actions.Add(rCategorieVehicule.CategorieEnum.CL, GestionDroitsExecution.ActionsFicheControleur.DEACL);

                if (GestionDroitsExecution.isRightActionForControleur(this.User, c, actions[categorieVehicule]))
                {
                    ActionAgrementControleurModel model = new ActionAgrementControleurModel();
                    model.controleur = c;
                    model.categorieVehicule = categorieVehicule;
                    model.NombreChargement = 0;

                    return View(model);
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

        //
        // POST: /ControleurTechnique/DemandeAgrement
        [HttpPost]
        public ActionResult DemandeAgrement(int id, ActionAgrementControleurModel model)
        {
            try
            {
                validateDemandeAgrement(model, this.User);
                if (ModelState.IsValid)
                {
                    this._controleurTechniqueService.demandeAgrement(id, model.controleur, model.categorieVehicule, this.User.Identity.Name);

                    return this.RedirectToAction("Fiche", new { id = id });
                }
                pContoleurEncour c = this._controleurTechniqueService.FindBy(id);
                model.controleur.Nom = c.Nom;
                model.controleur.Prenom = c.Prenom;
                model.NombreChargement++;
                return View(model);
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        //
        // GET: /ControleurTechnique/RejetAgrement/313
        public ActionResult RejetAgrement(int id, rCategorieVehicule.CategorieEnum categorieVehicule)
        {
            try
            {
                //1 on retrieve l'id
                pContoleurEncour c = this._controleurTechniqueService.FindBy(id);
                Dictionary<rCategorieVehicule.CategorieEnum, GestionDroitsExecution.ActionsFicheControleur> actions = new Dictionary<rCategorieVehicule.CategorieEnum, GestionDroitsExecution.ActionsFicheControleur>();
                actions.Add(rCategorieVehicule.CategorieEnum.VL, GestionDroitsExecution.ActionsFicheControleur.REJVL);
                actions.Add(rCategorieVehicule.CategorieEnum.PL, GestionDroitsExecution.ActionsFicheControleur.REJPL);
                actions.Add(rCategorieVehicule.CategorieEnum.CL, GestionDroitsExecution.ActionsFicheControleur.REJCL);

                if (GestionDroitsExecution.isRightActionForControleur(this.User, c, actions[categorieVehicule]))
                {

                    ActionAgrementControleurModel model = new ActionAgrementControleurModel();
                    model.controleur = c;
                    model.categorieVehicule = categorieVehicule;

                    return View(model);
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

        //
        // POST: /ControleurTechnique/RejetAgrement
        [HttpPost]
        public ActionResult RejetAgrement(int id, ActionAgrementControleurModel model)
        {
            try
            {
                this._controleurTechniqueService.rejetAgrement(id, model.controleur, model.categorieVehicule, this.User.Identity.Name);

                return this.RedirectToAction("Fiche", new { id = id });

            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        //Debut

        //
        // GET: /ControleurTechnique/AnnulerAgrement/313
        public ActionResult AnnulerAgrement(int id, rCategorieVehicule.CategorieEnum categorieVehicule)
        {
            try
            {
                //1 on retrieve l'id
                pContoleurEncour c = this._controleurTechniqueService.FindBy(id);
                //actions possibles
                Dictionary<rCategorieVehicule.CategorieEnum, GestionDroitsExecution.ActionsFicheControleur> actions = new Dictionary<rCategorieVehicule.CategorieEnum, GestionDroitsExecution.ActionsFicheControleur>();
                actions.Add(rCategorieVehicule.CategorieEnum.VL, GestionDroitsExecution.ActionsFicheControleur.ANUVL);
                actions.Add(rCategorieVehicule.CategorieEnum.PL, GestionDroitsExecution.ActionsFicheControleur.ANUPL);
                actions.Add(rCategorieVehicule.CategorieEnum.CL, GestionDroitsExecution.ActionsFicheControleur.ANUCL);

                if (GestionDroitsExecution.isRightActionForControleur(this.User, c, actions[categorieVehicule]))
                {
                    ActionAgrementControleurModel model = new ActionAgrementControleurModel();
                    model.controleur = c;
                    model.categorieVehicule = categorieVehicule;

                    return View(model);
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

        //
        // POST: /ControleurTechnique/AnnulerAgrement
        [HttpPost]
        public ActionResult AnnulerAgrement(int id, ActionAgrementControleurModel model)
        {
            try
            {
                validateAnnulationAgrement(model);
                if (ModelState.IsValid)
                {
                    this._controleurTechniqueService.annulerAgrement(id, model.controleur, model.categorieVehicule, this.User.Identity.Name);

                    //return this.RedirectToAction("Fiche", new { id = id });
                    return this.RedirectToAction("ResultatAnnulerAgrement", new { id = id, categorie = model.categorieVehicule });
                }

                return View(model);
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        public ActionResult ResultatAnnulerAgrement(int id, rCategorieVehicule.CategorieEnum categorie)
        {
            try
            {
                //1 on retrieve l'id
                pContoleurEncour c = this._controleurTechniqueService.FindBy(id);

                ActionAgrementControleurModel model = new ActionAgrementControleurModel();
                model.controleur = c;
                model.categorieVehicule = categorie;

                return View(model);
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        //
        // GET: /ControleurTechnique/NotifierAgrement/313/Categorie/VL
        public ActionResult NotifierAgrement(int id, rCategorieVehicule.CategorieEnum categorieVehicule)
        {
            try
            {
                //1 on retrieve l'id
                pContoleurEncour c = this._controleurTechniqueService.FindBy(id);
                Dictionary<rCategorieVehicule.CategorieEnum, GestionDroitsExecution.ActionsFicheControleur> actions = new Dictionary<rCategorieVehicule.CategorieEnum, GestionDroitsExecution.ActionsFicheControleur>();
                actions.Add(rCategorieVehicule.CategorieEnum.VL, GestionDroitsExecution.ActionsFicheControleur.NOTVL);
                actions.Add(rCategorieVehicule.CategorieEnum.PL, GestionDroitsExecution.ActionsFicheControleur.NOTPL);
                actions.Add(rCategorieVehicule.CategorieEnum.CL, GestionDroitsExecution.ActionsFicheControleur.NOTCL);

                if (GestionDroitsExecution.isRightActionForControleur(this.User, c, actions[categorieVehicule]))
                {
                    ActionAgrementControleurModel model = new ActionAgrementControleurModel();
                    model.controleur = c;
                    model.categorieVehicule = categorieVehicule;
                    //PLE le 11/04/2024 initialisation SVA à false par défaut
                    switch (categorieVehicule)
                    {
                        case rCategorieVehicule.CategorieEnum.VL:
                            c.SvaVL = false;
                            break;
                        case rCategorieVehicule.CategorieEnum.PL:
                            c.SvaPL = false;
                            break;
                        case rCategorieVehicule.CategorieEnum.CL:
                            c.SvaCL = false;
                            break;
                    }

                    return View(model);
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

        //
        // POST: /ControleurTechnique/NotifierAgrement/313/Categorie/VL
        [HttpPost]
        public ActionResult NotifierAgrement(int id, ActionAgrementControleurModel model)
        {
            try
            {
                validateNotification(model, this.User);
                if (ModelState.IsValid)
                {
                    this._controleurTechniqueService.notificationAgrement(id, model.controleur, model.categorieVehicule, this.User.Identity.Name);

                    return this.RedirectToAction("ResultatNotifierAgrement", new { id = id, categorie = model.categorieVehicule });
                }
                return View(model);
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        public ActionResult ResultatNotifierAgrement(int id, rCategorieVehicule.CategorieEnum categorie)
        {
            try
            {
                //1 on retrieve l'id
                pContoleurEncour c = this._controleurTechniqueService.FindBy(id);

                ActionAgrementControleurModel model = new ActionAgrementControleurModel();
                model.controleur = c;
                model.categorieVehicule = categorie;

                return View(model);
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        //
        // GET: /ControleurTechnique/DeclarerHabilitationGaz/313/Categorie/VL      
        public ActionResult DeclarerHabilitationGaz(int id)
        {
            try
            {
                //1 on retrieve l'id
                pContoleurEncour c = this._controleurTechniqueService.FindBy(id);

                if (GestionDroitsExecution.isRightActionForControleur(this.User, c, GestionDroitsExecution.ActionsFicheControleur.HABILGAZ))
                {

                    ActionAgrementControleurModel model = new ActionAgrementControleurModel() { controleur = c, categorieVehicule = rCategorieVehicule.CategorieEnum.VL, NombreChargement = 0 };

                    return View(model);
                }
                else
                {
                    Exception e = new Exception("Vous n'êtes pas autorisé à exécuter cette action....");
                    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                }
            }
            catch (Exception)
            {
                return ErrorHelper.InvaliAccessError(this.ControllerContext);
            }
        }

        //
        // POST: /ControleurTechnique/DeclarerHabilitationGaz/313/Categorie/VL
        [HttpPost]
        public ActionResult DeclarerHabilitationGaz(int id, ActionAgrementControleurModel model)
        {
            try
            {
                validateDeclarationHabilitationGaz(model);
                if (ModelState.IsValid)
                {
                    this._controleurTechniqueService.declarerHabilitationGaz(id, model.controleur, this.User.Identity.Name);

                    return this.RedirectToAction("Fiche", new { id = id });
                }
                model.NombreChargement++;
                return View(model);
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // GET: /ControleurTechnique/AjouterAmende/62
        public ActionResult AjouterAmende(int id)
        {
            try
            {
                //1 on retrieve l'id
                pContoleurEncour c = this._controleurTechniqueService.FindBy(id);
                if (GestionDroitsExecution.isRightActionForControleur(this.User, c, GestionDroitsExecution.ActionsFicheControleur.AMEND))
                {
                    AjouterAmendeControleurModel m = new AjouterAmendeControleurModel();
                    m.Controleur = c;

                    return View(m);
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

        // POST: /ControleurTechnique/AjouterAmende
        [HttpPost]
        public ActionResult AjouterAmende(AjouterAmendeControleurModel m)
        {
            try
            {
                m.Controleur = this._controleurTechniqueService.FindBy(m.Controleur.Id);
                //if (GestionDroitsExecution.isRightActionForCentre(this.User, m.Controleur, GestionDroitsExecution.ActionsFicheCentre.AMEND))
                //{
                validateAjouterAmende(m);
                if (ModelState.IsValid)
                {
                    //On utilise le certificatService nouvelleAmende (historisation et enregistrement amende)
                    pAmendeControleur a = this._pAmendeControleurService.nouvelleAmende(m.Controleur, m.Amende, this.User.Identity.Name);

                    //On affiche la fiche du centre
                    return this.RedirectToAction("ListeAmendes", new { id = m.Controleur.Id });
                }
                return View("AjouterAmende", m);
                //}
                //else
                //{
                //    Exception e = new Exception("Vous n'êtes pas autorisé....");
                //    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                //}
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // GET: /ControleurTechnique/ListeAmendes/62
        public ActionResult ListeAmendes(int id)
        {
            try
            {
                pContoleurEncour c = this._controleurTechniqueService.FindBy(id);
                if (GestionDroitsExecution.isRightActionForControleur(this.User, c, GestionDroitsExecution.ActionsFicheControleur.VOIR_AMEND))
                {
                    return View(c);
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

        // GET: /ControleurTechnique/ListeAudits/62
        public ActionResult ListeAuditsPL(int id)
        {
            try
            {
                pContoleurEncour c = this._controleurTechniqueService.FindBy(id);

                return View(c);

            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // GET: /ControleurTechnique/ModifierAmende/62
        public ActionResult ModifierAmende(int id)
        {
            try
            {
                pAmendeControleur a = this._pAmendeControleurService.FindBy(id);
                if (GestionDroitsExecution.isRightActionForControleur(this.User, a.PContoleurEncour, GestionDroitsExecution.ActionsFicheControleur.VOIR_AMEND))
                {
                    AjouterAmendeControleurModel m = new AjouterAmendeControleurModel();
                    m.Amende = a;
                    m.Controleur = a.PContoleurEncour;

                    return View(m);
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

        // POST: /ControleurTechnique/ModifierAmende
        [HttpPost]
        public ActionResult ModifierAmende(AjouterAmendeControleurModel m)
        {
            try
            {
                m.Controleur = this._controleurTechniqueService.FindBy(m.Controleur.Id);
                //if (GestionDroitsExecution.isRightActionForCentre(this.User, m.Controleur, GestionDroitsExecution.ActionsFicheCentre.AMEND))
                //{
                validateAjouterAmende(m);
                if (ModelState.IsValid)
                {
                    //On utilise le certificatService nouvelleSanction (historisation, annulation agrément et enregistrement sanction)
                    pAmendeControleur a = this._pAmendeControleurService.modifierAmende(m.Amende, this.User.Identity.Name);

                    //On affiche la liste des amendes
                    return this.RedirectToAction("ListeAmendes", new { id = m.Amende.PContoleurEncour.Id });
                }
                return View("ModifierSanction", m);
                //}
                //else
                //{
                //    Exception e = new Exception("Vous n'êtes pas autorisé....");
                //    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                //}
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // GET: /Centre/ConfirmationDeleteAmende/60
        public ActionResult ConfirmationDeleteAmende(int id)
        {
            try
            {
                pAmendeControleur a = this._pAmendeControleurService.FindBy(id);
                //if (GestionDroitsExecution.isRightActionForCentre(this.User, a.PCentreEncour, GestionDroitsExecution.ActionsFicheCentre.AMEND))
                //{
                return View(a);
                //}
                //else
                //{
                //    Exception e = new Exception("Vous n'êtes pas autorisé....");
                //    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                //}
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

        // GET: /Centre/DeleteAmende/60
        [HttpPost]
        public ActionResult DeleteAmende(int id)
        {
            try
            {
                pAmendeControleur a = this._pAmendeControleurService.FindBy(id);
                //if (GestionDroitsExecution.isRightActionForCentre(this.User, a.PContoleurEncour, GestionDroitsExecution.ActionsFicheCentre.AMEND))
                //{
                // on supprime l'amende
                this._pAmendeControleurService.deleteAmende(a);

                //on affiche la liste des sanctions
                return this.RedirectToAction("ListeAmendes", new { id = a.PContoleurEncour.Id });
                //}
                //else
                //{
                //    Exception e = new Exception("Vous n'êtes pas autorisé....");
                //    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                //}
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

        private void validateAjouterAmende(AjouterAmendeControleurModel m)
        {
            if (m.Amende.LibContravention == null)
                ModelState.AddModelError("amende.LibContravention", "Le libellé de l'amende doit être renseigné.");
            if (m.Amende.DateEmisAmende == null)
                ModelState.AddModelError("amende.DateEmisAmende", "La date de l'amende doit être renseignée.");
        }

        private void validateDeclarationHabilitationGaz(ActionAgrementControleurModel model)
        {
            //On vérifie que l'habilitation gaz est saisie
            if (model.controleur.HabilitationGazVL == null)
            {
                ModelState.AddModelError("controleur.HabilitationGazVL", "habilitation gaz obligatoire (choisir oui/non)");
            }

            //On vérifie que la date de début ou fin d'habilitation est saisie
            if (model.controleur.DateDebutFinHabilitationGazVL == null)
            {
                ModelState.AddModelError("controleur.DateDebutFinHabilitationGazVL", "La date est obligatoire");
            }
            else
            {
                //On vérifie que la date de début ou fin d'habilitation est supérieure ou égale à la date de rattachement au centre
                if (model.controleur.DateDebutFinHabilitationGazVL < model.controleur.DateDebutRattachementVl)
                {
                    ModelState.AddModelError("controleur.DateDebutFinHabilitationGazVL", "La date doit être supérieure ou égale à la date de rattachement au centre");
                }
            }

        }

        public ActionResult TelechargerNotificationPDF(int id, rCategorieVehicule.CategorieEnum categorieVehicule)
        {
            try
            {
                //Définition des actions possibles
                Dictionary<rCategorieVehicule.CategorieEnum, GestionDroitsExecution.ActionsFicheControleur> actions = new Dictionary<rCategorieVehicule.CategorieEnum, GestionDroitsExecution.ActionsFicheControleur>();
                actions.Add(rCategorieVehicule.CategorieEnum.VL, GestionDroitsExecution.ActionsFicheControleur.TELECHARGE_NOTIFVL_PDF);
                actions.Add(rCategorieVehicule.CategorieEnum.PL, GestionDroitsExecution.ActionsFicheControleur.TELECHARGE_NOTIFPL_PDF);
                actions.Add(rCategorieVehicule.CategorieEnum.CL, GestionDroitsExecution.ActionsFicheControleur.TELECHARGE_NOTIFCL_PDF);

                //Récupération du contrôleur
                pContoleurEncour controleur = this._controleurTechniqueService.FindBy(id);

                //Vérification des droits d'éxécution
                //if (canEditPdf(controleur))
                if (GestionDroitsExecution.isRightActionForControleur(this.User, controleur, actions[categorieVehicule]))
                {
                    rTypeDemandeControleur.TypeModification dernierModification = (rTypeDemandeControleur.TypeModification)Enum.Parse(typeof(rTypeDemandeControleur.TypeModification), controleur.RTypeDemandeControleur.Id);
                    Dictionary<string, string> rpts = new Dictionary<string, string>();
                    rpts.Add(rTypeDemandeControleur.TypeModification.NOTVL.ToString(), "cntlr_notification_decision_agrem_init_annexe1-AB");
                    rpts.Add(rTypeDemandeControleur.TypeModification.NOTPL.ToString(), "cntlr_notification_decision_agrem_init_annexe1-AB");
                    rpts.Add(rTypeDemandeControleur.TypeModification.NOTCL.ToString(), "cntlr_notification_decision_agrem_init_annexe1-AB");

                    rpts.Add(rTypeDemandeControleur.TypeModification.ANUVL.ToString(), "cntlr_notification_decision_agrem_modif_annexe1-CD");
                    rpts.Add(rTypeDemandeControleur.TypeModification.ANUPL.ToString(), "cntlr_notification_decision_agrem_modif_annexe1-CD");
                    rpts.Add(rTypeDemandeControleur.TypeModification.ANUCL.ToString(), "cntlr_notification_decision_agrem_modif_annexe1-CD");
                    rpts.Add(rTypeDemandeControleur.TypeModification.CHCEVL.ToString(), "cntlr_notification_decision_agrem_modif_annexe1-CD");
                    rpts.Add(rTypeDemandeControleur.TypeModification.CHCEPL.ToString(), "cntlr_notification_decision_agrem_modif_annexe1-CD");
                    rpts.Add(rTypeDemandeControleur.TypeModification.CHCECL.ToString(), "cntlr_notification_decision_agrem_modif_annexe1-CD");

                    //On charge le document Crystal
                    ReportDocument boReportDocument = new ReportDocument();
                    string rptFileName = "~/bin/reporting/" + rpts[controleur.RTypeDemandeControleur.Id] + ".rpt";
                    boReportDocument.Load(Server.MapPath(rptFileName));

                    ControleurDataRowAdapter dr = null;
                    System.Data.DataTable dt = null;
                    if (
                         rTypeDemandeControleur.TypeModification.NOTVL.Equals(dernierModification) ||
                         rTypeDemandeControleur.TypeModification.NOTPL.Equals(dernierModification) ||
                         rTypeDemandeControleur.TypeModification.NOTCL.Equals(dernierModification)
                        )
                    {
                        dt = new cntlr_notification_decision_agrem_init_annexe1_AB().notification_controleur;
                        dr = new ControleurDataRowAdapter(((cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurDataTable)dt).Newnotification_controleurRow());
                    }
                    else
                    {
                        dt = new cntlr_notification_decision_agrem_modif_annexe1_CD().notification_controleur;
                        dr = new ControleurDataRowAdapter(((cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurDataTable)dt).Newnotification_controleurRow());
                    }



                    dr.est_type_agrem_vl = controleur.RTypeDemandeControleur.Id.Equals(rTypeDemandeControleur.TypeModification.NOTVL.ToString()) ||
                                           controleur.RTypeDemandeControleur.Id.Equals(rTypeDemandeControleur.TypeModification.ANUVL.ToString()) ||
                                           controleur.RTypeDemandeControleur.Id.Equals(rTypeDemandeControleur.TypeModification.CHCEVL.ToString());

                    dr.est_type_agrem_pl = controleur.RTypeDemandeControleur.Id.Equals(rTypeDemandeControleur.TypeModification.NOTPL.ToString()) ||
                                           controleur.RTypeDemandeControleur.Id.Equals(rTypeDemandeControleur.TypeModification.ANUPL.ToString()) ||
                                           controleur.RTypeDemandeControleur.Id.Equals(rTypeDemandeControleur.TypeModification.CHCEPL.ToString());

                    dr.est_type_agrem_cl = controleur.RTypeDemandeControleur.Id.Equals(rTypeDemandeControleur.TypeModification.NOTCL.ToString()) ||
                                           controleur.RTypeDemandeControleur.Id.Equals(rTypeDemandeControleur.TypeModification.ANUCL.ToString()) ||
                                           controleur.RTypeDemandeControleur.Id.Equals(rTypeDemandeControleur.TypeModification.CHCECL.ToString());

                    dr.est_prestataire = controleur.EstPrestataire;
                    dr.nom = controleur.Nom;
                    dr.prenom = controleur.Prenom;
                    //PLE le 01/03/2024 ajout de la date d'edition de la notification
                    dr.date_edition = controleur.DateMiseaJour;

                    //PLE le 11/04/2025 prise en compte SVA
                    switch (categorieVehicule)
                    {
                        case rCategorieVehicule.CategorieEnum.VL:
                            dr.est_sva = controleur.SvaVL;
                            break;
                        case rCategorieVehicule.CategorieEnum.PL:
                            dr.est_sva = controleur.SvaPL;
                            break;
                        case rCategorieVehicule.CategorieEnum.CL:
                            dr.est_sva = controleur.SvaCL;
                            break;
                    }
                    if (controleur.DateNaissance != null)
                        dr.date_naissance = controleur.DateNaissance.Value;

                    if (
                         rTypeDemandeControleur.TypeModification.NOTVL.Equals(dernierModification) ||
                         rTypeDemandeControleur.TypeModification.ANUVL.Equals(dernierModification) ||
                         rTypeDemandeControleur.TypeModification.CHCEVL.Equals(dernierModification)
                       )
                    {
                        dr.cod_agrem_controleur = controleur.CodAgremVl;
                        dr.cod_agrem_centre_rattach = controleur.CodAgremRattachementVl;
                        dr.titre_departement = "PREFET " + controleur.PCentreEncourVL.RPrefecture.PrefixeSansSlashe + controleur.PCentreEncourVL.RPrefecture.LibellePrefecture;
                        dr.libelle_prefet = controleur.PCentreEncourVL.RPrefecture.PrefixeSansSlashe + controleur.PCentreEncourVL.RPrefecture.LibellePrefecture;
                        dr.date_effet_notification = controleur.DateEffetNotificationVl.Value;
                    }

                    if (
                        rTypeDemandeControleur.TypeModification.NOTPL.Equals(dernierModification) ||
                        rTypeDemandeControleur.TypeModification.ANUPL.Equals(dernierModification) ||
                        rTypeDemandeControleur.TypeModification.CHCEPL.Equals(dernierModification)
                      )
                    {
                        dr.cod_agrem_controleur = controleur.CodAgremPl;
                        dr.cod_agrem_centre_rattach = controleur.CodAgremRattachementPl;
                        dr.titre_departement = "PREFET " + controleur.PCentreEncourPL.RPrefecture.PrefixeSansSlashe + controleur.PCentreEncourPL.RPrefecture.LibellePrefecture;
                        dr.libelle_prefet = controleur.PCentreEncourPL.RPrefecture.PrefixeSansSlashe + controleur.PCentreEncourPL.RPrefecture.LibellePrefecture;
                        dr.date_effet_notification = controleur.DateEffetNotificationPl.Value;
                    }

                    if (
                      rTypeDemandeControleur.TypeModification.NOTCL.Equals(dernierModification) ||
                      rTypeDemandeControleur.TypeModification.ANUCL.Equals(dernierModification) ||
                      rTypeDemandeControleur.TypeModification.CHCECL.Equals(dernierModification)
                    )
                    {
                        dr.cod_agrem_controleur = controleur.CodAgremCl;
                        dr.cod_agrem_centre_rattach = controleur.CodAgremRattachementCl;
                        dr.titre_departement = "PREFET " + controleur.PCentreEncourCL.RPrefecture.PrefixeSansSlashe + controleur.PCentreEncourCL.RPrefecture.LibellePrefecture;
                        dr.libelle_prefet = controleur.PCentreEncourCL.RPrefecture.PrefixeSansSlashe + controleur.PCentreEncourCL.RPrefecture.LibellePrefecture;
                        dr.date_effet_notification = controleur.DateEffetNotificationCl.Value;
                    }

                    if (
                      rTypeDemandeControleur.TypeModification.ANUVL.Equals(dernierModification) ||
                      rTypeDemandeControleur.TypeModification.ANUPL.Equals(dernierModification) ||
                      rTypeDemandeControleur.TypeModification.ANUCL.Equals(dernierModification) ||
                      rTypeDemandeControleur.TypeModification.CHCEVL.Equals(dernierModification) ||
                      rTypeDemandeControleur.TypeModification.CHCEPL.Equals(dernierModification) ||
                      rTypeDemandeControleur.TypeModification.CHCECL.Equals(dernierModification)
                        )
                    {
                        dr.cod_agrem_centre_rattach_ancien =
                            rTypeDemandeControleur.TypeModification.ANUVL.Equals(dernierModification) || rTypeDemandeControleur.TypeModification.CHCEVL.Equals(dernierModification) ?
                            controleur.CodAgremRattachementVl :
                            rTypeDemandeControleur.TypeModification.ANUPL.Equals(dernierModification) || rTypeDemandeControleur.TypeModification.CHCEPL.Equals(dernierModification) ?
                            controleur.CodAgremRattachementPl : controleur.CodAgremRattachementCl;

                        /* EG le 22/02/2012 : bug détecté par TAVERNIER Philippe PREF84 du 21/02/2012
                        dr.date_effet_notification =  
                            rTypeDemandeControleur.TypeModification.ANUVL.Equals(dernierModification) || rTypeDemandeControleur.TypeModification.CHCEVL.Equals(dernierModification) ? 
                            controleur.DateEffetNotificationVl.Value :
                            rTypeDemandeControleur.TypeModification.ANUPL.Equals(dernierModification) || rTypeDemandeControleur.TypeModification.CHCEPL.Equals(dernierModification) ?
                            controleur.DateEffetNotificationPl.Value : controleur.DateEffetNotificationCl.Value;
                         */

                    }

                    if (
                      rTypeDemandeControleur.TypeModification.ANUVL.Equals(dernierModification) ||
                      rTypeDemandeControleur.TypeModification.ANUPL.Equals(dernierModification) ||
                      rTypeDemandeControleur.TypeModification.ANUCL.Equals(dernierModification)
                        )
                    {

                        dr.cod_agrem_centre_rattach_nouveau = null;
                        dr.est_motif_cessation_activite = true;
                        dr.est_motif_chang_centre_rattach = false;
                        /* EG le 22/02/2012 : bug détecté par TAVERNIER Philippe PREF84 du 21/02/2012 */

                        if (rTypeDemandeControleur.TypeModification.ANUVL.Equals(dernierModification))
                        {
                            dr.date_effet_notification = controleur.DateFinAgrementVl.Value;
                        }

                        if (rTypeDemandeControleur.TypeModification.ANUPL.Equals(dernierModification))
                        {
                            dr.date_effet_notification = controleur.DateFinAgrementPl.Value;
                        }

                        if (rTypeDemandeControleur.TypeModification.ANUCL.Equals(dernierModification))
                        {
                            dr.date_effet_notification = controleur.DateFinAgrementCl.Value;
                        }

                    }

                    if (
                     rTypeDemandeControleur.TypeModification.CHCEVL.Equals(dernierModification) ||
                     rTypeDemandeControleur.TypeModification.CHCEPL.Equals(dernierModification) ||
                     rTypeDemandeControleur.TypeModification.CHCECL.Equals(dernierModification)
                       )
                    {
                        pControleurHisto histo = controleur.PControleurHistoriques.OrderByDescending(h => h.DateHisto).ElementAt(0);
                        dr.cod_agrem_centre_rattach_ancien =
                            rTypeDemandeControleur.TypeModification.CHCEVL.Equals(dernierModification) ?
                            histo.CodAgremRattachementVl :
                            rTypeDemandeControleur.TypeModification.CHCEPL.Equals(dernierModification) ?
                            histo.CodAgremRattachementPl : histo.CodAgremRattachementCl;

                        dr.cod_agrem_centre_rattach_nouveau =
                            rTypeDemandeControleur.TypeModification.CHCEVL.Equals(dernierModification) ?
                            controleur.CodAgremRattachementVl :
                            rTypeDemandeControleur.TypeModification.CHCEPL.Equals(dernierModification) ?
                            controleur.CodAgremRattachementPl : controleur.CodAgremRattachementCl;

                        dr.est_motif_cessation_activite = false;
                        dr.est_motif_chang_centre_rattach = true;

                        /* EG le 22/02/2012 : bug détecté par TAVERNIER Philippe PREF84 du 21/02/2012 */
                        if (rTypeDemandeControleur.TypeModification.CHCEVL.Equals(dernierModification))
                        {
                            dr.date_effet_notification = controleur.DateEffetNotificationVl.Value;
                        }

                        if (rTypeDemandeControleur.TypeModification.CHCEPL.Equals(dernierModification))
                        {
                            dr.date_effet_notification = controleur.DateEffetNotificationPl.Value;
                        }

                        if (rTypeDemandeControleur.TypeModification.CHCECL.Equals(dernierModification))
                        {
                            dr.date_effet_notification = controleur.DateEffetNotificationCl.Value;
                        }

                    }

                    dt.Rows.Add(dr.RealDataRow);
                    boReportDocument.SetDataSource((System.Data.DataTable)dt);

                    //Définition du format d'export PDF
                    ExportOptions exportOptions = new ExportOptions();
                    //This sets which format we will export to.
                    exportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    //Create the FormatOptions class for PDF.
                    PdfFormatOptions PDFexportOptions = new PdfFormatOptions();
                    PDFexportOptions.FirstPageNumber = 1;
                    PDFexportOptions.LastPageNumber = 2;
                    PDFexportOptions.CreateBookmarksFromGroupTree = true;

                    //Set the export format options with the page range we just set.
                    exportOptions.FormatOptions = PDFexportOptions;

                    //On exporte le document au format PDF
                    //boReportDocument.Export(exportOptions);

                    Stream pdfStream = boReportDocument.ExportToStream(ExportFormatType.PortableDocFormat);

                    MemoryStream memStream = new MemoryStream();

                    byte[] respBuffer = new byte[1024];

                    int bytesRead = pdfStream.Read(respBuffer, 0, respBuffer.Length);
                    while (bytesRead > 0)
                    {
                        memStream.Write(respBuffer, 0, bytesRead);
                        bytesRead = pdfStream.Read(respBuffer, 0, respBuffer.Length);
                    }

                    string pdfFileName = null;

                    if (rTypeDemandeControleur.TypeModification.NOTVL.Equals(dernierModification)) { pdfFileName = controleur.CodAgremVl + "_notification_agrement_initial.pdf"; }
                    if (rTypeDemandeControleur.TypeModification.NOTPL.Equals(dernierModification)) { pdfFileName = controleur.CodAgremPl + "_notification_agrement_initial.pdf"; }
                    if (rTypeDemandeControleur.TypeModification.NOTCL.Equals(dernierModification)) { pdfFileName = controleur.CodAgremCl + "_notification_agrement_initial.pdf"; }

                    if (rTypeDemandeControleur.TypeModification.ANUVL.Equals(dernierModification)) { pdfFileName = controleur.CodAgremVl + "_notification_modification_agrement.pdf"; }
                    if (rTypeDemandeControleur.TypeModification.ANUPL.Equals(dernierModification)) { pdfFileName = controleur.CodAgremPl + "_notification_modification_agrement.pdf"; }
                    if (rTypeDemandeControleur.TypeModification.ANUCL.Equals(dernierModification)) { pdfFileName = controleur.CodAgremCl + "_notification_modification_agrement.pdf"; }

                    if (rTypeDemandeControleur.TypeModification.CHCEVL.Equals(dernierModification)) { pdfFileName = controleur.CodAgremVl + "_notification_modification_agrement.pdf"; }
                    if (rTypeDemandeControleur.TypeModification.CHCEPL.Equals(dernierModification)) { pdfFileName = controleur.CodAgremPl + "_notification_modification_agrement.pdf"; }
                    if (rTypeDemandeControleur.TypeModification.CHCECL.Equals(dernierModification)) { pdfFileName = controleur.CodAgremCl + "_notification_modification_agrement.pdf"; }

                    //string pdfFileName = rpts[controleur.RTypeDemandeControleur.Id] + "_" +  controleur.Id + ".pdf";

                    //PLE 06/06/2012 libération du rapport
                    boReportDocument.Close();
                    boReportDocument.Dispose();
                    return File(memStream.GetBuffer(), "application/pdf", pdfFileName);

                }
                else
                {
                    //Interdiction d'imprimer
                    //return RedirectToAction("Fiche", new { id = id });
                    return ErrorHelper.InvaliAccessError(this.ControllerContext);
                }

            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        //
        // GET ControleurTechnique/TelechargeFichePDF
        //06/03/2012 PLE : implémentation de la fiche controleur

        public ActionResult TelechargeFichePDF(int id)
        {
            try
            {
                pContoleurEncour c = this._controleurTechniqueService.FindBy(id);
                if (GestionDroitsExecution.isRightActionForControleur(this.User, c, GestionDroitsExecution.ActionsFicheControleur.TELECHARGE_FICHE_CONTROLEUR))
                {
                    //On charge le document Crystal
                    ReportDocument boReportDocument = new ReportDocument();
                    string rptFileName = "~/bin/reporting/cntlr_ficheControleur.rpt";
                    boReportDocument.Load(Server.MapPath(rptFileName));

                    // Chargement du DATASET de la fiche contrôleur dans l'état crystal
                    DSficheControleur ds = new DSficheControleur();
                    boReportDocument.SetDataSource(ds.mapControleurDataset(c));

                    //Définition du format d'export PDF
                    ExportOptions exportOptions = new ExportOptions();
                    //This sets which format we will export to.
                    exportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    //Create the FormatOptions class for PDF.
                    PdfFormatOptions PDFexportOptions = new PdfFormatOptions();
                    PDFexportOptions.FirstPageNumber = 1;
                    PDFexportOptions.LastPageNumber = 2;
                    PDFexportOptions.CreateBookmarksFromGroupTree = true;

                    //Set the export format options with the page range we just set.
                    exportOptions.FormatOptions = PDFexportOptions;

                    //On exporte le document au format PDF
                    //boReportDocument.Export(exportOptions);

                    Stream pdfStream = boReportDocument.ExportToStream(ExportFormatType.PortableDocFormat);

                    MemoryStream memStream = new MemoryStream();

                    byte[] respBuffer = new byte[1024];

                    int bytesRead = pdfStream.Read(respBuffer, 0, respBuffer.Length);
                    while (bytesRead > 0)
                    {
                        memStream.Write(respBuffer, 0, bytesRead);
                        bytesRead = pdfStream.Read(respBuffer, 0, respBuffer.Length);
                    }

                    //PLE 06/06/2012 libération du rapport
                    boReportDocument.Close();
                    boReportDocument.Dispose();
                    return File(memStream.GetBuffer(), "application/pdf", "RNC2ficheControleur_" + c.Nom + "_" + c.Prenom + ".pdf");
                }

                throw new NotImplementedException();
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        public ActionResult TelechargeFicheQualificationPLPDF(int id)
        {
            try
            {
                pContoleurEncour c = this._controleurTechniqueService.FindBy(id);
                if (GestionDroitsExecution.isRightActionForControleur(this.User, c, GestionDroitsExecution.ActionsFicheControleur.TELECHARGE_FICHE_QUALIFICATION_CONTROLEUR))
                {
                    //PLE 12/03/2012 implémentation de la fiche de suivi de qualification PL d'un contrôleur

                    //Récupération des données
                    IStatistiquesControleurService controleurStatistiqueService = null;
                    controleurStatistiqueService = NhibernateServiceFactory.getInstance().getStatistiquesControleurService();
                    DateTime? datedebut = null;
                    DateTime? datefin = null;
                    int anneeNMoins1 = 0;
                    ItemListeSuiviQualificationPL item = controleurStatistiqueService.SuivisQualificationControleurs(c, ref datedebut, ref datefin, ref anneeNMoins1);

                    //On charge le document Crystal
                    ReportDocument boReportDocument = new ReportDocument();
                    boReportDocument.Load(Server.MapPath("~/bin/reporting/cntlr_ficheControleurSuiviQualificationPL.rpt"));

                    DSficheControleurSuiviQualificationPL ds = new DSficheControleurSuiviQualificationPL();
                    DSficheControleurSuiviQualificationPL.dtItemListeSuiviQualificationPLDataTable dt = ds.dtItemListeSuiviQualificationPL;
                    DSficheControleurSuiviQualificationPL.dtItemListeSuiviQualificationPLRow dr = dt.NewdtItemListeSuiviQualificationPLRow();
                    if (item != null)
                    {
                        dr.CodAgrement = item.CodAgrement;
                        dr.CodeAgrementCentre = item.CodeAgrementCentre;
                        dr.CodeDepartement = item.CodeDepartement;
                        dr.CodeHabilitation = item.CodeHabilitation;
                        dr.CodReseau = item.CodReseau;
                        dr.CumulActiviteQ1 = item.CumulActiviteQ1;
                        dr.CumulActiviteQ2 = item.CumulActiviteQ2;
                        dr.CumulActiviteQ3 = item.CumulActiviteQ3;
                        dr.DateAgrement = item.DateAgrement;
                        dr.DateDebutHabilitation = item.DateDebutHabilitation;
                        dr.DateDebutRattachementCentre = item.DateDebutRattachementCentre;
                        dr.DateDernierAuditQ1 = item.DateDernierAuditQ1;
                        dr.DateDernierAuditQ2 = item.DateDernierAuditQ2;
                        dr.DateDernierAuditQ3 = item.DateDernierAuditQ3;
                        dr.DateFormationQ1 = item.DateFormationQ1;
                        dr.DateFormationQ2 = item.DateFormationQ2;
                        dr.DateFormationQ3 = item.DateFormationQ3;
                        dr.DateNaissance = item.DateNaissance.Value;
                        dr.LibelleRegion = item.LibelleRegion;
                        dr.Nom = item.Nom;
                        dr.Prenom = item.Prenom;
                        dr.DateDebutPeriode = datedebut.Value;
                        dr.DateFinPeriode = datefin.Value;
                        //27/08/2018 -PLE- Suivi qualification contrôleur PL
                        dr.CumulActiviteQ1AnneeMoins1 = item.CumulActiviteQ1AnneeMoins1;
                        dr.CumulActiviteQ2AnneeMoins1 = item.CumulActiviteQ2AnneeMoins1;
                        dr.CumulActiviteQ3AnneeMoins1 = item.CumulActiviteQ3AnneeMoins1;
                        dr.anneeNMoins1 = anneeNMoins1;
                        dt.Rows.Add(dr);
                    }
                    // Chargement du DATASET de la fiche contrôleur dans l'état crystal

                    boReportDocument.SetDataSource(ds);

                    //Définition du format d'export PDF
                    ExportOptions exportOptions = new ExportOptions();
                    //This sets which format we will export to.
                    exportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    //Create the FormatOptions class for PDF.
                    PdfFormatOptions PDFexportOptions = new PdfFormatOptions();
                    PDFexportOptions.FirstPageNumber = 1;
                    PDFexportOptions.LastPageNumber = 2;
                    PDFexportOptions.CreateBookmarksFromGroupTree = true;

                    //Set the export format options with the page range we just set.
                    exportOptions.FormatOptions = PDFexportOptions;

                    //On exporte le document au format PDF
                    //boReportDocument.Export(exportOptions);

                    Stream pdfStream = boReportDocument.ExportToStream(ExportFormatType.PortableDocFormat);

                    MemoryStream memStream = new MemoryStream();

                    byte[] respBuffer = new byte[1024];

                    int bytesRead = pdfStream.Read(respBuffer, 0, respBuffer.Length);
                    while (bytesRead > 0)
                    {
                        memStream.Write(respBuffer, 0, bytesRead);
                        bytesRead = pdfStream.Read(respBuffer, 0, respBuffer.Length);
                    }

                    //PLE 06/06/2012 libération du rapport
                    boReportDocument.Close();
                    boReportDocument.Dispose();
                    return File(memStream.GetBuffer(), "application/pdf", "ficheControleurSuiviQualificationPL_" + c.CodAgremPl + ".pdf");

                }

                throw new NotImplementedException();
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // YMA 14/10/2013 suivi qualification VL
        public ActionResult TelechargeFicheQualificationVLPDF(int id)
        {
            try
            {
                //Données du controleur
                pContoleurEncour c = this._controleurTechniqueService.FindBy(id);
                //Données de l'activité du controleur
                pActiviteControleurVL a = this._pActiviteControleurVLService.FindBy(id);

                if (GestionDroitsExecution.isRightActionForControleur(this.User, c, GestionDroitsExecution.ActionsFicheControleur.TELECHARGE_FICHE_QUALIFICATION_CONTROLEUR_VL))
                {
                    //On charge le document Crystal
                    ReportDocument boReportDocument = new ReportDocument();
                    boReportDocument.Load(Server.MapPath("~/bin/reporting/cntlr_ficheControleurSuiviActiviteVL.rpt"));

                    DSficheControleurSuiviActiviteVL ds = new DSficheControleurSuiviActiviteVL();
                    DSficheControleurSuiviActiviteVL.dtItemListeSuiviActiviteVLDataTable dt = ds.dtItemListeSuiviActiviteVL;
                    DSficheControleurSuiviActiviteVL.dtItemListeSuiviActiviteVLRow dr = dt.NewdtItemListeSuiviActiviteVLRow();

                    dr.nom = c.Nom;
                    dr.prenom = c.Prenom;
                    dr.DateNaissance = c.DateNaissance.Value;
                    dr.codAgremCntlr = c.CodAgremVl;
                    if (c.DateEffetNotificationVl != null)
                    {
                        dr.dateEffetNotificationVl = Convert.ToDateTime(c.DateEffetNotificationVl);
                    }
                    if (c.DateFinAgrementVl != null)
                    {
                        dr.dateFinAgrementVl = Convert.ToDateTime(c.DateFinAgrementVl);
                    }
                    dr.codAgremCentre = c.CodAgremRattachementVl;
                    if (c.DateDebutRattachementVl != null)
                    {
                        dr.dateDebutRattachementVl = Convert.ToDateTime(c.DateDebutRattachementVl);
                    }
                    dr.HabilitationGazVLAsString = c.HabilitationGazVLAsString;
                    if (c.DateDebutFinHabilitationGazVL != null)
                    {
                        dr.DateDebutFinHabilitationGazVL = Convert.ToDateTime(c.DateDebutFinHabilitationGazVL);
                    }

                    if (a != null)
                    {
                        if (a.DateDernierCtrl != null)
                        {
                            dr.dateDernierCtrl = Convert.ToDateTime(a.DateDernierCtrl);
                        }
                        dr.activiteJanvierN1 = a.ActiviteJanvierN1.Value;
                        dr.activiteJanvierN = a.ActiviteJanvierN.Value;
                        dr.activiteFevrierN1 = a.ActiviteFevrierN1.Value;
                        dr.activiteFevrierN = a.ActiviteFevrierN.Value;
                        dr.activiteMarsN1 = a.ActiviteMarsN1.Value;
                        dr.activiteMarsN = a.ActiviteMarsN.Value;
                        dr.activiteAvrilN1 = a.ActiviteAvrilN1.Value;
                        dr.activiteAvrilN = a.ActiviteAvrilN.Value;
                        dr.activiteMaiN1 = a.ActiviteMaiN1.Value;
                        dr.activiteMaiN = a.ActiviteMaiN.Value;
                        dr.activiteJuinN1 = a.ActiviteJuinN1.Value;
                        dr.activiteJuinN = a.ActiviteJuinN.Value;
                        dr.activiteJuilletN1 = a.ActiviteJuilletN1.Value;
                        dr.activiteJuilletN = a.ActiviteJuilletN.Value;
                        dr.activiteAoutN1 = a.ActiviteAoutN1.Value;
                        dr.activiteAoutN = a.ActiviteAoutN.Value;
                        dr.activiteSeptembreN1 = a.ActiviteSeptembreN1.Value;
                        dr.activiteSeptembreN = a.ActiviteSeptembreN.Value;
                        dr.activiteOctobreN1 = a.ActiviteOctobreN1.Value;
                        dr.activiteOctobreN = a.ActiviteOctobreN.Value;
                        dr.activiteNovembreN1 = a.ActiviteNovembreN1.Value;
                        dr.activiteNovembreN = a.ActiviteNovembreN.Value;
                        dr.activiteDecembreN1 = a.ActiviteDecembreN1.Value;
                        dr.activiteDecembreN = a.ActiviteDecembreN.Value;
                        dr.totalVtpN1 = a.TotalVtpN1.Value;
                        dr.totalVtpN = a.TotalVtpN.Value;
                        dr.seuilN1ARealiser = a.SeuilN1ARealiser.Value;
                        dr.seuilNARealiser = a.SeuilNARealiser.Value;
                        dr.seuilAtteintN1 = a.SeuilAtteintN1;
                        dr.seuilAtteintN = a.SeuilAtteintN;
                        dr.anneeMois = a.AnneeMois;

                    }

                    dt.Rows.Add(dr);

                    // Chargement du DATASET de la fiche contrôleur dans l'état crystal

                    boReportDocument.SetDataSource(ds);

                    //Définition du format d'export PDF
                    ExportOptions exportOptions = new ExportOptions();
                    //This sets which format we will export to.
                    exportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    //Create the FormatOptions class for PDF.
                    PdfFormatOptions PDFexportOptions = new PdfFormatOptions();
                    PDFexportOptions.FirstPageNumber = 1;
                    PDFexportOptions.LastPageNumber = 2;
                    PDFexportOptions.CreateBookmarksFromGroupTree = true;

                    //Set the export format options with the page range we just set.
                    exportOptions.FormatOptions = PDFexportOptions;

                    //On exporte le document au format PDF
                    //boReportDocument.Export(exportOptions);

                    Stream pdfStream = boReportDocument.ExportToStream(ExportFormatType.PortableDocFormat);

                    MemoryStream memStream = new MemoryStream();

                    byte[] respBuffer = new byte[1024];

                    int bytesRead = pdfStream.Read(respBuffer, 0, respBuffer.Length);
                    while (bytesRead > 0)
                    {
                        memStream.Write(respBuffer, 0, bytesRead);
                        bytesRead = pdfStream.Read(respBuffer, 0, respBuffer.Length);
                    }

                    //libération du rapport
                    boReportDocument.Close();
                    boReportDocument.Dispose();
                    return File(memStream.GetBuffer(), "application/pdf", "ficheControleurSuiviQualificationVL_" + c.CodAgremVl + ".pdf");

                }

                throw new NotImplementedException();
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }
        //YM 24/05/2013
        //public ActionResult TelechargeFicheQualificationVLPDF(int id)
        //{
        //    try
        //    {
        //        pContoleurEncour c = this._controleurTechniqueService.FindBy(id);
        //        if (GestionDroitsExecution.isRightActionForControleur(this.User, c, GestionDroitsExecution.ActionsFicheControleur.TELECHARGE_FICHE_QUALIFICATION_CONTROLEUR_VL))
        //        {
        //            //YM 24/05/2013 implémentation de la fiche de suivi de qualification VL d'un contrôleur

        //            //Récupération des données
        //            IStatistiquesControleurService controleurStatistiqueService = null;
        //            controleurStatistiqueService = NhibernateServiceFactory.getInstance().getStatistiquesControleurService();
        //            DateTime? datedebut = null;
        //            DateTime? datefin = null;
        //            ItemListeSuiviQualificationVL item = controleurStatistiqueService.SuivisQualificationControleurs_VL(c, ref datedebut, ref datefin);

        //            //On charge le document Crystal
        //            ReportDocument boReportDocument = new ReportDocument();
        //            boReportDocument.Load(Server.MapPath("~/bin/reporting/cntlr_ficheControleurSuiviQualificationVL.rpt"));

        //            DSficheControleurSuiviQualificationVL ds = new DSficheControleurSuiviQualificationVL();
        //            DSficheControleurSuiviQualificationVL.dtItemListeSuiviQualificationVLDataTable dt = ds.dtItemListeSuiviQualificationVL;
        //            DSficheControleurSuiviQualificationVL.dtItemListeSuiviQualificationVLRow dr = dt.NewdtItemListeSuiviQualificationVLRow();
        //            if (item != null)
        //            {
        //                dr.CodAgrement = item.CodAgrement;
        //                dr.CodeAgrementCentre = item.CodeAgrementCentre;
        //                dr.CodeDepartement = item.CodeDepartement;
        //                dr.CodeHabilitation = item.CodeHabilitation;
        //                dr.CodReseau = item.CodReseau;
        //                dr.CumulActiviteQ1 = item.CumulActiviteQ1;
        //                dr.CumulActiviteQ2 = item.CumulActiviteQ2;
        //                dr.CumulActiviteQ3 = item.CumulActiviteQ3;
        //                dr.DateAgrement = item.DateAgrement;
        //                dr.DateDebutHabilitation = item.DateDebutHabilitation;
        //                dr.DateDebutRattachementCentre = item.DateDebutRattachementCentre;
        //                dr.DateDernierAuditQ1 = item.DateDernierAuditQ1;
        //                dr.DateDernierAuditQ2 = item.DateDernierAuditQ2;
        //                dr.DateDernierAuditQ3 = item.DateDernierAuditQ3;
        //                dr.DateFormationQ1 = item.DateFormationQ1;
        //                dr.DateFormationQ2 = item.DateFormationQ2;
        //                dr.DateFormationQ3 = item.DateFormationQ3;
        //                dr.DateNaissance = item.DateNaissance.Value;
        //                dr.LibelleRegion = item.LibelleRegion;
        //                dr.Nom = item.Nom;
        //                dr.Prenom = item.Prenom;
        //                dr.DateDebutPeriode = datedebut.Value;
        //                dr.DateFinPeriode = datefin.Value;
        //                dt.Rows.Add(dr);
        //            }
        //            // Chargement du DATASET de la fiche contrôleur dans l'état crystal

        //            boReportDocument.SetDataSource(ds);

        //            //Définition du format d'export PDF
        //            ExportOptions exportOptions = new ExportOptions();
        //            //This sets which format we will export to.
        //            exportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
        //            //Create the FormatOptions class for PDF.
        //            PdfFormatOptions PDFexportOptions = new PdfFormatOptions();
        //            PDFexportOptions.FirstPageNumber = 1;
        //            PDFexportOptions.LastPageNumber = 2;
        //            PDFexportOptions.CreateBookmarksFromGroupTree = true;

        //            //Set the export format options with the page range we just set.
        //            exportOptions.FormatOptions = PDFexportOptions;

        //            //On exporte le document au format PDF
        //            //boReportDocument.Export(exportOptions);

        //            Stream pdfStream = boReportDocument.ExportToStream(ExportFormatType.PortableDocFormat);

        //            MemoryStream memStream = new MemoryStream();

        //            byte[] respBuffer = new byte[1024];

        //            int bytesRead = pdfStream.Read(respBuffer, 0, respBuffer.Length);
        //            while (bytesRead > 0)
        //            {
        //                memStream.Write(respBuffer, 0, bytesRead);
        //                bytesRead = pdfStream.Read(respBuffer, 0, respBuffer.Length);
        //            }

        //            //PLE 06/06/2012 libération du rapport
        //            boReportDocument.Close();
        //            return File(memStream.GetBuffer(), "application/pdf", "ficheControleurSuiviQualificationVL_" + c.CodAgremVl + ".pdf");

        //        }

        //        throw new NotImplementedException();
        //    }
        //    catch (Exception e)
        //    {
        //        return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
        //    }
        //}

        /* EG commenté le 24/11/2011
        private bool canEditPdf(pContoleurEncour c)
        {
            if (
                 c.RTypeDemandeControleur.Id.Equals(rTypeDemandeControleur.TypeModification.NOTVL.ToString()) ||
                 c.RTypeDemandeControleur.Id.Equals(rTypeDemandeControleur.TypeModification.NOTPL.ToString()) ||
                 c.RTypeDemandeControleur.Id.Equals(rTypeDemandeControleur.TypeModification.NOTCL.ToString()) ||
                 c.RTypeDemandeControleur.Id.Equals(rTypeDemandeControleur.TypeModification.ANUVL.ToString()) ||
                 c.RTypeDemandeControleur.Id.Equals(rTypeDemandeControleur.TypeModification.ANUPL.ToString()) ||
                 c.RTypeDemandeControleur.Id.Equals(rTypeDemandeControleur.TypeModification.ANUCL.ToString()) ||
                 c.RTypeDemandeControleur.Id.Equals(rTypeDemandeControleur.TypeModification.CHCEVL.ToString()) ||
                 c.RTypeDemandeControleur.Id.Equals(rTypeDemandeControleur.TypeModification.CHCEPL.ToString()) ||
                 c.RTypeDemandeControleur.Id.Equals(rTypeDemandeControleur.TypeModification.CHCECL.ToString())
                 )
            {

                return true;

            }
            else
                return false;

        }
        */

        //
        // GET: /ControleurTechnique/Test

        public ActionResult Test()
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
        // POST: /ControleurTechnique/ConfirmerCreation

        public ActionResult ConfirmerCreation(NouveauControleurTechniqueModel model)
        {
            try
            {

                return View(model);
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

        //
        // GET: /ControleurTechnique/New/
        public ActionResult New(string nom, string prenom, DateTime dateNaissance, string agremVL, string agremPL, string agremCL, rCategorieVehicule.CategorieEnum categorie, int nombreChargement, string estPrestataire, String token, string confirmPrestataire, string confirmcontroleursEnDoublons)
        {
            try
            {
                NouveauControleurTechniqueModel model = new NouveauControleurTechniqueModel();
                model.controleur = new pContoleurEncour();
                model.controleur.Nom = nom;
                model.controleur.Prenom = prenom;
                model.controleur.DateNaissance = dateNaissance;
                model.controleur.CodAgremRattachementVl = agremVL;
                model.controleur.CodAgremRattachementPl = agremPL;
                model.controleur.CodAgremRattachementCl = agremCL;
                model.categorie = categorie;
                model.NombreChargement = nombreChargement;
                model.EstPrestataire = estPrestataire;
                model.Token = token;
                model.ConfirmPrestataire = confirmPrestataire;
                model.ConfirmControleursEnDoublons = confirmcontroleursEnDoublons;

                return View(model);
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

        //
        // POST: /ControleurTechnique/New
        [HttpPost]
        public ActionResult New(NouveauControleurTechniqueModel model)
        {
            try
            {

                model.Token = Convert.ToString(System.DateTime.Now);
                Session["token"] = model.Token;
                model.NombreChargement = 0;
                return View(model);
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

        //
        // POST: /ControleurTechnique/Create

        [HttpPost]
        public ActionResult Create(NouveauControleurTechniqueModel model)
        {
            try
            {
                //Vérification bouton précédent ?
                string token = Session["token"] != null ? Convert.ToString(Session["token"]) : null;
                if (string.IsNullOrEmpty(token))
                {
                    throw new Exception("TOKEN !!!");
                }
                else
                {
                    if (!token.Equals(model.Token))
                    {
                        throw new Exception("TOKEN PAS PAREIL !!!");
                    }
                }
                validateNew(model, this.User);
                if (ModelState.IsValid)
                {
                    //PLE le 04/03 Configuration détection contrôleur en doublon
                    if (model.controleursEnDoublons != null && model.controleursEnDoublons.Count() > 0)
                    {
                        model.ConfirmControleursEnDoublons = "XXX";
                        return View("New", model);
                    }

                    //YM le 22/10/2013 si préstataire demander une confirmation
                    if (Convert.ToBoolean(model.EstPrestataire) == true && model.ConfirmPrestataire != "oui")
                    {
                        return View("ConfirmationCreationPrestataire", model);
                    }

                    //EG le 21/10/2011 on transforme en booléen
                    model.controleur.EstPrestataire = Convert.ToBoolean(model.EstPrestataire);
                    this._controleurTechniqueServiceAutoCommit.creerNouveau(model.controleur, model.categorie, this.User.Identity.Name);
                    Session.Remove("token");
                    return RedirectToAction("Fiche", new { id = model.controleur.Id });
                }

                model.NombreChargement++;
                return View("New", model);
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        /* EG le 15/09/2011 prise en compte du changement de centre de rattachement */
        //
        // GET: /ControleurTechnique/NouveauCentreRattachement/313
        public ActionResult NouveauCentreRattachement(int id, rCategorieVehicule.CategorieEnum categorieVehicule)
        {
            try
            {
                //1 on retrieve l'id
                pContoleurEncour c = this._controleurTechniqueService.FindBy(id);
                Dictionary<rCategorieVehicule.CategorieEnum, GestionDroitsExecution.ActionsFicheControleur> actions = new Dictionary<rCategorieVehicule.CategorieEnum, GestionDroitsExecution.ActionsFicheControleur>();
                actions.Add(rCategorieVehicule.CategorieEnum.VL, GestionDroitsExecution.ActionsFicheControleur.CHCEVL);
                actions.Add(rCategorieVehicule.CategorieEnum.PL, GestionDroitsExecution.ActionsFicheControleur.CHCEPL);
                actions.Add(rCategorieVehicule.CategorieEnum.CL, GestionDroitsExecution.ActionsFicheControleur.CHCECL);

                if (GestionDroitsExecution.isRightActionForControleur(this.User, c, actions[categorieVehicule]))
                {

                    ActionAgrementControleurModel model = new ActionAgrementControleurModel();
                    model.controleur = c;
                    model.categorieVehicule = categorieVehicule;
                    //EG le 17/10/2011 : pour gérer la remise à zéro des champs à saisir
                    model.NombreChargement = 0;

                    return View(model);
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

        //
        // POST: /ControleurTechnique/NouveauCentreRattachement
        [HttpPost]
        public ActionResult NouveauCentreRattachement(int id, ActionAgrementControleurModel model)
        {
            try
            {
                //validateNouveauCentreRattachement(model);
                //On utilise le même validateur que pour la notification
                validateNotification(model, this.User);
                if (this.ModelState.IsValid)
                {
                    this._controleurTechniqueService.changementCentreRattachement(id, model.controleur, model.categorieVehicule, this.User.Identity.Name);

                    //return this.RedirectToAction("Fiche", new { id = id });
                    return this.RedirectToAction("ResultatChangementCentreRattachement", new { id = id, categorieVehicule = model.categorieVehicule });
                }
                else
                {
                    model.NombreChargement++;
                    return View(model);
                }

            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // GET: /ControleurTechnique/ResultatChangementCentreRattachement/313
        public ActionResult ResultatChangementCentreRattachement(int id, rCategorieVehicule.CategorieEnum categorieVehicule)
        {
            try
            {
                //1 on retrieve l'id
                pContoleurEncour c = this._controleurTechniqueService.FindBy(id);

                ActionAgrementControleurModel model = new ActionAgrementControleurModel();
                model.controleur = c;
                model.categorieVehicule = categorieVehicule;

                return View(model);
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

        // GET: /ControleurTechnique/AjouterSanction/62
        public ActionResult AjouterSanction(int id)
        {
            try
            {
                //1 on retrieve l'id
                pContoleurEncour c = this._controleurTechniqueService.FindBy(id);
                if (GestionDroitsExecution.isRightActionForControleur(this.User, c, GestionDroitsExecution.ActionsFicheControleur.SANCT))
                {
                    AjouterSanctionControleurModel s = new AjouterSanctionControleurModel();
                    s.Controleur = c;

                    //on rempli la liste des sanctions
                    s.ListeSanction = this._rTypeSanctionControleurService.FindAll();
                    s.ListeSanction.Insert(0, null);

                    //on rempli la liste des organismes demandeurs
                    s.ListeOrganismeDemandeur = this._rTypeOrganismeDemandeurService.FindAll();
                    s.ListeOrganismeDemandeur.Insert(0, null);

                    return View(s);
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

        // POST: /ControleurTechnique/AjouterSanction
        [HttpPost]
        public ActionResult AjouterSanction(AjouterSanctionControleurModel m)
        {
            try
            {
                m.Controleur = this._controleurTechniqueService.FindBy(m.Controleur.Id);
                //if (GestionDroitsExecution.isRightActionForCentre(this.User, m.Controleur, GestionDroitsExecution.ActionsFicheCentre.SANCT))
                //{
                validateAjouterSanction(m);
                if (ModelState.IsValid)
                {
                    //On utilise le certificatService nouvelleSanction (annulation agrément et enregistrement sanction)
                    pSanctionControleur s = this._pSanctionControleurService.nouvelleSanction(m.Controleur, m.Sanction, this.User.Identity.Name);

                    //On affiche la fiche du centre
                    return this.RedirectToAction("Fiche", new { id = m.Controleur.Id });

                }

                //on rempli la liste des sanctions
                m.ListeSanction = this._rTypeSanctionControleurService.FindAll();
                m.ListeSanction.Insert(0, null);

                //on rempli la liste des organismes demandeurs
                m.ListeOrganismeDemandeur = this._rTypeOrganismeDemandeurService.FindAll();
                m.ListeOrganismeDemandeur.Insert(0, null);

                return View("AjouterSanction", m);
                //}
                //else
                //{
                //     Exception e = new Exception("Vous n'êtes pas autorisé....");
                //     return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                // }
            }

            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // GET: /ControleurTechnique/ConfirmationDeleteSanction/60
        public ActionResult ConfirmationDeleteSanction(int id)
        {
            try
            {
                pSanctionControleur s = this._pSanctionControleurService.FindBy(id);
                //if (GestionDroitsExecution.isRightActionForCentre(this.User, s.PCentreEncour, GestionDroitsExecution.ActionsFicheCentre.SANCT))
                //{
                return View(s);
                //}
                //else
                //{
                //    Exception e = new Exception("Vous n'êtes pas autorisé....");
                //    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                //}
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

        // GET: /ControleurTechnique/ModifierSanction/62
        public ActionResult ModifierSanction(int id)
        {
            try
            {
                //1 on retrieve l'id
                pSanctionControleur sanction = this._pSanctionControleurService.FindBy(id);
                //if (GestionDroitsExecution.isRightActionForCentre(this.User, sanction.PContoleurEncour, GestionDroitsExecution.ActionsFicheCentre.SANCT))
                //{
                AjouterSanctionControleurModel m = new AjouterSanctionControleurModel();
                m.Controleur = sanction.PContoleurEncour;
                m.Sanction = sanction;

                //on rempli la liste des sanctions
                m.ListeSanction = this._rTypeSanctionControleurService.FindAll();
                m.ListeSanction.Insert(0, null);

                //on rempli la liste des organismes demandeurs
                m.ListeOrganismeDemandeur = this._rTypeOrganismeDemandeurService.FindAll();
                //s.ListeOrganismeDemandeur.Insert(0, new rTypeOrganismeDemandeur() { Id = -1 , Libelle = "--------" });
                m.ListeOrganismeDemandeur.Insert(0, null);

                return View(m);
                //}
                //else
                //{
                //    Exception e = new Exception("Vous n'êtes pas autorisé....");
                //    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                //}
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // POST: /ControleurTechnique/ModifierSanction
        [HttpPost]
        public ActionResult ModifierSanction(AjouterSanctionControleurModel m)
        {
            try
            {
                m.Controleur = this._controleurTechniqueService.FindBy(m.Controleur.Id);
                //if (GestionDroitsExecution.isRightActionForCentre(this.User, m.Controleur, GestionDroitsExecution.ActionsFicheCentre.SANCT))
                //{
                validateAjouterSanction(m);
                if (ModelState.IsValid)
                {
                    //On utilise le certificatService nouvelleSanction (historisation, annulation agrément et enregistrement sanction)
                    pSanctionControleur s = this._pSanctionControleurService.modifierSanction(m.Sanction, this.User.Identity.Name);

                    //On affiche la fiche du centre
                    return this.RedirectToAction("ListeSanctions", new { id = m.Sanction.PContoleurEncour.Id });

                }

                //on rempli la liste des sanctions
                m.ListeSanction = this._rTypeSanctionControleurService.FindAll();
                m.ListeSanction.Insert(0, null);

                //on rempli la liste des organismes demandeurs
                m.ListeOrganismeDemandeur = this._rTypeOrganismeDemandeurService.FindAll();
                m.ListeOrganismeDemandeur.Insert(0, null);

                return View(m);
                //}
                //else
                //{
                //    Exception e = new Exception("Vous n'êtes pas autorisé....");
                //    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                //}
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // POST: /ControleurTechnique/DeleteSanction/60
        public ActionResult DeleteSanction(int id)
        {
            try
            {
                pSanctionControleur s = this._pSanctionControleurService.FindBy(id);
                //if (GestionDroitsExecution.isRightActionForCentre(this.User, s.PContoleurEncour, GestionDroitsExecution.ActionsFicheCentre.SANCT))
                //{
                // on supprime la sanction
                this._pSanctionControleurService.deleteSanction(s);

                //on affiche la liste des sanctions
                return this.RedirectToAction("ListeSanctions", new { id = s.PContoleurEncour.Id });
                //}
                //else
                //{
                //    Exception e = new Exception("Vous n'êtes pas autorisé....");
                //    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                //}
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

        //
        // GET: /ControleurTechnique/DeclarerHabilitationPL/313
        public ActionResult DeclarerHabilitationPL(int id)
        {
            try
            {
                //1 on retrieve l'id
                pContoleurEncour c = this._controleurTechniqueService.FindBy(id);
                if (GestionDroitsExecution.isRightActionForControleur(this.User, c, GestionDroitsExecution.ActionsFicheControleur.HABIL))
                {
                    IList<rHabilitation> listeHabilitation = this._rHabilitationService.FindAll();
                    ControleurHabilitationPLModel model = new ControleurHabilitationPLModel() { Controleur = c, ListeHabilitation = listeHabilitation, NombreChargement = 0 };

                    return View(model);
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

        //
        // POST: /ControleurTechnique/DeclarerHabilitationPL/313
        [HttpPost]
        public ActionResult DeclarerHabilitationPL(int id, ControleurHabilitationPLModel model)
        {
            try
            {
                validateDeclarationHabilitationPL(model);
                if (ModelState.IsValid)
                {
                    this._controleurTechniqueService.declarerHabilitationPL(id, model.Controleur, this.User.Identity.Name);

                    return this.RedirectToAction("Fiche", new { id = id });
                }
                IList<rHabilitation> listeHabilitation = this._rHabilitationService.FindAll();
                model.ListeHabilitation = listeHabilitation;
                model.NombreChargement++;
                return View(model);
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        //
        // GET: /ControleurTechnique/ModifierAuditPL/313
        public ActionResult ModifierAuditPL(int id)
        {
            try
            {
                IList<rCategorieVehicule> listeCategorieVehicule = this._rCategorieVehiculeService.FindAll();

                AjoutAuditControleurPLModel model = new AjoutAuditControleurPLModel();
                model.ListeCategorieVehicule = listeCategorieVehicule;
                model.Audit = this._pAuditControleurService.FindBy(id);
                model.Controleur = model.Audit.Controleur;

                return View(model);
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        //
        // POST: /ControleurTechnique/ModifierAuditPL/313
        [HttpPost]
        public ActionResult ModifierAuditPL(int id, AjoutAuditControleurPLModel model)
        {
            try
            {
                validateAjouterAuditPL(model);
                if (ModelState.IsValid)
                {
                    model.Audit = this._pAuditControleurService.modifierAudit(id, model.Audit, this.User.Identity.Name);

                    return this.RedirectToAction("ListeAuditsPL", new { id = model.Audit.Controleur.Id });
                }
                model.Controleur = this._controleurTechniqueService.FindBy(id);
                IList<rCategorieVehicule> listeCategorieVehicule = this._rCategorieVehiculeService.FindAll();
                model.ListeCategorieVehicule = listeCategorieVehicule;
                return View(model);
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // GET: /ControleurTechnique/ConfirmationDeleteAuditPL/60
        public ActionResult ConfirmationDeleteAuditPL(int id)
        {
            try
            {
                pAuditControleur a = this._pAuditControleurService.FindBy(id);
                //if (GestionDroitsExecution.isRightActionForCentre(this.User, s.PCentreEncour, GestionDroitsExecution.ActionsFicheCentre.SANCT))
                //{
                return View(a);
                //}
                //else
                //{
                //    Exception e = new Exception("Vous n'êtes pas autorisé....");
                //    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                //}
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

        // POST: /ControleurTechnique/DeleteAuditPL/60
        public ActionResult DeleteAuditPL(int id)
        {
            try
            {
                pAuditControleur a = this._pAuditControleurService.FindBy(id);
                //if (GestionDroitsExecution.isRightActionForCentre(this.User, s.PContoleurEncour, GestionDroitsExecution.ActionsFicheCentre.SANCT))
                //{
                // on supprime la sanction
                this._pAuditControleurService.deleteAudit(a);

                //on affiche la liste des sanctions
                return this.RedirectToAction("ListeSanctions", new { id = a.Controleur.Id });
                //}
                //else
                //{
                //    Exception e = new Exception("Vous n'êtes pas autorisé....");
                //    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                //}
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

        //
        // GET: /ControleurTechnique/AjouterAuditPL/313
        public ActionResult AjouterAuditPL(int id)
        {
            try
            {
                pContoleurEncour c = this._controleurTechniqueService.FindBy(id);
                if (GestionDroitsExecution.isRightActionForControleur(this.User, c, GestionDroitsExecution.ActionsFicheControleur.AUDITPL))
                {
                    IList<rCategorieVehicule> listeCategorieVehicule = this._rCategorieVehiculeService.FindAll();

                    AjoutAuditControleurPLModel model = new AjoutAuditControleurPLModel();
                    model.Controleur = c;
                    model.ListeCategorieVehicule = listeCategorieVehicule;
                    model.Audit = new pAuditControleur();

                    return View(model);
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

        //
        // POST: /ControleurTechnique/AjouterAuditPL/313
        [HttpPost]
        public ActionResult AjouterAuditPL(int id, AjoutAuditControleurPLModel model)
        {
            try
            {
                validateAjouterAuditPL(model);
                if (ModelState.IsValid)
                {
                    model.Controleur = this._controleurTechniqueService.FindBy(id);
                    this._pAuditControleurService.nouvelAudit(model.Controleur, model.Audit, this.User.Identity.Name);

                    return this.RedirectToAction("Fiche", new { id = id });
                }
                model.Controleur = this._controleurTechniqueService.FindBy(id);
                IList<rCategorieVehicule> listeCategorieVehicule = this._rCategorieVehiculeService.FindAll();
                model.ListeCategorieVehicule = listeCategorieVehicule;
                return View(model);
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        private void validateAjouterAuditPL(AjoutAuditControleurPLModel model)
        {
            if (model.Audit.RCategorieVehicule == null)
            {
                ModelState.AddModelError("Audit.RCategorieVehicule.Id", "La catégorie de véhicule est obligatoire");
            }

            if (model.Audit.DateAudit == null)
            {
                ModelState.AddModelError("Audit.DateAudit", "La date de l'audit est obligatoire");
            }

            if (model.Audit.CodAgremCntr == null)
            {
                ModelState.AddModelError("Audit.CodAgremCntr", "L'agrément du centre est obligatoire");
            }

            if (model.Audit.ImmatVehicule == null)
            {
                ModelState.AddModelError("Audit.ImmatVehicule", "L'immatriculation est obligatoire");
            }

            if (model.Audit.NumPv == null)
            {
                ModelState.AddModelError("Audit.NumPv", "Le numéro de PV est obligatoire");
            }

        }

        #region Gestion des audits VL
        /* Gestion des audits VL */
        //
        // GET: /ControleurTechnique/ModifierAuditVL/313
        public ActionResult ModifierAuditVL(int id)
        {
            try
            {
                pAuditControleurVL audit = this._pAuditControleurVLService.FindBy(id);

                if (GestionDroitsExecution.canModify(this.User, new BeanModifiable<pAuditControleurVL>(audit)))
                {
                    AjoutAuditControleurVLModel model = new AjoutAuditControleurVLModel();
                    model.Audit = audit;

                    model.Controleur = model.Audit.Controleur;

                    return View(model);
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

        //
        // POST: /ControleurTechnique/ModifierAuditVL/313
        [HttpPost]
        public ActionResult ModifierAuditVL(int id, AjoutAuditControleurVLModel model)
        {
            try
            {
                validateAjouterAuditVL(model);
                if (ModelState.IsValid)
                {
                    model.Audit = this._pAuditControleurVLService.modifierAudit(id, model.Audit, this.User.Identity.Name);

                    return this.RedirectToAction("ListeAuditsVL", new { id = model.Audit.Controleur.Id });
                }
                model.Controleur = this._controleurTechniqueService.FindBy(id);
                return View(model);
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // GET: /ControleurTechnique/ConfirmationDeleteAuditVL/60
        public ActionResult ConfirmationDeleteAuditVL(int id)
        {
            try
            {
                pAuditControleurVL a = this._pAuditControleurVLService.FindBy(id);
                //if (GestionDroitsExecution.isRightActionForCentre(this.User, s.PCentreEncour, GestionDroitsExecution.ActionsFicheCentre.SANCT))
                //{
                return View(a);
                //}
                //else
                //{
                //    Exception e = new Exception("Vous n'êtes pas autorisé....");
                //    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                //}
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

        // POST: /ControleurTechnique/DeleteAudit/60
        public ActionResult DeleteAuditVL(int id)
        {
            try
            {
                pAuditControleurVL a = this._pAuditControleurVLService.FindBy(id);
                //if (GestionDroitsExecution.isRightActionForCentre(this.User, s.PContoleurEncour, GestionDroitsExecution.ActionsFicheCentre.SANCT))
                //{
                // on supprime la sanction
                this._pAuditControleurVLService.deleteAudit(a);

                //on affiche la liste des sanctions
                return this.RedirectToAction("ListeAuditsVL", new { id = a.Controleur.Id });
                //}
                //else
                //{
                //    Exception e = new Exception("Vous n'êtes pas autorisé....");
                //    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                //}
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

        //
        // GET: /ControleurTechnique/AjouterAuditVL/313
        public ActionResult AjouterAuditVL(int id)
        {
            try
            {
                pContoleurEncour c = this._controleurTechniqueService.FindBy(id);
                if (GestionDroitsExecution.isRightActionForControleur(this.User, c, GestionDroitsExecution.ActionsFicheControleur.AUDITVL))
                {

                    AjoutAuditControleurVLModel model = new AjoutAuditControleurVLModel();
                    model.Controleur = c;
                    model.Audit = new pAuditControleurVL();

                    return View(model);
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

        //
        // POST: /ControleurTechnique/AjouterAuditVL/313
        [HttpPost]
        public ActionResult AjouterAuditVL(int id, AjoutAuditControleurVLModel model)
        {
            try
            {
                validateAjouterAuditVL(model);
                if (ModelState.IsValid)
                {
                    model.Controleur = this._controleurTechniqueService.FindBy(id);
                    this._pAuditControleurVLService.nouvelAudit(model.Controleur, model.Audit, this.User.Identity.Name);

                    return this.RedirectToAction("Fiche", new { id = id });
                }
                model.Controleur = this._controleurTechniqueService.FindBy(id);
                return View(model);
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // GET: /ControleurTechnique/ListeAudits/62
        public ActionResult ListeAuditsVL(int id)
        {
            try
            {
                pContoleurEncour c = this._controleurTechniqueService.FindBy(id);

                return View(c);

            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }
        private void validateAjouterAuditVL(AjoutAuditControleurVLModel model)
        {
            /*if (model.Audit.RCategorieVehicule == null)
            {
                ModelState.AddModelError("Audit.RCategorieVehicule.Id", "La catégorie de véhicule est obligatoire");
            }*/

            if (model.Audit.DateAudit == null)
            {
                ModelState.AddModelError("Audit.DateAudit", "La date de l'audit est obligatoire");
            }
            /*
            if (model.Audit.CodAgremCntr == null)
            {
                ModelState.AddModelError("Audit.CodAgremCntr", "L'agrément du centre est obligatoire");
            }

            if (model.Audit.ImmatVehicule == null)
            {
                ModelState.AddModelError("Audit.ImmatVehicule", "L'immatriculation est obligatoire");
            }

            if (model.Audit.NumPv == null)
            {
                ModelState.AddModelError("Audit.NumPv", "Le numéro de PV est obligatoire");
            }*/

        }

        #endregion

        #region Gestion des audits CL
        /* Gestion des audits CL */
        //
        // GET: /ControleurTechnique/ModifierAuditCL/313
        public ActionResult ModifierAuditCL(int id)
        {
            try
            {
                AjoutAuditControleurCLModel model = new AjoutAuditControleurCLModel();
                model.Audit = this._pAuditControleurCLService.FindBy(id);
                model.Controleur = model.Audit.Controleur;

                return View(model);
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        //
        // POST: /ControleurTechnique/ModifierAuditCL/313
        [HttpPost]
        public ActionResult ModifierAuditCL(int id, AjoutAuditControleurCLModel model)
        {
            try
            {
                validateAjouterAuditCL(model);
                if (ModelState.IsValid)
                {
                    model.Audit = this._pAuditControleurCLService.modifierAudit(id, model.Audit, this.User.Identity.Name);

                    return this.RedirectToAction("ListeAuditsCL", new { id = model.Audit.Controleur.Id });
                }
                model.Controleur = this._controleurTechniqueService.FindBy(id);
                return View(model);
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // GET: /ControleurTechnique/ConfirmationDeleteAuditCL/60
        public ActionResult ConfirmationDeleteAuditCL(int id)
        {
            try
            {
                pAuditControleurCL a = this._pAuditControleurCLService.FindBy(id);
                //if (GestionDroitsExecution.isRightActionForCentre(this.User, s.PCentreEncour, GestionDroitsExecution.ActionsFicheCentre.SANCT))
                //{
                return View(a);
                //}
                //else
                //{
                //    Exception e = new Exception("Vous n'êtes pas autorisé....");
                //    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                //}
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

        // POST: /ControleurTechnique/DeleteAudit/60
        public ActionResult DeleteAuditCL(int id)
        {
            try
            {
                pAuditControleurCL a = this._pAuditControleurCLService.FindBy(id);
                //if (GestionDroitsExecution.isRightActionForCentre(this.User, s.PContoleurEncour, GestionDroitsExecution.ActionsFicheCentre.SANCT))
                //{
                // on supprime la sanction
                this._pAuditControleurCLService.deleteAudit(a);

                //on affiche la liste des sanctions
                return this.RedirectToAction("ListeAuditsCL", new { id = a.Controleur.Id });
                //}
                //else
                //{
                //    Exception e = new Exception("Vous n'êtes pas autorisé....");
                //    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                //}
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

        //
        // GET: /ControleurTechnique/AjouterAuditCL/313
        public ActionResult AjouterAuditCL(int id)
        {
            try
            {
                pContoleurEncour c = this._controleurTechniqueService.FindBy(id);
                if (GestionDroitsExecution.isRightActionForControleur(this.User, c, GestionDroitsExecution.ActionsFicheControleur.AUDITCL))
                {

                    AjoutAuditControleurCLModel model = new AjoutAuditControleurCLModel();
                    model.Controleur = c;
                    model.Audit = new pAuditControleurCL();

                    return View(model);
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

        //
        // POST: /ControleurTechnique/AjouterAuditCL/313
        [HttpPost]
        public ActionResult AjouterAuditCL(int id, AjoutAuditControleurCLModel model)
        {
            try
            {
                validateAjouterAuditCL(model);
                if (ModelState.IsValid)
                {
                    model.Controleur = this._controleurTechniqueService.FindBy(id);
                    this._pAuditControleurCLService.nouvelAudit(model.Controleur, model.Audit, this.User.Identity.Name);

                    return this.RedirectToAction("Fiche", new { id = id });
                }
                model.Controleur = this._controleurTechniqueService.FindBy(id);
                return View(model);
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // GET: /ControleurTechnique/ListeAudits/62
        public ActionResult ListeAuditsCL(int id)
        {
            try
            {
                pContoleurEncour c = this._controleurTechniqueService.FindBy(id);

                return View(c);

            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }
        private void validateAjouterAuditCL(AjoutAuditControleurCLModel model)
        {
            /*if (model.Audit.RCategorieVehicule == null)
            {
                ModelState.AddModelError("Audit.RCategorieVehicule.Id", "La catégorie de véhicule est obligatoire");
            }*/

            if (model.Audit.DateAudit == null)
            {
                ModelState.AddModelError("Audit.DateAudit", "La date de l'audit est obligatoire");
            }
            /*
            if (model.Audit.CodAgremCntr == null)
            {
                ModelState.AddModelError("Audit.CodAgremCntr", "L'agrément du centre est obligatoire");
            }

            if (model.Audit.ImmatVehicule == null)
            {
                ModelState.AddModelError("Audit.ImmatVehicule", "L'immatriculation est obligatoire");
            }

            if (model.Audit.NumPv == null)
            {
                ModelState.AddModelError("Audit.NumPv", "Le numéro de PV est obligatoire");
            }*/

        }

        #endregion

        #region Gestion des formations PL
        /* Gestion des formations PL */
        //
        // GET: /ControleurTechnique/ModifierFormationPL/313
        public ActionResult ModifierFormationPL(int id)
        {
            try
            {
                AjoutFormationControleurPLModel model = new AjoutFormationControleurPLModel();
                model.Formation = this._pFormationControleurPLService.FindBy(id);
                model.Controleur = model.Formation.Controleur;
                //PLE  le 26/01/2024 Extension Cat. L / filtrage formations PL
                //model.ListeFormation = this._rTypeFormationService.FindAll();
                model.ListeFormation = this._rTypeFormationService.FindSelonCodCategorieVeh(rCategorieVehicule.CategorieEnum.PL);

                return View(model);
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        //
        // POST: /ControleurTechnique/ModifierFormationPL/313
        [HttpPost]
        public ActionResult ModifierFormationPL(int id, AjoutFormationControleurPLModel model)
        {
            try
            {
                if (((short?)rTypeFormation.TypeFormationEnum.FORMATION_INIT_CL_PL).Equals(model.Formation.RTypeFormation.Id))
                {
                    //forçage de la date de début das le cas une formation initial CL
                    model.Formation.DateDebut = model.Formation.DateFin;
                }
                validateAjouterFormationPL(model);
                if (ModelState.IsValid)
                {
                    model.Formation = this._pFormationControleurPLService.modifierFormation(id, model.Formation, this.User.Identity.Name);

                    return this.RedirectToAction("ListeformationsPL", new { id = model.Formation.Controleur.Id });
                }
                model.Controleur = this._controleurTechniqueService.FindBy(model.Controleur.Id);
                model.ListeFormation = this._rTypeFormationService.FindAll();
                return View(model);
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // GET: /ControleurTechnique/ConfirmationDeleteFormationPL/60
        public ActionResult ConfirmationDeleteFormationPL(int id)
        {
            try
            {
                pFormationControleurPL a = this._pFormationControleurPLService.FindBy(id);
                //if (GestionDroitsExecution.isRightActionForCentre(this.User, s.PCentreEncour, GestionDroitsExecution.ActionsFicheCentre.SANCT))
                //{
                return View(a);
                //}
                //else
                //{
                //    Exception e = new Exception("Vous n'êtes pas autorisé....");
                //    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                //}
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

        // POST: /ControleurTechnique/DeleteFormation/60
        public ActionResult DeleteFormationPL(int id)
        {
            try
            {
                pFormationControleurPL a = this._pFormationControleurPLService.FindBy(id);
                //if (GestionDroitsExecution.isRightActionForCentre(this.User, s.PContoleurEncour, GestionDroitsExecution.ActionsFicheCentre.SANCT))
                //{
                // on supprime la sanction
                this._pFormationControleurPLService.deleteFormation(a);

                //on affiche la liste des sanctions
                return this.RedirectToAction("ListeformationsPL", new { id = a.Controleur.Id });
                //}
                //else
                //{
                //    Exception e = new Exception("Vous n'êtes pas autorisé....");
                //    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                //}
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

        //
        // GET: /ControleurTechnique/AjouterFormationPL/313
        public ActionResult AjouterFormationPL(int id,String ReturnLettreReseau, String ReturnCodDep)
        {
            try
            {
                pContoleurEncour c = this._controleurTechniqueService.FindBy(id);

                if (GestionDroitsExecution.isRightActionForControleur(this.User, c, GestionDroitsExecution.ActionsFicheControleur.FORMAPL))
                {
                    AjoutFormationControleurPLModel model = new AjoutFormationControleurPLModel();
                    model.Controleur = c;
                    model.Formation = new pFormationControleurPL();
                    //PLE  le 26/01/2024 Extension Cat. L / filtrage formations PL
                    //model.ListeFormation = this._rTypeFormationService.FindAll();
                    model.ListeFormation = this._rTypeFormationService.FindSelonCodCategorieVeh(rCategorieVehicule.CategorieEnum.PL);

                    model.ReturnLettreReseau = ReturnLettreReseau;
                    model.ReturnCodDep = ReturnCodDep;
                    return View(model);
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

        //
        // POST: /ControleurTechnique/AjouterFormationPL/313
        [HttpPost]
        public ActionResult AjouterFormationPL(int id, AjoutFormationControleurPLModel model)
        {
            try
            {
                if (((short?)rTypeFormation.TypeFormationEnum.FORMATION_INIT_CL_PL).Equals(model.Formation.RTypeFormation.Id))
                {
                    //formçage de la date de début das le cas une formation initial CL
                    model.Formation.DateDebut = model.Formation.DateFin;
                }

                validateAjouterFormationPL(model);
                if (ModelState.IsValid)
                {
                    model.Controleur = this._controleurTechniqueService.FindBy(id);
                    this._pFormationControleurPLService.nouvelFormation(model.Controleur, model.Formation, this.User.Identity.Name);

                    if (!String.IsNullOrEmpty(model.ReturnLettreReseau))
                    {
                        return this.RedirectToAction("DeclarerExtensionAgrementCatL", new { Id = id, Categorie = rCategorieVehicule.CategorieEnum.PL, returnLettreReseau = model.ReturnLettreReseau, returnCodDep = model.ReturnCodDep });
                    }

                    return this.RedirectToAction("Fiche", new { id = id });
                }
                model.Controleur = this._controleurTechniqueService.FindBy(id);
                //model.ListeFormation = this._rTypeFormationService.FindAll();
                model.ListeFormation = this._rTypeFormationService.FindSelonCodCategorieVeh(rCategorieVehicule.CategorieEnum.PL);
                return View(model);
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // GET: /ControleurTechnique/Listeformations/62
        public ActionResult ListeformationsPL(int id)
        {
            try
            {
                pContoleurEncour c = this._controleurTechniqueService.FindBy(id);

                return View(c);

            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }
        private void validateAjouterFormationPL(AjoutFormationControleurPLModel model)
        {
            if (model.Formation.RTypeFormation == null)
            {
                ModelState.AddModelError("Formation.RTypeFormation.Id", "Le type de formation est obligatoire");
            }

            if (model.Formation.DateDebut == null)
            {
                ModelState.AddModelError("Formation.DateDebut", "La date de début de la formation est obligatoire");
            }

            if (model.Formation.DateFin == null)
            {
                ModelState.AddModelError("Formation.DateFin", "La date de fin de la formation est obligatoire");
            }
            else
            if (((short?)rTypeFormation.TypeFormationEnum.FORMATION_INIT_CL_PL).Equals(model.Formation.RTypeFormation.Id))
            {
                if (model.Formation.DateFin < new DateTime(2023, 06, 01) || model.Formation.DateFin > DateTime.Today)
                {
                    ModelState.AddModelError("Formation.DateFin", "La date de fin de la formation intiale CL doit être postérieure au 01/06/2023 et antérieure à la date du jour");
                }
            }

        }

        #endregion

        #region Gestion des formations VL
        /* Gestion des formations VL */
        //
        // GET: /ControleurTechnique/ModifierFormationVL/313
        public ActionResult ModifierFormationVL(int id)
        {
            try
            {
                AjoutFormationControleurVLModel model = new AjoutFormationControleurVLModel();
                model.Formation = this._pFormationControleurVLService.FindBy(id);
                model.Controleur = model.Formation.Controleur;
                //PLE  le 26/01/2024 Extension Cat. L / filtrage formations PL
                //model.ListeFormation = this._rTypeFormationService.FindAll();
                model.ListeFormation = this._rTypeFormationService.FindSelonCodCategorieVeh(rCategorieVehicule.CategorieEnum.VL);

                return View(model);
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        //
        // POST: /ControleurTechnique/ModifierFormationVL/313
        [HttpPost]
        public ActionResult ModifierFormationVL(int id, AjoutFormationControleurVLModel model)
        {
            try
            {
                if (((short?)rTypeFormation.TypeFormationEnum.FORMATION_INIT_CL_VL).Equals(model.Formation.RTypeFormation.Id))
                {
                    //forçage de la date de début das le cas une formation initial CL
                    model.Formation.DateDebut = model.Formation.DateFin;
                }
                validateAjouterFormationVL(model);
                if (ModelState.IsValid)
                {
                    model.Formation = this._pFormationControleurVLService.modifierFormation(id, model.Formation, this.User.Identity.Name);

                    return this.RedirectToAction("ListeformationsVL", new { id = model.Formation.Controleur.Id });
                }
                model.Controleur = this._controleurTechniqueService.FindBy(model.Controleur.Id);
                //PLE  le 26/01/2024 Extension Cat. L / filtrage formations VL
                model.ListeFormation = this._rTypeFormationService.FindSelonCodCategorieVeh(rCategorieVehicule.CategorieEnum.VL);
                return View(model);
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // GET: /ControleurTechnique/ConfirmationDeleteFormationVL/60
        public ActionResult ConfirmationDeleteFormationVL(int id)
        {
            try
            {
                pFormationControleurVL a = this._pFormationControleurVLService.FindBy(id);
                //if (GestionDroitsExecution.isRightActionForCentre(this.User, s.PCentreEncour, GestionDroitsExecution.ActionsFicheCentre.SANCT))
                //{
                return View(a);
                //}
                //else
                //{
                //    Exception e = new Exception("Vous n'êtes pas autorisé....");
                //    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                //}
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

        // POST: /ControleurTechnique/DeleteFormation/60
        public ActionResult DeleteFormationVL(int id)
        {
            try
            {
                pFormationControleurVL a = this._pFormationControleurVLService.FindBy(id);
                //if (GestionDroitsExecution.isRightActionForCentre(this.User, s.PContoleurEncour, GestionDroitsExecution.ActionsFicheCentre.SANCT))
                //{
                // on supprime la sanction
                this._pFormationControleurVLService.deleteFormation(a);

                //on affiche la liste des sanctions
                return this.RedirectToAction("ListeformationsVL", new { id = a.Controleur.Id });
                //}
                //else
                //{
                //    Exception e = new Exception("Vous n'êtes pas autorisé....");
                //    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                //}
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

        //
        // GET: /ControleurTechnique/AjouterFormationVL/313
        public ActionResult AjouterFormationVL(int id, String ReturnLettreReseau, String ReturnCodDep)
        {
            try
            {
                pContoleurEncour c = this._controleurTechniqueService.FindBy(id);
                if (GestionDroitsExecution.isRightActionForControleur(this.User, c, GestionDroitsExecution.ActionsFicheControleur.FORMAVL))
                {
                    AjoutFormationControleurVLModel model = new AjoutFormationControleurVLModel();
                    model.Controleur = c;
                    model.Formation = new pFormationControleurVL();
                    //PLE  le 26/01/2024 Extension Cat. L / filtrage formations VL
                    model.ListeFormation = this._rTypeFormationService.FindSelonCodCategorieVeh(rCategorieVehicule.CategorieEnum.VL);

                    model.ReturnLettreReseau = ReturnLettreReseau;
                    model.ReturnCodDep = ReturnCodDep;
                    return View(model);
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

        //
        // POST: /ControleurTechnique/AjouterFormationVL/313
        [HttpPost]
        public ActionResult AjouterFormationVL(int id, AjoutFormationControleurVLModel model)
        {
            try
            {
                if (((short?)rTypeFormation.TypeFormationEnum.FORMATION_INIT_CL_VL).Equals(model.Formation.RTypeFormation.Id))
                {
                    //formçage de la date de début das le cas une formation initial CL
                    model.Formation.DateDebut = model.Formation.DateFin;
                }
                validateAjouterFormationVL(model);
                if (ModelState.IsValid)
                {
                    model.Controleur = this._controleurTechniqueService.FindBy(id);
                    this._pFormationControleurVLService.nouvelFormation(model.Controleur, model.Formation, this.User.Identity.Name);

                    if (!String.IsNullOrEmpty(model.ReturnLettreReseau))
                    {
                        return this.RedirectToAction("DeclarerExtensionAgrementCatL", new { Id = id, Categorie = rCategorieVehicule.CategorieEnum.VL, returnLettreReseau = model.ReturnLettreReseau, returnCodDep = model.ReturnCodDep });
                    }

                    return this.RedirectToAction("Fiche", new { id = id });
                }
                model.Controleur = this._controleurTechniqueService.FindBy(id);
                //PLE  le 26/01/2024 Extension Cat. L / filtrage formations VL
                model.ListeFormation = this._rTypeFormationService.FindSelonCodCategorieVeh(rCategorieVehicule.CategorieEnum.VL);
                return View(model);
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // GET: /ControleurTechnique/Listeformations/62
        public ActionResult ListeformationsVL(int id)
        {
            try
            {
                pContoleurEncour c = this._controleurTechniqueService.FindBy(id);

                return View(c);

            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        private void validateAjouterFormationVL(AjoutFormationControleurVLModel model)
        {

            if (model.Formation.RTypeFormation == null)
            {
                ModelState.AddModelError("Formation.RTypeFormation.Id", "Le type de formation est obligatoire");
            }

            if (model.Formation.DateDebut == null)
            {
                ModelState.AddModelError("Formation.DateDebut", "La date de début de la formation est obligatoire");
            }

            if (model.Formation.DateFin == null)
            {
                ModelState.AddModelError("Formation.DateFin", "La date de fin de la formation est obligatoire");
            }
            else
            if (((short?)rTypeFormation.TypeFormationEnum.FORMATION_INIT_CL_VL).Equals(model.Formation.RTypeFormation.Id))
            {
                if (model.Formation.DateFin < new DateTime(2023, 06, 01) || model.Formation.DateFin > DateTime.Today)
                {
                    ModelState.AddModelError("Formation.DateFin", "La date de fin de la formation intiale CL doit être postérieure au 01/06/2023 et antérieure à la date du jour");
                }
            }

        }

        #endregion

        #region Gestion des formations CL
        /* Gestion des formations CL */
        //
        // GET: /ControleurTechnique/ModifierFormationCL/313
        public ActionResult ModifierFormationCL(int id)
        {
            try
            {
                AjoutFormationControleurCLModel model = new AjoutFormationControleurCLModel();
                model.Formation = this._pFormationControleurCLService.FindBy(id);
                model.Controleur = model.Formation.Controleur;
                model.ListeFormation = this._rTypeFormationService.FindSelonCodCategorieVeh(rCategorieVehicule.CategorieEnum.CL);

                return View(model);
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        //
        // POST: /ControleurTechnique/ModifierFormationCL/313
        [HttpPost]
        public ActionResult ModifierFormationCL(int id, AjoutFormationControleurCLModel model)
        {
            try
            {
                validateAjouterFormationCL(model);
                if (ModelState.IsValid)
                {
                    model.Formation = this._pFormationControleurCLService.modifierFormation(id, model.Formation, this.User.Identity.Name);

                    return this.RedirectToAction("ListeformationsCL", new { id = model.Formation.Controleur.Id });
                }
                model.Controleur = this._controleurTechniqueService.FindBy(id);
                return View(model);
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // GET: /ControleurTechnique/ConfirmationDeleteFormationCL/60
        public ActionResult ConfirmationDeleteFormationCL(int id)
        {
            try
            {
                pFormationControleurCL a = this._pFormationControleurCLService.FindBy(id);
                //if (GestionDroitsExecution.isRightActionForCentre(this.User, s.PCentreEncour, GestionDroitsExecution.ActionsFicheCentre.SANCT))
                //{
                return View(a);
                //}
                //else
                //{
                //    Exception e = new Exception("Vous n'êtes pas autorisé....");
                //    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                //}
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

        // POST: /ControleurTechnique/DeleteFormation/60
        public ActionResult DeleteFormationCL(int id)
        {
            try
            {
                pFormationControleurCL a = this._pFormationControleurCLService.FindBy(id);
                //if (GestionDroitsExecution.isRightActionForCentre(this.User, s.PContoleurEncour, GestionDroitsExecution.ActionsFicheCentre.SANCT))
                //{
                // on supprime la sanction
                this._pFormationControleurCLService.deleteFormation(a);

                //on affiche la liste des sanctions
                return this.RedirectToAction("ListeformationsCL", new { id = a.Controleur.Id });
                //}
                //else
                //{
                //    Exception e = new Exception("Vous n'êtes pas autorisé....");
                //    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                //}
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

        //
        // GET: /ControleurTechnique/AjouterFormationCL/313
        public ActionResult AjouterFormationCL(int id)
        {
            try
            {
                pContoleurEncour c = this._controleurTechniqueService.FindBy(id);
                if (GestionDroitsExecution.isRightActionForControleur(this.User, c, GestionDroitsExecution.ActionsFicheControleur.FORMACL))
                {
                    AjoutFormationControleurCLModel model = new AjoutFormationControleurCLModel();
                    model.Controleur = c;
                    model.Formation = new pFormationControleurCL();

                    //PLE  le 26/01/2024 Extension Cat. L / filtrage formations CL
                    model.ListeFormation = this._rTypeFormationService.FindSelonCodCategorieVeh(rCategorieVehicule.CategorieEnum.CL);

                    return View(model);
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

        //
        // POST: /ControleurTechnique/AjouterFormationCL/313
        [HttpPost]
        public ActionResult AjouterFormationCL(int id, AjoutFormationControleurCLModel model)
        {
            try
            {
                validateAjouterFormationCL(model);
                if (ModelState.IsValid)
                {
                    model.Controleur = this._controleurTechniqueService.FindBy(id);
                    this._pFormationControleurCLService.nouvelFormation(model.Controleur, model.Formation, this.User.Identity.Name);

                    return this.RedirectToAction("Fiche", new { id = id });
                }
                model.Controleur = this._controleurTechniqueService.FindBy(id);
                return View(model);
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // GET: /ControleurTechnique/Listeformations/62
        public ActionResult ListeformationsCL(int id)
        {
            try
            {
                pContoleurEncour c = this._controleurTechniqueService.FindBy(id);

                return View(c);

            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        private void validateAjouterFormationCL(AjoutFormationControleurCLModel model)
        {
            if (model.Formation.RTypeFormation == null)
            {
                ModelState.AddModelError("Formation.RTypeFormation.Id", "Le type de formation est obligatoire");
            }
            if (model.Formation.DateDebut == null)
            {
                ModelState.AddModelError("Formation.DateDebut", "La date de début de la formation est obligatoire");
            }
            if (model.Formation.DateFin == null)
            {
                ModelState.AddModelError("Formation.DateFin", "La date de fin de la formation est obligatoire");
            }

        }

        #endregion
        private void validateDeclarationHabilitationPL(ControleurHabilitationPLModel model)
        {
            if (model.Controleur.RHabilitation == null)
            {
                ModelState.AddModelError("Controleur.RHabilitation.Id", "L'habilitation est obligatoire");
            }

            if (model.Controleur.DateDebutHabilitationPl == null)
            {
                ModelState.AddModelError("Controleur.DateDebutHabilitationPl", "La date d'habilitation est obligatoire");
            }

        }

        private void validateAjouterSanction(AjouterSanctionControleurModel m)
        {
            //Type de sanction obligatoire
            if (m.Sanction.RTypeSanctionControleur.Id == 0)
            {
                ModelState.Remove("Sanction.RTypeSanctionControleur.Id");
                ModelState.AddModelError("Sanction.RTypeSanctionControleur.Id", "Le type de sanction est obligatoire");
            }
            else
            {
                //si "retrait définitif" alors date début obligatoire	
                if (m.Sanction.RTypeSanctionControleur.Id == (int)rTypeSanctionControleur.MotifSanctionEnum.RETRAIT_DEFINITIF)
                {
                    //Date de début obligatoire
                    if (m.Sanction.DateDebut == null)
                    {
                        ModelState.AddModelError("Sanction.DateDebut", "La date de début de sanction est obligatoire pour un retrait définitif");
                    }
                }
                else
                {
                    if (
                        m.Sanction.RTypeSanctionControleur.Id == (int)rTypeSanctionControleur.MotifSanctionEnum.SUSPENSION ||
                        m.Sanction.RTypeSanctionControleur.Id == (int)rTypeSanctionControleur.MotifSanctionEnum.SUSP_CONSERVATOIRE
                        )
                    {
                        //si "suspension*" alors date début et date de fin obligatoire	
                        if (m.Sanction.DateDebut == null)
                        {
                            ModelState.AddModelError("Sanction.DateDebut", "La date de début de sanction est obligatoire pour une suspension");
                        }

                        //Date de fin obligatoire
                        if (m.Sanction.DateFin == null)
                        {
                            ModelState.AddModelError("Sanction.DateFin", "La date de fin de sanction est obligatoire pour une suspension");
                        }
                    }
                }
            }

            if ((m.Sanction.DateFin != null && m.Sanction.DateDebut != null) && (m.Sanction.DateFin < m.Sanction.DateDebut))
            {
                ModelState.AddModelError("Sanction.DateFin", "La date de fin de sanction doit être inférieure ou égale à la date de début.");
            }

            //Demandeur obligatoire
            if (m.Sanction.RTypeOrganismeDemandeur.Id == 0)
            {
                ModelState.Remove("Sanction.RTypeOrganismeDemandeur.Id");
                ModelState.AddModelError("Sanction.RTypeOrganismeDemandeur.Id", "Le demandeur est obligatoire");
            }

            //Date évènement
            if (m.Sanction.DateEvenementOrigine == null)
            {
                ModelState.AddModelError("Sanction.DateEvenementOrigine", "La date d'évènement est obligatoire");
            }

            //EG le 07/11/2011, on doit cocher au moins un agrément impacté
            var nbAgrementValide = 0;
            var nbAgrementCoche = 0;
            if (m.Controleur.EstAgrementValideVL) nbAgrementValide++;
            if (m.Controleur.EstAgrementValidePL) nbAgrementValide++;
            if (m.Controleur.EstAgrementValideCL) nbAgrementValide++;

            if (nbAgrementValide > 0)
            {
                if (m.Sanction.IsAgrementAffecteVL) nbAgrementCoche++;
                if (m.Sanction.IsAgrementAffectePL) nbAgrementCoche++;
                if (m.Sanction.IsAgrementAffecteCL) nbAgrementCoche++;
                if (nbAgrementCoche == 0)
                {
                    ModelState.AddModelError("Sanction.IsAgrementAffecteVL", "Cocher au moins un agrément impacté");
                    ModelState.AddModelError("Sanction.IsAgrementAffectePL", "Cocher au moins un agrément impacté");
                    ModelState.AddModelError("Sanction.IsAgrementAffecteCL", "Cocher au moins un agrément impacté");
                }
            }

        }

        #region ActionsFicheCentre Administration OTC
        //
        // GET: /ControleurTechnique/ModifierFiche/313
        public ActionResult ModifierFiche(int id)
        {
            try
            {
                pContoleurEncour c = this._controleurTechniqueService.FindBy(id);
                IList<rHabilitation> listeHabilitation = this._rHabilitationService.FindAll();
                listeHabilitation.Insert(0, null);
                ControleurHabilitationPLModel model = new ControleurHabilitationPLModel() { Controleur = c, ListeHabilitation = listeHabilitation };

                return View(model);

            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        //
        // POST: /ControleurTechnique/ModifierFiche/313
        [HttpPost]
        public ActionResult ModifierFiche(int id, ControleurHabilitationPLModel model)
        {
            try
            {
                validateFiche(model, this.User);
                if (ModelState.IsValid)
                {
                    this._controleurTechniqueService.modifFiche(id, model.Controleur, this.User.Identity.Name);

                    return this.RedirectToAction("Fiche", new { id = id });
                }
                IList<rHabilitation> listeHabilitation = this._rHabilitationService.FindAll();
                listeHabilitation.Insert(0, null);
                model.ListeHabilitation = listeHabilitation;

                return View(model);
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        private void validateFiche(ControleurHabilitationPLModel model, System.Security.Principal.IPrincipal user)
        {
            //vérifier validité centre VL
            //On vérifie s'il est agréé
            if (!string.IsNullOrEmpty(model.Controleur.CodAgremRattachementVl))
            {
                RechercheCentreAgreeResultat r = this._centreService.estValide(model.Controleur.CodAgremRattachementVl, rCategorieVehicule.CategorieEnum.VL, false);
                if (!r.EstRechercheValide)
                {
                    ModelState.AddModelError("Controleur.CodAgremRattachementVl", "L'agrément centre VL n'est pas valide.");
                }
                else
                {
                    //EG le 14/10/2011 : on ne peut pas rattacher un centre de l'administration
                    if (r.EstCentreAuxiliaire)
                    {
                        ModelState.AddModelError("Controleur.CodAgremRattachementVl", "L'agrément centre VL est un centre auxiliaire.");
                    }
                    else
                    {
                        //EG le 17/10/2011 , si profil = PREFECTURE vérifier le code département
                        if (user.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()))
                        {
                            rUtilisateur u = this._utilisateurService.FindBy(user.Identity.Name);
                            if (!u.RPrefecture.CodDepartement.Equals(r.CentreTrouve.CodDepartement))
                            {
                                ModelState.AddModelError("Controleur.CodAgremRattachementVl", "L'agrément centre VL est un centre d'un autre département.");
                            }
                        }
                    }
                }
            }

            //vérifier validité centre PL
            if (!string.IsNullOrEmpty(model.Controleur.CodAgremRattachementPl))
            {
                RechercheCentreAgreeResultat r = this._centreService.estValide(model.Controleur.CodAgremRattachementPl, rCategorieVehicule.CategorieEnum.PL, false);
                if (!r.EstRechercheValide)
                {
                    ModelState.AddModelError("Controleur.CodAgremRattachementPl", "L'agrément centre PL n'est pas valide.");
                }
                else
                {
                    //EG le 14/10/2011 : on ne peut pas rattacher un centre de l'administration
                    if (r.EstCentreAuxiliaire)
                    {
                        ModelState.AddModelError("Controleur.CodAgremRattachementPl", "L'agrément centre PL est un centre auxiliaire.");
                    }
                    else
                    {
                        //EG le 17/10/2011 , si profil = PREFECTURE vérifier le code département
                        if (user.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()))
                        {
                            rUtilisateur u = this._utilisateurService.FindBy(user.Identity.Name);
                            if (!u.RPrefecture.CodDepartement.Equals(r.CentreTrouve.CodDepartement))
                            {
                                ModelState.AddModelError("Controleur.CodAgremRattachementPl", "L'agrément centre PL est un centre d'un autre département.");
                            }
                        }
                    }
                }
            }

            //vérifier validité centre CL
            if (!string.IsNullOrEmpty(model.Controleur.CodAgremRattachementCl))
            {
                RechercheCentreAgreeResultat r = this._centreService.estValide(model.Controleur.CodAgremRattachementCl, rCategorieVehicule.CategorieEnum.CL, false);
                if (!r.EstRechercheValide)
                {
                    ModelState.AddModelError("Controleur.CodAgremRattachementCl", "L'agrément centre CL n'est pas valide.");
                }
                else
                {
                    //EG le 14/10/2011 : on ne peut pas rattacher un centre de l'administration
                    if (r.EstCentreAuxiliaire)
                    {
                        ModelState.AddModelError("controleurForm.CodAgremRattachementCl", "L'agrément centre CL est un centre auxiliaire.");
                    }
                    else
                    {
                        //EG le 17/10/2011 , si profil = PREFECTURE vérifier le code département
                        if (user.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()))
                        {
                            rUtilisateur u = this._utilisateurService.FindBy(user.Identity.Name);
                            if (!u.RPrefecture.CodDepartement.Equals(r.CentreTrouve.CodDepartement))
                            {
                                ModelState.AddModelError("Controleur.CodAgremRattachementCl", "L'agrément centre CL est un centre d'un autre département.");
                            }
                        }
                    }
                }
            }

            //vérifier validité des dates
            if (model.Controleur.DateEffetNotificationVl != null && model.Controleur.DateFinAgrementVl != null)
            {
                if (DateTime.Compare(model.Controleur.DateFinAgrementVl.Value, model.Controleur.DateEffetNotificationVl.Value) < 0)
                    ModelState.AddModelError("Controleur.DateFinAgrementVl", "La date de fin d'agrément est antérieure à la date de notification !");
            }

            if (model.Controleur.DateEffetNotificationPl != null && model.Controleur.DateFinAgrementPl != null)
            {
                if (DateTime.Compare(model.Controleur.DateFinAgrementPl.Value, model.Controleur.DateEffetNotificationPl.Value) < 0)
                    ModelState.AddModelError("Controleur.DateFinAgrementPl", "La date de fin d'agrément est antérieure à la date de notification !");
            }

            if (model.Controleur.DateEffetNotificationCl != null && model.Controleur.DateFinAgrementCl != null)
            {
                if (DateTime.Compare(model.Controleur.DateFinAgrementCl.Value, model.Controleur.DateEffetNotificationCl.Value) < 0)
                    ModelState.AddModelError("Controleur.DateFinAgrementCl", "La date de fin d'agrément est antérieure à la date de notification !");
            }

            //vérifier saisie du Nom
            if (string.IsNullOrEmpty(model.Controleur.Nom))
            {
                ModelState.AddModelError("Controleur.Nom", "Le nom est obligatoire.");
            }

            //vérifier saisie du prénom
            if (string.IsNullOrEmpty(model.Controleur.Prenom))
            {
                ModelState.AddModelError("Controleur.Prenom", "Le prenom est obligatoire.");
            }

        }

        //
        // GET: /ControleurTechnique/ConfirmationDeleteFiche/313
        public ActionResult ConfirmationDeleteFiche(int id)
        {
            try
            {
                pContoleurEncour c = this._controleurTechniqueService.FindBy(id);

                if (GestionDroitsExecution.isRightActionForControleur(this.User, c, GestionDroitsExecution.ActionsFicheControleur.ADMSUPFI))
                {

                    return View(c);
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

        //
        // POST: /ControleurTechnique/DeleteFiche/313
        [HttpPost]
        public ActionResult DeleteFiche(int id)
        {
            try
            {
                pContoleurEncour c = this._controleurTechniqueService.FindBy(id);
                this._controleurTechniqueService.supprimeFiche(c);

                return RedirectToAction("Recherche");

            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        //
        // GET: /ControleurTechnique/AuteurModification/313
        public ActionResult AuteurModification(int id)
        {
            try
            {
                pContoleurEncour c = this._controleurTechniqueService.FindBy(id);

                return View(c);

            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        //
        // GET: /ControleurTechnique/ConfirmationAnnulerModification/313
        public ActionResult ConfirmationAnnulerModification(int id)
        {
            try
            {
                pContoleurEncour c = this._controleurTechniqueService.FindBy(id);
                if (GestionDroitsExecution.isRightActionForControleur(this.User, c, GestionDroitsExecution.ActionsFicheControleur.ADMANUFI))
                {

                    return View(c);
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

        //GET: /ControleurTechnique/ListeControleurAgreeSansCentre
        public ActionResult ListeControleurAgreeSansCentre()
        {
            try
            {
                HomeModel model = new HomeModel();
                model.ListeControleurAgreeSansCentreRattachementVL = this._controleurTechniqueService.FindAllAgreesSansCentreRattachement(rCategorieVehicule.CategorieEnum.VL, null);
                model.ListeControleurAgreeSansCentreRattachementPL = this._controleurTechniqueService.FindAllAgreesSansCentreRattachement(rCategorieVehicule.CategorieEnum.PL, null);
                model.ListeControleurAgreeSansCentreRattachementCL = this._controleurTechniqueService.FindAllAgreesSansCentreRattachement(rCategorieVehicule.CategorieEnum.CL, null);

                return View(model);
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        //
        // POST: /ControleurTechnique/AnnulerModification/313
        [HttpPost]
        public ActionResult AnnulerModification(int id)
        {
            try
            {
                pContoleurEncour c = this._controleurTechniqueService.FindBy(id);
                //EG le 23/11/2011
                //PLE 07/05/2015 : mise en commun des fonctions "Retour Arrière" et "Annulation modification"
                //this._controleurTechniqueService.annulerModification(c);
                bool userMode = this.User.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) == false;
                this._controleurTechniqueService.retourArriere(id, userMode, this.User.Identity.Name, true);

                return RedirectToAction("Fiche", new { id = id });

            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        #endregion

        //Membres privés
        #region Membres privés
        private Query getRechercheControleurQuery(RechercheControleurModel form)
        {
            Query q = new Query();
            Criteria c;

            if (!string.IsNullOrEmpty(form.Nom))
            {
                c = new Criteria("Nom", form.Nom.ToUpper() + "%", CriteriaOperator.LIKE);
                q.Criteria.Add(c);
            }

            if (!string.IsNullOrEmpty(form.Prenom))
            {
                c = new Criteria("Prenom", form.Prenom + "%", CriteriaOperator.LIKE);
                q.Criteria.Add(c);
            }

            if (form.DateNaissance.HasValue)
            {
                c = new Criteria("DateNaissance", form.DateNaissance , CriteriaOperator.EQUALS);
                q.Criteria.Add(c);
            }

            if (!string.IsNullOrEmpty(form.CodeAgrementVL))
            {
                c = new Criteria("CodAgremVl", form.CodeAgrementVL + "%", CriteriaOperator.LIKE);
                q.Criteria.Add(c);
            }

            if (!string.IsNullOrEmpty(form.CodeAgrementPL))
            {
                c = new Criteria("CodAgremPl", form.CodeAgrementPL + "%", CriteriaOperator.LIKE);
                q.Criteria.Add(c);
            }

            if (!string.IsNullOrEmpty(form.CodeAgrementCL))
            {
                c = new Criteria("CodAgremCl", form.CodeAgrementCL + "%", CriteriaOperator.LIKE);
                q.Criteria.Add(c);
            }

            if (!string.IsNullOrEmpty(form.CodeAgrementCentreRattachementVL))
            {
                c = new Criteria("CodAgremRattachementVl", form.CodeAgrementCentreRattachementVL + "%", CriteriaOperator.LIKE);
                q.Criteria.Add(c);
            }

            if (!string.IsNullOrEmpty(form.CodeAgrementCentreRattachementPL))
            {
                c = new Criteria("CodAgremRattachementPl", form.CodeAgrementCentreRattachementPL + "%", CriteriaOperator.LIKE);
                q.Criteria.Add(c);
            }

            if (!string.IsNullOrEmpty(form.CodeAgrementCentreRattachementCL))
            {
                c = new Criteria("CodAgremRattachementCl", form.CodeAgrementCentreRattachementCL + "%", CriteriaOperator.LIKE);
                q.Criteria.Add(c);
            }

            /*  ETAT agrement
             *  0 : AGREMENT_KO : le contrôleur n'a pas d'agrément valide. -->annulation de l'agrément précédent ou retrait définitif.
                1 : AGREMENT_OK : La préfecture a notifié l'agrément du contrôleur .
                2 : AGREMENT_DEMANDE_EN COURS : L'agrément est en cours de validation par la prefecture.
                3 : AGREMENT_REJETE : La demande d'agrément a été rejeté par la préfecture. 
            */
            /* VL */
            string etatAgrementVlIN = string.Empty;
            if (form.IsAgrementVLAnnule)
            {
                etatAgrementVlIN = "0";
                c = new Criteria("CodAgremVl", null, CriteriaOperator.IS_NOT_NULL);
                q.Criteria.Add(c);
            }

            if (form.IsAgrementVLValide)
            {
                if (string.IsNullOrEmpty(etatAgrementVlIN))
                    etatAgrementVlIN = "1";
                else
                    etatAgrementVlIN += ",1";
            }

            if (form.IsAgrementVLDemandeEnCours)
            {
                if (string.IsNullOrEmpty(etatAgrementVlIN))
                    etatAgrementVlIN = "2";
                else
                    etatAgrementVlIN += ",2";
            }

            if (form.IsAgrementVLRejete)
            {
                if (string.IsNullOrEmpty(etatAgrementVlIN))
                    etatAgrementVlIN = "3";
                else
                    etatAgrementVlIN += ",3";

            }

            if (!string.IsNullOrEmpty(etatAgrementVlIN))
            {
                string[] etats = etatAgrementVlIN.Split(',');
                c = new Criteria("EtatAgrementVl", etats);
                q.Criteria.Add(c);
            }

            /* PL */
            string etatAgrementPlIN = string.Empty;
            if (form.IsAgrementPLAnnule)
            {
                etatAgrementPlIN = "0";
                c = new Criteria("CodAgremPl", null, CriteriaOperator.IS_NOT_NULL);
                q.Criteria.Add(c);
            }

            if (form.IsAgrementPLValide)
            {
                if (string.IsNullOrEmpty(etatAgrementPlIN))
                    etatAgrementPlIN = "1";
                else
                    etatAgrementPlIN += ",1";
            }

            if (form.IsAgrementPLDemandeEnCours)
            {
                if (string.IsNullOrEmpty(etatAgrementPlIN))
                    etatAgrementPlIN = "2";
                else
                    etatAgrementPlIN += ",2";
            }

            if (form.IsAgrementPLRejete)
            {
                if (string.IsNullOrEmpty(etatAgrementPlIN))
                    etatAgrementPlIN = "3";
                else
                    etatAgrementPlIN += ",3";
            }

            if (!string.IsNullOrEmpty(etatAgrementPlIN))
            {
                string[] etats = etatAgrementPlIN.Split(',');
                c = new Criteria("EtatAgrementPl", etats);
                q.Criteria.Add(c);
            }

            /* CL */
            string etatAgrementClIN = string.Empty;
            if (form.IsAgrementCLAnnule)
            {
                etatAgrementClIN = "0";
                c = new Criteria("CodAgremCl", null, CriteriaOperator.IS_NOT_NULL);
                q.Criteria.Add(c);
            }

            if (form.IsAgrementCLValide)
            {
                if (string.IsNullOrEmpty(etatAgrementClIN))
                    etatAgrementClIN = "1";
                else
                    etatAgrementClIN += ",1";
            }

            if (form.IsAgrementCLDemandeEnCours)
            {
                if (string.IsNullOrEmpty(etatAgrementClIN))
                    etatAgrementClIN = "2";
                else
                    etatAgrementClIN += ",2";
            }

            if (form.IsAgrementCLRejete)
            {
                if (string.IsNullOrEmpty(etatAgrementClIN))
                    etatAgrementClIN = "3";
                else
                    etatAgrementClIN += ",3";
            }

            if (!string.IsNullOrEmpty(etatAgrementClIN))
            {
                string[] etats = etatAgrementClIN.Split(',');
                c = new Criteria("EtatAgrementCl", etats);
                q.Criteria.Add(c);
            }

            //EG le 21/10/2011 : prise en compte du prestataire
            if (!string.IsNullOrEmpty(form.EstPrestataire))
            {
                c = new Criteria("EstPrestataire", Convert.ToBoolean(form.EstPrestataire), CriteriaOperator.EQUALS);
                q.Criteria.Add(c);
            }

            //PLE le 23/01/2024
            if (form.AvecExtentionAgrement)
            {
                c = new Criteria("ExtAgremClStatut", "", CriteriaOperator.IS_NOT_NULL);
                q.Criteria.Add(c);
            }

            q.OrderBy.Add(new OrderBy() { Ascending = true, PropertyName = "Nom" });
            return q;

        }

        #region Validations
        private void validateNew(NouveauControleurTechniqueModel model, System.Security.Principal.IPrincipal user)
        {
            //Nom obligatoire
            if (string.IsNullOrEmpty(model.controleur.Nom))
                ModelState.AddModelError("controleur.Nom", "Le nom est obligatoire.");

            //Prenom obligatoire
            if (string.IsNullOrEmpty(model.controleur.Prenom))
                ModelState.AddModelError("controleur.Prenom", "Le prénom est obligatoire.");

            //PLE le 05/03/2024 date de naissance obligatoire
            if (model.controleur.DateNaissance == null)
                ModelState.AddModelError("controleur.DateNaissance", "La date de naissance est obligatoire.");

            //vérification du code centre
            //controleurForm.CodAgremRattachementVl
            string codCentre = string.Empty;
            switch (model.categorie)
            {
                case rCategorieVehicule.CategorieEnum.VL: codCentre = model.controleur.CodAgremRattachementVl; break;
                case rCategorieVehicule.CategorieEnum.PL: codCentre = model.controleur.CodAgremRattachementPl; break;
                case rCategorieVehicule.CategorieEnum.CL: codCentre = model.controleur.CodAgremRattachementCl; break;
            }

            if (string.IsNullOrEmpty(codCentre))
            {
                switch (model.categorie)
                {
                    case rCategorieVehicule.CategorieEnum.VL: ModelState.AddModelError("controleur.CodAgremRattachementVl", "L'agrément du centre VL est obligatoire."); break;
                    case rCategorieVehicule.CategorieEnum.PL: ModelState.AddModelError("controleur.CodAgremRattachementPl", "L'agrément du centre PL est obligatoire."); break;
                    case rCategorieVehicule.CategorieEnum.CL: ModelState.AddModelError("controleur.CodAgremRattachementCl", "L'agrément du centre CL est obligatoire."); break;
                }
            }
            else
            {
                //On vérifie s'il est agréé
                RechercheCentreAgreeResultat r = this._centreService.estValide(codCentre, model.categorie, false);
                if (!r.EstRechercheValide)
                {
                    switch (model.categorie)
                    {
                        case rCategorieVehicule.CategorieEnum.VL: ModelState.AddModelError("controleur.CodAgremRattachementVl", "L'agrément du centre VL n'est pas valide."); break;
                        case rCategorieVehicule.CategorieEnum.PL: ModelState.AddModelError("controleur.CodAgremRattachementPl", "L'agrément du centre PL n'est pas valide."); break;
                        case rCategorieVehicule.CategorieEnum.CL: ModelState.AddModelError("controleur.CodAgremRattachementCl", "L'agrément du centre CL n'est pas valide."); break;
                    }
                }
                else
                {
                    //EG le 14/10/2011 : on ne peut pas rattacher un centre de l'administration
                    if (r.EstCentreAuxiliaire)
                    {
                        switch (model.categorie)
                        {
                            case rCategorieVehicule.CategorieEnum.VL: ModelState.AddModelError("controleur.CodAgremRattachementVl", "L'agrément centre VL est un centre auxiliaire."); break;
                            case rCategorieVehicule.CategorieEnum.PL: ModelState.AddModelError("controleur.CodAgremRattachementPl", "L'agrément centre PL est un centre auxiliaire."); break;
                            case rCategorieVehicule.CategorieEnum.CL: ModelState.AddModelError("controleur.CodAgremRattachementCl", "L'agrément centre CL est un centre auxiliaire."); break;
                        }
                    }
                    else
                    {
                        //EG le 17/10/2011 , si profil = PREFECTURE vérifier le code département
                        if (user.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()))
                        {
                            rUtilisateur u = this._utilisateurService.FindBy(user.Identity.Name);
                            if (!u.RPrefecture.CodDepartement.Equals(r.CentreTrouve.CodDepartement))
                            {
                                switch (model.categorie)
                                {
                                    case rCategorieVehicule.CategorieEnum.VL: ModelState.AddModelError("controleur.CodAgremRattachementVl", "L'agrément centre VL est un centre d'un autre département."); break;
                                    case rCategorieVehicule.CategorieEnum.PL: ModelState.AddModelError("controleur.CodAgremRattachementPl", "L'agrément centre PL est un centre d'un autre département."); break;
                                    case rCategorieVehicule.CategorieEnum.CL: ModelState.AddModelError("controleur.CodAgremRattachementCl", "L'agrément centre CL est un centre d'un autre département."); break;
                                }                                
                            }
                        }
                        //PLE le 01/03/2024 Détection doublon
                        if (ModelState.IsValid)
                        {
                            if (String.IsNullOrEmpty(model.ConfirmControleursEnDoublons))
                            {
                                //PLE le 01/03/2024 Recherche des contrôleurs sur Non et Date de naissance
                                RechercheControleurModel form = new RechercheControleurModel();
                                form.Nom = model.controleur.Nom.ToUpper().Trim();
                                form.DateNaissance = model.controleur.DateNaissance;
                                Query q = getRechercheControleurQuery(form);
                                model.controleursEnDoublons = this._controleurTechniqueService.FindBy(q);
                                //if (model.controleursEnDoublons != null && model.controleursEnDoublons.Count() > 0)
                                //{
                                //   ModelState.AddModelError("DetectionControleursEnDoublons", "un contrôleur existe déjà dans le registre avec un nom et une date de naissance identique. Veuillez cliquer sur le lien pour ouvrir la fiche et donner l'agrément sur ce contrôleur. ");
                                //}
                            }
                        }

                    }
                }
            }

            //EG le 21/10/2011 : vérification de la saisie du prestataire
            if (string.IsNullOrEmpty(model.EstPrestataire))
            {
                ModelState.AddModelError("EstPrestataire", "Vous devez indiquer par oui ou non si l'agrément porte sur un prestataire");
            }

        }

        private void validateNotification(ActionAgrementControleurModel model, System.Security.Principal.IPrincipal user)
        {
            DateTime? dateNotification = default(DateTime);
            string codCentre = string.Empty;
            string numAgrementControleur = string.Empty;

            //validation centre de rattachement
            switch (model.categorieVehicule)
            {
                case rCategorieVehicule.CategorieEnum.VL:
                    codCentre = model.controleur.CodAgremRattachementVl;
                    dateNotification = model.controleur.DateEffetNotificationVl;
                    numAgrementControleur = model.controleur.CodAgremVl;
                    break;
                case rCategorieVehicule.CategorieEnum.PL:
                    codCentre = model.controleur.CodAgremRattachementPl;
                    dateNotification = model.controleur.DateEffetNotificationPl;
                    numAgrementControleur = model.controleur.CodAgremPl;
                    break;
                case rCategorieVehicule.CategorieEnum.CL:
                    codCentre = model.controleur.CodAgremRattachementCl;
                    dateNotification = model.controleur.DateEffetNotificationCl;
                    numAgrementControleur = model.controleur.CodAgremCl;
                    break;
            }

            if (string.IsNullOrEmpty(codCentre))
            {
                switch (model.categorieVehicule)
                {
                    case rCategorieVehicule.CategorieEnum.VL: ModelState.AddModelError("controleur.CodAgremRattachementVl", "L'agrément centre VL est obligatoire."); break;
                    case rCategorieVehicule.CategorieEnum.PL: ModelState.AddModelError("controleur.CodAgremRattachementPl", "L'agrément centre PL est obligatoire."); break;
                    case rCategorieVehicule.CategorieEnum.CL: ModelState.AddModelError("controleur.CodAgremRattachementCl", "L'agrément centre CL est obligatoire."); break;
                }
            }
            else
            {
                //On vérifie s'il est agréé
                RechercheCentreAgreeResultat r = this._centreService.estValide(codCentre, model.categorieVehicule, false);
                if (!r.EstRechercheValide)
                {
                    switch (model.categorieVehicule)
                    {
                        case rCategorieVehicule.CategorieEnum.VL: ModelState.AddModelError("controleur.CodAgremRattachementVl", "L'agrément centre VL n'est pas valide."); break;
                        case rCategorieVehicule.CategorieEnum.PL: ModelState.AddModelError("controleur.CodAgremRattachementPl", "L'agrément centre PL n'est pas valide."); break;
                        case rCategorieVehicule.CategorieEnum.CL: ModelState.AddModelError("controleur.CodAgremRattachementCl", "L'agrément centre CL n'est pas valide."); break;
                    }
                }
                else
                {
                    //EG le 14/10/2011 : on ne peut pas rattacher un centre de l'administration
                    if (r.EstCentreAuxiliaire)
                    {
                        switch (model.categorieVehicule)
                        {
                            case rCategorieVehicule.CategorieEnum.VL: ModelState.AddModelError("controleur.CodAgremRattachementVl", "L'agrément centre VL est un centre auxiliaire."); break;
                            case rCategorieVehicule.CategorieEnum.PL: ModelState.AddModelError("controleur.CodAgremRattachementPl", "L'agrément centre PL est un centre auxiliaire."); break;
                            case rCategorieVehicule.CategorieEnum.CL: ModelState.AddModelError("controleur.CodAgremRattachementCl", "L'agrément centre CL est un centre auxiliaire."); break;
                        }
                    }
                    else
                    {
                        //EG le 17/10/2011 , si profil = PREFECTURE vérifier le code département
                        if (user.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()))
                        {
                            rUtilisateur u = this._utilisateurService.FindBy(user.Identity.Name);
                            if (!u.RPrefecture.CodDepartement.Equals(r.CentreTrouve.CodDepartement))
                            {
                                switch (model.categorieVehicule)
                                {
                                    case rCategorieVehicule.CategorieEnum.VL: ModelState.AddModelError("controleur.CodAgremRattachementVl", "L'agrément centre VL est un centre d'un autre département."); break;
                                    case rCategorieVehicule.CategorieEnum.PL: ModelState.AddModelError("controleur.CodAgremRattachementPl", "L'agrément centre PL est un centre d'un autre département."); break;
                                    case rCategorieVehicule.CategorieEnum.CL: ModelState.AddModelError("controleur.CodAgremRattachementCl", "L'agrément centre CL est un centre d'un autre département."); break;
                                }
                            }
                        }
                    }
                }
            }
            //validation date effet notification
            if (dateNotification == null || dateNotification == default(DateTime))
            {
                switch (model.categorieVehicule)
                {
                    case rCategorieVehicule.CategorieEnum.VL: ModelState.AddModelError("controleur.DateEffetNotificationVl", "La date d'effet n'est pas valide."); break;
                    case rCategorieVehicule.CategorieEnum.PL: ModelState.AddModelError("controleur.DateEffetNotificationPl", "La date d'effet n'est pas valide."); break;
                    case rCategorieVehicule.CategorieEnum.CL: ModelState.AddModelError("controleur.DateEffetNotificationCl", "La date d'effet n'est pas valide."); break;
                }
            }
            else if (model.categorieVehicule.Equals(rCategorieVehicule.CategorieEnum.CL) && (dateNotification < new DateTime(2024, 04, 15)))
            {
                ModelState.AddModelError("controleur.DateEffetNotificationCl", "La date d'effet doit être postérieure au 15/04/2024.");
            }

            //validation numéro agrément (optionnel)
            if (!string.IsNullOrEmpty(numAgrementControleur))
            {
                //vérification du format
                //RegExp Vl : [A-B0-9]{3}(A|C|D|F|S|T|V|Z)[0-9]{4}
                //RegExp Pl : [A-Z0-9]{3}(A|J|K|Z)[0-9]{4}
                //todo RegExp Cl
                string strRegex = null;
                switch (model.categorieVehicule)
                {
                    case rCategorieVehicule.CategorieEnum.VL: strRegex = "[A-B0-9]{3}(A|C|D|F|S|T|V|Z)[0-9]{4}"; break;
                    case rCategorieVehicule.CategorieEnum.PL: strRegex = "[A-Z0-9]{3}(A|J|K|Z)[0-9]{4}"; break;
                    case rCategorieVehicule.CategorieEnum.CL: strRegex = "[A-B0-9]{3}[A|C|D|F|S|T|Z][0-9]{4}"; break;
                }
                System.Text.RegularExpressions.Regex _Regex = new System.Text.RegularExpressions.Regex(strRegex);
                if (!_Regex.IsMatch(numAgrementControleur))
                {
                    switch (model.categorieVehicule)
                    {
                        case rCategorieVehicule.CategorieEnum.VL: ModelState.AddModelError("controleur.CodAgremVl", "L'agrément contrôleur n'est pas valide."); break;
                        case rCategorieVehicule.CategorieEnum.PL: ModelState.AddModelError("controleur.CodAgremPl", "L'agrément contrôleur n'est pas valide."); break;
                        case rCategorieVehicule.CategorieEnum.CL: ModelState.AddModelError("controleur.CodAgremCl", "L'agrément contrôleur n'est pas valide."); break;
                    }
                }
            }

        }

        private void validateDemandeAgrement(ActionAgrementControleurModel model, System.Security.Principal.IPrincipal user)
        {
            string codCentre = string.Empty;

            //validation centre de rattachement
            switch (model.categorieVehicule)
            {
                case rCategorieVehicule.CategorieEnum.VL:
                    codCentre = model.controleur.CodAgremRattachementVl;
                    break;
                case rCategorieVehicule.CategorieEnum.PL:
                    codCentre = model.controleur.CodAgremRattachementPl;
                    break;
                case rCategorieVehicule.CategorieEnum.CL:
                    codCentre = model.controleur.CodAgremRattachementCl;
                    break;
            }

            if (!string.IsNullOrEmpty(codCentre))
            {

                RechercheCentreAgreeResultat r = this._centreService.estValide(codCentre, model.categorieVehicule, false);
                if (!r.EstRechercheValide)
                {
                    switch (model.categorieVehicule)
                    {
                        case rCategorieVehicule.CategorieEnum.VL: ModelState.AddModelError("controleur.CodAgremRattachementVl", "L'agrément centre VL n'est pas valide."); break;
                        case rCategorieVehicule.CategorieEnum.PL: ModelState.AddModelError("controleur.CodAgremRattachementPl", "L'agrément centre PL n'est pas valide."); break;
                        case rCategorieVehicule.CategorieEnum.CL: ModelState.AddModelError("controleur.CodAgremRattachementCl", "L'agrément centre CL n'est pas valide."); break;
                    }
                }
                else
                {
                    //EG le 14/10/2011 : on ne peut pas rattacher un centre de l'administration
                    if (r.EstCentreAuxiliaire)
                    {
                        switch (model.categorieVehicule)
                        {
                            case rCategorieVehicule.CategorieEnum.VL: ModelState.AddModelError("controleur.CodAgremRattachementVl", "L'agrément centre VL est un centre auxiliaire."); break;
                            case rCategorieVehicule.CategorieEnum.PL: ModelState.AddModelError("controleur.CodAgremRattachementPl", "L'agrément centre PL est un centre auxiliaire."); break;
                            case rCategorieVehicule.CategorieEnum.CL: ModelState.AddModelError("controleur.CodAgremRattachementCl", "L'agrément centre CL est un centre auxiliaire."); break;
                        }
                    }
                    else
                    {
                        //EG le 17/10/2011 , si profil = PREFECTURE vérifier le code département
                        if (user.IsInRole(rUtilisateur.Profil.PREFECTURE.ToString()))
                        {
                            rUtilisateur u = this._utilisateurService.FindBy(user.Identity.Name);
                            if (!u.RPrefecture.CodDepartement.Equals(r.CentreTrouve.CodDepartement))
                            {
                                switch (model.categorieVehicule)
                                {
                                    case rCategorieVehicule.CategorieEnum.VL: ModelState.AddModelError("controleur.CodAgremRattachementVl", "L'agrément centre VL est un centre d'un autre département."); break;
                                    case rCategorieVehicule.CategorieEnum.PL: ModelState.AddModelError("controleur.CodAgremRattachementPl", "L'agrément centre PL est un centre d'un autre département."); break;
                                    case rCategorieVehicule.CategorieEnum.CL: ModelState.AddModelError("controleur.CodAgremRattachementCl", "L'agrément centre CL est un centre d'un autre département."); break;
                                }
                            }
                        }
                    }
                }
            }

        }

        private void validateAnnulationAgrement(ActionAgrementControleurModel model)
        {
            switch (model.categorieVehicule)
            {
                case rCategorieVehicule.CategorieEnum.VL: if (model.controleur.DateFinAgrementVl == null) ModelState.AddModelError("controleur.DateFinAgrementVl", "La date de fin d'agrément VL n'est pas valide."); break;
                case rCategorieVehicule.CategorieEnum.PL: if (model.controleur.DateFinAgrementPl == null) ModelState.AddModelError("controleur.DateFinAgrementPl", "La date de fin d'agrément PL n'est pas valide."); break;
                case rCategorieVehicule.CategorieEnum.CL: if (model.controleur.DateFinAgrementCl == null) ModelState.AddModelError("controleur.DateFinAgrementCl", "La date de fin d'agrément CL n'est pas valide."); break;
            }

        }

        //PLE le 22/01/2024
        // GET: /Controleurtechnique/DeclarerExtensionAgrementCatL
        public ActionResult DeclarerExtensionAgrementCatL(int id, rCategorieVehicule.CategorieEnum Categorie, String ReturnLettreReseau, String ReturnCodDep)
        {
            try
            {
                pContoleurEncour c = this._controleurTechniqueService.FindBy(id);
                if (
                    (rCategorieVehicule.CategorieEnum.VL.Equals(Categorie)) && (GestionDroitsExecution.isRightActionForControleur(this.User, c, GestionDroitsExecution.ActionsFicheControleur.EXTDECLA_VL))
                    ||
                    (rCategorieVehicule.CategorieEnum.PL.Equals(Categorie)) && (GestionDroitsExecution.isRightActionForControleur(this.User, c, GestionDroitsExecution.ActionsFicheControleur.EXTDECLA_PL))
                   )
                {

                    //PLE le 28/01/2024 Recherche si le contrôleur à au moins une formation initiale CL
                    Boolean possedeUneFormationIntialeCL = true;
                    switch (Categorie)
                    {
                        case rCategorieVehicule.CategorieEnum.VL:
                            possedeUneFormationIntialeCL = (c.PFormationControleurVLs.Where(p => p.RTypeFormation != null).Where(p => p.RTypeFormation.Id == ((short?)rTypeFormation.TypeFormationEnum.FORMATION_INIT_CL_VL)).Count() > 0);
                            break;
                        case rCategorieVehicule.CategorieEnum.PL:
                            possedeUneFormationIntialeCL = (c.PFormationControleurPLs.Where(p => p.RTypeFormation != null).Where(p => p.RTypeFormation.Id == ((short?)rTypeFormation.TypeFormationEnum.FORMATION_INIT_CL_PL)).Count() > 0);
                            break;
                    }
                    if (!possedeUneFormationIntialeCL)
                    {
                        return this.RedirectToAction("AlerteFormationInitialeCL", new { id = id, Categorie = Categorie, ReturnLettreReseau = ReturnLettreReseau, ReturnCodDep = ReturnCodDep });
                    }

                    ExtensionAgrementControleurCatLModel model = new ExtensionAgrementControleurCatLModel();
                    model.Controleur = c;
                    model.Categorie = Categorie;
                    model.Controleur.ExtAgremClStatut = null;
                    model.Controleur.ExtAgremClDateValid = new DateTime(2024, 04, 15);
                    model.ReturnLettreReseau = ReturnLettreReseau;
                    model.ReturnCodDep = ReturnCodDep;
                    return View(model);
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
        public ActionResult DeclarerExtensionAgrementCatL(int id, ExtensionAgrementControleurCatLModel model)
        {
            try
            {
                pContoleurEncour c = this._controleurTechniqueService.FindBy(id);
                if (
                    (rCategorieVehicule.CategorieEnum.VL.Equals(model.Categorie)) && (GestionDroitsExecution.isRightActionForControleur(this.User, c, GestionDroitsExecution.ActionsFicheControleur.EXTDECLA_VL))
                    ||
                    (rCategorieVehicule.CategorieEnum.PL.Equals(model.Categorie)) && (GestionDroitsExecution.isRightActionForControleur(this.User, c, GestionDroitsExecution.ActionsFicheControleur.EXTDECLA_PL))
                   )
                {
                    validateDeclarerExtensionAgrementCatL(model);
                    if (ModelState.IsValid)
                    {
                        this._controleurTechniqueService.EnregistrerExtensionAgrementCatL(id, model.Controleur, model.Categorie, this.User.Identity.Name);
                        TempData["Message"] = "Déclaration de l'extension d'agrément Catégorie L enregistrée ...";
                        TempData["MessageType"] = "Success";

                        if (!String.IsNullOrEmpty(model.ReturnLettreReseau))
                        {
                            return this.RedirectToAction("ReturnToListeControleurPourReseau", "Home", new { returnLettreReseau = model.ReturnLettreReseau, returnCodDep = model.ReturnCodDep });
                        }

                        return this.RedirectToAction("Fiche", new { id = id });
                    }
                    else
                    {
                        return View(model);
                    }
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

        private void validateDeclarerExtensionAgrementCatL(ExtensionAgrementControleurCatLModel model)
        {
            if (model.Controleur.ExtAgremClStatut == null)
            {
                ModelState.AddModelError("Controleur.ExtAgremClStatut", "Sélectionnez <OUI> pour confirmer votre déclaration d'extension d'agrément");
            }
            if (model.Controleur.ExtAgremClDateValid == null)
            {
                ModelState.AddModelError("Controleur.ExtAgremClDateValid", "La date de début de validité de l'extension d'agrément doit être renseignée");
            }
            if (model.Controleur.ExtAgremClDateValid < new DateTime(2024, 04, 15) || model.Controleur.ExtAgremClDateValid > new DateTime(2025, 08, 31))
            {
                ModelState.AddModelError("Controleur.ExtAgremClDateValid", "La date de début de validité de l'extension d'agrément doit être comprise entre le 15/04/2024 et le 31/08/2025");
            }


        }

        //PLE le 22/01/2024
        // GET: /Controleur/RetirerExtensionAgrementCatL
        public ActionResult RetirerExtensionAgrementCatL(int id, rCategorieVehicule.CategorieEnum Categorie)
        {
            try
            {
                pContoleurEncour c = this._controleurTechniqueService.FindBy(id);
                if (
                    (rCategorieVehicule.CategorieEnum.VL.Equals(Categorie)) && (GestionDroitsExecution.isRightActionForControleur(this.User, c, GestionDroitsExecution.ActionsFicheControleur.EXTRETRA_VL))
                    ||
                    (rCategorieVehicule.CategorieEnum.PL.Equals(Categorie)) && (GestionDroitsExecution.isRightActionForControleur(this.User, c, GestionDroitsExecution.ActionsFicheControleur.EXTRETRA_PL))
                   )
                {
                    ExtensionAgrementControleurCatLModel model = new ExtensionAgrementControleurCatLModel();
                    model.Controleur = c;
                    model.Categorie = Categorie;
                    model.Controleur.ExtAgremClStatut = null;
                    model.Controleur.ExtAgremClDateValid = null;

                    return View(model);
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
        public ActionResult RetirerExtensionAgrementCatL(int id, ExtensionAgrementControleurCatLModel model)
        {
            try
            {
                pContoleurEncour c = this._controleurTechniqueService.FindBy(id);
                if (
                    (rCategorieVehicule.CategorieEnum.VL.Equals(model.Categorie)) && (GestionDroitsExecution.isRightActionForControleur(this.User, c, GestionDroitsExecution.ActionsFicheControleur.EXTRETRA_VL))
                    ||
                    (rCategorieVehicule.CategorieEnum.PL.Equals(model.Categorie)) && (GestionDroitsExecution.isRightActionForControleur(this.User, c, GestionDroitsExecution.ActionsFicheControleur.EXTRETRA_PL))
                   )
                {
                    validateRetirerExtensionAgrementCatL(model, c);
                    if (ModelState.IsValid)
                    {
                        this._controleurTechniqueService.EnregistrerExtensionAgrementCatL(id, model.Controleur, model.Categorie, this.User.Identity.Name);
                        TempData["Message"] = "Retrait de l'extension d'agrément Catégorie L enregistrée ...";
                        TempData["MessageType"] = "Success";

                        return this.RedirectToAction("Fiche", new { id = id });
                    }
                    else
                    {
                        return View(model);
                    }
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

        private void validateRetirerExtensionAgrementCatL(ExtensionAgrementControleurCatLModel model, pContoleurEncour c)
        {
            if (model.Controleur.ExtAgremClStatut == null)
            {
                ModelState.AddModelError("Controleur.ExtAgremClStatut", "Sélectionnez <NON> pour confirmer votre retrait d'extension d'agrément");
            }
            if (model.Controleur.ExtAgremClDateValid == null)
            {
                ModelState.AddModelError("Controleur.ExtAgremClDateValid", "La date de fin de validité de l'extension d'agrément doit être renseignée");
            }
            else
            if (model.Controleur.ExtAgremClDateValid.Value.CompareTo(c.ExtAgremClDateValid.Value) < 0)
            {
                ModelState.AddModelError("Controleur.ExtAgremClDateValid", "La date de fin de validité de l'extension d'agrément doit être postérieure à la date de début de validité (" + c.ExtAgremClDateValid.Value.ToString("dd/MM/yyyy") + ")");
            }
            else
            if (model.Controleur.ExtAgremClDateValid < new DateTime(2024, 04, 15) || model.Controleur.ExtAgremClDateValid > new DateTime(2025, 08, 31))
            {
                ModelState.AddModelError("Controleur.ExtAgremClDateValid", "La date de fin de validité de l'extension d'agrément doit être comprise entre le 15/04/2024 et le 31/08/2025");
            }


        }

        //PLE le 22/01/2024
        // GET: /Controleurtechnique/AlerteFormationInitialeCL
        public ActionResult AlerteFormationInitialeCL(int id, rCategorieVehicule.CategorieEnum Categorie, String ReturnLettreReseau, String ReturnCodDep)
        {
            try
            {
                pContoleurEncour c = this._controleurTechniqueService.FindBy(id);
                if (
                    (rCategorieVehicule.CategorieEnum.VL.Equals(Categorie)) && (GestionDroitsExecution.isRightActionForControleur(this.User, c, GestionDroitsExecution.ActionsFicheControleur.EXTDECLA_VL))
                    ||
                    (rCategorieVehicule.CategorieEnum.PL.Equals(Categorie)) && (GestionDroitsExecution.isRightActionForControleur(this.User, c, GestionDroitsExecution.ActionsFicheControleur.EXTDECLA_PL))
                   )
                {


                    ExtensionAgrementControleurCatLModel model = new ExtensionAgrementControleurCatLModel();
                    model.Controleur = c;
                    model.Categorie = Categorie;

                    model.ReturnLettreReseau = ReturnLettreReseau;
                    model.ReturnCodDep = ReturnCodDep;
                    return View(model);
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

        // PLE 25/03/2026 suivi qualification CL
        public ActionResult TelechargeFicheQualificationCLPDF(int id)
        {
            try
            {
                //Données du controleur
                pContoleurEncour c = this._controleurTechniqueService.FindBy(id);
                //Données de l'activité du controleur
                pActiviteControleurCL a = this._pActiviteControleurCLService.FindBy(id);

                if (GestionDroitsExecution.isRightActionForControleur(this.User, c, GestionDroitsExecution.ActionsFicheControleur.TELECHARGE_FICHE_QUALIFICATION_CONTROLEUR_CL))
                {
                    //On charge le document Crystal
                    ReportDocument boReportDocument = new ReportDocument();
                    boReportDocument.Load(Server.MapPath("~/bin/reporting/cntlr_ficheControleurSuiviActiviteCL.rpt"));

                    DSficheControleurSuiviActiviteCL ds = new DSficheControleurSuiviActiviteCL();
                    DSficheControleurSuiviActiviteCL.dtItemListeSuiviActiviteCLDataTable dt = ds.dtItemListeSuiviActiviteCL;
                    DSficheControleurSuiviActiviteCL.dtItemListeSuiviActiviteCLRow dr = dt.NewdtItemListeSuiviActiviteCLRow();

                    dr.nom = c.Nom;
                    dr.prenom = c.Prenom;
                    dr.DateNaissance = c.DateNaissance.Value;
                    dr.codAgremCntlr = c.CodAgremCl;
                    if (c.DateEffetNotificationCl != null)
                    {
                        dr.dateEffetNotificationCl = Convert.ToDateTime(c.DateEffetNotificationCl);
                    }
                    if (c.DateFinAgrementCl != null)
                    {
                        dr.dateFinAgrementCl = Convert.ToDateTime(c.DateFinAgrementCl);
                    }
                    dr.codAgremCentre = c.CodAgremRattachementCl;
                    if (c.DateDebutRattachementCl != null)
                    {
                        dr.dateDebutRattachementCl = Convert.ToDateTime(c.DateDebutRattachementCl);
                    }

                    if (a != null)
                    {
                        if (a.DateDernierCtrl != null)
                        {
                            dr.dateDernierCtrl = Convert.ToDateTime(a.DateDernierCtrl);
                        }
                        dr.activiteJanvierN1 = a.ActiviteJanvierN1.Value;
                        dr.activiteJanvierN = a.ActiviteJanvierN.Value;
                        dr.activiteFevrierN1 = a.ActiviteFevrierN1.Value;
                        dr.activiteFevrierN = a.ActiviteFevrierN.Value;
                        dr.activiteMarsN1 = a.ActiviteMarsN1.Value;
                        dr.activiteMarsN = a.ActiviteMarsN.Value;
                        dr.activiteAvrilN1 = a.ActiviteAvrilN1.Value;
                        dr.activiteAvrilN = a.ActiviteAvrilN.Value;
                        dr.activiteMaiN1 = a.ActiviteMaiN1.Value;
                        dr.activiteMaiN = a.ActiviteMaiN.Value;
                        dr.activiteJuinN1 = a.ActiviteJuinN1.Value;
                        dr.activiteJuinN = a.ActiviteJuinN.Value;
                        dr.activiteJuilletN1 = a.ActiviteJuilletN1.Value;
                        dr.activiteJuilletN = a.ActiviteJuilletN.Value;
                        dr.activiteAoutN1 = a.ActiviteAoutN1.Value;
                        dr.activiteAoutN = a.ActiviteAoutN.Value;
                        dr.activiteSeptembreN1 = a.ActiviteSeptembreN1.Value;
                        dr.activiteSeptembreN = a.ActiviteSeptembreN.Value;
                        dr.activiteOctobreN1 = a.ActiviteOctobreN1.Value;
                        dr.activiteOctobreN = a.ActiviteOctobreN.Value;
                        dr.activiteNovembreN1 = a.ActiviteNovembreN1.Value;
                        dr.activiteNovembreN = a.ActiviteNovembreN.Value;
                        dr.activiteDecembreN1 = a.ActiviteDecembreN1.Value;
                        dr.activiteDecembreN = a.ActiviteDecembreN.Value;
                        dr.totalVtpN1 = a.TotalVtpN1.Value;
                        dr.totalVtpN = a.TotalVtpN.Value;
                        dr.seuilN1ARealiser = a.SeuilN1ARealiser.Value;
                        dr.seuilNARealiser = a.SeuilNARealiser.Value;
                        dr.seuilAtteintN1 = a.SeuilAtteintN1;
                        dr.seuilAtteintN = a.SeuilAtteintN;
                        dr.anneeMois = a.AnneeMois;

                    }

                    dt.Rows.Add(dr);

                    // Chargement du DATASET de la fiche contrôleur dans l'état crystal

                    boReportDocument.SetDataSource(ds);

                    //Définition du format d'export PDF
                    ExportOptions exportOptions = new ExportOptions();
                    //This sets which format we will export to.
                    exportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    //Create the FormatOptions class for PDF.
                    PdfFormatOptions PDFexportOptions = new PdfFormatOptions();
                    PDFexportOptions.FirstPageNumber = 1;
                    PDFexportOptions.LastPageNumber = 2;
                    PDFexportOptions.CreateBookmarksFromGroupTree = true;

                    //Set the export format options with the page range we just set.
                    exportOptions.FormatOptions = PDFexportOptions;

                    //On exporte le document au format PDF
                    //boReportDocument.Export(exportOptions);

                    Stream pdfStream = boReportDocument.ExportToStream(ExportFormatType.PortableDocFormat);

                    MemoryStream memStream = new MemoryStream();

                    byte[] respBuffer = new byte[1024];

                    int bytesRead = pdfStream.Read(respBuffer, 0, respBuffer.Length);
                    while (bytesRead > 0)
                    {
                        memStream.Write(respBuffer, 0, bytesRead);
                        bytesRead = pdfStream.Read(respBuffer, 0, respBuffer.Length);
                    }

                    //libération du rapport
                    boReportDocument.Close();
                    boReportDocument.Dispose();
                    return File(memStream.GetBuffer(), "application/pdf", "ficheControleurSuiviQualificationCL_" + c.CodAgremCl + ".pdf");

                }

                throw new NotImplementedException();
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }
        #endregion
        #endregion

    }

}
