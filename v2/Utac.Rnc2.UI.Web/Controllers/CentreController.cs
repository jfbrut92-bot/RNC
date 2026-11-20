#pragma warning disable IDE0073 // L'en-tête du fichier est manquant ou ne se trouve pas en haut du fichier
using System;
#pragma warning restore IDE0073 // L'en-tête du fichier est manquant ou ne se trouve pas en haut du fichier
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using log4net;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.BusinessObjects.certificats;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Fabriques;
using Utac.Rnc2.Infrastructure.Queries;
using Utac.Rnc2.MVC.Extensions.validator;
using Utac.Rnc2.UI.Web.DataSets;
using Utac.Rnc2.UI.Web.Helpers;
using Utac.Rnc2.UI.Web.Models;

namespace Utac.Rnc2.UI.Web.Controllers
{
    public class CentreController : RestrictedAcessController
    {

        private readonly IpCentreEncourService _pCentreEnCourservice;
        private readonly IpCentreEncourService _pCentreEnCourserviceAutoCommit;
        private readonly IpCertificatService _pCertificatService;
        private readonly IrReseauService _rReseauService;
        private readonly IrStatutJuridiqueService _rStatutJuridiqueService;
        private readonly IrMotifModificationService _rMotifModificationService;
        private readonly IrPrefectureService _rPrefectureService;
        private readonly IrUtilisateurService _rUtilisateurService;
        private readonly IrTypeSanctionCentreService _rTypeSanctionCentreService;
        private readonly IrTypeOrganismeDemandeurService _rTypeOrganismeDemandeurService;
        private readonly IpSanctionCentreService _pSanctionCentreService;
        private readonly IpAmendeCentreService _pAmendeCentreService;
        private readonly IpRapportExceptionService _pRapportExceptionService;
        private readonly IpRapportExceptionVLService _pRapportExceptionVLService;
        private readonly IpGestionCertificatService _pGestionCertificatService;
        private readonly IpCentreHistoService _pCentreHistoService;
        private readonly IpEquipementCentreVLService _pEquipementCentreVLService;
        private readonly IpEquipementCentrePLService _pEquipementCentrePLService;
        private readonly IpEquipementCentreCLService _pEquipementCentreCLService;
        private readonly IpInfoRdvService _pInfoRdvService;
        private readonly IpHoraireOuvertureService _pHoraireOuvertureService;
        private readonly IpJournalActiviteService _pJournalActiviteService;
        private readonly IpDerogationCentreService _pDerogationCentreService;
        private readonly IpRapportExceptionCLService _pRapportExceptionCLService;

        public CentreController(IpCentreEncourService pCentreEnCourService,
                                IpCertificatService pCertificatService,
                                IrReseauService rReseauService,
                                IrStatutJuridiqueService rStatutJuridique,
                                IrMotifModificationService rMotifModification,
                                IpCentreEncourService pCentreEnCourserviceAutoCommit,
                                IrPrefectureService rPrefecture,
                                IrUtilisateurService rUtilisateur,
                                IrTypeSanctionCentreService rTypeSanctionCentre,
                                IrTypeOrganismeDemandeurService rTypeOrganismeDemandeur,
                                IpSanctionCentreService pSanctionCentreService,
                                IpAmendeCentreService pAmendeCentreService,
                                IpRapportExceptionService pRapportExceptionService,
                                IpRapportExceptionVLService pRapportExceptionVlService,
                                IpGestionCertificatService pGestionCertificatService,
                                IpCentreHistoService pCentreHistoService,
                                IpEquipementCentreVLService equipementCentreVLService,
                                IpEquipementCentrePLService equipementCentrePLService,
                                IpEquipementCentreCLService equipementCentreCLService,
                                IpInfoRdvService pInfoRdvService,
                                IpHoraireOuvertureService pHoraireOuvertureService,
                                IpJournalActiviteService pJournalActiviteService,
                                IpDerogationCentreService pDerogationCentreService,
                                IpRapportExceptionCLService pRapportExceptionCLService
                                )
        {
            this._pCentreEnCourservice = pCentreEnCourService;
            this._pCertificatService = pCertificatService;
            this._rReseauService = rReseauService;
            this._rStatutJuridiqueService = rStatutJuridique;
            this._rMotifModificationService = rMotifModification;
            this._pCentreEnCourserviceAutoCommit = pCentreEnCourserviceAutoCommit;
            this._rPrefectureService = rPrefecture;
            this._rUtilisateurService = rUtilisateur;
            this._rTypeSanctionCentreService = rTypeSanctionCentre;
            this._rTypeOrganismeDemandeurService = rTypeOrganismeDemandeur;
            this._pSanctionCentreService = pSanctionCentreService;
            this._pAmendeCentreService = pAmendeCentreService;
            this._pRapportExceptionService = pRapportExceptionService;
            this._pRapportExceptionVLService = pRapportExceptionVlService;
            this._pGestionCertificatService = pGestionCertificatService;
            this._pCentreHistoService = pCentreHistoService;
            this._pEquipementCentreVLService = equipementCentreVLService;
            this._pEquipementCentrePLService = equipementCentrePLService;
            this._pEquipementCentreCLService = equipementCentreCLService;
            this._pInfoRdvService = pInfoRdvService;
            this._pHoraireOuvertureService = pHoraireOuvertureService;
            this._pJournalActiviteService = pJournalActiviteService;
            this._pDerogationCentreService = pDerogationCentreService;
            this._pRapportExceptionCLService = pRapportExceptionCLService;
        }

        public CentreController()
            : this(
                   NhibernateServiceFactory.getInstance().getpCentreEncourService(true, true),
                   NhibernateServiceFactory.getInstance().getpCertificatService(true),
                   NhibernateServiceFactory.getInstance().getrReseauService(true),
                   NhibernateServiceFactory.getInstance().getrStatutJuridiqueService(true),
                   NhibernateServiceFactory.getInstance().getrMotifModificationService(true),
                   NhibernateServiceFactory.getInstance().getpCentreEncourService(false, true),
                   NhibernateServiceFactory.getInstance().getrPrefectureService(true),
                   NhibernateServiceFactory.getInstance().getrUtilisateurService(true, true),
                   NhibernateServiceFactory.getInstance().getrTypeSanctionCentreService(true),
                   NhibernateServiceFactory.getInstance().getrTypeOrganismeDemandeurService(true),
                   NhibernateServiceFactory.getInstance().getpSanctionCentreService(true),
                   NhibernateServiceFactory.getInstance().getpAmendeCentreService(true),
                   NhibernateServiceFactory.getInstance().getpRapportExceptionService(true),
                   NhibernateServiceFactory.getInstance().getpRapportExceptionVLService(true),
                   NhibernateServiceFactory.getInstance().getpGestionCertificatService(true),
                   NhibernateServiceFactory.getInstance().getpCentreHistoService(true),
                   NhibernateServiceFactory.getInstance().getpEquipementCentreVLService(true),
                   NhibernateServiceFactory.getInstance().getpEquipementCentrePLService(true),
                   NhibernateServiceFactory.getInstance().getpEquipementCentreCLService(true),
                   NhibernateServiceFactory.getInstance().getpInfoRdvService(true),
                   NhibernateServiceFactory.getInstance().getpHoraireOuvertureService(true),
                   NhibernateServiceFactory.getInstance().getpJournalActiviteService(true),
                   NhibernateServiceFactory.getInstance().getpDerogationCentreService(true),
                   NhibernateServiceFactory.getInstance().getpRapportExceptionCLService(true)
               )
        {
        }

        private readonly ILog log = LogManager.GetLogger(typeof(CentreController));

        //
        // GET: /Centre/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Centre/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: /Centre/ListeSansCertificat
        public ActionResult ListeSansCertificat(rCategorieVehicule.CategorieEnum categorie)
        {
            try
            {
                IList<pCentreEncour> liste = this._pCentreEnCourservice.findAllAgreeSansCertificatSSL(categorie);
                ListeSansCertificatModel model = new ListeSansCertificatModel() { Liste = liste, Categorie = categorie };

                return View(model);
            }
            catch (Exception e)
            {
                //Gestion de l'erreur
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        //
        // GET: /Centre/Recherche
        public ActionResult Recherche()
        {
            try
            {
                return View();
            }
            catch (Exception e)
            {
                //Gestion de l'erreur
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        //
        // POST: /Centre/Recherche
        [HttpPost]
        public ActionResult Recherche(RechercheCentreModel form)
        {
            try
            {
                validateRechercheCentre(form);
                if (ModelState.IsValid)
                {

                    //Critère de recherche
                    Query q = getRechercheCentreQuery(form);

                    //2 Execution de la recherche
                    //20/08/2021 PLE ajout d'un alias pour forcer fetch de l'entité "PSanctionCentres"
                    //IList<pCentreEncour> resultat = this._pCentreEnCourservice.FindBy(q);
                    IList<pCentreEncour> resultat = this._pCentreEnCourservice.FindBy(q, "PSanctionCentres");
                    form.Resultat = resultat;

                    return View(form);
                }
                //Traitement des erreurs car si Ok redirection plus haut
                form.Resultat = null;

                return View(form);

            }
            catch (Exception e)
            {
                //Gestion de l'erreur
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        //EG le 07/12/2011 pour harmonisation avec contrôleur et ajout du cumul de critère pour les états de l'agrément
        private Query getRechercheCentreQuery(RechercheCentreModel form)
        {
            //1 Constitution de la requette pour le certificatService
            Query q = new Query();
            Criteria c;
            if (!string.IsNullOrEmpty(form.RaisonSociale))
            {
                //cf le nom de la propriete du businessObject
                c = new Criteria("RaisonSociale", form.RaisonSociale.ToUpper() + "%", CriteriaOperator.LIKE);
                q.Criteria.Add(c);
            }

            if (!string.IsNullOrEmpty(form.NumSiren))
            {
                //cf le nom de la propriete du businessObject
                c = new Criteria("NumSiren", form.NumSiren + "%", CriteriaOperator.LIKE);
                q.Criteria.Add(c);
            }

            if (!string.IsNullOrEmpty(form.CodePostal))
            {
                //cf le nom de la propriete du businessObject
                c = new Criteria("CpCentre", form.CodePostal + "%", CriteriaOperator.LIKE);
                q.Criteria.Add(c);
            }

            if (!string.IsNullOrEmpty(form.Ville))
            {
                //cf le nom de la propriete du businessObject
                c = new Criteria("Ville", form.Ville.ToUpper() + "%", CriteriaOperator.LIKE);
                q.Criteria.Add(c);
            }

            if (!string.IsNullOrEmpty(form.NumAgrementVl))
            {
                //cf le nom de la propriete du businessObject
                c = new Criteria("CodAgremVl", form.NumAgrementVl.ToUpper() + "%", CriteriaOperator.LIKE);
                q.Criteria.Add(c);
            }

            if (!string.IsNullOrEmpty(form.NumAgrementPl))
            {
                //cf le nom de la propriete du businessObject
                c = new Criteria("CodAgremPl", form.NumAgrementPl.ToUpper() + "%", CriteriaOperator.LIKE);
                q.Criteria.Add(c);
            }

            if (!string.IsNullOrEmpty(form.NumAgrementCl))
            {
                //cf le nom de la propriete du businessObject
                c = new Criteria("CodAgremCl", form.NumAgrementCl.ToUpper() + "%", CriteriaOperator.LIKE);
                q.Criteria.Add(c);
            }

            //EG le 28/02/2012
            if (!string.IsNullOrEmpty(form.AncienNumAgrementVl))
            {
                //cf le nom de la propriete du businessObject
                c = new Criteria("AncienNumeroAgrementVl", form.AncienNumAgrementVl.ToUpper() + "%", CriteriaOperator.LIKE);
                q.Criteria.Add(c);
            }

            if (!string.IsNullOrEmpty(form.AncienNumAgrementPl))
            {
                //cf le nom de la propriete du businessObject
                c = new Criteria("AncienNumeroAgrementPl", form.AncienNumAgrementPl.ToUpper() + "%", CriteriaOperator.LIKE);
                q.Criteria.Add(c);
            }

            if (!string.IsNullOrEmpty(form.AncienNumAgrementCl))
            {
                //cf le nom de la propriete du businessObject
                c = new Criteria("AncienNumeroAgrementCl", form.AncienNumAgrementCl.ToUpper() + "%", CriteriaOperator.LIKE);
                q.Criteria.Add(c);
            }

            // VL

            /*  ETAT agrement
          *  0 : AGREMENT_KO : le contrôleur n'a pas d'agrément valide. -->annulation de l'agrément précédent ou retrait définitif.
             1 : AGREMENT_OK : La préfecture a notifié l'agrément du contrôleur .
             2 : AGREMENT_DEMANDE_EN COURS : L'agrément est en cours de validation par la prefecture.
             3 : AGREMENT_REJETE : La demande d'agrément a été rejeté par la préfecture. 
         */
            /* VL */
            string etatAgrementVlIN = string.Empty;
            if (form.IsAgrementVlAnnule)
            {
                etatAgrementVlIN = "0";
                c = new Criteria("CodAgremVl", null, CriteriaOperator.IS_NOT_NULL);
                q.Criteria.Add(c);
            }

            if (form.IsAgrementVlValide)
            {
                if (string.IsNullOrEmpty(etatAgrementVlIN))
                    etatAgrementVlIN = "1";
                else
                    etatAgrementVlIN += ",1";
            }

            if (form.IsAgrementVlDemandeEnCours)
            {
                if (string.IsNullOrEmpty(etatAgrementVlIN))
                    etatAgrementVlIN = "2";
                else
                    etatAgrementVlIN += ",2";
            }

            if (form.IsAgrementVlRejete)
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
            if (form.IsAgrementPlAnnule)
            {
                etatAgrementPlIN = "0,6";
                c = new Criteria("CodAgremPl", null, CriteriaOperator.IS_NOT_NULL);
                q.Criteria.Add(c);
            }

            if (form.IsAgrementPlValide)
            {
                if (string.IsNullOrEmpty(etatAgrementPlIN))
                    etatAgrementPlIN = "1,5";
                else
                    etatAgrementPlIN += ",1,5";
            }

            if (form.IsAgrementPlDemandeEnCours)
            {
                if (string.IsNullOrEmpty(etatAgrementPlIN))
                    etatAgrementPlIN = "2,4";
                else
                    etatAgrementPlIN += ",2,4";
            }

            if (form.IsAgrementPlRejete)
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
            if (form.IsAgrementClAnnule)
            {
                etatAgrementClIN = "0";
                c = new Criteria("CodAgremCl", null, CriteriaOperator.IS_NOT_NULL);
                q.Criteria.Add(c);
            }

            if (form.IsAgrementClValide)
            {
                if (string.IsNullOrEmpty(etatAgrementClIN))
                    etatAgrementClIN = "1";
                else
                    etatAgrementClIN += ",1";
            }

            if (form.IsAgrementClDemandeEnCours)
            {
                if (string.IsNullOrEmpty(etatAgrementClIN))
                    etatAgrementClIN = "2";
                else
                    etatAgrementClIN += ",2";
            }

            if (form.IsAgrementClRejete)
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
            //PLE le 23/01/2024
            if (form.AvecExtentionAgrement)
            {
                c = new Criteria("ExtAgremClStatut", "", CriteriaOperator.IS_NOT_NULL);
                q.Criteria.Add(c);
            }

            return q;
        }

        private void validateRechercheCentre(RechercheCentreModel form)
        {
            if (form.RaisonSociale != null && form.RaisonSociale.Length < 3)
            {
                ModelState.AddModelError("RaisonSociale", "Saisissez au moins les 3 premières lettres.");
            }
            if (form.NumSiren != null && form.NumSiren.Length < 9)
            {
                ModelState.AddModelError("NumSiren", "Saisissez les 9 chiffres.");
            }
            if (form.CodePostal != null && form.CodePostal.Length < 2)
            {
                ModelState.AddModelError("CodePostal", "Saisissez au moins les 2 premiers caractères.");
            }
            if (form.Ville != null && form.Ville.Length < 3)
            {
                ModelState.AddModelError("Ville", "Saisissez au moins les 3 premières lettres.");
            }
            if (form.NumAgrementVl != null && form.NumAgrementVl.Length < 4)
            {
                ModelState.AddModelError("NumAgrementVl", "Saisissez au moins les 4 premiers caractères.");
            }
            if (form.NumAgrementPl != null && form.NumAgrementPl.Length < 4)
            {
                ModelState.AddModelError("NumAgrementPl", "Saisissez au moins les 4 premiers caractères.");
            }
            if (form.NumAgrementCl != null && form.NumAgrementCl.Length < 4)
            {
                ModelState.AddModelError("NumAgrementCl", "Saisissez au moins les 4 premiers caractères.");
            }
            if (form.NumAgrementCl != null && !form.NumAgrementCl.StartsWith("L"))
            {
                ModelState.AddModelError("NumAgrementCl", "L'agrément d'un centre de catégorie L commence par la lettre 'L'.");
            }
            if (form.RaisonSociale == null && form.NumSiren == null && form.CodePostal == null && form.Ville == null &&
                form.NumAgrementVl == null && form.NumAgrementPl == null && form.NumAgrementCl == null &&
                //EG corrigée le 29/02/2012
                form.AncienNumAgrementVl == null && form.AncienNumAgrementPl == null && form.AncienNumAgrementCl == null
                /* 
                PLE 21/11/2011 Les cases à cocher sont exclues des critères obligatoires
                && form.IsAgrementVlValide == false && form.IsAgrementVlRejete == false && form.IsAgrementVlAnnule == false && form.IsAgrementVlDemandeEnCours == false &&
                form.IsAgrementPlValide == false && form.IsAgrementPlRejete == false && form.IsAgrementPlAnnule == false && form.IsAgrementPlDemandeEnCours == false &&
                form.IsAgrementClValide == false && form.IsAgrementClRejete == false && form.IsAgrementClAnnule == false && form.IsAgrementClDemandeEnCours == false
                */
                )
            {
                ModelState.AddModelError("Resultat", "Saisissez au moins 1 critère de recherche !");
            }
            //EG le 28/02/2012
            if (form.AncienNumAgrementVl != null && form.AncienNumAgrementVl.Length < 4)
            {
                ModelState.AddModelError("AncienNumAgrementVl", "Saisissez au moins les 4 premiers caractères.");
            }

            if (form.AncienNumAgrementPl != null && form.AncienNumAgrementPl.Length < 4)
            {
                ModelState.AddModelError("AncienNumAgrementPl", "Saisissez au moins les 4 premiers caractères.");
            }

            if (form.AncienNumAgrementCl != null && form.AncienNumAgrementCl.Length < 4)
            {
                ModelState.AddModelError("AncienNumAgrementCl", "Saisissez au moins les 4 premiers caractères.");
            }
            if (form.AncienNumAgrementCl != null && !form.AncienNumAgrementCl.StartsWith("L"))
            {
                ModelState.AddModelError("AncienNumAgrementCl", "L'agrément d'un centre de catégorie L commence par la lettre 'L'.");
            }
        }

        // GET: /Centre/fiche
        public ActionResult Fiche(int id)
        {
            try
            {
                //on supprime la session centre 
                this.Session.Remove("Centre");

                //1 retrieve de l'id
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);

                //09/01/2012
                if (c == null)
                {
                    throw new HttpException(404, "Le centre d'identifiant : " + id + "  n'existe pas");
                }

                //EG le 07/01/2012 pour gérer le cas de la fiche "annulée" remplacée par une fiche plus récente
                pCentreEncour nouveauCentre = this._pCentreEnCourservice.FindNewCentreFor(c);

                FicheModel model = new FicheModel();
                model.Centre = c;
                model.NouveauCentre = nouveauCentre;

                //Affichage de la vue
                return View(model);

            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

        // GET: /Centre/ModifierFiche
        public ActionResult ModifierFiche(int id)
        {
            try
            {
                //1 retrieve de l'id
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);
                //EG le 13/12/2011 pour modification de la ville + code postale et éviter de créer un nouveau model spécifique
                NouveauCentreModel model = new NouveauCentreModel();
                model.Centre = c; //le seul champ nécessaire pour la vue

                //Affichage de la vue
                return View(model);

            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

        // POST: /Centre/ModifierFiche
        [HttpPost]
        public ActionResult ModifierFiche(int id, NouveauCentreModel model)
        {
            try
            {

                if (GestionDroitsExecution.isRightActionForCentre(this.User, model.Centre, GestionDroitsExecution.ActionsFicheCentre.ADMMODFINT))
                {
                    validateFiche(model);
                    if (ModelState.IsValid)
                    {
                        this._pCentreEnCourservice.modifierFiche(id, model.Centre, this.User.Identity.Name);

                        //On affiche la fiche
                        return this.RedirectToAction("Fiche", new { id = id });
                    }
                    return View(model);
                }
                else
                {
                    Exception e = new Exception("Vous n'êtes pas autorisé....");
                    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                }
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        private void validateFiche(NouveauCentreModel model)
        {
            //todo:
        }

        // GET: /Centre/ConfirmationDeleteFiche/60
        public ActionResult ConfirmationDeleteFiche(int id)
        {
            try
            {
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.ADMSUPFI))
                {
                    return View(c);
                }
                else
                {
                    Exception e = new Exception("Vous n'êtes pas autorisé....");
                    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                }
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

        // POST: /Centre/ConfirmationDeleteFiche/60
        [HttpPost]
        public ActionResult ConfirmationDeleteFiche(int id, pCentreEncour centre)
        {
            try
            {
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.ADMMODFI))
                {
                    // on supprime la fiche
                    this._pCentreEnCourservice.deleteFiche(c);

                    //on affiche la recherche des centre
                    return this.RedirectToAction("Recherche");
                }
                else
                {
                    Exception e = new Exception("Vous n'êtes pas autorisé....");
                    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                }
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

        // GET: /Centre/ListeSanctions/62
        public ActionResult ListeSanctions(int id)
        {
            try
            {
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);

                if (GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.SHOW_SANCT))
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

        // GET: /Centre/ListeAmendes/62
        public ActionResult ListeAmendes(int id)
        {
            try
            {
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);

                if (GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.SHOW_AMEND))
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

        // GET: /Centre/ListeHistorique/62
        public ActionResult ListeHistorique(int id)
        {
            try
            {
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);

                if (GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.SHOW_HISTO))
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

        // GET: /Centre/ModificationDenominationSociale/62
        public ActionResult ModificationDenominationSociale(int id)
        {
            try
            {
                //1 on retrieve l'id
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);

                if (GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.CHDECOM))
                {
                    ModificationDenominationSocialeModel m = new ModificationDenominationSocialeModel();
                    m.Centre = c;
                    m.Centre.DateEffetModification = null;
                    return View(m);
                }
                else
                {
                    Exception e = new Exception("Vous n'êtes pas autorisé....");
                    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                }

            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // POST: /Centre/ModificationDenominationSociale
        [HttpPost]
        public ActionResult ModificationDenominationSociale(int id, ModificationDenominationSocialeModel m)
        {
            try
            {
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.CHDECOM))
                {
                    validateRaisonSociale(m);
                    if (ModelState.IsValid)
                    {
                        //DateTime dt = Convert.ToDateTime(dateEffetModification);
                        this._pCentreEnCourservice.modifyDenominationSociale(id, m.Centre, this.User.Identity.Name);

                        if (c.AunAgrementValide)
                        {
                            //On affiche l'écran de confirmation génération du pdf
                            return this.RedirectToAction("ChoixTelechargerModifDenominationCommercialePDF", new { id = id });
                        }
                        return this.RedirectToAction("Fiche", new { id = id });
                    }
                    //Traitement des erreurs car si Ok redirection plus haut
                    return View(m);
                }
                else
                {
                    Exception e = new Exception("Vous n'êtes pas autorisé....");
                    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                }
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        private void validateRaisonSociale(ModificationDenominationSocialeModel m)
        {
            if (m.Centre.RaisonSociale == null)
                ModelState.AddModelError("Centre.RaisonSociale", "La raison sociale doit être renseignée.");
            if (m.Centre.RaisonSociale == "")
                ModelState.AddModelError("Centre.RaisonSociale", "La raison sociale doit être renseignée.");
            if (m.Centre.DateEffetModification == null)
                ModelState.AddModelError("Centre.DateEffetModification", "La date d'effet de la modification doit être renseignée.");
        }

        // GET: /Centre/ModificationRepresentantLegal/62
        public ActionResult ModificationRepresentantLegal(int id)
        {
            try
            {
                // on retrieve l'id
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.CHREPLEG))
                {
                    return View(c);
                }
                else
                {
                    Exception e = new Exception("Vous n'êtes pas autorisé....");
                    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                }
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // POST: /Centre/ModificationRepresentantLegal
        [HttpPost]
        public ActionResult ModificationRepresentantLegal(int id, string nomrepresentantlegal)
        {
            try
            {
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.CHREPLEG))
                {
                    this._pCentreEnCourservice.modifyRepresentantLegal(id, nomrepresentantlegal, this.User.Identity.Name);

                    return this.RedirectToAction("Fiche", new { id = id });
                }
                else
                {
                    Exception e = new Exception("Vous n'êtes pas autorisé....");
                    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                }
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // GET: /Centre/AutreModification/62
        public ActionResult AutreModification(int id)
        {
            try
            {
                // on retrieve l'id
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.MODAUTRE))
                {
                    AutreModificationModel m = new AutreModificationModel();
                    m.Centre = c;
                    if (c.RStatutJuridique != null && rStatutJuridique.Statuts.ADMIN.ToString().Equals(c.RStatutJuridique.Id))
                    {
                        m.ListeStatutJuridique = new List<rStatutJuridique>();
                        m.ListeStatutJuridique.Add(this._rStatutJuridiqueService.FindBy(rStatutJuridique.Statuts.ADMIN.ToString()));
                    }
                    else
                    {
                        //on rempli la liste statut juridique
                        m.ListeStatutJuridique = this._rStatutJuridiqueService.FindAll();
                        m.ListeStatutJuridique.Insert(0, new rStatutJuridique() { Id = "-1", Libelle = " " });
                        //pour les centres non administatifs supprimer "Centre de l'Administration" de la liste déroulante
                        m.ListeStatutJuridique.RemoveAt(m.ListeStatutJuridique.IndexOf(this._rStatutJuridiqueService.FindBy(rStatutJuridique.Statuts.ADMIN.ToString())));
                    }

                    return View(m);
                }
                else
                {
                    Exception e = new Exception("Vous n'êtes pas autorisé....");
                    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                }
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // POST: /Centre/AutreModification
        [HttpPost]
        public ActionResult AutreModification(int id, AutreModificationModel m)
        {
            try
            {
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.MODAUTRE))
                {
                    this._pCentreEnCourservice.modifyAutreModification(id, m.Centre.NumSiren, m.Centre.NomRepresentantLegal, m.Centre.EnseigneCommerciale, m.Centre.NumTel, m.Centre.RStatutJuridique, this.User.Identity.Name);

                    return this.RedirectToAction("Fiche", new { id = id });
                }
                else
                {
                    Exception e = new Exception("Vous n'êtes pas autorisé....");
                    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                }
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // GET: /Centre/AuteurModification/62
        public ActionResult AuteurModification(int id)
        {
            try
            {
                // on retrieve l'id
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);

                return View(c);
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // GET: /Centre/ModificationRetourArriere/62
        public ActionResult ConfirmationModificationRetourArriere(int id, bool deleteHisto)
        {
            try
            {
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.RETOUR))
                {
                    RetourArriereModel m = new RetourArriereModel();
                    m.Centre = c;

                    //dans le cas des centres indépendants pour un retour arrière suite à notification
                    //ne pas autoriser le retour arrière si l'utilisateur s'est déjà connecté
                    if (
                        ((c.RMotifModification.Id.Equals(Convert.ToString(rMotifModification.TypeModification.NOTVL)) ||
                          c.RMotifModification.Id.Equals(Convert.ToString(rMotifModification.TypeModification.CHEXPLVL)) ||
                          c.RMotifModification.Id.Equals(Convert.ToString(rMotifModification.TypeModification.CHLOCAVL)) ||
                          c.RMotifModification.Id.Equals(Convert.ToString(rMotifModification.TypeModification.CHRATTVL)))
                          && c.ReseauRattachementVL.Id.Equals(Convert.ToString(rReseau.IdCodeReseau.Z))) ||
                        ((c.RMotifModification.Id.Equals(Convert.ToString(rMotifModification.TypeModification.NOTPL)) ||
                          c.RMotifModification.Id.Equals(Convert.ToString(rMotifModification.TypeModification.CHEXPLPL)) ||
                          c.RMotifModification.Id.Equals(Convert.ToString(rMotifModification.TypeModification.CHLOCAPL)) ||
                          c.RMotifModification.Id.Equals(Convert.ToString(rMotifModification.TypeModification.CHRATTPL)))
                        && c.ReseauRattachementPL.Id.Equals(Convert.ToString(rReseau.IdCodeReseau.Z))) ||
                        ((c.RMotifModification.Id.Equals(Convert.ToString(rMotifModification.TypeModification.NOTCL)) ||
                          c.RMotifModification.Id.Equals(Convert.ToString(rMotifModification.TypeModification.CHEXPLCL)) ||
                          c.RMotifModification.Id.Equals(Convert.ToString(rMotifModification.TypeModification.CHLOCACL)) ||
                          c.RMotifModification.Id.Equals(Convert.ToString(rMotifModification.TypeModification.CHRATTCL)))
                        && c.ReseauRattachementCL.Id.Equals(Convert.ToString(rReseau.IdCodeReseau.Z)))
                       )
                    {
                        rUtilisateur u = new rUtilisateur();
                        switch (c.RMotifModification.Id)
                        {
                            case "NOTVL":
                            case "CHEXPLVL":
                            case "CHLOCAVL":
                            case "CHRATTVL":
                                u = this._rUtilisateurService.FindBy(c.CodAgremVl.ToLower());
                                break;
                            case "NOTPL":
                            case "CHEXPLPL":
                            case "CHLOCAPL":
                            case "CHRATTPL":
                                u = this._rUtilisateurService.FindBy(c.CodAgremPl.ToLower());
                                break;
                            case "NOTCL":
                            case "CHEXPLCL":
                            case "CHLOCACL":
                            case "CHRATTCL":
                                //PLE le 21/02/2024 login utilisateur CL = codeagrément CL
                                //u = this._rUtilisateurService.FindBy(c.CodAgremCl.ToLower());
                                u = this._rUtilisateurService.FindBy(c.CodAgremCl);
                                break;
                        }

                        //PLE le 05/06/2012 si le compte utilisateur a été supprimé manuellement
                        //if (u==null)
                        //{
                        //    m.deleteHisto = deleteHisto;
                        //    return View(m);
                        //}

                        if (u != null)
                        {
                            if (u.MotDePasseTemporaire == null)
                            {
                                return View("ConfirmationModificationRetourArriereErreur", m);
                            }
                        }
                    }

                    //PLE le 06/05/2015 Détection si les contrôleurs rattachés sont valides
                    switch (c.RMotifModification.Id)
                    {
                        case "NOTVL":
                        case "CHEXPLVL":
                        case "CHLOCAVL":
                        case "CHRATTVL":
                            if (c.ExisteControleurValide(rCategorieVehicule.CategorieEnum.VL))
                            {
                                return View("ConfirmationModificationRetourArriereErreurCntlrRattache", m);
                            }
                            break;
                        case "NOTPL":
                        case "CHEXPLPL":
                        case "CHLOCAPL":
                        case "CHRATTPL":
                            if (c.ExisteControleurValide(rCategorieVehicule.CategorieEnum.PL))
                            {
                                return View("ConfirmationModificationRetourArriereErreurCntlrRattache", m);
                            }
                            break;
                        case "NOTCL":
                        case "CHEXPLCL":
                        case "CHLOCACL":
                        case "CHRATTCL":
                            if (c.ExisteControleurValide(rCategorieVehicule.CategorieEnum.CL))
                            {
                                return View("ConfirmationModificationRetourArriereErreurCntlrRattache", m);
                            }
                            break;
                    }

                    m.deleteHisto = deleteHisto;
                    return View(m);
                }
                else
                {
                    Exception e = new Exception("Vous n'êtes pas autorisé....");
                    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                }
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // POST: /Centre/ModificationRetourArriere
        [HttpPost]
        public ActionResult ModificationRetourArriere(RetourArriereModel m)
        {
            try
            {
                pCentreEncour c = this._pCentreEnCourservice.FindBy(m.Centre.Id);
                //PLE 28/03/2012 le centre n'a pas forcement de centre précédent !
                int idCentrePrecedent = 0;
                if (c.PCentreEncourPrecedent != null)
                    idCentrePrecedent = c.PCentreEncourPrecedent.Id;

                if (GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.RETOUR))
                {
                    bool userMode = this.User.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) == false;
                    this._pCentreEnCourservice.retourArriere(m.Centre.Id, userMode, this.User.Identity.Name, m.deleteHisto);
                    //this.User.Identity.Name
                    //on recherche le centre. S'il n'existe pas on est sur un centre supprimé par le retour arrière
                    //il faut donc affiché l'ancien centre
                    c = this._pCentreEnCourservice.FindBy(m.Centre.Id);
                    if (c == null)
                    {
                        return RedirectToAction("Fiche", new { id = idCentrePrecedent });
                    }
                    else
                    {
                        return RedirectToAction("Fiche", new { id = m.Centre.Id });
                    }
                }
                else
                {
                    Exception e = new Exception("Vous n'êtes pas autorisé....");
                    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                }

            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // GET: /Centre/NotificationAgrement/62
        public ActionResult NotificationAgrement(int id, rCategorieVehicule.CategorieEnum categVehicule)
        {
            try
            {
                //1 on retrieve l'id
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);
                if (
                   ((GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.NOTVL)) && (categVehicule.ToString() == "VL"))
                || ((GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.NOTPL)) && (categVehicule.ToString() == "PL"))
                || ((GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.NOTRPL)) && (categVehicule.ToString() == "PL"))
                || ((GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.NOTCL)) && (categVehicule.ToString() == "CL"))
                   )
                {
                    NotificationAgrementModel d = new NotificationAgrementModel();
                    d.Centre = c;
                    d.CategVehicule = categVehicule;
                    d.DateValide = false;

                    switch (categVehicule)
                    {
                        case rCategorieVehicule.CategorieEnum.VL:
                            d.Centre.DateEffetNotificationVl = null;
                            d.Centre.SvaVL = false;
                            break;
                        case rCategorieVehicule.CategorieEnum.PL:
                            d.Centre.DateEffetNotificationPl = null;
                            d.Centre.DateEcheanceAgrementPl = null;
                            d.Centre.SvaPL = false;
                            break;
                        case rCategorieVehicule.CategorieEnum.CL:
                            d.Centre.DateEffetNotificationCl = null;
                            d.Centre.SvaCL = false;
                            break;
                    }

                    return View(d);
                }
                else
                {
                    Exception e = new Exception("Vous n'êtes pas autorisé....");
                    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                }
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // POST: /Centre/NotificationAgrement
        [HttpPost]
        public ActionResult NotificationAgrement(int id, NotificationAgrementModel m)
        {
            try
            {
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);
                if (
                   ((GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.NOTVL)) && (m.CategVehicule.ToString() == "VL"))
                || ((GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.NOTPL)) && (m.CategVehicule.ToString() == "PL"))
                || ((GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.NOTRPL)) && (m.CategVehicule.ToString() == "PL"))
                || ((GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.NOTCL)) && (m.CategVehicule.ToString() == "CL"))
                   )
                {
                    validateNotificationAgrement(m);
                    if (ModelState.IsValid)
                    {
                        if (m.DateValide == false)
                        {
                            switch (m.CategVehicule)
                            {
                                case rCategorieVehicule.CategorieEnum.VL:
                                    if (m.Centre.DateEffetNotificationVl < DateTime.Now.AddDays(1))
                                        return View("ConfirmationDateNotification", m);
                                    break;
                                case rCategorieVehicule.CategorieEnum.PL:
                                    if (m.Centre.DateEffetNotificationPl < DateTime.Now.AddDays(1))
                                        return View("ConfirmationDateNotification", m);
                                    break;
                                case rCategorieVehicule.CategorieEnum.CL:
                                    if (m.Centre.DateEffetNotificationCl < DateTime.Now.AddDays(1))
                                        return View("ConfirmationDateNotification", m);
                                    break;
                            }
                        }

                        //Génération du n° d'agrément et création d'un utilisateur tenporaire pour les centres indépendants
                        this._pCentreEnCourservice.notificationAgrement(id, m.Centre, m.CategVehicule, this.User.Identity.Name);

                        //On affiche l'écran de confirmation génération du pdf
                        return this.RedirectToAction("ResultatNotifierAgrement", new { id = id, categorie = m.CategVehicule });

                        //return this.RedirectToAction("Fiche", new { id = id });

                    }
                    //Traitement des erreurs car si Ok redirection plus haut
                    return View(m);
                }
                else
                {
                    Exception e = new Exception("Vous n'êtes pas autorisé....");
                    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                }
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // POST: /Centre/NotificationAgrementSansVerification
        [HttpPost]
        public ActionResult NotificationAgrementSansVerification(int id, NotificationAgrementModel m)
        {
            try
            {
                validateNotificationAgrement(m);
                if (ModelState.IsValid)
                {
                    //Génération du n° d'agrément
                    this._pCentreEnCourservice.notificationAgrement(id, m.Centre, m.CategVehicule, this.User.Identity.Name);

                    //On affiche l'écran de confirmation génération du pdf
                    return this.RedirectToAction("ResultatNotifierAgrement", new { id = id, categorie = m.CategVehicule });

                    //return this.RedirectToAction("Fiche", new { id = id });

                }
                //Traitement des erreurs car si Ok redirection plus haut
                return View("NotificationAgrement", m);
                //return View("NotificationAgrement", new { id = m.Centre.Id, categVehicule = m.CategVehicule });
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        private void validateNotificationAgrement(NotificationAgrementModel m)
        {
            switch (m.CategVehicule)
            {
                case rCategorieVehicule.CategorieEnum.VL:
                    if (m.Centre.DateEffetNotificationVl == null)
                    {
                        ModelState.AddModelError("centre.DateEffetNotificationVl", "La date d'effet de l'agrément doit être renseignée.");
                    }
                    else
                    {
                        //vérification de l'antériorité de la date
                        if (m.Centre.DateEffetNotificationVl > System.DateTime.Now)
                        {
                            m.DateValide = true;
                        }
                    }
                    break;

                case rCategorieVehicule.CategorieEnum.PL:
                    if (m.Centre.DateEffetNotificationPl == null)
                    {
                        ModelState.AddModelError("centre.DateEffetNotificationPl", "La date d'effet de l'agrément doit être renseignée.");
                    }
                    else
                    {
                        //vérification de l'antériorité de la date
                        if (m.Centre.DateEffetNotificationPl > System.DateTime.Now)
                        {
                            m.DateValide = true;
                        }
                    }

                    if (m.Centre.TypeCentre == "A" && m.Centre.DateEcheanceAgrementPl == null)
                    {
                        ModelState.AddModelError("centre.DateEcheanceAgrementPl", "La date d'échéance de l'agrément doit être renseignée.");
                    }
                    break;

                case rCategorieVehicule.CategorieEnum.CL:
                    if (m.Centre.DateEffetNotificationCl == null)
                    {
                        ModelState.AddModelError("centre.DateEffetNotificationCl", "La date d'effet de l'agrément doit être renseignée.");
                    }
                    else if (m.Centre.DateEffetNotificationCl < new DateTime(2024, 04, 15))
                    {
                        ModelState.AddModelError("centre.DateEffetNotificationCl", "La date d'effet de l'agrément doit être postérieure ou égale au 15/04/2024.");
                    }
                    else
                    {
                        //vérification de l'antériorité de la date
                        if (m.Centre.DateEffetNotificationCl > System.DateTime.Now)
                        {
                            m.DateValide = true;
                        }
                    }
                    break;

            }
        }

        // GET: /Centre/ConfirmationDateNotification
        public ActionResult ConfirmationDateNotification(NotificationAgrementModel m)
        {
            try
            {
                return View(m);
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // GET: /Centre/ResultatNotifierAgrement
        public ActionResult ResultatNotifierAgrement(int id, rCategorieVehicule.CategorieEnum categorie)
        {
            try
            {
                //1 on retrieve l'id
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);

                NotificationAgrementModel m = new NotificationAgrementModel();
                m.Centre = c;
                m.CategVehicule = categorie;

                return View(m);
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        public ActionResult TelechargerNotificationSpecialisePDF(int id, rCategorieVehicule.CategorieEnum categVehicule)
        {
            try
            {
                //On retrieve l'id
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);

                pCentreHisto historique = null;
                //si l'action de la fiche ne permet pas d'éditer le notification, rechercher dans l'historique la dernière action permettant d'éditer la notification
                /* EG le 05/03/2012
                if (
                    (c.RMotifModification.Id != Convert.ToString(rMotifModification.TypeModification.NOTVL) && categVehicule == rCategorieVehicule.CategorieEnum.VL) &&
                    (c.RMotifModification.Id != Convert.ToString(rMotifModification.TypeModification.CHEXPLVL) && categVehicule == rCategorieVehicule.CategorieEnum.VL) &&
                    (c.RMotifModification.Id != Convert.ToString(rMotifModification.TypeModification.CHLOCAVL) && categVehicule == rCategorieVehicule.CategorieEnum.VL) &&
                    (c.RMotifModification.Id != Convert.ToString(rMotifModification.TypeModification.CHRATTVL) && categVehicule == rCategorieVehicule.CategorieEnum.VL) &&
                    (c.RMotifModification.Id != Convert.ToString(rMotifModification.TypeModification.NOTPL) && categVehicule == rCategorieVehicule.CategorieEnum.PL) &&
                    (c.RMotifModification.Id != Convert.ToString(rMotifModification.TypeModification.CHEXPLPL) && categVehicule == rCategorieVehicule.CategorieEnum.PL) &&
                    (c.RMotifModification.Id != Convert.ToString(rMotifModification.TypeModification.CHLOCAPL) && categVehicule == rCategorieVehicule.CategorieEnum.PL) &&
                    (c.RMotifModification.Id != Convert.ToString(rMotifModification.TypeModification.CHRATTPL) && categVehicule == rCategorieVehicule.CategorieEnum.PL) &&
                    (c.RMotifModification.Id != Convert.ToString(rMotifModification.TypeModification.NOTRPL) && categVehicule == rCategorieVehicule.CategorieEnum.PL) &&
                    (c.RMotifModification.Id != Convert.ToString(rMotifModification.TypeModification.REJRPL) && categVehicule == rCategorieVehicule.CategorieEnum.PL) &&
                    (c.RMotifModification.Id != Convert.ToString(rMotifModification.TypeModification.NOTCL) && categVehicule == rCategorieVehicule.CategorieEnum.CL) &&
                    (c.RMotifModification.Id != Convert.ToString(rMotifModification.TypeModification.CHEXPLCL) && categVehicule == rCategorieVehicule.CategorieEnum.CL) &&
                    (c.RMotifModification.Id != Convert.ToString(rMotifModification.TypeModification.CHLOCACL) && categVehicule == rCategorieVehicule.CategorieEnum.CL) &&
                    (c.RMotifModification.Id != Convert.ToString(rMotifModification.TypeModification.CHRATTCL) && categVehicule == rCategorieVehicule.CategorieEnum.CL)
                   )
                */
                if (!verifModifForPDF(c.RMotifModification, categVehicule))
                {
                    //rechercher dans l'historique la dernière action permettant d'éditer la notification
                    foreach (pCentreHisto h in c.PCentreHisto.OrderByDescending(a => a.DateHisto))
                    {
                        /*if (
                            (h.RMotifModification.Id == Convert.ToString(rMotifModification.TypeModification.NOTVL) && categVehicule == rCategorieVehicule.CategorieEnum.VL) ||
                            (h.RMotifModification.Id == Convert.ToString(rMotifModification.TypeModification.CHEXPLVL) && categVehicule == rCategorieVehicule.CategorieEnum.VL) ||
                            (h.RMotifModification.Id == Convert.ToString(rMotifModification.TypeModification.CHLOCAVL) && categVehicule == rCategorieVehicule.CategorieEnum.VL) ||
                            (h.RMotifModification.Id == Convert.ToString(rMotifModification.TypeModification.CHRATTVL) && categVehicule == rCategorieVehicule.CategorieEnum.VL) ||
                            (h.RMotifModification.Id == Convert.ToString(rMotifModification.TypeModification.NOTPL) && categVehicule == rCategorieVehicule.CategorieEnum.PL) ||
                            (h.RMotifModification.Id == Convert.ToString(rMotifModification.TypeModification.CHEXPLPL) && categVehicule == rCategorieVehicule.CategorieEnum.PL) ||
                            (h.RMotifModification.Id == Convert.ToString(rMotifModification.TypeModification.CHLOCAPL) && categVehicule == rCategorieVehicule.CategorieEnum.PL) ||
                            (h.RMotifModification.Id == Convert.ToString(rMotifModification.TypeModification.CHRATTPL) && categVehicule == rCategorieVehicule.CategorieEnum.PL) ||
                            (h.RMotifModification.Id == Convert.ToString(rMotifModification.TypeModification.NOTRPL) && categVehicule == rCategorieVehicule.CategorieEnum.PL) ||
                            (h.RMotifModification.Id == Convert.ToString(rMotifModification.TypeModification.REJRPL) && categVehicule == rCategorieVehicule.CategorieEnum.PL) ||
                            (h.RMotifModification.Id == Convert.ToString(rMotifModification.TypeModification.NOTCL) && categVehicule == rCategorieVehicule.CategorieEnum.CL) ||
                            (h.RMotifModification.Id == Convert.ToString(rMotifModification.TypeModification.CHEXPLCL) && categVehicule == rCategorieVehicule.CategorieEnum.CL) ||
                            (h.RMotifModification.Id == Convert.ToString(rMotifModification.TypeModification.CHLOCACL) && categVehicule == rCategorieVehicule.CategorieEnum.CL) ||
                            (h.RMotifModification.Id == Convert.ToString(rMotifModification.TypeModification.CHRATTCL) && categVehicule == rCategorieVehicule.CategorieEnum.CL)
                           )*/
                        if (verifModifForPDF(h.RMotifModification, categVehicule))
                        {
                            historique = h;
                            break;
                        }
                    }
                    if (historique == null)
                    {
                        NotificationAgrementModel m = new NotificationAgrementModel();
                        m.Centre = c;
                        m.CategVehicule = categVehicule;
                        return View("ErreurRechercheNouveauCentre", m);
                    }
                }

                rMotifModification motif = ((historique != null) ? historique.RMotifModification : c.RMotifModification);

                //On charge le document Crystal
                ReportDocument boReportDocument = new ReportDocument();
                boReportDocument.Load(Server.MapPath("~/bin/reporting/cntr_notification_decision_agrem_init_annexe2-A.rpt"));

                cntr_notification_decision_agrem_init_annexe2_A ds = new cntr_notification_decision_agrem_init_annexe2_A();
                cntr_notification_decision_agrem_init_annexe2_A.notification_centreDataTable dt = ds.notification_centre;
                cntr_notification_decision_agrem_init_annexe2_A.notification_centreRow dr = dt.Newnotification_centreRow();

                //On cré un DataSet
                dr.est_type_agrem_vl = false;
                dr.est_type_agrem_pl = false;
                dr.est_type_agrem_cl = false;
                dr.est_centre_independant = false;
                dr.num_accuse_reception = null;
                //PLE le 01/03/2024 Ajout de la date de notification
                dr.date_edition = Convert.ToDateTime((historique != null ? historique.DateHisto : c.DateMiseaJour));

                if (motif.Id.EndsWith("VL"))
                {
                    dr.est_type_agrem_vl = true;
                    dr.est_type_agrem_pl = false;
                    dr.est_type_agrem_cl = false;
                    dr.est_centre_independant = ((historique != null) ? (historique.ReseauRattachementVL.Id == "Z" ? true : false) : (c.ReseauRattachementVL.Id == "Z") ? true : false);
                    dr.num_accuse_reception = (historique != null ? historique.NumAccuseReceptionVL : c.NumAccuseReceptionVL);
                    //10/04/2025 PLE Ajout SVA
                    dr.est_sva = c.SvaVL;
                }
                if (motif.Id.EndsWith("PL"))
                {
                    dr.est_type_agrem_vl = false;
                    dr.est_type_agrem_pl = true;
                    dr.est_type_agrem_cl = false;
                    dr.est_centre_independant = ((historique != null) ? (historique.ReseauRattachementPL.Id == "Z" ? true : false) : (c.ReseauRattachementPL.Id == "Z") ? true : false);
                    dr.num_accuse_reception = (historique != null ? historique.NumAccuseReceptionPL : c.NumAccuseReceptionPL);
                    //10/04/2025 PLE Ajout SVA
                    dr.est_sva = c.SvaPL;
                }
                if (motif.Id.EndsWith("CL"))
                {
                    dr.est_type_agrem_vl = false;
                    dr.est_type_agrem_pl = false;
                    dr.est_type_agrem_cl = true;
                    dr.est_centre_independant = ((historique != null) ? (historique.ReseauRattachementCL.Id == "Z" ? true : false) : (c.ReseauRattachementCL.Id == "Z") ? true : false);
                    dr.num_accuse_reception = (historique != null ? historique.NumAccuseReceptionCL : c.NumAccuseReceptionCL);
                    //10/04/2025 PLE Ajout SVA
                    dr.est_sva = c.SvaCL;
                }

                //recharger les données de l'utilisateur temporaire qui vient d'être créé
                rUtilisateur uTemp = null;
                //todo verifier centre independant pur la recherche utilisateur
                if (categVehicule == rCategorieVehicule.CategorieEnum.VL)
                {
                    uTemp = _rUtilisateurService.FindBy((historique != null ? historique.CodAgremVl.ToLower() : c.CodAgremVl.ToLower()));
                    if (motif.Id.StartsWith("CH"))
                    {
                        dr.ancien_numero_agrem = (historique != null ? historique.AncienNumeroAgrementVl : c.AncienNumeroAgrementVl);
                    }
                    dr.cod_agrem = (historique != null ? historique.CodAgremVl : c.CodAgremVl);
                    dr.cod_reseau = (historique != null ? historique.ReseauRattachementVL.Id : c.ReseauRattachementVL.Id);
                    dr.libelle_reseau = (historique != null ? historique.ReseauRattachementVL.Nom : c.ReseauRattachementVL.Nom);
                    dr.date_effet_notification = Convert.ToDateTime((historique != null ? historique.DateEffetNotificationVl : c.DateEffetNotificationVl));
                    //PLE : 30/03/2012 Ajout des login de connexion à l'espace privé VL www.utac-otc.com
                    dr.identifiant_otc = ConfigurationManager.AppSettings["identifiantOTCVL"];
                    dr.pwd_otc = ConfigurationManager.AppSettings["pwdOTCVL"];

                }
                if (categVehicule == rCategorieVehicule.CategorieEnum.PL)
                {
                    uTemp = _rUtilisateurService.FindBy((historique != null ? historique.CodAgremPl.ToLower() : c.CodAgremPl.ToLower()));
                    if (motif.Id.StartsWith("CH"))
                    {
                        dr.ancien_numero_agrem = (historique != null ? historique.AncienNumeroAgrementPl : c.AncienNumeroAgrementPl);
                    }
                    dr.cod_agrem = (historique != null ? historique.CodAgremPl : c.CodAgremPl);
                    dr.cod_reseau = (historique != null ? historique.ReseauRattachementPL.Id : c.ReseauRattachementPL.Id);
                    dr.libelle_reseau = (historique != null ? historique.ReseauRattachementPL.Nom : c.ReseauRattachementPL.Nom);
                    dr.date_effet_notification = Convert.ToDateTime((historique != null ? historique.DateEffetNotificationPl : c.DateEffetNotificationPl));
                    //PLE : 30/03/2012 Ajout des login de connexion à l'espace privé VL www.utac-otc.com
                    dr.identifiant_otc = ConfigurationManager.AppSettings["identifiantOTCPL"];
                    dr.pwd_otc = ConfigurationManager.AppSettings["pwdOTCPL"];
                }
                if (categVehicule == rCategorieVehicule.CategorieEnum.CL)
                {
                    //PLE le 21/02/2024 supression du ToLower pour les utilisateurs Catégorie L
                    uTemp = _rUtilisateurService.FindBy((historique != null ? historique.CodAgremCl : c.CodAgremCl));
                    if (motif.Id.StartsWith("CH"))
                    {
                        dr.ancien_numero_agrem = (historique != null ? historique.AncienNumeroAgrementCl : c.AncienNumeroAgrementCl);
                    }
                    dr.cod_agrem = (historique != null ? historique.CodAgremCl : c.CodAgremCl);
                    dr.cod_reseau = (historique != null ? historique.ReseauRattachementCL.Id : c.ReseauRattachementCL.Id);
                    dr.libelle_reseau = (historique != null ? historique.ReseauRattachementCL.Nom : c.ReseauRattachementCL.Nom);
                    dr.date_effet_notification = Convert.ToDateTime((historique != null ? historique.DateEffetNotificationCl : c.DateEffetNotificationCl));
                    //PLE : 30/03/2012 Ajout des login de connexion à l'espace privé CL www.utac-otc.com
                    dr.identifiant_otc = ConfigurationManager.AppSettings["identifiantOTCCL"];
                    dr.pwd_otc = ConfigurationManager.AppSettings["pwdOTCCL"];
                }
                dr.titre_departement = "Préfet " + (historique != null ? historique.RPrefecture.Prefixe.Replace("/", "") : c.RPrefecture.Prefixe.Replace("/", "")) + (historique != null ? historique.RPrefecture.LibellePrefecture : c.RPrefecture.LibellePrefecture);
                dr.libelle_prefet = (historique != null ? historique.RPrefecture.Prefixe.Replace("/", "") : c.RPrefecture.Prefixe.Replace("/", "")) + (historique != null ? historique.RPrefecture.LibellePrefecture : c.RPrefecture.LibellePrefecture);
                dr.raison_sociale = (historique != null ? historique.RaisonSociale : c.RaisonSociale);
                dr.l1_adresse = (historique != null ? historique.L1Adresse : c.L1Adresse);
                dr.l2_adresse = (historique != null ? historique.L2Adresse : c.L2Adresse);
                dr.cp_centre = (historique != null ? historique.CpCentre : c.CpCentre);
                dr.ville = (historique != null ? historique.Ville : c.Ville);
                dr.num_tel = (historique != null ? historique.NumTel : c.NumTel);
                dr.nom_representant_legal = (historique != null ? historique.NomRepresentantLegal : c.NomRepresentantLegal);
                if (uTemp != null)
                {
                    dr.identifiant_rnc2 = uTemp.Id;
                    dr.pwd_rnc2 = uTemp.MotDePasseTemporaire;
                }

                dr.url_site_rnc2 = ConfigurationManager.AppSettings["url_site_rnc2"];
                dr.url_site_otc = ConfigurationManager.AppSettings["url_site_otc"];

                //PLE valeur par défaut avant recheche des derog éventuelles
                dr.est_soumis_derogation = false;

                dt.Rows.Add(dr);
                //07/03/2012 PLE :Ajout des dérogation dans la notification
                //Mapping des dérogations
                ds.mapCentreDerogationsDataset(ds, c);

                //07/03/2012 PLE : Adressage directe de tout le Dataset au rapport
                //boReportDocument.SetDataSource((System.Data.DataTable)dt);
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
                return File(memStream.GetBuffer(), "application/pdf", dr.cod_agrem + "_notification_agrement_initial.pdf");

            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        private bool verifModifForPDF(rMotifModification motif, rCategorieVehicule.CategorieEnum categVehicule)
        {
            switch (categVehicule)
            {
                case rCategorieVehicule.CategorieEnum.VL:
                    if (motif.Id.Equals(rMotifModification.TypeModification.NOTVL.ToString())) return true;
                    if (motif.Id.Equals(rMotifModification.TypeModification.CHEXPLVL.ToString())) return true;
                    if (motif.Id.Equals(rMotifModification.TypeModification.CHLOCAVL.ToString())) return true;
                    if (motif.Id.Equals(rMotifModification.TypeModification.CHRATTVL.ToString())) return true;
                    break;

                case rCategorieVehicule.CategorieEnum.PL:
                    if (motif.Id.Equals(rMotifModification.TypeModification.NOTPL.ToString())) return true;
                    if (motif.Id.Equals(rMotifModification.TypeModification.CHEXPLPL.ToString())) return true;
                    if (motif.Id.Equals(rMotifModification.TypeModification.CHLOCAPL.ToString())) return true;
                    if (motif.Id.Equals(rMotifModification.TypeModification.CHRATTPL.ToString())) return true;
                    //YM 23/05/2013 les actions suivantes permettent d'éditer la notification sans rechercher dans l'historique (fiche 123)
                    if (motif.Id.Equals(rMotifModification.TypeModification.NOTRPL.ToString())) return true;
                    if (motif.Id.Equals(rMotifModification.TypeModification.REJRPL.ToString())) return true;
                    break;

                case rCategorieVehicule.CategorieEnum.CL:
                    if (motif.Id.Equals(rMotifModification.TypeModification.NOTCL.ToString())) return true;
                    if (motif.Id.Equals(rMotifModification.TypeModification.CHEXPLCL.ToString())) return true;
                    if (motif.Id.Equals(rMotifModification.TypeModification.CHLOCACL.ToString())) return true;
                    if (motif.Id.Equals(rMotifModification.TypeModification.CHRATTCL.ToString())) return true;
                    break;

            }

            return false;
        }

        public ActionResult TelechargerNotificationAuxiliairePDF(int id, rCategorieVehicule.CategorieEnum categVehicule)
        {
            try
            {
                //On retrieve l'id
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);

                pCentreHisto historique = null;
                //si l'action de la fiche ne permet pas d'éditer le notification, rechercher dans l'historique la dernière action permettant d'éditer la notification
                /*if (
                    (c.RMotifModification.Id != Convert.ToString(rMotifModification.TypeModification.NOTVL) && categVehicule == rCategorieVehicule.CategorieEnum.VL) &&
                    (c.RMotifModification.Id != Convert.ToString(rMotifModification.TypeModification.CHEXPLVL) && categVehicule == rCategorieVehicule.CategorieEnum.VL) &&
                    (c.RMotifModification.Id != Convert.ToString(rMotifModification.TypeModification.CHLOCAVL) && categVehicule == rCategorieVehicule.CategorieEnum.VL) &&
                    (c.RMotifModification.Id != Convert.ToString(rMotifModification.TypeModification.CHRATTVL) && categVehicule == rCategorieVehicule.CategorieEnum.VL) &&
                    (c.RMotifModification.Id != Convert.ToString(rMotifModification.TypeModification.NOTPL) && categVehicule == rCategorieVehicule.CategorieEnum.PL) &&
                    (c.RMotifModification.Id != Convert.ToString(rMotifModification.TypeModification.CHEXPLPL) && categVehicule == rCategorieVehicule.CategorieEnum.PL) &&
                    (c.RMotifModification.Id != Convert.ToString(rMotifModification.TypeModification.CHLOCAPL) && categVehicule == rCategorieVehicule.CategorieEnum.PL) &&
                    (c.RMotifModification.Id != Convert.ToString(rMotifModification.TypeModification.CHRATTPL) && categVehicule == rCategorieVehicule.CategorieEnum.PL) &&
                    (c.RMotifModification.Id != Convert.ToString(rMotifModification.TypeModification.NOTRPL) && categVehicule == rCategorieVehicule.CategorieEnum.PL) &&
                    (c.RMotifModification.Id != Convert.ToString(rMotifModification.TypeModification.REJRPL) && categVehicule == rCategorieVehicule.CategorieEnum.PL) &&
                    (c.RMotifModification.Id != Convert.ToString(rMotifModification.TypeModification.NOTCL) && categVehicule == rCategorieVehicule.CategorieEnum.CL) &&
                    (c.RMotifModification.Id != Convert.ToString(rMotifModification.TypeModification.CHEXPLCL) && categVehicule == rCategorieVehicule.CategorieEnum.CL) &&
                    (c.RMotifModification.Id != Convert.ToString(rMotifModification.TypeModification.CHLOCACL) && categVehicule == rCategorieVehicule.CategorieEnum.CL) &&
                    (c.RMotifModification.Id != Convert.ToString(rMotifModification.TypeModification.CHRATTCL) && categVehicule == rCategorieVehicule.CategorieEnum.CL)
                   )
                {*/
                if (!verifModifForPDF(c.RMotifModification, categVehicule))
                {
                    //rechercher dans l'historique la dernière action permettant d'éditer la notification
                    foreach (pCentreHisto h in c.PCentreHisto.OrderByDescending(a => a.DateHisto))
                    {
                        /*if (
                            (h.RMotifModification.Id == Convert.ToString(rMotifModification.TypeModification.NOTVL) && categVehicule == rCategorieVehicule.CategorieEnum.VL) ||
                            (h.RMotifModification.Id == Convert.ToString(rMotifModification.TypeModification.CHEXPLVL) && categVehicule == rCategorieVehicule.CategorieEnum.VL) ||
                            (h.RMotifModification.Id == Convert.ToString(rMotifModification.TypeModification.CHLOCAVL) && categVehicule == rCategorieVehicule.CategorieEnum.VL) ||
                            (h.RMotifModification.Id == Convert.ToString(rMotifModification.TypeModification.CHRATTVL) && categVehicule == rCategorieVehicule.CategorieEnum.VL) ||
                            (h.RMotifModification.Id == Convert.ToString(rMotifModification.TypeModification.NOTPL) && categVehicule == rCategorieVehicule.CategorieEnum.PL) ||
                            (h.RMotifModification.Id == Convert.ToString(rMotifModification.TypeModification.CHEXPLPL) && categVehicule == rCategorieVehicule.CategorieEnum.PL) ||
                            (h.RMotifModification.Id == Convert.ToString(rMotifModification.TypeModification.CHLOCAPL) && categVehicule == rCategorieVehicule.CategorieEnum.PL) ||
                            (h.RMotifModification.Id == Convert.ToString(rMotifModification.TypeModification.CHRATTPL) && categVehicule == rCategorieVehicule.CategorieEnum.PL) ||
                            (h.RMotifModification.Id == Convert.ToString(rMotifModification.TypeModification.NOTRPL) && categVehicule == rCategorieVehicule.CategorieEnum.PL) ||
                            (h.RMotifModification.Id == Convert.ToString(rMotifModification.TypeModification.REJRPL) && categVehicule == rCategorieVehicule.CategorieEnum.PL) ||
                            (h.RMotifModification.Id == Convert.ToString(rMotifModification.TypeModification.NOTCL) && categVehicule == rCategorieVehicule.CategorieEnum.CL) ||
                            (h.RMotifModification.Id == Convert.ToString(rMotifModification.TypeModification.CHEXPLCL) && categVehicule == rCategorieVehicule.CategorieEnum.CL) ||
                            (h.RMotifModification.Id == Convert.ToString(rMotifModification.TypeModification.CHLOCACL) && categVehicule == rCategorieVehicule.CategorieEnum.CL) ||
                            (h.RMotifModification.Id == Convert.ToString(rMotifModification.TypeModification.CHRATTCL) && categVehicule == rCategorieVehicule.CategorieEnum.CL)
                           )*/
                        if (verifModifForPDF(h.RMotifModification, categVehicule))
                        {
                            historique = h;
                            break;
                        }
                    }
                    if (historique == null)
                    {
                        NotificationAgrementModel m = new NotificationAgrementModel();
                        m.Centre = c;
                        m.CategVehicule = categVehicule;
                        return View("ErreurRechercheNouveauCentre", m);
                    }
                }

                rMotifModification motif = (historique != null ? historique.RMotifModification : c.RMotifModification);

                //On charge le document Crystal
                ReportDocument boReportDocument = new ReportDocument();
                boReportDocument.Load(Server.MapPath("~/bin/reporting/auxi_notification_decision_agrem_init_annexe3-A.rpt"));

                auxi_notification_decision_agrem_init_annexe3_A ds = new auxi_notification_decision_agrem_init_annexe3_A();
                auxi_notification_decision_agrem_init_annexe3_A.notification_centreDataTable dt = ds.notification_centre;
                auxi_notification_decision_agrem_init_annexe3_A.notification_centreRow dr = dt.Newnotification_centreRow();

                //On cré un DataSet
                dr.est_type_agrem_vl = false;
                dr.est_type_agrem_pl = false;
                dr.est_type_agrem_cl = false;
                dr.num_accuse_reception = null;

                if (motif.Id.Equals(Convert.ToString(rMotifModification.TypeModification.NOTVL)) ||
                    motif.Id.Equals(Convert.ToString(rMotifModification.TypeModification.NOTPL)) ||
                    motif.Id.Equals(Convert.ToString(rMotifModification.TypeModification.NOTCL)))
                {
                    dr.est_agrem_premiere_demande = true;
                    dr.est_agrem_renouvellement = false;
                    dr.est_agrem_modification = false;
                }
                if (motif.Id.Equals(Convert.ToString(rMotifModification.TypeModification.NOTRPL)))
                {
                    dr.est_agrem_premiere_demande = false;
                    dr.est_agrem_renouvellement = true;
                    dr.est_agrem_modification = false;
                }

                if (motif.Id.Equals(Convert.ToString(rMotifModification.TypeModification.CHEXPLCL)) ||
                    motif.Id.Equals(Convert.ToString(rMotifModification.TypeModification.CHEXPLPL)) ||
                    motif.Id.Equals(Convert.ToString(rMotifModification.TypeModification.CHEXPLVL)) ||
                    motif.Id.Equals(Convert.ToString(rMotifModification.TypeModification.CHHORAI)) ||
                    motif.Id.Equals(Convert.ToString(rMotifModification.TypeModification.CHLOCACL)) ||
                    motif.Id.Equals(Convert.ToString(rMotifModification.TypeModification.CHLOCAPL)) ||
                    motif.Id.Equals(Convert.ToString(rMotifModification.TypeModification.CHLOCAVL)) ||
                    motif.Id.Equals(Convert.ToString(rMotifModification.TypeModification.CHRATTCL)) ||
                    motif.Id.Equals(Convert.ToString(rMotifModification.TypeModification.CHRATTVL)) ||
                    motif.Id.Equals(Convert.ToString(rMotifModification.TypeModification.CHRATTPL))
                    )
                {
                    dr.est_agrem_premiere_demande = false;
                    dr.est_agrem_renouvellement = false;
                    dr.est_agrem_modification = true;
                }

                if (motif.Id.EndsWith("VL"))
                {
                    dr.est_type_agrem_vl = true;
                    dr.est_type_agrem_pl = false;
                    dr.est_type_agrem_cl = false;
                    dr.num_accuse_reception = (historique != null ? historique.NumAccuseReceptionVL : c.NumAccuseReceptionVL);
                }
                if (motif.Id.EndsWith("PL"))
                {
                    dr.est_type_agrem_vl = false;
                    dr.est_type_agrem_pl = true;
                    dr.est_type_agrem_cl = false;
                    dr.num_accuse_reception = (historique != null ? historique.NumAccuseReceptionPL : c.NumAccuseReceptionPL);

                }
                if (motif.Id.EndsWith("CL"))
                {
                    dr.est_type_agrem_vl = false;
                    dr.est_type_agrem_pl = false;
                    dr.est_type_agrem_cl = true;
                    dr.num_accuse_reception = (historique != null ? historique.NumAccuseReceptionCL : c.NumAccuseReceptionCL);
                }

                if (categVehicule == rCategorieVehicule.CategorieEnum.VL)
                {
                    if (motif.Id.StartsWith("CH"))
                    {
                        dr.ancien_numero_agrem = (historique != null ? historique.AncienNumeroAgrementVl : c.AncienNumeroAgrementVl);
                    }
                    dr.cod_agrem = (historique != null ? historique.CodAgremVl : c.CodAgremVl);
                    dr.cod_reseau = (historique != null ? historique.ReseauRattachementVL.Id : c.ReseauRattachementVL.Id);
                    dr.libelle_reseau = (historique != null ? historique.ReseauRattachementVL.Nom : c.ReseauRattachementVL.Nom);
                    dr.date_effet_notification = Convert.ToDateTime((historique != null ? historique.DateEffetNotificationVl : c.DateEffetNotificationVl));

                }
                if (categVehicule == rCategorieVehicule.CategorieEnum.PL)
                {
                    if (motif.Id.StartsWith("CH"))
                    {
                        dr.ancien_numero_agrem = (historique != null ? historique.AncienNumeroAgrementPl : c.AncienNumeroAgrementPl);
                    }
                    dr.cod_agrem = (historique != null ? historique.CodAgremPl : c.CodAgremPl);
                    dr.cod_reseau = (historique != null ? historique.ReseauRattachementPL.Id : c.ReseauRattachementPL.Id);
                    dr.libelle_reseau = (historique != null ? historique.ReseauRattachementPL.Nom : c.ReseauRattachementPL.Nom);
                    dr.date_effet_notification = Convert.ToDateTime((historique != null ? historique.DateEffetNotificationPl : c.DateEffetNotificationPl));
                    //PLE 23/03/2012 initialisation de la date d'échéance
                    dr.date_echeance_notification = Convert.ToDateTime((historique != null ? historique.DateEcheanceAgrementPl : c.DateEcheanceAgrementPl));
                }
                if (categVehicule == rCategorieVehicule.CategorieEnum.CL)
                {
                    if (motif.Id.StartsWith("CH"))
                    {
                        dr.ancien_numero_agrem = (historique != null ? historique.AncienNumeroAgrementCl : c.AncienNumeroAgrementCl);
                    }
                    dr.cod_agrem = (historique != null ? historique.CodAgremCl : c.CodAgremCl);
                    dr.cod_reseau = (historique != null ? historique.ReseauRattachementCL.Id : c.ReseauRattachementCL.Id);
                    dr.libelle_reseau = (historique != null ? historique.ReseauRattachementCL.Nom : c.ReseauRattachementCL.Nom);
                    dr.date_effet_notification = Convert.ToDateTime((historique != null ? historique.DateEffetNotificationCl : c.DateEffetNotificationCl));
                }
                dr.titre_departement = "Préfet " + (historique != null ? historique.RPrefecture.Prefixe.Replace("/", "") : c.RPrefecture.Prefixe.Replace("/", "")) + (historique != null ? historique.RPrefecture.LibellePrefecture : c.RPrefecture.LibellePrefecture);
                dr.libelle_prefet = (historique != null ? historique.RPrefecture.Prefixe.Replace("/", "") : c.RPrefecture.Prefixe.Replace("/", "")) + (historique != null ? historique.RPrefecture.LibellePrefecture : c.RPrefecture.LibellePrefecture);
                dr.raison_sociale = (historique != null ? historique.RaisonSociale : c.RaisonSociale);
                dr.l1_adresse = (historique != null ? historique.L1Adresse : c.L1Adresse);
                dr.l2_adresse = (historique != null ? historique.L2Adresse : c.L2Adresse);
                dr.cp_centre = (historique != null ? historique.CpCentre : c.CpCentre);
                dr.ville = (historique != null ? historique.Ville : c.Ville);
                dr.num_tel = (historique != null ? historique.NumTel : c.NumTel);
                dr.est_soumis_derogation = false;

                dt.Rows.Add(dr);

                //08/03/2012 PLE :Ajout des dérogation dans la notification
                //Mapping des dérogations
                ds.mapCentreDerogationsDataset(ds, c);

                //08/03/2012 PLE : Adressage directe de tout le Dataset au rapport
                //boReportDocument.SetDataSource((System.Data.DataTable)dt);
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
                return File(memStream.GetBuffer(), "application/pdf", dr.cod_agrem + "_notification_agrement_initial.pdf");

            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        public ActionResult ChoixTelechargerModifDenominationCommercialePDF(int id)
        {
            try
            {
                //1 on retrieve l'id
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);

                // choix des notifications à télécharger
                TelechargerNotificationAgrementModel t = new TelechargerNotificationAgrementModel();

                t.EtatVl = c.EstAgrementValideVL;
                t.EtatPl = c.EstAgrementValidePL;
                t.EtatCl = c.EstAgrementValideCL;
                t.Centre = c;
                //20/02/2024 PLE BUG : n'est pas redirigé vers la bonne notification
                t.ModifNotif = true;
                return View("TelechargerNotification", t);
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }
        public ActionResult ChoixTelechargerModifNotificationPDF(int id)
        {
            try
            {
                //1 on retrieve l'id
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);

                // choix des notifications à télécharger
                TelechargerNotificationAgrementModel t = new TelechargerNotificationAgrementModel();

                t.EtatVl = false;
                t.EtatPl = false;
                t.EtatCl = false;
                t.Centre = c;
                t.ModifNotif = true;

                //21/05/2013 YMA Edition notification pour changement exploitatnt PL
                if (c.RMotifModification.Id == Convert.ToString(rMotifModification.TypeModification.CHEXPLPL))
                {
                    t.EtatPl = true;
                    t.ModifNotif = false;
                    return View("TelechargerNotification", t);
                }
                if (c.RMotifModification.Id == Convert.ToString(rMotifModification.TypeModification.CHEXPLVL))
                {
                    t.EtatVl = true;
                    t.ModifNotif = false;
                    return View("TelechargerNotification", t);
                }
                if (c.RMotifModification.Id == Convert.ToString(rMotifModification.TypeModification.CHEXPLCL))
                {
                    t.EtatCl = true;
                    t.ModifNotif = false;
                    return View("TelechargerNotification", t);
                }

                //02/05/2012 PLE Ajout de la possibilité de réimprimer les notifications de changement de dénomination sociale - fiche 111
                if (c.RMotifModification.Id == Convert.ToString(rMotifModification.TypeModification.CHDECOM))
                {
                    t.EtatVl = c.EstAgrementValideVL;
                    t.EtatPl = c.EstAgrementValidePL;
                    t.EtatCl = c.EstAgrementValideCL;
                    t.Centre = c;
                    return View("TelechargerNotification", t);
                }

                if (c.RMotifModification.Id == Convert.ToString(rMotifModification.TypeModification.ANUVL))
                {
                    //Imprimer l’état avec les informations de l’agrément VL
                    t.EtatVl = true;
                    //, rCategorieVehicule.CategorieEnum categVehicule
                    //string e1 = Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementVL.CHANGEMENT_EXPLOITANT);
                    //string e2 = Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementVL.CHANGEMENT_LOCALISATION);
                    //string e3 = Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_KO);

                    if
                    ((c.MotifFinAgrementVl == Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementVL.CHANGEMENT_EXPLOITANT) ||
                         c.MotifFinAgrementPl == Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementVL.CHANGEMENT_LOCALISATION))
                     && (c.EtatAgrementPl == Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_KO))
                     && (c.DateFinAgrementVl == c.DateFinAgrementPl)
                    )
                    {
                        //Imprimer l’état avec les informations de l’agrément PL
                        t.EtatPl = true;
                    }

                    if
                    ((c.MotifFinAgrementVl == Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementVL.CHANGEMENT_EXPLOITANT) ||
                         c.MotifFinAgrementCl == Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementVL.CHANGEMENT_LOCALISATION))
                     && (c.EtatAgrementCl == Convert.ToString((int)pCentreEncour.CodeEtatAgrementCl.AGREMENT_KO))
                     && (c.DateFinAgrementVl == c.DateFinAgrementCl)
                    )
                    {
                        //Imprimer l’état avec les informations de l’agrément CL
                        t.EtatCl = true;
                    }

                    return View("TelechargerNotification", t);
                }

                if (c.RMotifModification.Id == Convert.ToString(rMotifModification.TypeModification.ANUPL))
                {
                    //Imprimer l’état avec les informations de l’agrément PL
                    t.EtatPl = true;

                    if
                    ((c.MotifFinAgrementPl == Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementPL.CHANGEMENT_EXPLOITANT) ||
                         c.MotifFinAgrementVl == Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementPL.CHANGEMENT_LOCALISATION))
                     && (c.EtatAgrementVl == Convert.ToString((int)pCentreEncour.CodeEtatAgrementVl.AGREMENT_KO))
                     && (c.DateFinAgrementPl == c.DateFinAgrementVl)
                    )
                    {
                        //Imprimer l’état avec les informations de l’agrément VL
                        t.EtatVl = true;
                    }

                    if
                    ((c.MotifFinAgrementPl == Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementPL.CHANGEMENT_EXPLOITANT) ||
                         c.MotifFinAgrementCl == Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementPL.CHANGEMENT_LOCALISATION))
                     && (c.EtatAgrementCl == Convert.ToString((int)pCentreEncour.CodeEtatAgrementCl.AGREMENT_KO))
                     && (c.DateFinAgrementPl == c.DateFinAgrementCl)
                    )
                    {
                        //Imprimer l’état avec les informations de l’agrément CL
                        t.EtatCl = true;
                    }

                    return View("TelechargerNotification", t);
                }

                if (c.RMotifModification.Id == Convert.ToString(rMotifModification.TypeModification.ANUCL))
                {
                    //Imprimer l’état avec les informations de l’agrément CL
                    t.EtatCl = true;

                    if
                    ((c.MotifFinAgrementCl == Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementCL.CHANGEMENT_EXPLOITANT) ||
                         c.MotifFinAgrementVl == Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementCL.CHANGEMENT_LOCALISATION))
                     && (c.EtatAgrementVl == Convert.ToString((int)pCentreEncour.CodeEtatAgrementVl.AGREMENT_KO))
                     && (c.DateFinAgrementCl == c.DateFinAgrementVl)
                    )
                    {
                        //Imprimer l’état avec les informations de l’agrément VL
                        t.EtatVl = true;
                    }

                    if
                    ((c.MotifFinAgrementCl == Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementCL.CHANGEMENT_EXPLOITANT) ||
                         c.MotifFinAgrementPl == Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementCL.CHANGEMENT_LOCALISATION))
                     && (c.EtatAgrementPl == Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_KO))
                     && (c.DateFinAgrementCl == c.DateFinAgrementPl)
                    )
                    {
                        //Imprimer l’état avec les informations de l’agrément PL
                        t.EtatPl = true;
                    }

                    return View("TelechargerNotification", t);
                }

                return this.RedirectToAction("Fiche", new { id = id });
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        public ActionResult TelechargerModifNotificationAuxiliairePDF(int id, bool etatVl, bool etatPl, bool etatCl)
        {
            try
            {
                //1 on retrieve l'id
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);

                //On charge le document Crystal
                ReportDocument boReportDocument = new ReportDocument();
                boReportDocument.Load(Server.MapPath("~/bin/reporting/auxi_notification_decision_agrem_modif_annexe3-B.rpt"));
                auxi_notification_decision_agrem_modif_annexe3_B ds = new auxi_notification_decision_agrem_modif_annexe3_B();
                auxi_notification_decision_agrem_modif_annexe3_B.notification_centreDataTable dt = ds.notification_centre;
                auxi_notification_decision_agrem_modif_annexe3_B.notification_centreRow dr = dt.Newnotification_centreRow();

                //On cré un DataSet
                dr.est_type_agrem_vl = false;
                dr.est_type_agrem_pl = false;
                dr.est_type_agrem_cl = false;

                dr.est_type_agrem_vl = etatVl;
                dr.est_type_agrem_pl = etatPl;
                dr.est_type_agrem_cl = etatCl;

                if (etatVl)
                {
                    dr.cod_agrem = c.CodAgremVl;
                    dr.date_effet_notification = Convert.ToDateTime(c.DateFinAgrementVl);
                }

                if (etatPl)
                {
                    dr.cod_agrem = c.CodAgremPl;
                    dr.date_effet_notification = Convert.ToDateTime(c.DateFinAgrementPl);
                }

                if (etatCl)
                {
                    dr.cod_agrem = c.CodAgremCl;
                    dr.date_effet_notification = Convert.ToDateTime(c.DateFinAgrementCl);
                }

                dr.est_motif_fermeture = false;
                dr.est_motif_changement_exploitant = false;
                dr.est_motif_changement_localisation = false;
                dr.est_motif_changement_rattachement = false;
                dr.est_motif_chagement_denomination = false;
                dr.est_motif_agrem_PL_non_renouvelle = false;

                rMotifModification.TypeModification v = (rMotifModification.TypeModification)Enum.Parse(typeof(rMotifModification.TypeModification), c.RMotifModification.Id);

                switch (v)
                {
                    case rMotifModification.TypeModification.REJRPL:
                        dr.est_motif_agrem_PL_non_renouvelle = true;
                        break;

                    case rMotifModification.TypeModification.ANUVL:
                        if (c.MotifFinAgrementVl == Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementVL.FERMETURE_DEFINITIVE))
                        {
                            dr.est_motif_fermeture = true;
                        }
                        if (c.MotifFinAgrementVl == Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementVL.CHANGEMENT_EXPLOITANT))
                        {
                            dr.est_motif_changement_exploitant = true;
                        }
                        if (c.MotifFinAgrementVl == Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementVL.CHANGEMENT_LOCALISATION))
                        {
                            dr.est_motif_changement_localisation = true;
                        }
                        if (c.MotifFinAgrementVl == Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementVL.CHANGEMENT_RESEAU))
                        {
                            dr.est_motif_changement_rattachement = true;
                        }
                        break;
                    case rMotifModification.TypeModification.ANUPL:
                        if (c.MotifFinAgrementPl == Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementPL.FERMETURE_DEFINITIVE))
                        {
                            dr.est_motif_fermeture = true;
                        }
                        if (c.MotifFinAgrementPl == Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementPL.CHANGEMENT_EXPLOITANT))
                        {
                            dr.est_motif_changement_exploitant = true;
                        }
                        if (c.MotifFinAgrementPl == Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementPL.CHANGEMENT_LOCALISATION))
                        {
                            dr.est_motif_changement_localisation = true;
                        }
                        if (c.MotifFinAgrementPl == Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementPL.CHANGEMENT_RESEAU))
                        {
                            dr.est_motif_changement_rattachement = true;
                        }
                        //YM 21/05/2013
                        if (c.MotifFinAgrementPl == Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementPL.NON_RENOUVELLEMENT))
                        {
                            dr.est_motif_agrem_PL_non_renouvelle = true;
                        }
                        break;
                    case rMotifModification.TypeModification.ANUCL:
                        if (c.MotifFinAgrementCl == Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementCL.FERMETURE_DEFINITIVE))
                        {
                            dr.est_motif_fermeture = true;
                        }
                        if (c.MotifFinAgrementCl == Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementCL.CHANGEMENT_EXPLOITANT))
                        {
                            dr.est_motif_changement_exploitant = true;
                        }
                        if (c.MotifFinAgrementCl == Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementCL.CHANGEMENT_LOCALISATION))
                        {
                            dr.est_motif_changement_localisation = true;
                        }
                        if (c.MotifFinAgrementCl == Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementCL.CHANGEMENT_RESEAU))
                        {
                            dr.est_motif_changement_rattachement = true;
                        }
                        break;
                }

                dr.titre_departement = "Préfet " + c.RPrefecture.Prefixe.Replace("/", "") + c.RPrefecture.LibellePrefecture;
                dr.raison_sociale = c.RaisonSociale;
                dr.l1_adresse = c.L1Adresse;
                dr.l2_adresse = c.L2Adresse;
                dr.cp_centre = c.CpCentre;
                dr.ville = c.Ville;
                dr.num_tel = c.NumTel;

                if (c.RMotifModification.Id.Equals(rMotifModification.TypeModification.CHDECOM.ToString()))
                {
                    dr.raison_sociale_nouveau = c.RaisonSociale;
                    dr.est_motif_chagement_denomination = true;
                    dr.date_effet_notification = Convert.ToDateTime(c.DateEffetModification);

                    pCentreHisto h = new pCentreHisto();
                    h = _pCentreHistoService.findLastBy(c.Id);
                    if (h != null)
                    {
                        dr.raison_sociale = h.RaisonSociale;
                    }
                }

                dt.Rows.Add(dr);
                //PLE 21/03/2012 intégration de la dérogation dans l'état de modification de notification auxiliaire
                ds.mapCentreDerogationsDataset(ds, c);
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
                return File(memStream.GetBuffer(), "application/pdf", dr.cod_agrem + "_notification_modification_agrement.pdf");

            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        public ActionResult TelechargerModifNotificationSpecialisePDF(int id, bool etatVl, bool etatPl, bool etatCl)
        {
            try
            {
                //1 on retrieve l'id
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);

                //On charge le document Crystal
                ReportDocument boReportDocument = new ReportDocument();
                boReportDocument.Load(Server.MapPath("~/bin/reporting/cntr_notification_decision_agrem_modif_annexe2-B.rpt"));
                cntr_notification_decision_agrem_modif_annexe2_B ds = new cntr_notification_decision_agrem_modif_annexe2_B();
                cntr_notification_decision_agrem_modif_annexe2_B.notification_centreDataTable dt = ds.notification_centre;
                cntr_notification_decision_agrem_modif_annexe2_B.notification_centreRow dr = dt.Newnotification_centreRow();

                //On cré un DataSet
                dr.est_type_agrem_vl = false;
                dr.est_type_agrem_pl = false;
                dr.est_type_agrem_cl = false;

                dr.est_type_agrem_vl = etatVl;
                dr.est_type_agrem_pl = etatPl;
                dr.est_type_agrem_cl = etatCl;

                if (etatVl)
                {
                    dr.cod_agrem = c.CodAgremVl;
                    dr.date_effet_notification = Convert.ToDateTime(c.DateFinAgrementVl);
                }

                if (etatPl)
                {
                    dr.cod_agrem = c.CodAgremPl;
                    dr.date_effet_notification = Convert.ToDateTime(c.DateFinAgrementPl);
                }

                if (etatCl)
                {
                    dr.cod_agrem = c.CodAgremCl;
                    dr.date_effet_notification = Convert.ToDateTime(c.DateFinAgrementCl);
                }

                dr.est_motif_fermeture = false;
                dr.est_motif_changement_exploitant = false;
                dr.est_motif_changement_localisation = false;
                dr.est_motif_changement_rattachement = false;
                dr.est_motif_chagement_denomination = false;

                rMotifModification.TypeModification v = (rMotifModification.TypeModification)Enum.Parse(typeof(rMotifModification.TypeModification), c.RMotifModification.Id);

                switch (v)
                {
                    case rMotifModification.TypeModification.ANUVL:
                        if (c.MotifFinAgrementVl == Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementVL.FERMETURE_DEFINITIVE))
                        {
                            dr.est_motif_fermeture = true;
                            dr.est_motif_changement_exploitant = false;
                            dr.est_motif_changement_localisation = false;
                            dr.est_motif_changement_rattachement = false;
                            dr.est_motif_chagement_denomination = false;
                        }
                        if (c.MotifFinAgrementVl == Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementVL.CHANGEMENT_EXPLOITANT))
                        {
                            dr.est_motif_fermeture = false;
                            dr.est_motif_changement_exploitant = true;
                            dr.est_motif_changement_localisation = false;
                            dr.est_motif_changement_rattachement = false;
                            dr.est_motif_chagement_denomination = false;
                        }
                        if (c.MotifFinAgrementVl == Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementVL.CHANGEMENT_LOCALISATION))
                        {
                            dr.est_motif_fermeture = false;
                            dr.est_motif_changement_exploitant = false;
                            dr.est_motif_changement_localisation = true;
                            dr.est_motif_changement_rattachement = false;
                            dr.est_motif_chagement_denomination = false;
                        }
                        if (c.MotifFinAgrementVl == Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementVL.CHANGEMENT_RESEAU))
                        {
                            dr.est_motif_fermeture = false;
                            dr.est_motif_changement_exploitant = false;
                            dr.est_motif_changement_localisation = false;
                            dr.est_motif_changement_rattachement = true;
                            dr.est_motif_chagement_denomination = false;
                        }
                        break;
                    case rMotifModification.TypeModification.ANUPL:
                        if (c.MotifFinAgrementPl == Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementPL.FERMETURE_DEFINITIVE))
                        {
                            dr.est_motif_fermeture = true;
                            dr.est_motif_changement_exploitant = false;
                            dr.est_motif_changement_localisation = false;
                            dr.est_motif_changement_rattachement = false;
                            dr.est_motif_chagement_denomination = false;
                        }
                        if (c.MotifFinAgrementPl == Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementPL.CHANGEMENT_EXPLOITANT))
                        {
                            dr.est_motif_fermeture = false;
                            dr.est_motif_changement_exploitant = true;
                            dr.est_motif_changement_localisation = false;
                            dr.est_motif_changement_rattachement = false;
                            dr.est_motif_chagement_denomination = false;
                        }
                        if (c.MotifFinAgrementPl == Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementPL.CHANGEMENT_LOCALISATION))
                        {
                            dr.est_motif_fermeture = false;
                            dr.est_motif_changement_exploitant = false;
                            dr.est_motif_changement_localisation = true;
                            dr.est_motif_changement_rattachement = false;
                            dr.est_motif_chagement_denomination = false;
                        }
                        if (c.MotifFinAgrementPl == Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementPL.CHANGEMENT_RESEAU))
                        {
                            dr.est_motif_fermeture = false;
                            dr.est_motif_changement_exploitant = false;
                            dr.est_motif_changement_localisation = false;
                            dr.est_motif_changement_rattachement = true;
                            dr.est_motif_chagement_denomination = false;
                        }
                        break;
                    case rMotifModification.TypeModification.ANUCL:
                        if (c.MotifFinAgrementCl == Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementCL.FERMETURE_DEFINITIVE))
                        {
                            dr.est_motif_fermeture = true;
                            dr.est_motif_changement_exploitant = false;
                            dr.est_motif_changement_localisation = false;
                            dr.est_motif_changement_rattachement = false;
                            dr.est_motif_chagement_denomination = false;
                        }
                        if (c.MotifFinAgrementCl == Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementCL.CHANGEMENT_EXPLOITANT))
                        {
                            dr.est_motif_fermeture = false;
                            dr.est_motif_changement_exploitant = true;
                            dr.est_motif_changement_localisation = false;
                            dr.est_motif_changement_rattachement = false;
                            dr.est_motif_chagement_denomination = false;
                        }
                        if (c.MotifFinAgrementCl == Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementCL.CHANGEMENT_LOCALISATION))
                        {
                            dr.est_motif_fermeture = false;
                            dr.est_motif_changement_exploitant = false;
                            dr.est_motif_changement_localisation = true;
                            dr.est_motif_changement_rattachement = false;
                            dr.est_motif_chagement_denomination = false;
                        }
                        if (c.MotifFinAgrementCl == Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementCL.CHANGEMENT_RESEAU))
                        {
                            dr.est_motif_fermeture = false;
                            dr.est_motif_changement_exploitant = false;
                            dr.est_motif_changement_localisation = false;
                            dr.est_motif_changement_rattachement = true;
                            dr.est_motif_chagement_denomination = false;
                        }
                        break;
                }

                dr.titre_departement = "Préfet " + c.RPrefecture.Prefixe.Replace("/", "") + c.RPrefecture.LibellePrefecture;
                dr.raison_sociale = c.RaisonSociale;
                dr.l1_adresse = c.L1Adresse;
                dr.l2_adresse = c.L2Adresse;
                dr.cp_centre = c.CpCentre;
                dr.ville = c.Ville;
                dr.num_tel = c.NumTel;

                if (c.RMotifModification.Id.Equals(rMotifModification.TypeModification.CHDECOM.ToString()))
                {
                    dr.raison_sociale_nouveau = c.RaisonSociale;
                    dr.est_motif_chagement_denomination = true;
                    dr.date_effet_notification = Convert.ToDateTime(c.DateEffetModification);

                    pCentreHisto h = new pCentreHisto();
                    h = _pCentreHistoService.findLastBy(c.Id);
                    if (h != null)
                    {
                        dr.raison_sociale = h.RaisonSociale;
                    }
                }

                dt.Rows.Add(dr);
                //PLE 20/03/2012 : Ajout des dérogations dans la notification de modification (pour le cas changement de dénominiation sociale)
                ds.mapCentreDerogationsDataset(ds, c);
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
                return File(memStream.GetBuffer(), "application/pdf", dr.cod_agrem + "_notification_modification_agrement.pdf");

            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // GET: /Centre/DemandeAgrement/62
        public ActionResult DemandeAgrement(int id, rCategorieVehicule.CategorieEnum categVehicule)
        {
            try
            {
                // on retrieve l'id
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);

                Dictionary<rCategorieVehicule.CategorieEnum, GestionDroitsExecution.ActionsFicheCentre> actions = new Dictionary<rCategorieVehicule.CategorieEnum, GestionDroitsExecution.ActionsFicheCentre>();
                actions.Add(rCategorieVehicule.CategorieEnum.VL, GestionDroitsExecution.ActionsFicheCentre.DEAVL);
                actions.Add(rCategorieVehicule.CategorieEnum.PL, GestionDroitsExecution.ActionsFicheCentre.DEAPL);
                actions.Add(rCategorieVehicule.CategorieEnum.CL, GestionDroitsExecution.ActionsFicheCentre.DEACL);

                if (GestionDroitsExecution.isRightActionForCentre(this.User, c, actions[categVehicule]))
                {
                    DemandeAgrementModel d = new DemandeAgrementModel();
                    d.Centre = c;
                    d.CategorieVehicule = categVehicule;

                    //on rempli la liste des réseaux en fonction du type d'agrément Vl Pl ou Cl
                    switch (categVehicule)
                    {
                        case rCategorieVehicule.CategorieEnum.VL:
                            //PLE 23/03/2012 différenciation liste réseaux disponibles selon Spécialisé /auxilaires
                            //d.ListeReseau = this._rReseauService.FindAllReseauVLAvecZ();
                            if (c.TypeCentre == "S")
                            {
                                d.ListeReseau = this._rReseauService.FindAllReseauVLAvecZ();
                            }
                            else
                            {
                                d.ListeReseau = this._rReseauService.FindAllReseauVLSansZ();
                            }
                            d.Centre.ReseauRattachementVL = null;
                            break;
                        case rCategorieVehicule.CategorieEnum.PL:
                            //PLE 23/03/2012 différenciation liste réseaux disponibles selon Spécialisé /auxilaires
                            //d.ListeReseau = this._rReseauService.FindAllReseauPLAvecZ();
                            if (c.TypeCentre == "S")
                            {
                                d.ListeReseau = this._rReseauService.FindAllReseauPLAvecZ();
                            }
                            else
                            {
                                d.ListeReseau = this._rReseauService.FindAllReseauPLSansZ();
                            }
                            d.Centre.ReseauRattachementPL = null;
                            break;
                        case rCategorieVehicule.CategorieEnum.CL:
                            //PLE 23/03/2012 différenciation liste réseaux disponibles selon Spécialisé /auxilaires
                            //d.ListeReseau = this._rReseauService.FindAllReseauCLAvecW();
                            if (c.TypeCentre == "S")
                            {
                                d.ListeReseau = this._rReseauService.FindAllReseauCLAvecW();
                            }
                            else
                            {
                                d.ListeReseau = this._rReseauService.FindAllReseauCLSansW();
                            }
                            d.Centre.ReseauRattachementCL = null;
                            break;
                    }
                    //EG le 25/11/2011
                    d.ListeReseau.Insert(0, null);

                    //pour les centres non administatifs supprimer Administration de la liste déroulante
                    if (c.RStatutJuridique == null || c.RStatutJuridique.Id != rStatutJuridique.Statuts.ADMIN.ToString())
                    {
                        d.ListeReseau.RemoveAt(d.ListeReseau.IndexOf(_rReseauService.FindBy("A")));
                    }
                    else
                    {
                        d.ListeReseau.Clear();
                        d.ListeReseau.Add(this._rReseauService.FindBy(rReseau.IdCodeReseau.A.ToString()));
                    }

                    //on rempli la liste des statut juridique
                    d.ListeStatut = this._rStatutJuridiqueService.FindAll();
                    //pour les centres non administatifs supprimer "Centre de l'Administration" de la liste déroulante
                    if (c.RStatutJuridique == null || c.RStatutJuridique.Id != rStatutJuridique.Statuts.ADMIN.ToString())
                    {
                        d.ListeStatut.RemoveAt(d.ListeStatut.IndexOf(_rStatutJuridiqueService.FindBy(rStatutJuridique.Statuts.ADMIN.ToString())));
                    }
                    else
                    {
                        d.ListeStatut.Clear();
                        d.ListeStatut.Add(this._rStatutJuridiqueService.FindBy(rStatutJuridique.Statuts.ADMIN.ToString()));
                    }

                    return View(d);
                }
                else
                {
                    Exception e = new Exception("Vous n'êtes pas autorisé....");
                    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                }
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // POST: /Centre/DemandeAgrement
        [HttpPost]
        public ActionResult DemandeAgrement(int id, DemandeAgrementModel d)
        {
            try
            {
                // on retrieve l'id
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);

                Dictionary<rCategorieVehicule.CategorieEnum, GestionDroitsExecution.ActionsFicheCentre> actions = new Dictionary<rCategorieVehicule.CategorieEnum, GestionDroitsExecution.ActionsFicheCentre>();
                actions.Add(rCategorieVehicule.CategorieEnum.VL, GestionDroitsExecution.ActionsFicheCentre.DEAVL);
                actions.Add(rCategorieVehicule.CategorieEnum.PL, GestionDroitsExecution.ActionsFicheCentre.DEAPL);
                actions.Add(rCategorieVehicule.CategorieEnum.CL, GestionDroitsExecution.ActionsFicheCentre.DEACL);

                if (GestionDroitsExecution.isRightActionForCentre(this.User, c, actions[d.CategorieVehicule]))

                {
                    validateDemandeAgrement(d);
                    if (ModelState.IsValid)
                    {
                        //EG le 04/01/2012  pour gérer le cas d’agrément de centres mixtes avec un nom de réseau (forcéments) différents
                        bool confirmerReseauMixte = false;
                        switch (d.CategorieVehicule)
                        {
                            case rCategorieVehicule.CategorieEnum.VL:
                                if (
                                    c.EstAgrementValidePL &&
                                    !rReseau.IdCodeReseau.Z.Equals(c.ReseauRattachementPL.Id) &&
                                    !rReseau.IdCodeReseau.Z.Equals(d.Centre.ReseauRattachementVL.Id)
                                    )
                                {
                                    confirmerReseauMixte = true;
                                }
                                /*
                                if (
                                    c.EstAgrementValideCL &&
                                    !rReseau.IdCodeReseau.Z.Equals(c.ReseauRattachementCL.Id) &&
                                    !rReseau.IdCodeReseau.Z.Equals(d.Centre.ReseauRattachementVL.Id)
                                    )
                                */
                                if (
                                    c.EstAgrementValideCL &&
                                    !(c.ReseauRattachementCL.Id.ToUpper().Equals(d.Centre.ReseauRattachementVL.Id.ToUpper()))
                                    )
                                {
                                    confirmerReseauMixte = true;
                                }
                                break;

                            case rCategorieVehicule.CategorieEnum.PL:
                                if (
                                   c.EstAgrementValideVL &&
                                   !rReseau.IdCodeReseau.Z.Equals(c.ReseauRattachementVL.Id) &&
                                   !rReseau.IdCodeReseau.Z.Equals(d.Centre.ReseauRattachementPL.Id)
                                   )
                                {
                                    confirmerReseauMixte = true;
                                }

                                if (
                                    c.EstAgrementValideCL &&
                                    /*
                                    !rReseau.IdCodeReseau.Z.Equals(c.ReseauRattachementCL.Id) &&
                                    !rReseau.IdCodeReseau.Z.Equals(d.Centre.ReseauRattachementPL.Id)
                                    */
                                    !(c.ReseauRattachementCL.Id.ToUpper().Equals(d.Centre.ReseauRattachementPL.Id.ToUpper()))
                                    )
                                {
                                    confirmerReseauMixte = true;
                                }
                                break;

                            case rCategorieVehicule.CategorieEnum.CL:
                                /*
                                if (
                                   c.EstAgrementValideVL &&
                                   !rReseau.IdCodeReseau.Z.Equals(c.ReseauRattachementVL.Id) &&
                                   !rReseau.IdCodeReseau.Z.Equals(d.Centre.ReseauRattachementCL.Id)
                                   )
                                */
                                if (
                                    c.EstAgrementValideVL &&
                                    !(c.ReseauRattachementVL.Id.ToUpper().Equals(d.Centre.ReseauRattachementCL.Id.ToUpper()))
                                    )
                                {
                                    confirmerReseauMixte = true;
                                }

                                if (
                                    c.EstAgrementValidePL &&
                                    !rReseau.IdCodeReseau.Z.Equals(c.ReseauRattachementPL.Id) &&
                                    !rReseau.IdCodeReseau.Z.Equals(d.Centre.ReseauRattachementCL.Id)
                                    )
                                {
                                    confirmerReseauMixte = true;
                                }
                                break;

                        }
                        if (confirmerReseauMixte)
                        {
                            return View("ConfirmationReseauMixte", d);
                        }
                        else
                        {
                            this._pCentreEnCourservice.demandeAgrement(id, d.Centre, d.CategorieVehicule, this.User.Identity.Name);
                        }

                        return this.RedirectToAction("Fiche", new { id = id });
                    }

                    //on rempli la liste des réseaux en fonction du type d'agrément Vl Pl ou Cl
                    switch (d.CategorieVehicule)
                    {
                        case rCategorieVehicule.CategorieEnum.VL:
                            //PLE 23/03/2012 différenciation liste réseaux disponibles selon Spécialisé /auxilaires
                            //d.ListeReseau = this._rReseauService.FindAllReseauVLAvecZ();
                            if (c.TypeCentre == "S")
                            {
                                d.ListeReseau = this._rReseauService.FindAllReseauVLAvecZ();
                            }
                            else
                            {
                                d.ListeReseau = this._rReseauService.FindAllReseauVLSansZ();
                            }
                            d.Centre.ReseauRattachementVL = null;
                            break;
                        case rCategorieVehicule.CategorieEnum.PL:
                            //PLE 23/03/2012 différenciation liste réseaux disponibles selon Spécialisé /auxilaires
                            //d.ListeReseau = this._rReseauService.FindAllReseauPLAvecZ();
                            if (c.TypeCentre == "S")
                            {
                                d.ListeReseau = this._rReseauService.FindAllReseauPLAvecZ();
                            }
                            else
                            {
                                d.ListeReseau = this._rReseauService.FindAllReseauPLSansZ();
                            }
                            d.Centre.ReseauRattachementPL = null;
                            break;
                        case rCategorieVehicule.CategorieEnum.CL:
                            //PLE 23/03/2012 différenciation liste réseaux disponibles selon Spécialisé /auxilaires
                            //d.ListeReseau = this._rReseauService.FindAllReseauCLAvecW();
                            if (c.TypeCentre == "S")
                            {
                                d.ListeReseau = this._rReseauService.FindAllReseauCLAvecW();
                            }
                            else
                            {
                                d.ListeReseau = this._rReseauService.FindAllReseauCLSansW();
                            }
                            d.Centre.ReseauRattachementCL = null;
                            break;
                    }
                    //EG le 25/11/2011
                    d.ListeReseau.Insert(0, null);

                    //pour les centres non administatifs supprimer Administration de la liste déroulante
                    if (c.RStatutJuridique == null || c.RStatutJuridique.Id != rStatutJuridique.Statuts.ADMIN.ToString())
                    {
                        d.ListeReseau.RemoveAt(d.ListeReseau.IndexOf(_rReseauService.FindBy("A")));
                    }
                    else
                    {
                        d.ListeReseau.Clear();
                        d.ListeReseau.Add(this._rReseauService.FindBy(rReseau.IdCodeReseau.A.ToString()));
                    }

                    //on rempli la liste des statut juridique
                    d.ListeStatut = this._rStatutJuridiqueService.FindAll();
                    //pour les centres non administatifs supprimer "Centre de l'Administration" de la liste déroulante
                    if (c.RStatutJuridique == null || c.RStatutJuridique.Id != rStatutJuridique.Statuts.ADMIN.ToString())
                    {
                        d.ListeStatut.RemoveAt(d.ListeStatut.IndexOf(_rStatutJuridiqueService.FindBy(rStatutJuridique.Statuts.ADMIN.ToString())));
                    }
                    else
                    {
                        d.ListeStatut.Clear();
                        d.ListeStatut.Add(this._rStatutJuridiqueService.FindBy(rStatutJuridique.Statuts.ADMIN.ToString()));
                    }

                    return View(d);
                }
                else
                {
                    Exception e = new Exception("Vous n'êtes pas autorisé....");
                    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                }
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        [HttpPost]
        public ActionResult DemandeAgrementSansConfirmation(int id, DemandeAgrementModel d)
        {
            try
            {
                // on retrieve l'id
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);

                Dictionary<rCategorieVehicule.CategorieEnum, GestionDroitsExecution.ActionsFicheCentre> actions = new Dictionary<rCategorieVehicule.CategorieEnum, GestionDroitsExecution.ActionsFicheCentre>();
                actions.Add(rCategorieVehicule.CategorieEnum.VL, GestionDroitsExecution.ActionsFicheCentre.DEAVL);
                actions.Add(rCategorieVehicule.CategorieEnum.PL, GestionDroitsExecution.ActionsFicheCentre.DEAPL);
                actions.Add(rCategorieVehicule.CategorieEnum.CL, GestionDroitsExecution.ActionsFicheCentre.DEACL);

                if (GestionDroitsExecution.isRightActionForCentre(this.User, c, actions[d.CategorieVehicule]))
                {
                    validateDemandeAgrement(d);
                    if (ModelState.IsValid)
                    {
                        this._pCentreEnCourservice.demandeAgrement(id, d.Centre, d.CategorieVehicule, this.User.Identity.Name);
                        return this.RedirectToAction("Fiche", new { id = id });
                    }

                    //on rempli la liste des réseaux en fonction du type d'agrément Vl Pl ou Cl
                    switch (d.CategorieVehicule)
                    {
                        case rCategorieVehicule.CategorieEnum.VL:
                            //PLE 23/03/2012 différenciation liste réseaux disponibles selon Spécialisé /auxilaires
                            //d.ListeReseau = this._rReseauService.FindAllReseauVLAvecZ();
                            if (c.TypeCentre == "S")
                            {
                                d.ListeReseau = this._rReseauService.FindAllReseauVLAvecZ();
                            }
                            else
                            {
                                d.ListeReseau = this._rReseauService.FindAllReseauVLSansZ();
                            }
                            d.Centre.ReseauRattachementVL = null;
                            break;
                        case rCategorieVehicule.CategorieEnum.PL:
                            //PLE 23/03/2012 différenciation liste réseaux disponibles selon Spécialisé /auxilaires
                            //d.ListeReseau = this._rReseauService.FindAllReseauPLAvecZ();
                            if (c.TypeCentre == "S")
                            {
                                d.ListeReseau = this._rReseauService.FindAllReseauPLAvecZ();
                            }
                            else
                            {
                                d.ListeReseau = this._rReseauService.FindAllReseauPLSansZ();
                            }
                            d.Centre.ReseauRattachementPL = null;
                            break;
                        case rCategorieVehicule.CategorieEnum.CL:
                            //PLE 23/03/2012 différenciation liste réseaux disponibles selon Spécialisé /auxilaires
                            //d.ListeReseau = this._rReseauService.FindAllReseauCLAvecW();
                            if (c.TypeCentre == "S")
                            {
                                d.ListeReseau = this._rReseauService.FindAllReseauCLAvecW();
                            }
                            else
                            {
                                d.ListeReseau = this._rReseauService.FindAllReseauCLSansW();
                            }
                            d.Centre.ReseauRattachementCL = null;
                            break;
                    }
                    //EG le 25/11/2011
                    d.ListeReseau.Insert(0, null);

                    //pour les centres non administatifs supprimer Administration de la liste déroulante
                    if (c.RStatutJuridique == null || c.RStatutJuridique.Id != rStatutJuridique.Statuts.ADMIN.ToString())
                    {
                        d.ListeReseau.RemoveAt(d.ListeReseau.IndexOf(_rReseauService.FindBy("A")));
                    }
                    else
                    {
                        d.ListeReseau.Clear();
                        d.ListeReseau.Add(this._rReseauService.FindBy(rReseau.IdCodeReseau.A.ToString()));
                    }

                    //on rempli la liste des statut juridique
                    d.ListeStatut = this._rStatutJuridiqueService.FindAll();
                    //pour les centres non administatifs supprimer "Centre de l'Administration" de la liste déroulante
                    if (c.RStatutJuridique == null || c.RStatutJuridique.Id != rStatutJuridique.Statuts.ADMIN.ToString())
                    {
                        d.ListeStatut.RemoveAt(d.ListeStatut.IndexOf(_rStatutJuridiqueService.FindBy(rStatutJuridique.Statuts.ADMIN.ToString())));
                    }
                    else
                    {
                        d.ListeStatut.Clear();
                        d.ListeStatut.Add(this._rStatutJuridiqueService.FindBy(rStatutJuridique.Statuts.ADMIN.ToString()));
                    }

                    return View(d);
                }
                else
                {
                    Exception e = new Exception("Vous n'êtes pas autorisé....");
                    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                }
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // GET: /Centre/RejetAgrement/62
        public ActionResult RejetAgrement(int id, rCategorieVehicule.CategorieEnum categVehicule)
        {
            try
            {
                // on retrieve l'id
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);

                if (
                   ((GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.REJVL)) && (categVehicule.ToString() == "VL"))
                || ((GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.REJPL)) && (categVehicule.ToString() == "PL"))
                || ((GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.REJRPL)) && (categVehicule.ToString() == "PL"))
                || ((GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.REJCL)) && (categVehicule.ToString() == "CL"))
                   )
                {
                    RejetAgrementModel m = new RejetAgrementModel();
                    m.Centre = this._pCentreEnCourservice.FindBy(id);
                    m.CategVehicule = categVehicule;

                    return View(m);
                }
                else
                {
                    Exception e = new Exception("Vous n'êtes pas autorisé....");
                    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                }
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // POST: /Centre/RejetAgrement
        [HttpPost]
        public ActionResult RejetAgrement(int id, RejetAgrementModel form)
        {
            try
            {
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);
                if (
                   ((GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.REJVL)) && (form.CategVehicule.ToString() == "VL"))
                || ((GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.REJPL)) && (form.CategVehicule.ToString() == "PL"))
                || ((GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.REJRPL)) && (form.CategVehicule.ToString() == "PL"))
                || ((GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.REJCL)) && (form.CategVehicule.ToString() == "CL"))
                   )
                {

                    this._pCentreEnCourservice.rejetAgrement(id, form.Centre, form.CategVehicule, this.User.Identity.Name);

                    return this.RedirectToAction("Fiche", new { id = id });
                }
                else
                {
                    Exception e = new Exception("Vous n'êtes pas autorisé....");
                    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                }
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // GET: /Centre/RejetRenouvellementAgrement/62
        public ActionResult RejetRenouvellementAgrement(int id, rCategorieVehicule.CategorieEnum categVehicule)
        {
            try
            {
                // on retrieve l'id
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.REJRPL) && categVehicule.ToString() == "PL")
                {
                    RejetAgrementModel m = new RejetAgrementModel();
                    m.Centre = this._pCentreEnCourservice.FindBy(id);
                    m.CategVehicule = categVehicule;
                    m.Centre.DateFinAgrementPl = null;

                    return View(m);
                }
                else
                {
                    Exception e = new Exception("Vous n'êtes pas autorisé....");
                    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                }
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // POST: /Centre/RejetRenouvellementAgrement
        [HttpPost]
        public ActionResult RejetRenouvellementAgrement(int id, RejetAgrementModel form)
        {
            try
            {
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.REJRPL) && form.CategVehicule.ToString() == "PL")
                {
                    //EG le 30/01/2012 la date de fin est automatiquement définie à partir de la date d'échéance
                    //validateRejetRenouvellementAgrement(form);
                    if (ModelState.IsValid)
                    {
                        this._pCentreEnCourservice.rejetRenouvellementAgrement(id, form.Centre, form.CategVehicule, this.User.Identity.Name);

                        if (form.Centre.TypeCentre.Equals(pCentreEncour.TypeInstallation.A.ToString()))
                        {
                            //On affiche l'écran de confirmation génération du pdf pour les centres spécialisé PL
                            return this.RedirectToAction("ResultatNotifierAgrement", new { id = id, categorie = form.CategVehicule });
                        }
                        return this.RedirectToAction("Fiche", new { id = id });
                    }
                    return View(form);
                }
                else
                {
                    Exception e = new Exception("Vous n'êtes pas autorisé....");
                    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                }
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        private void validateRejetRenouvellementAgrement(RejetAgrementModel form)
        {
            if (form.Centre.DateFinAgrementPl == null)
            {
                ModelState.AddModelError("Centre.DateFinAgrementPl", "La date de fin d'agrément doit être renseignée.");
            }
        }

        // GET: /Centre/AnnulerAgrement/62
        public ActionResult AnnulerAgrement(int id, rCategorieVehicule.CategorieEnum categVehicule)
        {
            try
            {
                //1 on retrieve l'id
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);

                Dictionary<rCategorieVehicule.CategorieEnum, GestionDroitsExecution.ActionsFicheCentre> actions = new Dictionary<rCategorieVehicule.CategorieEnum, GestionDroitsExecution.ActionsFicheCentre>();
                actions.Add(rCategorieVehicule.CategorieEnum.VL, GestionDroitsExecution.ActionsFicheCentre.ANUVL);
                actions.Add(rCategorieVehicule.CategorieEnum.PL, GestionDroitsExecution.ActionsFicheCentre.ANUPL);
                actions.Add(rCategorieVehicule.CategorieEnum.CL, GestionDroitsExecution.ActionsFicheCentre.ANUCL);

                if (GestionDroitsExecution.isRightActionForCentre(this.User, c, actions[categVehicule]))

                {
                    AnnulerAgrementModel d = new AnnulerAgrementModel();
                    d.Centre = c;
                    d.CategVehicule = categVehicule;
                    d.MsgControlMotif = false;

                    // TODO: remplacer le foreach par le find sur table de ref
                    //on rempli la liste des motif
                    //d.ListeModification = new Dictionary<int, string>();
                    //foreach (pCentreEncour.MotifAnnulerAgrementVL m in Enum.GetValues(typeof(pCentreEncour.MotifAnnulerAgrementVL)))
                    //{
                    //    if (m.ToString() == "RETRAIT_SANCTION")
                    //    {
                    //        continue;
                    //    }
                    //    d.ListeModification.Add((int)m, m.ToString());
                    //}
                    d.ListeModification = _pCentreEnCourservice.ListeMotifAnnulationAgrement(categVehicule, false);
                    d.ListeModification.Add(0, null);

                    return View(d);
                }
                else
                {
                    Exception e = new Exception("Vous n'êtes pas autorisé....");
                    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                }
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // POST: /Centre/AnnulerAgrement
        [HttpPost]
        public ActionResult AnnulerAgrement(int id, AnnulerAgrementModel d)
        {
            try
            {
                pCentreEncour c = this._pCentreEnCourservice.FindBy(d.Centre.Id);

                Dictionary<rCategorieVehicule.CategorieEnum, GestionDroitsExecution.ActionsFicheCentre> actions = new Dictionary<rCategorieVehicule.CategorieEnum, GestionDroitsExecution.ActionsFicheCentre>();
                actions.Add(rCategorieVehicule.CategorieEnum.VL, GestionDroitsExecution.ActionsFicheCentre.ANUVL);
                actions.Add(rCategorieVehicule.CategorieEnum.PL, GestionDroitsExecution.ActionsFicheCentre.ANUPL);
                actions.Add(rCategorieVehicule.CategorieEnum.CL, GestionDroitsExecution.ActionsFicheCentre.ANUCL);

                if (GestionDroitsExecution.isRightActionForCentre(this.User, c, actions[d.CategVehicule]))

                {
                    validateAnnulerAgrement(d);
                    if (ModelState.IsValid)
                    {
                        // Si le motif sélectionné est "2" ou "3" et un autre agrément est valide afficher un message d'information
                        if (d.MsgControlMotif == false)
                        {
                            switch (d.CategVehicule)
                            {
                                case rCategorieVehicule.CategorieEnum.VL:
                                    if (
                                        (
                                            d.Centre.MotifFinAgrementVl == Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementVL.CHANGEMENT_EXPLOITANT) ||
                                            d.Centre.MotifFinAgrementVl == Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementVL.CHANGEMENT_LOCALISATION)
                                        ) &&
                                        (
                                            Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_OK) == d.Centre.EtatAgrementPl ||
                                            Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_RENOUVELE_OK) == d.Centre.EtatAgrementPl ||
                                            Convert.ToString((int)pCentreEncour.CodeEtatAgrementCl.AGREMENT_OK) == d.Centre.EtatAgrementCl
                                        )
                                        )
                                    {
                                        d.MsgControlMotif = true;
                                        d.ListeModification = _pCentreEnCourservice.ListeMotifAnnulationAgrement(d.CategVehicule, false);
                                        d.ListeModification.Add(0, null);
                                        return View(d);
                                    }
                                    break;

                                case rCategorieVehicule.CategorieEnum.PL:
                                    if (
                                        (
                                            d.Centre.MotifFinAgrementPl == Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementPL.CHANGEMENT_EXPLOITANT) ||
                                            d.Centre.MotifFinAgrementPl == Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementPL.CHANGEMENT_LOCALISATION)
                                        ) &&
                                        (
                                            Convert.ToString((int)pCentreEncour.CodeEtatAgrementVl.AGREMENT_OK) == d.Centre.EtatAgrementVl ||
                                            Convert.ToString((int)pCentreEncour.CodeEtatAgrementCl.AGREMENT_OK) == d.Centre.EtatAgrementCl
                                        )
                                        )
                                    {
                                        d.MsgControlMotif = true;
                                        d.ListeModification = _pCentreEnCourservice.ListeMotifAnnulationAgrement(d.CategVehicule, false);
                                        d.ListeModification.Add(0, null);
                                        return View(d);
                                    }
                                    break;

                                case rCategorieVehicule.CategorieEnum.CL:
                                    if (
                                        (
                                            d.Centre.MotifFinAgrementCl == Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementCL.CHANGEMENT_EXPLOITANT) ||
                                            d.Centre.MotifFinAgrementCl == Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementCL.CHANGEMENT_LOCALISATION)
                                        ) &&
                                        (
                                            Convert.ToString((int)pCentreEncour.CodeEtatAgrementVl.AGREMENT_OK) == d.Centre.EtatAgrementVl ||
                                            Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_OK) == d.Centre.EtatAgrementPl ||
                                            Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_RENOUVELE_OK) == d.Centre.EtatAgrementPl
                                        )
                                        )
                                    {
                                        d.MsgControlMotif = true;
                                        d.ListeModification = _pCentreEnCourservice.ListeMotifAnnulationAgrement(d.CategVehicule, false);
                                        d.ListeModification.Add(0, null);
                                        return View(d);
                                    }
                                    break;
                            }
                        }

                        //Annuler l'agrément
                        this._pCentreEnCourservice.annulerAgrement(id, d.Centre, d.CategVehicule, this.User.Identity.Name);

                        //On affiche l'écran de confirmation génération du pdf
                        return this.RedirectToAction("ChoixTelechargerModifNotificationPDF", new { id = id });

                        //return this.RedirectToAction("Fiche", new { id = id });
                    }
                    //Traitement des erreurs car si Ok redirection plus haut
                    //Ne pas oublier de remplir la liste
                    //d.ListeModification = new Dictionary<int, string>();
                    //foreach (pCentreEncour.MotifAnnulerAgrementVL m in Enum.GetValues(typeof(pCentreEncour.MotifAnnulerAgrementVL)))
                    //{
                    //    if (m.ToString() == "RETRAIT_SANCTION")
                    //    {
                    //        continue;
                    //    }
                    //    d.ListeModification.Add((int)m, m.ToString());
                    //}
                    d.ListeModification = _pCentreEnCourservice.ListeMotifAnnulationAgrement(d.CategVehicule, false);
                    d.ListeModification.Add(0, null);
                    d.MsgControlMotif = false;
                    return View(d);
                }
                else
                {
                    Exception e = new Exception("Vous n'êtes pas autorisé....");
                    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                }
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        private void validateAnnulerAgrement(AnnulerAgrementModel fiche)
        {
            switch (fiche.CategVehicule)
            {
                case rCategorieVehicule.CategorieEnum.VL:
                    if (fiche.Centre.DateFinAgrementVl == null)
                        ModelState.AddModelError("Centre.DateFinAgrementVl", "La date de fin d'agrément VL doit être renseignée.");
                    if (fiche.Centre.MotifFinAgrementVl == null || fiche.Centre.MotifFinAgrementVl == "0")
                        ModelState.AddModelError("Centre.MotifFinAgrementVl", "Le motif de fin d'agrément VL être renseigné.");
                    break;

                case rCategorieVehicule.CategorieEnum.PL:
                    if (fiche.Centre.DateFinAgrementPl == null)
                        ModelState.AddModelError("Centre.DateFinAgrementPl", "La date de fin d'agrément PL doit être renseignée.");
                    if (fiche.Centre.MotifFinAgrementPl == null || fiche.Centre.MotifFinAgrementPl == "0")
                        ModelState.AddModelError("Centre.MotifFinAgrementPl", "Le motif de fin d'agrément PL  doit être renseigné.");
                    break;

                case rCategorieVehicule.CategorieEnum.CL:
                    if (fiche.Centre.DateFinAgrementCl == null)
                        ModelState.AddModelError("Centre.DateFinAgrementCl", "La date de fin d'agrément CL doit être renseignée.");
                    if (fiche.Centre.MotifFinAgrementCl == null || fiche.Centre.MotifFinAgrementCl == "0")
                        ModelState.AddModelError("Centre.MotifFinAgrementCl", "Le motif de fin d'agrément CL doit être renseigné.");
                    break;

            }
        }

        // GET: /Centre/NouveauCentreEtape0
        //Pour gérer le cas du centre de l'administration
        public ActionResult NouveauCentreEtape0(Boolean centreAdministration)
        {
            try
            {
                if (
                   (GestionDroitsExecution.isRightActionForCentre(this.User, null, GestionDroitsExecution.ActionsFicheCentre.CRECEVL))
                || (GestionDroitsExecution.isRightActionForCentre(this.User, null, GestionDroitsExecution.ActionsFicheCentre.CRECEPL))
                || (GestionDroitsExecution.isRightActionForCentre(this.User, null, GestionDroitsExecution.ActionsFicheCentre.CRECECL))
                   )
                {

                    NouveauCentreModel m = new NouveauCentreModel();
                    m.EstCentreAdministration = centreAdministration;
                    return View(m);
                }
                else
                {
                    Exception e = new Exception("Vous n'êtes pas autorisé....");
                    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                }
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // POST: /Centre/NouveauCentreEtape1/S
        [HttpPost]
        public ActionResult NouveauCentreEtape1(NouveauCentreModel m)
        {
            try
            {
                if (
                   (GestionDroitsExecution.isRightActionForCentre(this.User, null, GestionDroitsExecution.ActionsFicheCentre.CRECEVL))
                || (GestionDroitsExecution.isRightActionForCentre(this.User, null, GestionDroitsExecution.ActionsFicheCentre.CRECEPL))
                || (GestionDroitsExecution.isRightActionForCentre(this.User, null, GestionDroitsExecution.ActionsFicheCentre.CRECECL))
                   )
                {
                    return View(m);
                }
                else
                {
                    Exception e = new Exception("Vous n'êtes pas autorisé....");
                    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                }
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

        // POST: /Centre/NouveauCentreEtape2/S
        [HttpPost]
        public ActionResult NouveauCentreEtape2(NouveauCentreModel m)
        {
            try
            {
                if (
                   (GestionDroitsExecution.isRightActionForCentre(this.User, null, GestionDroitsExecution.ActionsFicheCentre.CRECEVL))
                || (GestionDroitsExecution.isRightActionForCentre(this.User, null, GestionDroitsExecution.ActionsFicheCentre.CRECEPL))
                || (GestionDroitsExecution.isRightActionForCentre(this.User, null, GestionDroitsExecution.ActionsFicheCentre.CRECECL))
                   )
                {
                    //NouveauCentreModel m = new NouveauCentreModel();
                    //m.TypeCentre = typeCentre;

                    //this.ViewData.Model = typeCentre;
                    //PLE le 29/02/2024 Supression de l'étape 1 de choix du type de centre.
                    m.TypeCentre = pCentreEncour.TypeInstallation.S.ToString();
                    return View(m);
                }
                else
                {
                    Exception e = new Exception("Vous n'êtes pas autorisé....");
                    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                }
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

        // POST: /Centre/NouveauCentreConfirmation
        [HttpPost]
        public ActionResult NouveauCentreConfirmation(NouveauCentreModel m)
        {
            //this.ViewData["typCentre"] = typCentre;
            //this.ViewData["agrement"] = agrement;
            //NouveauCentreModel m = new NouveauCentreModel();
            //m.TypeCentre = typCentre;
            //m.Agrement = agrement;

            return View(m);
        }

        // POST: /Centre/NouveauCentre
        [HttpPost]
        public ActionResult NouveauCentre(NouveauCentreModel m)
        {
            try
            {
                if (
                   (GestionDroitsExecution.isRightActionForCentre(this.User, null, GestionDroitsExecution.ActionsFicheCentre.CRECEVL))
                || (GestionDroitsExecution.isRightActionForCentre(this.User, null, GestionDroitsExecution.ActionsFicheCentre.CRECEPL))
                || (GestionDroitsExecution.isRightActionForCentre(this.User, null, GestionDroitsExecution.ActionsFicheCentre.CRECECL))
                   )
                {
                    m.Centre = new pCentreEncour();

                    // initialisation du département pour les préfectures
                    rUtilisateur util = this._rUtilisateurService.FindBy(this.User.Identity.Name);
                    if (util.RPrefecture != null)
                    {
                        rPrefecture pref = this._rPrefectureService.FindBy(util.RPrefecture.Id);
                        m.Centre.CodDepartement = pref.CodDepartement;
                        m.Centre.RPrefecture = pref;
                    }

                    //EG le 12/08/2011 , vérification si centre de l'administration
                    if (m.EstCentreAdministration)
                    {
                        m.ListeReseau = new List<rReseau>();
                        m.ListeReseau.Add(this._rReseauService.FindBy("A"));

                        m.ListeStatutJuridique = new List<rStatutJuridique>();
                        m.ListeStatutJuridique.Add(this._rStatutJuridiqueService.FindBy(rStatutJuridique.Statuts.ADMIN.ToString()));
                    }
                    else
                    {
                        //on rempli la liste des réseaux en fonction du type d'agrément Vl Pl ou Cl
                        switch (m.CategorieVehicule)
                        {
                            case rCategorieVehicule.CategorieEnum.VL:
                                if (m.TypeCentre == "S")
                                {
                                    m.ListeReseau = this._rReseauService.FindAllReseauVLAvecZ();
                                }
                                else
                                {
                                    m.ListeReseau = this._rReseauService.FindAllReseauVLSansZ();
                                }
                                m.ListeReseau.Insert(0, new rReseau() { Id = "-1", Nom = " " });
                                break;
                            case rCategorieVehicule.CategorieEnum.PL:
                                if (m.TypeCentre == "S")
                                {
                                    m.ListeReseau = this._rReseauService.FindAllReseauPLAvecZ();
                                }
                                else
                                {
                                    m.ListeReseau = this._rReseauService.FindAllReseauPLSansZ();
                                }
                                m.ListeReseau.Insert(0, new rReseau() { Id = "-1", Nom = " " });
                                break;
                            case rCategorieVehicule.CategorieEnum.CL:
                                //EG le 29/11/2011 : un centre auxiliaire ne peut pas être rattaché à un réseau indépendant(W)
                                if ("S".Equals(m.TypeCentre))
                                {
                                    m.ListeReseau = this._rReseauService.FindAllReseauCLAvecW();
                                }
                                else
                                {
                                    m.ListeReseau = this._rReseauService.FindAllReseauCLSansW();
                                }

                                m.ListeReseau.Insert(0, new rReseau() { Id = "-1", Nom = " " });
                                break;
                        }

                        //pour les centres non administatifs supprimer Administration de la liste déroulante
                        m.ListeReseau.RemoveAt(m.ListeReseau.IndexOf(this._rReseauService.FindBy("A")));

                        //on rempli la liste statut juridique
                        m.ListeStatutJuridique = this._rStatutJuridiqueService.FindAll();
                        m.ListeStatutJuridique.Insert(0, new rStatutJuridique() { Id = "-1", Libelle = " " });

                        //pour les centres non administatifs supprimer "Centre de l'Administration" de la liste déroulante
                        m.ListeStatutJuridique.RemoveAt(m.ListeStatutJuridique.IndexOf(this._rStatutJuridiqueService.FindBy(rStatutJuridique.Statuts.ADMIN.ToString())));

                    }
                    m.Token = Convert.ToString(System.DateTime.Now);
                    Session["token"] = m.Token;

                    return View(m);
                }
                else
                {
                    Exception e = new Exception("Vous n'êtes pas autorisé....");
                    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                }
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // POST: /Centre/CreationCentre
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreationCentre(NouveauCentreModel m)
        {
            try
            {
                pCentreEncour centre = this._pCentreEnCourservice.FindBy(m.Centre.Id);
                if (
                   ((GestionDroitsExecution.isRightActionForCentre(this.User, centre, GestionDroitsExecution.ActionsFicheCentre.CRECEVL)) && (m.CategorieVehicule.ToString() == "VL"))
                || ((GestionDroitsExecution.isRightActionForCentre(this.User, centre, GestionDroitsExecution.ActionsFicheCentre.CRECEPL)) && (m.CategorieVehicule.ToString() == "PL"))
                || ((GestionDroitsExecution.isRightActionForCentre(this.User, centre, GestionDroitsExecution.ActionsFicheCentre.CRECECL)) && (m.CategorieVehicule.ToString() == "CL"))
                   )
                {
                    //Vérification bouton précédent ?
                    string token = Session["token"] != null ? Convert.ToString(Session["token"]) : null;
                    if (string.IsNullOrEmpty(token))
                    {
                        throw new Exception("TOKEN !!!");
                    }
                    else
                    {
                        if (!token.Equals(m.Token))
                        {
                            throw new Exception("TOKEN PAS PAREIL !!!");
                        }
                    }

                    validateNouveauCentre(m);
                    if (ModelState.IsValid)
                    {
                        m.Centre.TypeCentre = m.TypeCentre;
                        pCentreEncour c = this._pCentreEnCourserviceAutoCommit.nouveauCentre(m.Centre, m.CategorieVehicule, this.User.Identity.Name);
                        Session.Remove("token");
                        //On affiche la fiche du centre
                        return this.RedirectToAction("Fiche", new { id = c.Id });
                    }

                    //m.Centre = new pCentreEncour();

                    // initialisation du département pour les préfectures
                    rUtilisateur util = this._rUtilisateurService.FindBy(this.User.Identity.Name);
                    if (util.RPrefecture != null)
                    {
                        rPrefecture pref = this._rPrefectureService.FindBy(util.RPrefecture.Id);
                        m.Centre.CodDepartement = pref.CodDepartement;
                        m.Centre.RPrefecture = pref;
                    }

                    //EG le 12/08/2011 , vérification si centre de l'administration
                    if (m.EstCentreAdministration)
                    {
                        m.ListeReseau = new List<rReseau>();
                        m.ListeReseau.Add(this._rReseauService.FindBy("A"));

                        m.ListeStatutJuridique = new List<rStatutJuridique>();
                        m.ListeStatutJuridique.Add(this._rStatutJuridiqueService.FindBy(rStatutJuridique.Statuts.ADMIN.ToString()));
                    }
                    else
                    {
                        //on rempli la liste des réseaux en fonction du type d'agrément Vl Pl ou Cl
                        switch (m.CategorieVehicule)
                        {
                            case rCategorieVehicule.CategorieEnum.VL:
                                if (m.TypeCentre == "S")
                                {
                                    m.ListeReseau = this._rReseauService.FindAllReseauVLAvecZ();
                                }
                                else
                                {
                                    m.ListeReseau = this._rReseauService.FindAllReseauVLSansZ();
                                }
                                m.ListeReseau.Insert(0, new rReseau() { Id = "-1", Nom = " " });
                                break;
                            case rCategorieVehicule.CategorieEnum.PL:
                                if (m.TypeCentre == "S")
                                {
                                    m.ListeReseau = this._rReseauService.FindAllReseauPLAvecZ();
                                }
                                else
                                {
                                    m.ListeReseau = this._rReseauService.FindAllReseauPLSansZ();
                                }
                                m.ListeReseau.Insert(0, new rReseau() { Id = "-1", Nom = " " });
                                break;
                            case rCategorieVehicule.CategorieEnum.CL:
                                m.ListeReseau = this._rReseauService.FindAllReseauCLAvecW();
                                m.ListeReseau.Insert(0, new rReseau() { Id = "-1", Nom = " " });
                                break;
                        }
                        //pour les centres non administatifs supprimer Administration de la liste déroulante
                        m.ListeReseau.RemoveAt(m.ListeReseau.IndexOf(this._rReseauService.FindBy("A")));

                        //on rempli la liste statut juridique
                        m.ListeStatutJuridique = this._rStatutJuridiqueService.FindAll();
                        m.ListeStatutJuridique.Insert(0, new rStatutJuridique() { Id = "-1", Libelle = " " });

                        //pour les centres non administatifs supprimer "Centre de l'Administration" de la liste déroulante
                        m.ListeStatutJuridique.RemoveAt(m.ListeStatutJuridique.IndexOf(this._rStatutJuridiqueService.FindBy(rStatutJuridique.Statuts.ADMIN.ToString())));
                    }

                    return View("NouveauCentre", m);
                }
                else
                {
                    Exception e = new Exception("Vous n'êtes pas autorisé....");
                    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                }
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        private void validateDemandeAgrement(DemandeAgrementModel m)
        {
            if (m.Centre.RaisonSociale == null)
                ModelState.AddModelError("centre.RaisonSociale", "La raison sociale doit être renseignée.");
            switch (m.CategorieVehicule)
            {
                case rCategorieVehicule.CategorieEnum.VL:
                    if ((m.Centre.ReseauRattachementVL.Id == null) || (m.Centre.ReseauRattachementVL.Id == "-1"))
                        ModelState.AddModelError("centre.ReseauRattachementVL.Id", "Le réseau de rattachement doit être renseigné.");
                    break;
                case rCategorieVehicule.CategorieEnum.PL:
                    if ((m.Centre.ReseauRattachementPL.Id == null) || (m.Centre.ReseauRattachementPL.Id == "-1"))
                        ModelState.AddModelError("centre.ReseauRattachementPL.Id", "Le réseau de rattachement doit être renseigné.");
                    break;
                case rCategorieVehicule.CategorieEnum.CL:
                    if ((m.Centre.ReseauRattachementCL.Id == null) || (m.Centre.ReseauRattachementCL.Id == "-1"))
                        ModelState.AddModelError("centre.ReseauRattachementCL.Id", "Le réseau de rattachement doit être renseigné.");
                    break;
            }

            if (!Validator.IsValidNumeroSiren(m.Centre.NumSiren))
            {
                ModelState.AddModelError("centre.NumSiren", "Le n° SIREN doit contenir 9 chiffres.");
            }

            if (m.Centre.CodDepartement == null)
                ModelState.AddModelError("centre.CodDepartement", "Le département doit être renseigné.");
            //if (m.Centre.CodDepartement == null)
            //    ModelState.AddModelError("centre.CodDepartement", "Le département doit être renseigné.");
            if (m.Centre.L1Adresse == null)
                ModelState.AddModelError("centre.L1Adresse", "L'adresse doit être renseigné.");
            if (m.Centre.CpCentre == null)
                ModelState.AddModelError("centre.CpCentre", "Le code postal doit être renseigné.");
            if (m.Centre.Ville == null)
                ModelState.AddModelError("centre.Ville", "La ville doit être renseignée.");

            /* TODO ?
            rPrefecture p;
            p = this._rPrefectureService.findByCodeDepartement(m.Centre.CodDepartement);
            if (p == null)
                if (m.Centre.CodDepartement != null)
                {
                    ModelState.AddModelError("centre.CodDepartement", "Code département inconnu.");
                }
                else
                {
                    ModelState.AddModelError("centre.CodDepartement", "Le département doit être renseigné.");
                }
             */

        }

        private void validateNouveauCentre(NouveauCentreModel m)
        {
            string strRegex = "^[0-9]{5}$";
            System.Text.RegularExpressions.Regex _Regex = new System.Text.RegularExpressions.Regex(strRegex);

            if (m.Centre.RaisonSociale == null)
                ModelState.AddModelError("centre.RaisonSociale", "La raison sociale doit être renseignée.");
            switch (m.CategorieVehicule)
            {
                case rCategorieVehicule.CategorieEnum.VL:
                    if ((m.Centre.ReseauRattachementVL.Id == null) || (m.Centre.ReseauRattachementVL.Id == "-1"))
                        ModelState.AddModelError("centre.ReseauRattachementVL.Id", "Le réseau de rattachement doit être renseigné.");
                    break;
                case rCategorieVehicule.CategorieEnum.PL:
                    if ((m.Centre.ReseauRattachementPL.Id == null) || (m.Centre.ReseauRattachementPL.Id == "-1"))
                        ModelState.AddModelError("centre.ReseauRattachementPL.Id", "Le réseau de rattachement doit être renseigné.");
                    break;
                case rCategorieVehicule.CategorieEnum.CL:
                    if ((m.Centre.ReseauRattachementCL.Id == null) || (m.Centre.ReseauRattachementCL.Id == "-1"))
                        ModelState.AddModelError("centre.ReseauRattachementCL.Id", "Le réseau de rattachement doit être renseigné.");
                    break;
            }

            if (!Validator.IsValidNumeroSiren(m.Centre.NumSiren))
                ModelState.AddModelError("centre.NumSiren", "Le n° SIREN doit contenir 9 chiffres.");

            if (m.Centre.CodDepartement == null)
                ModelState.AddModelError("centre.CodDepartement", "Le département doit être renseigné.");
            if (m.Centre.L1Adresse == null)
                ModelState.AddModelError("centre.L1Adresse", "L'adresse doit être renseigné.");
            if (m.Centre.CpCentre == null)
            {
                ModelState.AddModelError("centre.CpCentre", "Le code postal doit être renseigné.");
            }
            else
            {
                strRegex = "^[0-9]{5}$";
                if (!_Regex.IsMatch(m.Centre.CpCentre))
                {
                    ModelState.AddModelError("centre.CpCentre", "Le code postal est mal renseigné.");
                }
            }

            if (m.Centre.Ville == null)
                ModelState.AddModelError("centre.Ville", "La ville doit être renseignée.");

            rPrefecture p;
            p = this._rPrefectureService.findByCodeDepartement(m.Centre.CodDepartement);
            if (p == null)
            {
                if (m.Centre.CodDepartement != null)
                {
                    ModelState.AddModelError("centre.CodDepartement", "Code département inconnu.");
                }
                else
                {
                    ModelState.AddModelError("centre.CodDepartement", "Le département doit être renseigné.");
                }
            }

        }

        // GET: /Centre/FicheHistorique/62
        public ActionResult FicheHistorique(int id, DateTime dateHisto)
        {
            try
            {
                //1 on retrieve l'id
                pCentreHisto h = this._pCentreHistoService.findByIdAndDate(id, dateHisto);
                return View(h);

            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        //YM 10/02/2012
        // GET: /Centre/AjouterDerogation/62
        public ActionResult AjouterDerogation(int id, rCategorieVehicule.CategorieEnum categorie)
        {
            try
            {
                Dictionary<rCategorieVehicule.CategorieEnum, GestionDroitsExecution.ActionsFicheCentre> actions = new Dictionary<rCategorieVehicule.CategorieEnum, GestionDroitsExecution.ActionsFicheCentre>();
                actions.Add(rCategorieVehicule.CategorieEnum.VL, GestionDroitsExecution.ActionsFicheCentre.DEROG_VL);
                actions.Add(rCategorieVehicule.CategorieEnum.PL, GestionDroitsExecution.ActionsFicheCentre.DEROG_PL);
                actions.Add(rCategorieVehicule.CategorieEnum.CL, GestionDroitsExecution.ActionsFicheCentre.DEROG_CL);

                //1 on retrieve l'id
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, c, actions[categorie]))
                {
                    AjouterDerogationModel d = new AjouterDerogationModel();
                    d.Centre = c;
                    d.CategorieVehicule = categorie;

                    return View(d);
                }
                else
                {
                    Exception e = new Exception("Vous n'êtes pas autorisé....");
                    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                }
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // POST: /Centre/AjouterDerogation
        [HttpPost]
        public ActionResult AjouterDerogation(AjouterDerogationModel m)
        {
            try
            {
                Dictionary<rCategorieVehicule.CategorieEnum, GestionDroitsExecution.ActionsFicheCentre> actions = new Dictionary<rCategorieVehicule.CategorieEnum, GestionDroitsExecution.ActionsFicheCentre>();
                actions.Add(rCategorieVehicule.CategorieEnum.VL, GestionDroitsExecution.ActionsFicheCentre.DEROG_VL);
                actions.Add(rCategorieVehicule.CategorieEnum.PL, GestionDroitsExecution.ActionsFicheCentre.DEROG_PL);
                actions.Add(rCategorieVehicule.CategorieEnum.CL, GestionDroitsExecution.ActionsFicheCentre.DEROG_CL);

                m.Centre = this._pCentreEnCourservice.FindBy(m.Centre.Id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, m.Centre, actions[m.CategorieVehicule]))
                {
                    validateAjouterDerogation(m);
                    if (ModelState.IsValid)
                    {
                        //On utilise le certificatService nouvelleDerogation (historisation, enregistrement dérogation)
                        pDerogationCentre d = this._pDerogationCentreService.nouvelleDerogation(m.Centre, m.Derogation, m.CategorieVehicule, this.User.Identity.Name);

                        //On affiche la liste des dérogations du centre
                        return this.RedirectToAction("ListeDerogations", new { id = m.Centre.Id });
                    }

                    return View("AjouterDerogation", m);
                }
                else
                {
                    Exception e = new Exception("Vous n'êtes pas autorisé....");
                    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                }
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        private void validateAjouterDerogation(AjouterDerogationModel m)
        {
            /* EG le 28/02/2012 : plus besoin 
            string isAgrementAffecte = "";

            if (m.Centre.EstAgrementValideCL)
            {
                isAgrementAffecte = "Derogation.IsAgrementAffecteCL";
            }
            if (m.Centre.EstAgrementValidePL)
            {
                isAgrementAffecte = "Derogation.IsAgrementAffectePL";
            }
            if (m.Centre.EstAgrementValideVL)
            {
                isAgrementAffecte = "Derogation.IsAgrementAffecteVL";
            }
            */

            if (m.Derogation.DateDebut == null)
                ModelState.AddModelError("Derogation.DateDebut", "La date de début de dérogation doit être renseignée.");
            if (m.Derogation.Commentaire == null)
                ModelState.AddModelError("Derogation.Commentaire", "Le commentaire de dérogation doit être renseigné.");
            /* EG le 28/02/2012 : plus besoin 
            if (m.Derogation.IsAgrementAffecteVL == false && m.Derogation.IsAgrementAffectePL == false && m.Derogation.IsAgrementAffecteCL == false )
                ModelState.AddModelError(isAgrementAffecte, "L'agrément affecté par la dérogation doit être renseigné.");
            */

        }

        // GET: /Centre/ListeDerogations/62
        public ActionResult ListeDerogations(int id)
        {
            try
            {
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);

                return View(c);

            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // GET: /Centre/ModifierDerogation/62
        public ActionResult ModifierDerogation(int id)
        {
            try
            {
                Dictionary<rCategorieVehicule.CategorieEnum, GestionDroitsExecution.ActionsFicheCentre> actions = new Dictionary<rCategorieVehicule.CategorieEnum, GestionDroitsExecution.ActionsFicheCentre>();
                actions.Add(rCategorieVehicule.CategorieEnum.VL, GestionDroitsExecution.ActionsFicheCentre.MOD_DEROG_VL);
                actions.Add(rCategorieVehicule.CategorieEnum.PL, GestionDroitsExecution.ActionsFicheCentre.MOD_DEROG_PL);
                actions.Add(rCategorieVehicule.CategorieEnum.CL, GestionDroitsExecution.ActionsFicheCentre.MOD_DEROG_CL);
                //1 on retrieve l'id
                pDerogationCentre derogation = this._pDerogationCentreService.FindBy(id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, derogation.PCentreEncour, actions[derogation.CategorieVehicule]))
                {
                    AjouterDerogationModel m = new AjouterDerogationModel();
                    m.Centre = derogation.PCentreEncour;
                    m.Derogation = derogation;
                    m.CategorieVehicule = derogation.CategorieVehicule;

                    return View(m);
                }
                else
                {
                    Exception e = new Exception("Vous n'êtes pas autorisé....");
                    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                }
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // POST: /Centre/ModifierDerogation
        [HttpPost]
        public ActionResult ModifierDerogation(AjouterDerogationModel m)
        {
            try
            {
                Dictionary<rCategorieVehicule.CategorieEnum, GestionDroitsExecution.ActionsFicheCentre> actions = new Dictionary<rCategorieVehicule.CategorieEnum, GestionDroitsExecution.ActionsFicheCentre>();
                actions.Add(rCategorieVehicule.CategorieEnum.VL, GestionDroitsExecution.ActionsFicheCentre.MOD_DEROG_VL);
                actions.Add(rCategorieVehicule.CategorieEnum.PL, GestionDroitsExecution.ActionsFicheCentre.MOD_DEROG_PL);
                actions.Add(rCategorieVehicule.CategorieEnum.CL, GestionDroitsExecution.ActionsFicheCentre.MOD_DEROG_CL);
                m.Centre = this._pCentreEnCourservice.FindBy(m.Centre.Id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, m.Centre, actions[m.CategorieVehicule]))
                {
                    validateAjouterDerogation(m);
                    if (ModelState.IsValid)
                    {
                        //On utilise le certificatService nouvelleDerogation (historisation, enregistrement sanction)
                        pDerogationCentre a = this._pDerogationCentreService.modifierDerogation(m.Derogation, this.User.Identity.Name);

                        //On affiche la liste des dérogations
                        return this.RedirectToAction("ListeDerogations", new { id = m.Derogation.PCentreEncour.Id });
                    }
                    return View("ModifierDerogation", m);
                }
                else
                {
                    Exception e = new Exception("Vous n'êtes pas autorisé....");
                    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                }
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // GET: /Centre/ConfirmationDeleteDerogation/60
        public ActionResult ConfirmationDeleteDerogation(int id)
        {
            try
            {
                Dictionary<rCategorieVehicule.CategorieEnum, GestionDroitsExecution.ActionsFicheCentre> actions = new Dictionary<rCategorieVehicule.CategorieEnum, GestionDroitsExecution.ActionsFicheCentre>();
                actions.Add(rCategorieVehicule.CategorieEnum.VL, GestionDroitsExecution.ActionsFicheCentre.MOD_DEROG_VL);
                actions.Add(rCategorieVehicule.CategorieEnum.PL, GestionDroitsExecution.ActionsFicheCentre.MOD_DEROG_PL);
                actions.Add(rCategorieVehicule.CategorieEnum.CL, GestionDroitsExecution.ActionsFicheCentre.MOD_DEROG_CL);

                pDerogationCentre d = this._pDerogationCentreService.FindBy(id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, d.PCentreEncour, actions[d.CategorieVehicule]))
                {
                    return View(d);
                }
                else
                {
                    Exception e = new Exception("Vous n'êtes pas autorisé....");
                    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                }
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

        // POST: /Centre/DeleteDerogation/60
        [HttpPost]
        public ActionResult DeleteDerogation(int id)
        {
            try
            {
                Dictionary<rCategorieVehicule.CategorieEnum, GestionDroitsExecution.ActionsFicheCentre> actions = new Dictionary<rCategorieVehicule.CategorieEnum, GestionDroitsExecution.ActionsFicheCentre>();
                actions.Add(rCategorieVehicule.CategorieEnum.VL, GestionDroitsExecution.ActionsFicheCentre.MOD_DEROG_VL);
                actions.Add(rCategorieVehicule.CategorieEnum.PL, GestionDroitsExecution.ActionsFicheCentre.MOD_DEROG_PL);
                actions.Add(rCategorieVehicule.CategorieEnum.CL, GestionDroitsExecution.ActionsFicheCentre.MOD_DEROG_CL);

                pDerogationCentre d = this._pDerogationCentreService.FindBy(id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, d.PCentreEncour, actions[d.CategorieVehicule]))
                {
                    // on supprime la dérogation
                    this._pDerogationCentreService.deleteDerogation(d, this.User.Identity.Name);

                    //on affiche la liste des dérogations
                    return this.RedirectToAction("ListeDerogations", new { id = d.PCentreEncour.Id });
                }
                else
                {
                    Exception e = new Exception("Vous n'êtes pas autorisé....");
                    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                }
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

        // GET: /Centre/AjouterSanction/62
        public ActionResult AjouterSanction(int id)
        {
            try
            {
                //1 on retrieve l'id
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.SANCT))
                {
                    AjouterSanctionModel s = new AjouterSanctionModel();
                    s.Centre = c;

                    //on rempli la liste des sanctions
                    s.ListeSanction = this._rTypeSanctionCentreService.FindAll();

                    //on rempli la liste des organismes demandeurs
                    s.ListeOrganismeDemandeur = this._rTypeOrganismeDemandeurService.FindAll();
                    //s.ListeOrganismeDemandeur.Insert(0, new rTypeOrganismeDemandeur() { Id = -1 , Libelle = "--------" });
                    s.ListeOrganismeDemandeur.Insert(0, new rTypeOrganismeDemandeur() { Id = -1, Libelle = " " });

                    return View(s);
                }
                else
                {
                    Exception e = new Exception("Vous n'êtes pas autorisé....");
                    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                }
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // POST: /Centre/AjouterSanction
        [HttpPost]
        public ActionResult AjouterSanction(AjouterSanctionModel m)
        {
            try
            {
                m.Centre = this._pCentreEnCourservice.FindBy(m.Centre.Id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, m.Centre, GestionDroitsExecution.ActionsFicheCentre.SANCT))
                {
                    validateAjouterSanction(m);
                    if (ModelState.IsValid)
                    {
                        //On utilise le certificatService nouvelleSanction (historisation, annulation agrément et enregistrement sanction)
                        pSanctionCentre s = this._pSanctionCentreService.nouvelleSanction(m.Centre, m.Sanction, this.User.Identity.Name);

                        //On affiche la fiche du centre
                        return this.RedirectToAction("ListeSanctions", new { id = m.Centre.Id });
                    }
                    //on rempli la liste des sanctions
                    m.ListeSanction = this._rTypeSanctionCentreService.FindAll();

                    //on rempli la liste des organismes demandeurs
                    m.ListeOrganismeDemandeur = this._rTypeOrganismeDemandeurService.FindAll();

                    return View("AjouterSanction", m);
                }
                else
                {
                    Exception e = new Exception("Vous n'êtes pas autorisé....");
                    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                }
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // GET: /Centre/ModifierSanction/62
        public ActionResult ModifierSanction(int id)
        {
            try
            {
                //1 on retrieve l'id
                pSanctionCentre sanction = this._pSanctionCentreService.FindBy(id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, sanction.PCentreEncour, GestionDroitsExecution.ActionsFicheCentre.SANCT))
                {
                    AjouterSanctionModel m = new AjouterSanctionModel();
                    m.Centre = sanction.PCentreEncour;
                    m.Sanction = sanction;

                    //on rempli la liste des sanctions
                    m.ListeSanction = this._rTypeSanctionCentreService.FindAll();

                    //on rempli la liste des organismes demandeurs
                    m.ListeOrganismeDemandeur = this._rTypeOrganismeDemandeurService.FindAll();
                    //s.ListeOrganismeDemandeur.Insert(0, new rTypeOrganismeDemandeur() { Id = -1 , Libelle = "--------" });
                    m.ListeOrganismeDemandeur.Insert(0, new rTypeOrganismeDemandeur() { Id = -1, Libelle = " " });

                    return View(m);
                }
                else
                {
                    Exception e = new Exception("Vous n'êtes pas autorisé....");
                    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                }
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // POST: /Centre/ModifierSanction
        [HttpPost]
        public ActionResult ModifierSanction(AjouterSanctionModel m)
        {
            try
            {
                m.Centre = this._pCentreEnCourservice.FindBy(m.Centre.Id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, m.Centre, GestionDroitsExecution.ActionsFicheCentre.SANCT))
                {
                    validateAjouterSanction(m);
                    if (ModelState.IsValid)
                    {
                        //On utilise le certificatService nouvelleSanction (historisation, annulation agrément et enregistrement sanction)
                        pSanctionCentre s = this._pSanctionCentreService.modifierSanction(m.Sanction, this.User.Identity.Name);

                        //On affiche la fiche du centre
                        return this.RedirectToAction("ListeSanctions", new { id = m.Sanction.PCentreEncour.Id });

                    }

                    //on rempli la liste des sanctions
                    m.ListeSanction = this._rTypeSanctionCentreService.FindAll();

                    //on rempli la liste des organismes demandeurs
                    m.ListeOrganismeDemandeur = this._rTypeOrganismeDemandeurService.FindAll();

                    return View("ModifierSanction", m);
                }
                else
                {
                    Exception e = new Exception("Vous n'êtes pas autorisé....");
                    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                }
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // GET: /Centre/ConfirmationDeleteSanction/60
        public ActionResult ConfirmationDeleteSanction(int id)
        {
            try
            {
                pSanctionCentre s = this._pSanctionCentreService.FindBy(id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, s.PCentreEncour, GestionDroitsExecution.ActionsFicheCentre.SANCT))
                {
                    return View(s);
                }
                else
                {
                    Exception e = new Exception("Vous n'êtes pas autorisé....");
                    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                }
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

        // POST: /Centre/DeleteSanction/60
        [HttpPost]
        public ActionResult DeleteSanction(int id)
        {
            try
            {
                pSanctionCentre s = this._pSanctionCentreService.FindBy(id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, s.PCentreEncour, GestionDroitsExecution.ActionsFicheCentre.SANCT))
                {
                    // on supprime la sanction
                    this._pSanctionCentreService.deleteSanction(s);

                    //on affiche la liste des sanctions
                    return this.RedirectToAction("ListeSanctions", new { id = s.PCentreEncour.Id });
                }
                else
                {
                    Exception e = new Exception("Vous n'êtes pas autorisé....");
                    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                }
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

        private void validateAjouterSanction(AjouterSanctionModel m)
        {
            //if (m.Sanction.RTypeSanctionCentre == null)
            //    ModelState.AddModelError("sanction.RTypeSanctionCentre.Id", "Le type de sanction doit être renseigné.");

            //int s = Convert.ToInt16(m.Sanction.RTypeSanctionCentre.Id);
            //s = Convert.ToInt16(m.Sanction.RTypeSanctionCentre.Id);
            string isAgrementAffecte = "";

            if (m.Centre.EstAgrementValideCL)
            {
                isAgrementAffecte = "Sanction.IsAgrementAffecteCL";
            }
            if (m.Centre.EstAgrementValidePL)
            {
                isAgrementAffecte = "Sanction.IsAgrementAffectePL";
            }
            if (m.Centre.EstAgrementValideVL)
            {
                isAgrementAffecte = "Sanction.IsAgrementAffecteVL";
            }

            switch (m.Sanction.RTypeSanctionCentre.Id)
            {
                case (int)rTypeSanctionCentre.MotifSanctionEnum.RETRAIT_DEFINITIF:
                    if (m.Sanction.DateDebut == null)
                        ModelState.AddModelError("Sanction.DateDebut", "La date de début de sanction doit être renseignée.");
                    if (m.Sanction.RTypeOrganismeDemandeur == null)
                        ModelState.AddModelError("Sanction.RTypeOrganismeDemandeur.Id", "L'organisme demandeur doit être renseigné.");
                    if (m.Sanction.DateEvenementOrigine == null)
                        ModelState.AddModelError("Sanction.DateEvenementOrigine", "La date de l'événement doit être renseignée.");
                    if (!m.Sanction.IsAgrementAffecteVL && !m.Sanction.IsAgrementAffectePL && !m.Sanction.IsAgrementAffecteCL)
                        ModelState.AddModelError(isAgrementAffecte, "Vous devez renseigner le(s) agrément(s) concerné(s).");
                    break;

                case (int)rTypeSanctionCentre.MotifSanctionEnum.SUSPENSION:
                    if (m.Sanction.DateDebut == null)
                        ModelState.AddModelError("Sanction.DateDebut", "La date de début de sanction doit être renseignée.");
                    if (m.Sanction.DateFin == null)
                        ModelState.AddModelError("Sanction.DateFin", "La date de fin de sanction doit être renseignée.");
                    if (m.Sanction.RTypeOrganismeDemandeur == null)
                        ModelState.AddModelError("Sanction.RTypeOrganismeDemandeur.Id", "L'organisme demandeur doit être renseigné.");
                    if (m.Sanction.DateEvenementOrigine == null)
                        ModelState.AddModelError("Sanction.DateEvenementOrigine", "La date de l'événement doit être renseignée.");
                    if (!m.Sanction.IsAgrementAffecteVL && !m.Sanction.IsAgrementAffectePL && !m.Sanction.IsAgrementAffecteCL)
                        ModelState.AddModelError(isAgrementAffecte, "Vous devez renseigner le(s) agrément(s) concerné(s).");
                    break;

                case (int)rTypeSanctionCentre.MotifSanctionEnum.SUSP_CONSERVATOIRE:
                    if (m.Sanction.DateDebut == null)
                        ModelState.AddModelError("Sanction.DateDebut", "La date de début de sanction doit être renseignée.");
                    if (m.Sanction.DateFin == null)
                        ModelState.AddModelError("Sanction.DateFin", "La date de fin de sanction doit être renseignée.");
                    if (m.Sanction.RTypeOrganismeDemandeur == null)
                        ModelState.AddModelError("Sanction.RTypeOrganismeDemandeur.Id", "L'organisme demandeur doit être renseigné.");
                    if (m.Sanction.DateEvenementOrigine == null)
                        ModelState.AddModelError("Sanction.DateEvenementOrigine", "La date de l'événement doit être renseignée.");
                    if (!m.Sanction.IsAgrementAffecteVL && !m.Sanction.IsAgrementAffectePL && !m.Sanction.IsAgrementAffecteCL)
                        ModelState.AddModelError(isAgrementAffecte, "Vous devez renseigner le(s) agrément(s) concerné(s).");
                    break;

                case (int)rTypeSanctionCentre.MotifSanctionEnum.AVERTISSEMENT:
                    if (m.Sanction.DateDebut == null)
                        ModelState.AddModelError("Sanction.DateDebut", "La date de début de sanction doit être renseignée.");
                    if (m.Sanction.RTypeOrganismeDemandeur == null)
                        ModelState.AddModelError("Sanction.RTypeOrganismeDemandeur.Id", "L'organisme demandeur doit être renseigné.");
                    if (m.Sanction.DateEvenementOrigine == null)
                        ModelState.AddModelError("Sanction.DateEvenementOrigine", "La date de l'événement doit être renseignée.");
                    if (!m.Sanction.IsAgrementAffecteVL && !m.Sanction.IsAgrementAffectePL && !m.Sanction.IsAgrementAffecteCL)
                        ModelState.AddModelError(isAgrementAffecte, "Vous devez renseigner le(s) agrément(s) concerné(s).");
                    break;

                case (int)rTypeSanctionCentre.MotifSanctionEnum.AUTRE_SANCTION:
                    if (m.Sanction.DateDebut == null)
                        ModelState.AddModelError("Sanction.DateDebut", "La date de début de sanction doit être renseignée.");
                    if (m.Sanction.TypeSanctionAutre == null)
                        ModelState.AddModelError("Sanction.TypeSanctionAutre", "La description de la sanction doit être renseignée.");
                    if (m.Sanction.RTypeOrganismeDemandeur == null)
                        ModelState.AddModelError("Sanction.RTypeOrganismeDemandeur.Id", "L'organisme demandeur doit être renseigné.");
                    if (m.Sanction.DateEvenementOrigine == null)
                        ModelState.AddModelError("Sanction.DateEvenementOrigine", "La date de l'événement doit être renseignée.");
                    break;
            }

            if ((m.Sanction.DateFin != null && m.Sanction.DateDebut != null) && (m.Sanction.DateFin < m.Sanction.DateDebut))
            {
                ModelState.AddModelError("Sanction.DateFin", "La date de fin de sanction doit être inférieure ou égale à la date de début.");
            }

        }

        // GET: /Centre/AjouterAmende/62
        public ActionResult AjouterAmende(int id)
        {
            try
            {
                //1 on retrieve l'id
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.AMEND))
                {
                    AjouterAmendeModel m = new AjouterAmendeModel();
                    m.Centre = c;

                    return View(m);
                }
                else
                {
                    Exception e = new Exception("Vous n'êtes pas autorisé....");
                    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                }
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // POST: /Centre/AjouterAmende
        [HttpPost]
        public ActionResult AjouterAmende(AjouterAmendeModel m)
        {
            try
            {
                m.Centre = this._pCentreEnCourservice.FindBy(m.Centre.Id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, m.Centre, GestionDroitsExecution.ActionsFicheCentre.AMEND))
                {
                    validateAjouterAmende(m);
                    if (ModelState.IsValid)
                    {
                        //On utilise le certificatService nouvelleAmende (historisation et enregistrement amende)
                        pAmendeCentre a = this._pAmendeCentreService.nouvelleAmende(m.Centre, m.Amende, this.User.Identity.Name);

                        //On affiche la fiche du centre
                        return this.RedirectToAction("ListeAmendes", new { id = m.Centre.Id });
                    }
                    return View("AjouterAmende", m);
                }
                else
                {
                    Exception e = new Exception("Vous n'êtes pas autorisé....");
                    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                }
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // GET: /Centre/ModifierAmende/62
        public ActionResult ModifierAmende(int id)
        {
            try
            {
                //1 on retrieve l'id
                pAmendeCentre amende = this._pAmendeCentreService.FindBy(id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, amende.PCentreEncour, GestionDroitsExecution.ActionsFicheCentre.AMEND))
                {
                    AjouterAmendeModel m = new AjouterAmendeModel();
                    m.Centre = amende.PCentreEncour;
                    m.Amende = amende;

                    return View(m);
                }
                else
                {
                    Exception e = new Exception("Vous n'êtes pas autorisé....");
                    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                }
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // POST: /Centre/ModifierAmende
        [HttpPost]
        public ActionResult ModifierAmende(AjouterAmendeModel m)
        {
            try
            {
                m.Centre = this._pCentreEnCourservice.FindBy(m.Centre.Id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, m.Centre, GestionDroitsExecution.ActionsFicheCentre.AMEND))
                {
                    validateAjouterAmende(m);
                    if (ModelState.IsValid)
                    {
                        //On utilise le certificatService nouvelleSanction (historisation, annulation agrément et enregistrement sanction)
                        pAmendeCentre a = this._pAmendeCentreService.modifierAmende(m.Amende, this.User.Identity.Name);

                        //On affiche la liste des amendes
                        return this.RedirectToAction("ListeAmendes", new { id = m.Amende.PCentreEncour.Id });
                    }
                    return View("ModifierSanction", m);
                }
                else
                {
                    Exception e = new Exception("Vous n'êtes pas autorisé....");
                    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                }
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
                pAmendeCentre a = this._pAmendeCentreService.FindBy(id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, a.PCentreEncour, GestionDroitsExecution.ActionsFicheCentre.AMEND))
                {
                    return View(a);
                }
                else
                {
                    Exception e = new Exception("Vous n'êtes pas autorisé....");
                    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                }
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

        // POST: /Centre/DeleteAmende/60
        [HttpPost]
        public ActionResult DeleteAmende(int id)
        {
            try
            {
                pAmendeCentre a = this._pAmendeCentreService.FindBy(id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, a.PCentreEncour, GestionDroitsExecution.ActionsFicheCentre.AMEND))
                {
                    // on supprime l'amende
                    this._pAmendeCentreService.deleteAmende(a);

                    //on affiche la liste des sanctions
                    return this.RedirectToAction("ListeAmendes", new { id = a.PCentreEncour.Id });
                }
                else
                {
                    Exception e = new Exception("Vous n'êtes pas autorisé....");
                    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                }
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

        private void validateAjouterAmende(AjouterAmendeModel m)
        {
            if (m.Amende.LibContravention == null)
                ModelState.AddModelError("amende.LibContravention", "Le libellé de l'amende doit être renseigné.");
            if (m.Amende.DateEmisAmende == null)
                ModelState.AddModelError("amende.DateEmisAmende", "La date de l'amende doit être renseignée.");
        }

        // GET: /Centre/ConfirmationRenouvellementAgrementPl/S
        public ActionResult ConfirmationRenouvellementAgrementPl(int id, pCentreEncour s)
        {
            try
            {
                s = _pCentreEnCourservice.FindBy(id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, s, GestionDroitsExecution.ActionsFicheCentre.DERPL))
                {
                    return View(s);
                }
                else
                {
                    Exception e = new Exception("Vous n'êtes pas autorisé....");
                    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                }
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // POST: /Centre/RenouvellementAgrementPl
        [HttpPost]
        public ActionResult RenouvellementAgrementPl(int id)
        {
            try
            {
                pCentreEncour s = this._pCentreEnCourservice.FindBy(id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, s, GestionDroitsExecution.ActionsFicheCentre.DERPL))
                {
                    //On utilise le certificatService RenouvellementAgrementPl (historisation, modification du centre)
                    this._pCentreEnCourservice.renouvellementAgrementPl(id, this.User.Identity.Name);

                    //On affiche la fiche du cente
                    return this.RedirectToAction("Fiche", new { id = id });
                }
                else
                {
                    Exception e = new Exception("Vous n'êtes pas autorisé....");
                    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                }
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // GET: /Centre/ListeRapportException/62
        public ActionResult ListeRapportException(int id, rCategorieVehicule.CategorieEnum categorie)
        {
            try
            {
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);
                if ((categorie.Equals(rCategorieVehicule.CategorieEnum.VL) && GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.TELECHARGE_CE_VL))
                || (categorie.Equals(rCategorieVehicule.CategorieEnum.PL) && GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.TELECHARGE_CE_PL))
                || (categorie.Equals(rCategorieVehicule.CategorieEnum.CL) && GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.TELECHARGE_CE_CL)))
                {
                    RapportExceptionModel m = new RapportExceptionModel();
                    m.Centre = c;
                    m.CategVehicule = categorie;

                    //PLE 20/06/2012
                    if (categorie.Equals(rCategorieVehicule.CategorieEnum.VL))
                    {
                        m.ListeRapportVL = this._pRapportExceptionVLService.FindAllByCentre(c);
                    }

                    //PLE 20/06/2012
                    if (categorie.Equals(rCategorieVehicule.CategorieEnum.PL))
                    {
                        if ("Z".Equals(c.ReseauRattachementPL.Id))
                            m.ListeRapportPL = this._pRapportExceptionService.FindAllByCentre(c);
                        else
                            m.ListeRapportPL = this._pRapportExceptionService.FindAllByReseau(c.ReseauRattachementPL.Id);
                    }

                    //PLE 05/04/2024 Catégorie L
                    if (categorie.Equals(rCategorieVehicule.CategorieEnum.CL))
                    {
                        m.ListeRapportCL = this._pRapportExceptionCLService.FindAllByCentre(c);
                    }

                    return View(m);
                }
                else
                {
                    Exception e = new Exception("Vous n'êtes pas autorisé....");
                    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                }
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        public ActionResult TelechargerRapportException(int id, rCategorieVehicule.CategorieEnum categorie)
        {
            try
            {
                string nomFichierZip = null;
                int idCentre = new int();
                byte[] fichier = null;

                // recuperer le nom du rapport
                switch (categorie)
                {
                    case rCategorieVehicule.CategorieEnum.VL:
                        pRapportExceptionVl rVl = this._pRapportExceptionVLService.FindBy(id);
                        if (rVl == null)
                        {
                            Exception e = new Exception("Accès refusé !");
                            return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                        }
                        nomFichierZip = rVl.NomRapport;
                        idCentre = rVl.PCentreEncour.Id;
                        fichier = rVl.ImgFichier;
                        //EG le 28/11/2011 , cas particulier du propriétaire du centre indépendant
                        if (this.User.IsInRole(rUtilisateur.Profil.CENTRE_INDEPENDANT.ToString()))
                        {
                            if (rVl.DateTelechargementRapport == null)
                            {
                                this._pRapportExceptionVLService.traceTelechargement(id, this.User.Identity.Name);
                            }
                        }

                        //EG le 28/11/2011 , déplacé ici en attendant le CL
                        return File(fichier, "application/download", nomFichierZip);

                        // todo implémenter le chargement des rapports d'exceptions CL
                        //case rCategorieVehicule.CategorieEnum.CL:
                        //    pRapportException rCl = this._pRapportExceptionService.FindBy(id);
                        //    nomFichierZip = rCl.NomRapport;
                        //    break;
                }

                //EG le 28/11/2011 si on est là on fait un return vers la fiche... c pas normal mais bon...
                return RedirectToAction("Fiche", new { id = id });

            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

        // GET: /Centre/ModifierAdresseMail/62
        public ActionResult ModifierAdresseMail(string id)
        {
            try
            {
                rUtilisateur u = this._rUtilisateurService.FindBy(id);
                ModificationCourrielUtilisateurModel m = new ModificationCourrielUtilisateurModel();
                m.Utilisateur = u;
                m.NouveauCourriel = u.Email;
                m.IdCentre = u.MonCentre.Id;

                return View(m);
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // POST: /Centre/ModifierAdresseMail
        /* déplacé dans HomeController 
        [HttpPost]
        public ActionResult ModifierAdresseMail(ModificationCourrielUtilisateurModel m)
        {
            try
            {
                pCentreEncour centre = this._pCentreEnCourservice.FindBy(m.IdCentre);
                validateModifierCourriel(m);
                if (ModelState.IsValid)
                {
                    _rUtilisateurService.updateEmail(this.User.Identity.Name, m.NouveauCourriel, this.User.Identity.Name);
                    rUtilisateur declencheur = InformationHelper.getInformationPersonnelle().Utilisateur;
                    string logoPath = Server.MapPath("~/Ressources/logoOTC.gif");
                    MailTemplateSender sender = new MailTemplateSender(declencheur, declencheur, rTemplateMail.TemplateEnum.NOUVEAU_EMAIL, logoPath);
                    TempData["Message"] = "Adresse email changée...";
                    TempData["MessageType"] = "Success";

                    if (sender.Send())
                    {
                        TempData["Message"] += "email envoyé";
                    }
                    else
                    {
                        TempData["Message"] += "MAIS ECHEC ENVOI MAIL !";
                    }

                    //On affiche l'écran de bienvenue
                    return RedirectToAction("Index", "Home");
                }
                return View("ModifierAdresseMail", m);
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        private void validateModifierCourriel(ModificationCourrielUtilisateurModel m)
        {
            if (m.NouveauCourriel == null)
                ModelState.AddModelError("NouveauCourriel", "La nouvelle adresse courriel doit être renseignée.");
            if (m.ConfirmationCourriel == null)
                ModelState.AddModelError("ConfirmationCourriel", "La confirmation de l'adresse courriel doit être renseignée.");
            if ((m.NouveauCourriel != m.ConfirmationCourriel) && (m.NouveauCourriel != null && m.ConfirmationCourriel != null) )
                ModelState.AddModelError("NouveauCourriel", "Erreur de saisie de l'adresse courriel.");
        }
        */

        //EG le 29/11/2011
        // GET: /Centre/CreateEquipementSpecifiqueVL
        public ActionResult CreateEquipementSpecifiqueVL(int id)
        {
            try
            {
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.EQUIPVL))
                {
                    if (c.EquipementCentreVLs.Count == 0)
                    {
                        c.EquipementCentreVLs.Add(new pEquipementCentreVL());
                    }
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

        [HttpPost]
        public ActionResult CreateEquipementSpecifiqueVL(int id, pCentreEncour model)
        {
            try
            {
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.EQUIPVL))
                {
                    //todo validation
                    this._pEquipementCentreVLService.nouveauEquipementVL(c, model.EquipementCentreVLs[0], this.User.Identity.Name);
                    return RedirectToAction("ModifierEquipementSpecifiqueVL", new { id = id });
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

        public ActionResult ModifierEquipementSpecifiqueVL(int id)
        {
            try
            {
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.EQUIPVL))
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

        [HttpPost]
        public ActionResult ModifierEquipementSpecifiqueVL(int id, pCentreEncour model)
        {
            try
            {
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.EQUIPVL))
                {
                    //todo validation
                    this._pEquipementCentreVLService.modifierEquipementVL(c, model.EquipementCentreVLs[0], this.User.Identity.Name);
                    return RedirectToAction("ModifierEquipementSpecifiqueVL", new { id = id });
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

        public ActionResult VoirEquipementSpecifiqueVL(int id)
        {
            try
            {
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.SHOW_EQUIPVL))
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

        //EG le 01/12/2011
        // GET: /Centre/CreateEquipementSpecifiquePL
        public ActionResult CreateEquipementSpecifiquePL(int id)
        {
            try
            {
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.EQUIPPL))
                {
                    if (c.EquipementCentrePLs.Count == 0)
                    {
                        c.EquipementCentrePLs.Add(new pEquipementCentrePL());
                    }
                    //pour le PL TCP et TMP obligatoire
                    foreach (pEquipementCentrePL e in c.EquipementCentrePLs)
                    {
                        e.ControleTCP = true;
                        e.ControleTMD = true;
                    }
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

        [HttpPost]
        public ActionResult CreateEquipementSpecifiquePL(int id, pCentreEncour model)
        {
            try
            {
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.EQUIPPL))
                {
                    //todo validation
                    this._pEquipementCentrePLService.nouveauEquipementPL(c, model.EquipementCentrePLs[0], this.User.Identity.Name);
                    return RedirectToAction("ModifierEquipementSpecifiquePL", new { id = id });
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

        public ActionResult ModifierEquipementSpecifiquePL(int id)
        {
            try
            {
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.EQUIPPL))
                {
                    //pour le PL TCP et TMP obligatoire
                    foreach (pEquipementCentrePL e in c.EquipementCentrePLs)
                    {
                        e.ControleTCP = true;
                        e.ControleTMD = true;
                    }
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

        [HttpPost]
        public ActionResult ModifierEquipementSpecifiquePL(int id, pCentreEncour model)
        {
            try
            {
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.EQUIPPL))
                {
                    //todo validation
                    this._pEquipementCentrePLService.modifierEquipementPL(c, model.EquipementCentrePLs[0], this.User.Identity.Name);
                    return RedirectToAction("ModifierEquipementSpecifiquePL", new { id = id });
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

        public ActionResult VoirEquipementSpecifiquePL(int id)
        {
            try
            {
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.SHOW_EQUIPPL))
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

        // GET: /Centre/CreateEquipementSpecifiqueCL
        public ActionResult CreateEquipementSpecifiqueCL(int id)
        {
            try
            {
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.EQUIPCL))
                {
                    if (c.EquipementCentreCLs.Count == 0)
                    {
                        c.EquipementCentreCLs.Add(new pEquipementCentreCL());
                    }
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

        [HttpPost]
        public ActionResult CreateEquipementSpecifiqueCL(int id, pCentreEncour model)
        {
            try
            {
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.EQUIPCL))
                {
                    //todo validation
                    this._pEquipementCentreCLService.nouveauEquipementCL(c, model.EquipementCentreCLs[0], this.User.Identity.Name);
                    return RedirectToAction("ModifierEquipementSpecifiqueCL", new { id = id });
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

        public ActionResult ModifierEquipementSpecifiqueCL(int id)
        {
            try
            {
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.EQUIPCL))
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

        [HttpPost]
        public ActionResult ModifierEquipementSpecifiqueCL(int id, pCentreEncour model)
        {
            try
            {
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.EQUIPCL))
                {
                    //todo validation
                    this._pEquipementCentreCLService.modifierEquipementCL(c, model.EquipementCentreCLs[0], this.User.Identity.Name);
                    return RedirectToAction("ModifierEquipementSpecifiqueCL", new { id = id });
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

        public ActionResult VoirEquipementSpecifiqueCL(int id)
        {
            try
            {
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.SHOW_EQUIPCL))
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

        public ActionResult ListeControleurs(int id)
        {
            try
            {
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.SHOW_CONTROLEUR))
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

        //EG le 09/12/201
        //GET
        public ActionResult NouvelAgrement(int id, rCategorieVehicule.CategorieEnum categVehicule)
        {
            try
            {
                // on retrieve l'id
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);

                Dictionary<rCategorieVehicule.CategorieEnum, GestionDroitsExecution.ActionsFicheCentre> actions = new Dictionary<rCategorieVehicule.CategorieEnum, GestionDroitsExecution.ActionsFicheCentre>();
                actions.Add(rCategorieVehicule.CategorieEnum.VL, GestionDroitsExecution.ActionsFicheCentre.NOUVEL_AGREM_VL);
                actions.Add(rCategorieVehicule.CategorieEnum.PL, GestionDroitsExecution.ActionsFicheCentre.NOUVEL_AGREM_PL);
                actions.Add(rCategorieVehicule.CategorieEnum.CL, GestionDroitsExecution.ActionsFicheCentre.NOUVEL_AGREM_CL);

                if (GestionDroitsExecution.isRightActionForCentre(this.User, c, actions[categVehicule]))
                {
                    DemandeAgrementModel d = new DemandeAgrementModel();

                    //utilisation d'une variable de session pour conserver les informations saisies par l'opérateur
                    //quand on lui demande une confirmation de la date d'agrément et qu'il fait un retour arrière pour modifier sa date

                    if (this.Session["Centre"] != null)
                    {
                        d.Centre = (pCentreEncour)this.Session["Centre"];
                        //this.Session.Remove("Centre");
                    }
                    else
                    {
                        d.Centre = c;
                    }

                    //d.Centre = c;
                    d.CategorieVehicule = categVehicule;

                    //on rempli la liste des réseaux en fonction du type d'agrément Vl Pl ou Cl
                    switch (categVehicule)
                    {
                        case rCategorieVehicule.CategorieEnum.VL:
                            //PLE 23/03/2012 différenciation liste réseaux disponibles selon Spécialisé /auxilaires
                            //d.ListeReseau = this._rReseauService.FindAllReseauVLAvecZ();
                            if (d.Centre.TypeCentre == "S")
                            {
                                d.ListeReseau = this._rReseauService.FindAllReseauVLAvecZ();
                            }
                            else
                            {
                                d.ListeReseau = this._rReseauService.FindAllReseauVLSansZ();
                            }
                            if (this.Session["Centre"] == null)
                            {
                                d.Centre.ReseauRattachementVL = null;
                                d.Centre.CodAgremVl = null;
                            }
                            break;
                        case rCategorieVehicule.CategorieEnum.PL:
                            //PLE 23/03/2012 différenciation liste réseaux disponibles selon Spécialisé /auxilaires
                            //d.ListeReseau = this._rReseauService.FindAllReseauPLAvecZ();
                            if (d.Centre.TypeCentre == "S")
                            {
                                d.ListeReseau = this._rReseauService.FindAllReseauPLAvecZ();
                            }
                            else
                            {
                                d.ListeReseau = this._rReseauService.FindAllReseauPLSansZ();
                            }
                            if (this.Session["Centre"] == null)
                            {
                                d.Centre.ReseauRattachementPL = null;
                                d.Centre.CodAgremPl = null;
                            }
                            break;
                        case rCategorieVehicule.CategorieEnum.CL:
                            //PLE 23/03/2012 différenciation liste réseaux disponibles selon Spécialisé /auxilaires
                            //d.ListeReseau = this._rReseauService.FindAllReseauCLAvecW();
                            if (d.Centre.TypeCentre == "S")
                            {
                                d.ListeReseau = this._rReseauService.FindAllReseauCLAvecW();
                            }
                            else
                            {
                                d.ListeReseau = this._rReseauService.FindAllReseauCLSansW();
                            }
                            if (this.Session["Centre"] == null)
                            {
                                d.Centre.ReseauRattachementCL = null;
                                d.Centre.CodAgremCl = null;
                            }
                            break;
                    }

                    this.Session.Remove("Centre");

                    d.ListeReseau.Insert(0, null);

                    //pour les centres non administatifs supprimer Administration de la liste déroulante
                    if (c.RStatutJuridique == null || c.RStatutJuridique.Id != rStatutJuridique.Statuts.ADMIN.ToString())
                    {
                        d.ListeReseau.RemoveAt(d.ListeReseau.IndexOf(_rReseauService.FindBy("A")));
                    }
                    else
                    {
                        d.ListeReseau.Clear();
                        d.ListeReseau.Add(this._rReseauService.FindBy(rReseau.IdCodeReseau.A.ToString()));
                    }

                    //on rempli la liste des statut juridique
                    d.ListeStatut = this._rStatutJuridiqueService.FindAll();
                    //pour les centres non administatifs supprimer "Centre de l'Administration" de la liste déroulante
                    if (c.RStatutJuridique == null || c.RStatutJuridique.Id != rStatutJuridique.Statuts.ADMIN.ToString())
                    {
                        d.ListeStatut.RemoveAt(d.ListeStatut.IndexOf(_rStatutJuridiqueService.FindBy(rStatutJuridique.Statuts.ADMIN.ToString())));
                    }
                    else
                    {
                        d.ListeStatut.Clear();
                        d.ListeStatut.Add(this._rStatutJuridiqueService.FindBy(rStatutJuridique.Statuts.ADMIN.ToString()));
                    }

                    return View(d);

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

        // POST: /Centre/NouvelAgrement
        [HttpPost]
        public ActionResult NouvelAgrement(int id, DemandeAgrementModel d)
        {
            try
            {
                pCentreEncour c = null;
                //// on retrieve l'id
                //if (this.Session["Centre"] != null)
                //{
                //    c = (pCentreEncour)this.Session["Centre"];
                //    this.Session.Remove("Centre");
                //}
                //else
                //{
                c = this._pCentreEnCourservice.FindBy(id);
                //}

                Dictionary<rCategorieVehicule.CategorieEnum, GestionDroitsExecution.ActionsFicheCentre> actions = new Dictionary<rCategorieVehicule.CategorieEnum, GestionDroitsExecution.ActionsFicheCentre>();
                actions.Add(rCategorieVehicule.CategorieEnum.VL, GestionDroitsExecution.ActionsFicheCentre.NOUVEL_AGREM_VL);
                actions.Add(rCategorieVehicule.CategorieEnum.PL, GestionDroitsExecution.ActionsFicheCentre.NOUVEL_AGREM_PL);
                actions.Add(rCategorieVehicule.CategorieEnum.CL, GestionDroitsExecution.ActionsFicheCentre.NOUVEL_AGREM_CL);

                if (GestionDroitsExecution.isRightActionForCentre(this.User, c, actions[d.CategorieVehicule]))
                {
                    validateNouvelAgrement(d);
                    if (ModelState.IsValid)
                    {
                        if (d.DateValide == false)
                        {

                            // YM 31/01/2012
                            //On sauvegarde les données saisies par l'utilisateur avant l'enregistrement
                            c.RaisonSociale = d.Centre.RaisonSociale;
                            c.NumSiren = d.Centre.NumSiren;
                            c.EnseigneCommerciale = d.Centre.EnseigneCommerciale;
                            c.NomRepresentantLegal = d.Centre.NomRepresentantLegal;
                            c.L1Adresse = d.Centre.L1Adresse;
                            c.L2Adresse = d.Centre.L2Adresse;
                            c.CpCentre = d.Centre.CpCentre;
                            c.Ville = d.Centre.Ville;
                            c.NumTel = d.Centre.NumTel;
                            c.RStatutJuridique = d.Centre.RStatutJuridique;


                            switch (d.CategorieVehicule)
                            {
                                case rCategorieVehicule.CategorieEnum.VL:
                                    c.DateEffetNotificationVl = d.Centre.DateEffetNotificationVl;
                                    c.CodAgremVl = d.Centre.CodAgremVl;
                                    c.ReseauRattachementVL = d.Centre.ReseauRattachementVL;
                                    //10/04/2025 PLE Ajout SAV : intialisation à false
                                    c.SvaVL = false;
                                    break;
                                case rCategorieVehicule.CategorieEnum.PL:
                                    c.DateEffetNotificationPl = d.Centre.DateEffetNotificationPl;
                                    c.CodAgremPl = d.Centre.CodAgremPl;
                                    c.ReseauRattachementPL = d.Centre.ReseauRattachementPL;
                                    //10/04/2025 PLE Ajout SAV : intialisation à false
                                    c.SvaPL = false;
                                    break;
                                case rCategorieVehicule.CategorieEnum.CL:
                                    c.DateEffetNotificationCl = d.Centre.DateEffetNotificationCl;
                                    c.CodAgremCl = d.Centre.CodAgremCl;
                                    c.ReseauRattachementCL = d.Centre.ReseauRattachementCL;
                                    //10/04/2025 PLE Ajout SAV : intialisation à false
                                    c.SvaCL = false;
                                    break;
                            }
                            d.Centre = c;
                            this.Session["Centre"] = c;
                            switch (d.CategorieVehicule)
                            {
                                case rCategorieVehicule.CategorieEnum.VL:
                                    if (d.Centre.DateEffetNotificationVl < DateTime.Now.AddDays(1))
                                        return View("ConfirmationDateNotificationNouvelAgrement", d);
                                    break;
                                case rCategorieVehicule.CategorieEnum.PL:
                                    if (d.Centre.DateEffetNotificationPl < DateTime.Now.AddDays(1))
                                        return View("ConfirmationDateNotificationNouvelAgrement", d);
                                    break;
                                case rCategorieVehicule.CategorieEnum.CL:
                                    if (d.Centre.DateEffetNotificationCl < DateTime.Now.AddDays(1))
                                        return View("ConfirmationDateNotificationNouvelAgrement", d);
                                    break;
                            }

                        }
                        c = this._pCentreEnCourservice.nouvelAgrement(id, d.Centre, d.CategorieVehicule, this.User.Identity.Name);
                        this.Session.Remove("Centre");

                        //YM 16/02/2012 on test si @@IDENTITY retourne le nouvel id ou 0
                        //EG 17/02/2012 on test null car changement dans le code de nouvelAgrement()
                        if (c != null && c.Id != 0)
                        {
                            log.Debug("nouvelAgrement ON RECUPERE L ID");
                            //return this.RedirectToAction("Fiche", new { id = c.Id });
                            //YM 20/05/2013 On affiche l'écran de confirmation génération du pdf
                            return this.RedirectToAction("ChoixTelechargerModifNotificationPDF", new { id = c.Id });
                        }

                        //pour gérer le cas ou on récupère 0 à la place de l'id on recherche le nouveau centre avec l'id de l'ancien
                        //EG le 16/02/2012 17h49 ;) pCentreEncour nouveauCentre = this._pCentreEnCourservice.FindNewCentreFor(this._pCentreEnCourservice.FindBy(id));
                        pCentreEncour nouveauCentre = this._pCentreEnCourservice.FindNewCentreFor(id);
                        if (nouveauCentre != null)
                        {
                            log.Debug("nouvelAgrement ON NE RECUPERE PAS L ID ON RECHERCE LE NOUVEL ID");
                            //return this.RedirectToAction("Fiche", new { id = nouveauCentre.Id });
                            //YM 20/05/2013 On affiche l'écran de confirmation génération du pdf
                            return this.RedirectToAction("ChoixTelechargerModifNotificationPDF", new { id = nouveauCentre.Id });
                        }
                        //on ne peut pas afficher le nouveau centre donc on affiche une erreur avec lien sur l'ancienne fiche
                        log.Debug("nouvelAgrement ON NE RECUPERE PAS LES ID MESSAGE D ERREUR");
                        return this.RedirectToAction("ErreurRechercheNouveauCentre", new { id = id, categorie = d.CategorieVehicule });

                        //YM 16/02/2012 on n'affiche plus cette page, le téléchartgement se fera depuis la fiche
                        //return this.RedirectToAction("ResultatNotifierAgrement", new { id = c.Id, categorie = d.CategorieVehicule });
                    }

                    //on rempli la liste des réseaux en fonction du type d'agrément Vl Pl ou Cl
                    switch (d.CategorieVehicule)
                    {
                        case rCategorieVehicule.CategorieEnum.VL:
                            //PLE 23/03/2012 différenciation liste réseaux disponibles selon Spécialisé /auxilaires
                            //d.ListeReseau = this._rReseauService.FindAllReseauVLAvecZ();
                            if (c.TypeCentre == "S")
                            {
                                d.ListeReseau = this._rReseauService.FindAllReseauVLAvecZ();
                            }
                            else
                            {
                                d.ListeReseau = this._rReseauService.FindAllReseauVLSansZ();
                            }
                            d.Centre.ReseauRattachementVL = null;
                            break;
                        case rCategorieVehicule.CategorieEnum.PL:
                            //PLE 23/03/2012 différenciation liste réseaux disponibles selon Spécialisé /auxilaires
                            //d.ListeReseau = this._rReseauService.FindAllReseauPLAvecZ();
                            if (c.TypeCentre == "S")
                            {
                                d.ListeReseau = this._rReseauService.FindAllReseauPLAvecZ();
                            }
                            else
                            {
                                d.ListeReseau = this._rReseauService.FindAllReseauPLSansZ();
                            }
                            d.Centre.ReseauRattachementPL = null;
                            break;
                        case rCategorieVehicule.CategorieEnum.CL:
                            //PLE 23/03/2012 différenciation liste réseaux disponibles selon Spécialisé /auxilaires
                            //d.ListeReseau = this._rReseauService.FindAllReseauCLAvecW();
                            if (c.TypeCentre == "S")
                            {
                                d.ListeReseau = this._rReseauService.FindAllReseauCLAvecW();
                            }
                            else
                            {
                                d.ListeReseau = this._rReseauService.FindAllReseauCLSansW();
                            }
                            d.Centre.ReseauRattachementCL = null;
                            break;
                    }
                    d.ListeReseau.Insert(0, null);

                    //pour les centres non administatifs supprimer Administration de la liste déroulante
                    if (c.RStatutJuridique == null || c.RStatutJuridique.Id != rStatutJuridique.Statuts.ADMIN.ToString())
                    {
                        d.ListeReseau.RemoveAt(d.ListeReseau.IndexOf(_rReseauService.FindBy("A")));
                    }
                    else
                    {
                        d.ListeReseau.Clear();
                        d.ListeReseau.Add(this._rReseauService.FindBy(rReseau.IdCodeReseau.A.ToString()));
                    }

                    //on rempli la liste des statut juridique
                    d.ListeStatut = this._rStatutJuridiqueService.FindAll();
                    //pour les centres non administatifs supprimer "Centre de l'Administration" de la liste déroulante
                    if (c.RStatutJuridique == null || c.RStatutJuridique.Id != rStatutJuridique.Statuts.ADMIN.ToString())
                    {
                        d.ListeStatut.RemoveAt(d.ListeStatut.IndexOf(_rStatutJuridiqueService.FindBy(rStatutJuridique.Statuts.ADMIN.ToString())));
                    }
                    else
                    {
                        d.ListeStatut.Clear();
                        d.ListeStatut.Add(this._rStatutJuridiqueService.FindBy(rStatutJuridique.Statuts.ADMIN.ToString()));
                    }

                    d.Centre = c;
                    return View(d);
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

        // GET: /Centre/ResultatNotifierAgrement
        public ActionResult ErreurRechercheNouveauCentre(int id, rCategorieVehicule.CategorieEnum categorie)
        {
            try
            {
                //1 on retrieve l'id
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);

                NotificationAgrementModel m = new NotificationAgrementModel();
                m.Centre = c;
                m.CategVehicule = categorie;

                return View(m);
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        [HttpPost]
        public ActionResult NouvelAgrementSansVerification(int id, DemandeAgrementModel d)
        {
            try
            {
                validateNouvelAgrement(d);
                if (ModelState.IsValid)
                {
                    pCentreEncour nouveauCentre = this._pCentreEnCourservice.nouvelAgrement(id, d.Centre, d.CategorieVehicule, this.User.Identity.Name);

                    this.Session.Remove("Centre");

                    //YM 16/02/2012 on test si @@IDENTITY retourne le nouvel id ou 0
                    //EG 17/02/2012 on test null car changement dans le code de nouvelAgrement()
                    if (nouveauCentre != null && nouveauCentre.Id != 0)
                    {
                        log.Debug("nouvelAgrement ON RECUPERE L ID");
                        //return this.RedirectToAction("Fiche", new { id = nouveauCentre.Id });
                        //YM 20/05/2013 On affiche l'écran de confirmation génération du pdf
                        return this.RedirectToAction("ChoixTelechargerModifNotificationPDF", new { id = nouveauCentre.Id });
                    }

                    //pour gérer le cas ou on récupère 0 à la place de l'id on recherche le nouveau centre avec l'id de l'ancien
                    //EG le 16/02/2012 17h53 ;) pCentreEncour c1 = this._pCentreEnCourservice.FindNewCentreFor(this._pCentreEnCourservice.FindBy(id));
                    pCentreEncour c1 = this._pCentreEnCourservice.FindNewCentreFor(id);
                    if (c1 != null)
                    {
                        log.Debug("nouvelAgrement ON NE RECUPERE PAS L ID ON RECHERCE LE NOUVEL ID");
                        //return this.RedirectToAction("Fiche", new { id = c1.Id });
                        //YM 20/05/2013 On affiche l'écran de confirmation génération du pdf
                        return this.RedirectToAction("ChoixTelechargerModifNotificationPDF", new { id = c1.Id });
                    }
                    //on ne peut pas afficher le nouveau centre donc on affiche une erreur avec lien sur l'ancienne fiche
                    log.Debug("nouvelAgrement ON NE RECUPERE PAS LES ID MESSAGE D ERREUR");
                    return this.RedirectToAction("ErreurRechercheNouveauCentre", new { id = id, categorie = d.CategorieVehicule });

                    //YM 16/02/2012 on n'affiche plus cette page, le téléchartgement se fera depuis la fiche
                    //return this.RedirectToAction("ResultatNotifierAgrement", new { id = nouveauCentre.Id, categorie = d.CategorieVehicule });

                }
                //Traitement des erreurs car si Ok redirection plus haut
                //on rempli la liste des réseaux en fonction du type d'agrément Vl Pl ou Cl

                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);

                switch (d.CategorieVehicule)
                {
                    case rCategorieVehicule.CategorieEnum.VL:
                        //PLE 23/03/2012 différenciation liste réseaux disponibles selon Spécialisé /auxilaires
                        //d.ListeReseau = this._rReseauService.FindAllReseauVLAvecZ();
                        if (c.TypeCentre == "S")
                        {
                            d.ListeReseau = this._rReseauService.FindAllReseauVLAvecZ();
                        }
                        else
                        {
                            d.ListeReseau = this._rReseauService.FindAllReseauVLSansZ();
                        }
                        d.Centre.ReseauRattachementVL = null;
                        break;
                    case rCategorieVehicule.CategorieEnum.PL:
                        //PLE 23/03/2012 différenciation liste réseaux disponibles selon Spécialisé /auxilaires
                        //d.ListeReseau = this._rReseauService.FindAllReseauPLAvecZ();
                        if (c.TypeCentre == "S")
                        {
                            d.ListeReseau = this._rReseauService.FindAllReseauPLAvecZ();
                        }
                        else
                        {
                            d.ListeReseau = this._rReseauService.FindAllReseauPLSansZ();
                        }
                        d.Centre.ReseauRattachementPL = null;
                        break;
                    case rCategorieVehicule.CategorieEnum.CL:
                        //PLE 23/03/2012 différenciation liste réseaux disponibles selon Spécialisé /auxilaires
                        //d.ListeReseau = this._rReseauService.FindAllReseauCLAvecW();
                        if (c.TypeCentre == "S")
                        {
                            d.ListeReseau = this._rReseauService.FindAllReseauCLAvecW();
                        }
                        else
                        {
                            d.ListeReseau = this._rReseauService.FindAllReseauCLSansW();
                        }
                        d.Centre.ReseauRattachementCL = null;
                        break;
                }

                d.ListeReseau.Insert(0, null);

                //pour les centres non administatifs supprimer Administration de la liste déroulante
                if (c.RStatutJuridique == null || c.RStatutJuridique.Id != rStatutJuridique.Statuts.ADMIN.ToString())
                {
                    d.ListeReseau.RemoveAt(d.ListeReseau.IndexOf(_rReseauService.FindBy("A")));
                }
                else
                {
                    d.ListeReseau.Clear();
                    d.ListeReseau.Add(this._rReseauService.FindBy(rReseau.IdCodeReseau.A.ToString()));
                }

                //on rempli la liste des statut juridique
                d.ListeStatut = this._rStatutJuridiqueService.FindAll();
                //pour les centres non administatifs supprimer "Centre de l'Administration" de la liste déroulante
                if (c.RStatutJuridique == null || c.RStatutJuridique.Id != rStatutJuridique.Statuts.ADMIN.ToString())
                {
                    d.ListeStatut.RemoveAt(d.ListeStatut.IndexOf(_rStatutJuridiqueService.FindBy(rStatutJuridique.Statuts.ADMIN.ToString())));
                }
                else
                {
                    d.ListeStatut.Clear();
                    d.ListeStatut.Add(this._rStatutJuridiqueService.FindBy(rStatutJuridique.Statuts.ADMIN.ToString()));
                }
                d.Centre = c;
                return View("NouvelAgrement", d);
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        //EG le 09/12/2011
        private void validateNouvelAgrement(DemandeAgrementModel m)
        {
            if (m.Centre.RaisonSociale == null)
                ModelState.AddModelError("centre.RaisonSociale", "La raison sociale doit être renseignée.");
            switch (m.CategorieVehicule)
            {
                case rCategorieVehicule.CategorieEnum.VL:
                    if ((m.Centre.ReseauRattachementVL.Id == null) || (m.Centre.ReseauRattachementVL.Id == "-1"))
                        ModelState.AddModelError("centre.ReseauRattachementVL.Id", "Le réseau de rattachement doit être renseigné.");
                    break;
                case rCategorieVehicule.CategorieEnum.PL:
                    if ((m.Centre.ReseauRattachementPL.Id == null) || (m.Centre.ReseauRattachementPL.Id == "-1"))
                        ModelState.AddModelError("centre.ReseauRattachementPL.Id", "Le réseau de rattachement doit être renseigné.");
                    break;
                case rCategorieVehicule.CategorieEnum.CL:
                    if ((m.Centre.ReseauRattachementCL.Id == null) || (m.Centre.ReseauRattachementCL.Id == "-1"))
                        ModelState.AddModelError("centre.ReseauRattachementCL.Id", "Le réseau de rattachement doit être renseigné.");
                    break;
            }

            if (!Validator.IsValidNumeroSiren(m.Centre.NumSiren))
            {
                ModelState.AddModelError("centre.NumSiren", "Le n° SIREN doit contenir 9 chiffres.");
            }

            if (m.Centre.CodDepartement == null)
                ModelState.AddModelError("centre.CodDepartement", "Le département doit être renseigné.");
            if (m.Centre.L1Adresse == null)
                ModelState.AddModelError("centre.L1Adresse", "L'adresse doit être renseigné.");
            if (m.Centre.CpCentre == null)
                ModelState.AddModelError("centre.CpCentre", "Le code postal doit être renseigné.");
            if (m.Centre.Ville == null)
                ModelState.AddModelError("centre.Ville", "La ville doit être renseignée.");

            rPrefecture p;
            p = this._rPrefectureService.findByCodeDepartement(m.Centre.CodDepartement);
            if (p == null)
            {
                if (m.Centre.CodDepartement != null)
                {
                    ModelState.AddModelError("centre.CodDepartement", "Code département inconnu.");
                }
                else
                {
                    ModelState.AddModelError("centre.CodDepartement", "Le département doit être renseigné.");
                }
            }

            switch (m.CategorieVehicule)
            {
                case rCategorieVehicule.CategorieEnum.VL:
                    if (m.Centre.DateEffetNotificationVl == null)
                    {
                        ModelState.AddModelError("centre.DateEffetNotificationVl", "La date d'effet de l'agrément doit être renseignée.");
                    }
                    else
                    {
                        //vérification de l'antériorité de la date
                        if (m.Centre.DateEffetNotificationVl > System.DateTime.Now)
                        {
                            m.DateValide = true;
                        }
                    }
                    break;

                case rCategorieVehicule.CategorieEnum.PL:
                    if (m.Centre.DateEffetNotificationPl == null)
                    {
                        ModelState.AddModelError("centre.DateEffetNotificationPl", "La date d'effet de l'agrément doit être renseignée.");
                    }
                    else
                    {
                        //vérification de l'antériorité de la date
                        if (m.Centre.DateEffetNotificationPl > System.DateTime.Now)
                        {
                            m.DateValide = true;
                        }
                    }

                    if (m.Centre.TypeCentre == "A" && m.Centre.DateEcheanceAgrementPl == null)
                    {
                        ModelState.AddModelError("centre.DateEcheanceAgrementPl", "La date d'échéance de l'agrément doit être renseignée.");
                    }
                    break;

                case rCategorieVehicule.CategorieEnum.CL:
                    if (m.Centre.DateEffetNotificationCl == null)
                    {
                        ModelState.AddModelError("centre.DateEffetNotificationCl", "La date d'effet de l'agrément doit être renseignée.");
                    }
                    else
                    {
                        //vérification de l'antériorité de la date
                        if (m.Centre.DateEffetNotificationCl > System.DateTime.Now)
                        {
                            m.DateValide = true;
                        }
                    }
                    break;

            }

        }

        //EG le 13/12/2011
        // GET: /Centre/CreateInfoRDVPL
        public ActionResult CreateInfoRDVPL(int id)
        {
            try
            {
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.RDV))
                {
                    //YM le 28/03/2012
                    //pour les centre indépendants (Z), les infos sont spécifiques au centre
                    //pour les autres centres (J,K), les infos sont spécifiques au réseau
                    if (c.ReseauRattachementPL.Id == Convert.ToString(rReseau.IdCodeReseau.Z) && c.PInfoRdvs.Count == 0)
                    {
                        c.PInfoRdvs.Add(new pInfoRdv());
                    }
                    if (c.ReseauRattachementPL.Id != Convert.ToString(rReseau.IdCodeReseau.Z) && c.ReseauRattachementPL.PInfoRdvs.Count == 0)
                    {
                        c.ReseauRattachementPL.PInfoRdvs.Add(new pInfoRdv());
                    }
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

        [HttpPost]
        public ActionResult CreateInfoRDVPL(int id, pCentreEncour model)
        {
            try
            {
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.RDV))
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

        public ActionResult VoirInfoRDVPL(int id)
        {
            try
            {
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.RDV))
                {
                    return View(c);
                }
                if (GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.SHOW_RDV))
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

        public ActionResult ModifierInfoRDVPL(int id)
        {
            try
            {
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.RDV))
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

        [HttpPost]
        public ActionResult ModifierInfoRDVPL(int id, pCentreEncour model)
        {
            try
            {
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.RDV))
                {
                    //todo validation
                    if (c.ReseauRattachementPL.Id == Convert.ToString(rReseau.IdCodeReseau.Z))
                    {
                        this._pInfoRdvService.modifierInfoRdv(model, model.PInfoRdvs[0], this.User.Identity.Name);
                    }
                    if (c.ReseauRattachementPL.Id != Convert.ToString(rReseau.IdCodeReseau.Z))
                    {
                        this._pInfoRdvService.modifierInfoRdv(model.ReseauRattachementPL, model.ReseauRattachementPL.PInfoRdvs[0], this.User.Identity.Name);
                    }
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

        //EG le 14/12/2011
        // GET: /Centre/CreateHoraireOuverturePL
        public ActionResult CreateHoraireOuverturePL(int id)
        {
            try
            {
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.CHHORAI))
                {
                    if (c.PHoraireOuvertures.Count == 0)
                    {
                        c.PHoraireOuvertures.Add(new pHoraireOuverture());
                    }
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

        [HttpPost]
        public ActionResult CreateHoraireOuverturePL(int id, pCentreEncour model)
        {
            try
            {
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.CHHORAI))
                {
                    if (ModelState.IsValid)
                    {
                        this._pHoraireOuvertureService.nouvelleHoraire(model, model.PHoraireOuvertures[0], this.User.Identity.Name);
                        TempData["Message"] = "Horaires d'ouverture enregistrées...";
                        TempData["MessageType"] = "Success";

                        return RedirectToAction("ModifierHoraireOuverturePL", new { id = id });
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

        public ActionResult VoirHoraireOuverturePL(int id)
        {
            try
            {
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.SHOW_HORAIRE_OUVERTURE))
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

        public ActionResult ModifierHoraireOuverturePL(int id)
        {
            try
            {
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.CHHORAI))
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

        [HttpPost]
        public ActionResult ModifierHoraireOuverturePL(int id, pCentreEncour model)
        {
            try
            {
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.CHHORAI))
                {
                    if (ModelState.IsValid)
                    {
                        this._pHoraireOuvertureService.modifierHoraire(model, model.PHoraireOuvertures[0], this.User.Identity.Name);
                        TempData["Message"] = "Horaires d'ouverture modifiées...";
                        TempData["MessageType"] = "Success";

                        return RedirectToAction("ModifierHoraireOuverturePL", new { id = id });
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

        private void validateHoraireOuverture(pCentreEncour model)
        {

        }

        //EG le 19/12/2011
        // GET: /Centre/CreateJournalActivite
        public ActionResult CreateJournalActivite(int id)
        {
            try
            {
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.JOURNAL))
                {
                    JournalActiviteModel model = new JournalActiviteModel();
                    model.Centre = c;
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
        public ActionResult CreateJournalActivite(int id, JournalActiviteModel model)
        {
            try
            {
                model.Centre = this._pCentreEnCourservice.FindBy(id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, model.Centre, GestionDroitsExecution.ActionsFicheCentre.JOURNAL))
                {
                    if (ModelState.IsValid)
                    {
                        pJournalActivite journal = this._pJournalActiviteService.nouveauJournal(model.Centre, model.Journal, this.User.Identity.Name);
                        TempData["Message"] = "Une entrée de journal a été créée...";
                        TempData["MessageType"] = "Success";

                        return RedirectToAction("ModifierJournalActivite", new { id = journal.Id });
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

        public ActionResult VoirJournalActivite(int id)
        {
            try
            {
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.JOURNAL))
                {
                    return View(c.Journaux);
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

        public ActionResult ModifierJournalActivite(int id)
        {
            try
            {
                pJournalActivite j = this._pJournalActiviteService.FindBy(id);

                if (GestionDroitsExecution.isRightActionForCentre(this.User, j.CentreEncour, GestionDroitsExecution.ActionsFicheCentre.JOURNAL))
                {
                    JournalActiviteModel model = new JournalActiviteModel();
                    model.Centre = j.CentreEncour;
                    model.Journal = j;

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
        public ActionResult ModifierJournalActivite(int id, JournalActiviteModel model)
        {
            try
            {
                pJournalActivite j = this._pJournalActiviteService.FindBy(id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, j.CentreEncour, GestionDroitsExecution.ActionsFicheCentre.JOURNAL))
                {
                    if (ModelState.IsValid)
                    {
                        this._pJournalActiviteService.modifierJournal(model.Journal, this.User.Identity.Name);
                        TempData["Message"] = "Entrée du journal modifiée...";
                        TempData["MessageType"] = "Success";

                        return RedirectToAction("ModifierJournalActivite", new { id = id });
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

        public ActionResult SupprimerJournalActivite(int id)
        {
            try
            {
                pJournalActivite j = this._pJournalActiviteService.FindBy(id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, j.CentreEncour, GestionDroitsExecution.ActionsFicheCentre.JOURNAL))
                {
                    return View(j);
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
        public ActionResult SupprimerJournalActivite(pJournalActivite j)
        {
            try
            {
                j = this._pJournalActiviteService.FindBy(j.Id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, j.CentreEncour, GestionDroitsExecution.ActionsFicheCentre.JOURNAL))
                {
                    this._pJournalActiviteService.deleteJournal(j);
                    TempData["Message"] = "Entrée du journal supprimée...";
                    TempData["MessageType"] = "Success";

                    return RedirectToAction("VoirJournalActivite", new { id = j.CentreEncour.Id });

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

        //EG le 06/01/2012
        public ActionResult ListeAvecCertificatExpireOuRevoque(rCategorieVehicule.CategorieEnum categorie)
        {
            try
            {
                if (this.User.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()))
                {
                    IList<ItemListeDesInfosCertificatsExpireOuRevoque> liste = this._pCentreEnCourservice.findAllAgreeAvecCertificatExpireOuRevoque(categorie);
                    ListeDesInfosCertificatsExpireOuRevoqueModel model = new ListeDesInfosCertificatsExpireOuRevoqueModel() { Liste = liste, Categorie = categorie };
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

        //PLE le 19/03/2012
        public ActionResult TelechargeFichePDF(int id)
        {
            try
            {
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);
                if (c != null)
                {
                    if (GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.TELECHARGE_FICHE_CENTRE))
                    {
                        //On charge le document Crystal
                        ReportDocument boReportDocument = new ReportDocument();
                        string rptFileName = "~/bin/reporting/cntr_ficheInstallation.rpt";
                        boReportDocument.Load(Server.MapPath(rptFileName));

                        // Chargement du DATASET de la fiche contrôleur dans l'état crystal
                        DSficheInstallation ds = new DSficheInstallation();
                        boReportDocument.SetDataSource(ds.mapInstallationDataset(c));

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
                        return File(memStream.GetBuffer(), "application/pdf", "RNC2ficheInstallation" + (c.CodAgremVl == null ? "" : "_" + c.CodAgremVl) + (c.CodAgremPl == null ? "" : "_" + c.CodAgremPl) + ".pdf");

                    }
                }

                throw new NotImplementedException();
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        //29/09/2014 PLE Supression des actions sur la facture remplacé par des appels directes au controleur FACTURE dans la vue
        //
        //// GET: /Centre/listeFactureCentre
        //public ActionResult listeFactureCentre(int id, rCategorieVehicule.CategorieEnum categorie)
        //{
        //    try
        //    {
        //        if (this.User.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || this.User.IsInRole(rUtilisateur.Profil.OTC_Consultation.ToString()) )
        //        {
        //            FactureModel model = new FactureModel();
        //            model.MonCentre = this._pCentreEnCourservice.FindBy(id);
        //            model.Categorie = categorie;
        //            //rechercher l'utilisateur lié au centre VL, PL ou CL
        //            IEnumerable<rUtilisateur> utilisateurs = this._rUtilisateurService.GetUserByCentre(model.MonCentre, categorie);
        //            if (utilisateurs.Count() != 0)
        //            {
        //                model.UtilisateurCentre = utilisateurs.ElementAt(0);
        //                return View("ListeFactureCentre", model);
        //            }
        //            else
        //            {
        //                Exception e = new Exception("Aucune facture disponible...");
        //                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
        //            }
        //        }
        //        else
        //        {
        //            return ErrorHelper.InvaliAccessError(this.ControllerContext);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
        //    }
        //}

        ////
        //// GET: /Centre/ListeFactureReseau
        //public ActionResult ListeFactureReseau(string id)
        //{
        //    // récupération du réseau
        //    FactureModel model = new FactureModel();
        //    model.Reseau = this._rReseauService.FindBy(id);

        //    if ((GestionDroitsExecution.isUtilisateurAppartientReseau(this.User, model.Reseau)) || this.User.IsInRole(rUtilisateur.Profil.OTC.ToString()))
        //    {
        //        //model.Reseau.PFactures = this._factureService.FindAllByReseau(id);
        //        return View("ListeFactureReseau", model);
        //    }
        //    else
        //    {
        //        return ErrorHelper.InvaliAccessError(this.ControllerContext);
        //    }
        //}

        //PLE le 22/01/2024
        // GET: /Centre/DeclarerExtensionAgrementCatL
        public ActionResult DeclarerExtensionAgrementCatL(int id, rCategorieVehicule.CategorieEnum Categorie, String ReturnLettreReseau, String ReturnCodDep)
        {
            try
            {
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);

                if (
                    (rCategorieVehicule.CategorieEnum.VL.Equals(Categorie)) && (GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.EXTDECLA_VL))
                    ||
                    (rCategorieVehicule.CategorieEnum.PL.Equals(Categorie)) && (GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.EXTDECLA_PL))
                   )
                {
                    ExtensionAgrementCatLModel model = new ExtensionAgrementCatLModel();
                    model.Centre = c;
                    model.Categorie = Categorie;
                    model.Centre.ExtAgremClStatut = null;
                    model.Centre.ExtAgremClDateValid = new DateTime(2024, 04, 15);
                    model.ReturnLettreReseau = ReturnLettreReseau;
                    model.ReturnCodDep = ReturnCodDep;

                    //PLE le 08/03/2024 pour les centres non rattachés,vérifier que le certificat initial a été téléchargé
                    if (
                        (rCategorieVehicule.CategorieEnum.VL.Equals(Categorie) && c.ReseauRattachementVL.Id.Equals(rReseau.IdCodeReseau.Z.ToString()))
                        ||
                        (rCategorieVehicule.CategorieEnum.PL.Equals(Categorie) && c.ReseauRattachementPL.Id.Equals(rReseau.IdCodeReseau.Z.ToString()))
                       )
                    {
                        //PLE le 08/03/2024 pour les centres non rattachés,vérifier que le certificat initial a été téléchargé
                        if (this._pGestionCertificatService.findLastValideOuNonValide(c, Categorie) == null)
                        {
                            //PLE le 08/03/2024, Avant de déclarer votre Extension d'agrément, vous devez télécharger votre certiifcat initiale
                            return View("DeclarerExtensionAgrementCatLImpossible", model);
                        }
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

        [HttpPost]
        public ActionResult DeclarerExtensionAgrementCatL(int id, ExtensionAgrementCatLModel model)
        {
            try
            {
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);
                if (
                    (rCategorieVehicule.CategorieEnum.VL.Equals(model.Categorie)) && (GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.EXTDECLA_VL))
                    ||
                    (rCategorieVehicule.CategorieEnum.PL.Equals(model.Categorie)) && (GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.EXTDECLA_PL))
                   )
                {
                    validateDeclarerExtensionAgrementCatL(model);
                    if (ModelState.IsValid)
                    {
                        this._pCentreEnCourservice.EnregistrerExtensionAgrementCatL(id, model.Centre, model.Categorie, this.User.Identity.Name);
                        TempData["Message"] = "Déclaration de l'extension d'agrément Catégorie L enregistrée ...";
                        TempData["MessageType"] = "Success";

                        if (!String.IsNullOrEmpty(model.ReturnLettreReseau))
                        {
                            return this.RedirectToAction("ReturnToListeCentrePourReseau", "Home", new { returnLettreReseau = model.ReturnLettreReseau, returnCodDep = model.ReturnCodDep });
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

        private void validateDeclarerExtensionAgrementCatL(ExtensionAgrementCatLModel model)
        {
            if (model.Centre.ExtAgremClStatut == null)
            {
                ModelState.AddModelError("Centre.ExtAgremClStatut", "Sélectionnez <OUI> pour confirmer votre déclaration d'extension d'agrément");
            }
            if (model.Centre.ExtAgremClDateValid == null)
            {
                ModelState.AddModelError("Centre.ExtAgremClDateValid", "La date de début de validité de l'extension d'agrément doit être renseignée");
            }
            if (model.Centre.ExtAgremClDateValid < new DateTime(2024, 04, 15) || model.Centre.ExtAgremClDateValid > new DateTime(2025, 08, 31))
            {
                ModelState.AddModelError("Centre.ExtAgremClDateValid", "La date de début de validité de l'extension d'agrément doit être comprise entre le 15/04/2024 et le 31/08/2025");
            }


        }


        //PLE le 22/01/2024
        // GET: /Centre/RetirerExtensionAgrementCatL
        public ActionResult RetirerExtensionAgrementCatL(int id, rCategorieVehicule.CategorieEnum Categorie)
        {
            try
            {
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);
                if (
                    (rCategorieVehicule.CategorieEnum.VL.Equals(Categorie)) && (GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.EXTRETRA_VL))
                    ||
                    (rCategorieVehicule.CategorieEnum.PL.Equals(Categorie)) && (GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.EXTRETRA_PL))
                   )
                {
                    ExtensionAgrementCatLModel model = new ExtensionAgrementCatLModel();
                    model.Centre = c;
                    model.Categorie = Categorie;
                    model.Centre.ExtAgremClStatut = null;
                    model.Centre.ExtAgremClDateValid = null;

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
        public ActionResult RetirerExtensionAgrementCatL(int id, ExtensionAgrementCatLModel model)
        {
            try
            {
                pCentreEncour c = this._pCentreEnCourservice.FindBy(id);
                if (
                    (rCategorieVehicule.CategorieEnum.VL.Equals(model.Categorie)) && (GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.EXTRETRA_VL))
                    ||
                    (rCategorieVehicule.CategorieEnum.PL.Equals(model.Categorie)) && (GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.EXTRETRA_PL))
                   )
                {
                    validateRetirerExtensionAgrementCatL(model, c);
                    if (ModelState.IsValid)
                    {
                        this._pCentreEnCourservice.EnregistrerExtensionAgrementCatL(id, model.Centre, model.Categorie, this.User.Identity.Name);
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

        private void validateRetirerExtensionAgrementCatL(ExtensionAgrementCatLModel model, pCentreEncour c)
        {
            if (model.Centre.ExtAgremClStatut == null)
            {
                ModelState.AddModelError("Centre.ExtAgremClStatut", "Sélectionnez <NON> pour confirmer votre retrait d'extension d'agrément");
            }
            if (model.Centre.ExtAgremClDateValid == null)
            {
                ModelState.AddModelError("Centre.ExtAgremClDateValid", "La date de fin de validité de l'extension d'agrément doit être renseignée");
            }
            else
            if (model.Centre.ExtAgremClDateValid.Value.CompareTo(c.ExtAgremClDateValid.Value) < 0)
            {
                ModelState.AddModelError("Centre.ExtAgremClDateValid", "La date de fin de validité de l'extension d'agrément doit être postérieure à la date de début de validité (" + c.ExtAgremClDateValid.Value.ToString("dd/MM/yyyy") + ")");
            }
            else
            if (model.Centre.ExtAgremClDateValid < new DateTime(2024, 04, 15) || model.Centre.ExtAgremClDateValid > new DateTime(2025, 08, 31))
            {
                ModelState.AddModelError("Centre.ExtAgremClDateValid", "La date de fin de validité de l'extension d'agrément doit être comprise entre le 15/04/2024 et le 31/08/2025");
            }


        }

    }
}
