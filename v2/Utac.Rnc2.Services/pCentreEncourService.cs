using System;
using System.Collections.Generic;
using System.Linq;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Repositories;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Infrastructure;
using Utac.Rnc2.Infrastructure.Queries;
using Utac.Rnc2.Infrastructure.Utilitaires;

namespace Utac.Rnc2.Services
{
    public partial class pCentreEncourService : BaseService<pCentreEncour>, IpCentreEncourService
    {

        private readonly IpCentreHistoService _histoService;
        private readonly IrMotifModificationService _motifModificationService;
        private readonly IrUtilisateurService _utilisateurService;
        private readonly IrNumordreAgrementService _numOrdreAgrementService;
        private readonly IrPrefectureService _prefectureService;
        private readonly IrReseauService _reseauService;
        private readonly IrStatutJuridiqueService _statutJuridiqueService;
        private readonly IpGestionCertificatService _gestionCertificatService;
        private readonly IrProfilService _profilService;
        private readonly IrProfil2Service _profil2Service;
        private readonly IUtilisateurWebServiceVLService _utilisateurWebServiceVL;
        private readonly IUtilisateurWebServicePLService _utilisateurWebServicePL;
        private readonly IUtilisateurWebServiceCLService _utilisateurWebServiceCL;

        public pCentreEncourService(
                                    IpCentreEncourRepository repository,
                                    IUnitOfWork UnitOfWork,
                                    IpCentreHistoService histoService,
                                    IrMotifModificationService motifModificationService,
                                    IrUtilisateurService utilisateurService,
                                    IrNumordreAgrementService numOrdreAgrementService,
                                    IrPrefectureService prefectureService,
                                    IrReseauService reseauService,
                                    IrStatutJuridiqueService statutJuridiqueService,
                                    IpGestionCertificatService gestionCertificatService,
                                    IrProfilService profilService,
                                    IrProfil2Service profil2Service,
                                    IUtilisateurWebServiceVLService utilisateurWebServiceVL,
                                    IUtilisateurWebServicePLService utilisateurWebServicePL,
                                    IUtilisateurWebServiceCLService utilisateurWebServiceCL
            )
            : base(repository, UnitOfWork)
        {
            this._histoService = histoService;
            this._motifModificationService = motifModificationService;
            this._utilisateurService = utilisateurService;
            this._numOrdreAgrementService = numOrdreAgrementService;
            this._prefectureService = prefectureService;
            this._reseauService = reseauService;
            this._statutJuridiqueService = statutJuridiqueService;
            this._gestionCertificatService = gestionCertificatService;
            this._profilService = profilService;
            this._profil2Service = profil2Service;
            this._utilisateurWebServiceVL = utilisateurWebServiceVL;
            this._utilisateurWebServicePL = utilisateurWebServicePL;
            this._utilisateurWebServiceCL = utilisateurWebServiceCL;


        }

        public IList<pCentreEncour> findAllPL()
        {
            IpCentreEncourRepository repository = this._Repository as IpCentreEncourRepository;
            return repository.findAllPL();
        }

        public IList<pCentreEncour> findAllVL()
        {
            IpCentreEncourRepository repository = this._Repository as IpCentreEncourRepository;
            return repository.findAllVL();
        }

        private struct ListeProprietesQueryAllAgreeSansCertificatSSL
        {
            public string PGestionCertificatProp;
            public string CodeAgremProp;
            public string EtatAgremProp;
            public string ReseauRattachementIdProp;
        }

        public IList<pCentreEncour> findAllAgreeSansCertificatSSL(rCategorieVehicule.CategorieEnum categorie)
        {
            Dictionary<rCategorieVehicule.CategorieEnum, ListeProprietesQueryAllAgreeSansCertificatSSL> ListePropriete = new Dictionary<rCategorieVehicule.CategorieEnum, ListeProprietesQueryAllAgreeSansCertificatSSL>();
            ListePropriete.Add(rCategorieVehicule.CategorieEnum.VL, new ListeProprietesQueryAllAgreeSansCertificatSSL() { PGestionCertificatProp = "PGestionCertificatVLs", CodeAgremProp = "CodAgremVl", EtatAgremProp = "EtatAgrementVl", ReseauRattachementIdProp = "ReseauRattachementVL.Id" });
            ListePropriete.Add(rCategorieVehicule.CategorieEnum.PL, new ListeProprietesQueryAllAgreeSansCertificatSSL() { PGestionCertificatProp = "PGestionCertificatPLs", CodeAgremProp = "CodAgremPl", EtatAgremProp = "EtatAgrementPl", ReseauRattachementIdProp = "ReseauRattachementPL.Id" });
            ListePropriete.Add(rCategorieVehicule.CategorieEnum.CL, new ListeProprietesQueryAllAgreeSansCertificatSSL() { PGestionCertificatProp = "PGestionCertificatCLs", CodeAgremProp = "CodAgremCl", EtatAgremProp = "EtatAgrementCl", ReseauRattachementIdProp = "ReseauRattachementCL.Id" });

            Query q = new Query();
            //pas de certificats
            q.Criteria.Add(new Criteria(ListePropriete[categorie].PGestionCertificatProp, null, CriteriaOperator.IS_EMPTY));
            //c un centre VL
            q.Criteria.Add(new Criteria(ListePropriete[categorie].CodeAgremProp, null, CriteriaOperator.IS_NOT_NULL));
            //un centre agree
            q.Criteria.Add(new Criteria(ListePropriete[categorie].EtatAgremProp, "1", CriteriaOperator.EQUALS));
            //un centre indépendant
            q.Criteria.Add(new Criteria(ListePropriete[categorie].ReseauRattachementIdProp, rReseau.IdCodeReseau.Z.ToString(), CriteriaOperator.EQUALS));

            q.OrderBy.Add(new OrderBy() { PropertyName = ListePropriete[categorie].CodeAgremProp, Ascending = true });

            return this._Repository.FindBy(q);
        }

        public IList<pCentreEncour> findAllPLAgrees()
        {
            Query q = new Query();
            //un centre VL
            q.Criteria.Add(new Criteria("CodAgremPl", null, CriteriaOperator.IS_NOT_NULL));
            //un centre agree
            q.Criteria.Add(new Criteria("DateFinAgrementPl", null, CriteriaOperator.IS_NULL));
            return this._Repository.FindBy(q);
        }

        public IList<pCentreEncour> findAllVLAgrees()
        {
            Query q = new Query();
            //un centre VL
            q.Criteria.Add(new Criteria("CodAgremVl", null, CriteriaOperator.IS_NOT_NULL));
            //un centre agree
            q.Criteria.Add(new Criteria("DateFinAgrementVl", null, CriteriaOperator.IS_NULL));
            return this._Repository.FindBy(q);
        }

        public void modifyDenominationSociale(object id, pCentreEncour centre, string codeUtilisateur)
        {
            //1 récupération du centre
            pCentreEncour c = this.FindBy(id);

            IUnitOfWork transaction = this.getUnitOfWork();
            if (transaction != null)
            {
                this._histoService.setUnitOfWorks(transaction);
            }

            //2 on historise
            this._histoService.historise(c);

            //3 On modifie les informations du centre 
            c.RaisonSociale = centre.RaisonSociale.ToUpper();
            c.DateEffetModification = centre.DateEffetModification;

            //données système de suivi
            c.DateMiseaJour = System.DateTime.Now;
            c.RUtilisateur = this._utilisateurService.FindBy(codeUtilisateur);
            c.RMotifModification = this._motifModificationService.FindBy(rMotifModification.TypeModification.CHDECOM.ToString());

            //4 On sauvegarde le centre
            base.Save(c);

            if (transaction != null)
                transaction.Commit();

        }

        public void modifierFiche(object id, pCentreEncour centre, string codeUtilisateur)
        {
            //1 récupération du centre
            pCentreEncour c = this.FindBy(id);

            IUnitOfWork transaction = this.getUnitOfWork();
            if (transaction != null)
            {
                this._histoService.setUnitOfWorks(transaction);
            }

            //3 On modifie les informations du centre 
            c.RaisonSociale = centre.RaisonSociale.ToUpper();
            c.NumSiren = centre.NumSiren;
            c.EnseigneCommerciale = centre.EnseigneCommerciale;
            c.L1Adresse = centre.L1Adresse;
            c.L2Adresse = centre.L2Adresse;
            c.CpCentre = centre.CpCentre;
            c.Ville = centre.Ville.ToUpper();
            c.NumTel = centre.NumTel;
            c.NomRepresentantLegal = centre.NomRepresentantLegal == null ? null : centre.NomRepresentantLegal.ToUpper();
            c.TypeCentre = centre.TypeCentre;
            c.CodDepartement = centre.CodDepartement;
            c.CodAgremVl = centre.CodAgremVl;
            c.DateEffetNotificationVl = centre.DateEffetNotificationVl;
            c.AncienNumeroAgrementVl = centre.AncienNumeroAgrementVl;
            c.CodAgremPl = centre.CodAgremPl;
            c.AncienNumeroAgrementPl = centre.AncienNumeroAgrementPl;
            c.DateEffetNotificationPl = centre.DateEffetNotificationPl;
            c.DateFinAgrementPl = centre.DateFinAgrementPl;
            c.DateEcheanceAgrementPl = centre.DateEcheanceAgrementPl;
            c.DateFinAgrementVl = centre.DateFinAgrementVl;
            c.MotifFinAgrementPl = centre.MotifFinAgrementPl;
            c.MotifFinAgrementVl = centre.MotifFinAgrementVl;
            c.CommentaireAvisOtcVl = centre.CommentaireAvisOtcVl;
            c.EtatAgrementVl = centre.EtatAgrementVl;
            c.EtatAgrementPl = centre.EtatAgrementPl;
            c.CommentaireAvisOtcPl = centre.CommentaireAvisOtcPl;
            c.DateEffetModification = centre.DateEffetModification;
            c.NumFax = centre.NumFax;
            c.NumAccuseReceptionVL = centre.NumAccuseReceptionVL;
            c.NumAccuseReceptionPL = centre.NumAccuseReceptionPL;
            c.AccesPublicVl = centre.AccesPublicVl;
            c.AccesPublicPl = centre.AccesPublicPl;
            //PLE le 29/01/2024 Extensin catégorie L
            c.ExtAgremClStatut = centre.ExtAgremClStatut;
            c.ExtAgremClDateValid = centre.ExtAgremClDateValid;
            c.ExtAgremClDateDecla = centre.ExtAgremClDateDecla;
            c.ExtAgremClCatVeh = centre.ExtAgremClCatVeh;

            //23/02/2024 PLE Catégorie L

            c.CodAgremCl = centre.CodAgremCl;
            c.DateEffetNotificationCl = centre.DateEffetNotificationCl;
            c.AncienNumeroAgrementCl = centre.AncienNumeroAgrementCl;
            c.DateFinAgrementCl = centre.DateFinAgrementCl;
            c.MotifFinAgrementCl = centre.MotifFinAgrementCl;
            c.CommentaireAvisOtcCl = centre.CommentaireAvisOtcCl;
            c.EtatAgrementCl = centre.EtatAgrementCl;
            c.NumAccuseReceptionCL = centre.NumAccuseReceptionCL;
            c.AccesPublicCl = centre.AccesPublicCl;


            //4 On sauvegarde le centre
            base.Save(c);

            if (transaction != null)
                transaction.Commit();

        }

        public void modifyRepresentantLegal(object id, string representantlegal, string codeUtilisateur)
        {
            //1 récupération du centre
            pCentreEncour c = this.FindBy(id);

            IUnitOfWork transaction = this.getUnitOfWork();
            if (transaction != null)
            {
                this._histoService.setUnitOfWorks(transaction);
            }

            //2 on historise
            this._histoService.historise(c);

            //3 On modifie les informations du centre 
            c.NomRepresentantLegal = representantlegal == null ? null : representantlegal.ToUpper();

            //données système de suivi
            c.DateMiseaJour = System.DateTime.Now;
            //c.DateEffetModification = System.DateTime.Now;
            c.RUtilisateur = this._utilisateurService.FindBy(codeUtilisateur);
            c.RMotifModification = this._motifModificationService.FindBy(rMotifModification.TypeModification.CHREPLEG.ToString());

            //4 On sauvegarde le controlcentreeur
            base.Save(c);

            if (transaction != null)
                transaction.Commit();

        }

        public void modifyAutreModification(object id, string numsiren, string nomrepresentantlegal, string enseignecommerciale, string numtel, rStatutJuridique StatutJuridique, string codeUtilisateur)
        {
            //1 récupération du centre
            pCentreEncour c = this.FindBy(id);

            IUnitOfWork transaction = this.getUnitOfWork();
            if (transaction != null)
            {
                this._histoService.setUnitOfWorks(transaction);
            }

            //2 on historise
            this._histoService.historise(c);

            //3 On modifie les informations du centre 
            c.NumSiren = numsiren;
            c.NomRepresentantLegal = nomrepresentantlegal == null ? null : nomrepresentantlegal.ToUpper();
            c.EnseigneCommerciale = enseignecommerciale;
            c.NumTel = numtel;
            if (StatutJuridique != null && !"-1".Equals(StatutJuridique.Id))
                c.RStatutJuridique = StatutJuridique;
            else
                c.RStatutJuridique = null;

            //recherche du rutilisateur
            c.RUtilisateur = this._utilisateurService.FindBy(codeUtilisateur);

            //données système de suivi
            c.DateMiseaJour = System.DateTime.Now;
            //c.DateEffetModification = System.DateTime.Now;
            c.RUtilisateur = this._utilisateurService.FindBy(codeUtilisateur);
            c.RMotifModification = this._motifModificationService.FindBy(rMotifModification.TypeModification.MODAUTRE.ToString());

            //4 On sauvegarde le controlcentreeur
            base.Save(c);

            if (transaction != null)
                transaction.Commit();

        }

        public void demandeAgrement(object id, pCentreEncour centreformulaire, rCategorieVehicule.CategorieEnum categVehicule, string codeUtilisateur)
        {
            //1 récupération du centre
            pCentreEncour c = this.FindBy(id);

            IUnitOfWork transaction = this.getUnitOfWork();
            if (transaction != null)
            {
                this._histoService.setUnitOfWorks(transaction);
            }

            //2 on historise
            this._histoService.historise(c);

            //3 On modifie les informations du centre 
            c.RaisonSociale = centreformulaire.RaisonSociale;
            c.NumSiren = centreformulaire.NumSiren;
            c.EnseigneCommerciale = centreformulaire.EnseigneCommerciale;
            c.L1Adresse = centreformulaire.L1Adresse;
            c.L2Adresse = centreformulaire.L2Adresse;
            c.CpCentre = centreformulaire.CpCentre;
            c.Ville = centreformulaire.Ville;
            c.NumTel = centreformulaire.NumTel;
            switch (categVehicule)
            {
                case rCategorieVehicule.CategorieEnum.VL:
                    c.EtatAgrementVl = "2";
                    //EG le 11/08/2011 suppression de la colonne cod_reseau
                    c.ReseauRattachementVL = centreformulaire.ReseauRattachementVL;
                    c.CodAgremVl = null;
                    c.DateEffetNotificationVl = null;
                    c.DateFinAgrementVl = null;
                    c.MotifFinAgrementVl = null;
                    c.CommentaireAvisOtcVl = null;
                    break;
                case rCategorieVehicule.CategorieEnum.PL:
                    c.EtatAgrementPl = "2";
                    //EG le 11/08/2011 suppression de la colonne cod_reseau
                    c.ReseauRattachementPL = centreformulaire.ReseauRattachementPL;
                    c.CodAgremPl = null;
                    c.DateEffetNotificationPl = null;
                    c.DateFinAgrementPl = null;
                    c.MotifFinAgrementPl = null;
                    c.CommentaireAvisOtcPl = null;
                    c.DateEcheanceAgrementPl = null;
                    break;
                case rCategorieVehicule.CategorieEnum.CL:
                    c.EtatAgrementCl = "2";
                    //EG le 11/08/2011 suppression de la colonne cod_reseau
                    c.ReseauRattachementCL = centreformulaire.ReseauRattachementCL;
                    c.CodAgremCl = null;
                    c.DateEffetNotificationCl = null;
                    c.DateFinAgrementCl = null;
                    c.MotifFinAgrementCl = null;
                    c.CommentaireAvisOtcCl = null;
                    break;
            }

            c.NomRepresentantLegal = centreformulaire.NomRepresentantLegal == null ? null : centreformulaire.NomRepresentantLegal;
            c.RStatutJuridique = centreformulaire.RStatutJuridique;
            //EG le 11/08/2011 suppression de la colonne cod_reseau
            //c.RReseau = centreformulaire.RReseau;

            //recherche du rutilisateur
            c.RUtilisateur = this._utilisateurService.FindBy(codeUtilisateur);

            //données système de suivi
            c.DateMiseaJour = System.DateTime.Now;
            //c.DateEffetModification = System.DateTime.Now;
            c.RUtilisateur = this._utilisateurService.FindBy(codeUtilisateur);
            switch (categVehicule)
            {
                case rCategorieVehicule.CategorieEnum.VL:
                    c.RMotifModification = this._motifModificationService.FindBy(rMotifModification.TypeModification.DEAVL.ToString());
                    break;
                case rCategorieVehicule.CategorieEnum.PL:
                    c.RMotifModification = this._motifModificationService.FindBy(rMotifModification.TypeModification.DEAPL.ToString());
                    break;
                case rCategorieVehicule.CategorieEnum.CL:
                    c.RMotifModification = this._motifModificationService.FindBy(rMotifModification.TypeModification.DEACL.ToString());
                    break;
            }

            //4 On sauvegarde le controlcentreeur
            base.Save(c);

            if (transaction != null)
                transaction.Commit();

        }

        public void retourArriere(object id, bool userMode, string codeUtilisateur, bool deleteHisto)
        {
            //1 récupération du centre
            pCentreEncour c = this.FindBy(id);
            Boolean ExistHistorique = (c.PCentreHisto.Count() != 0);
            pCentreHisto centrehisto = this._histoService.findLastBy(id);
            IUnitOfWork transaction = this.getUnitOfWork();

            if (ExistHistorique)
            //{
            //PLE 05/06/2012 Prise en compte du retrou arrière avec suppression de l'utilisateur
            //// on est dans le cas d'un retour arrière sur une fiche créée automatiquement sur à un nouvel agrément, il n'y a donc pas d'historique
            //// sur cette fiche. Il faut la supprimer et revenir à la fiche précédente
            ////int idCentrePrecedent = c.PCentreEncourPrecedent.Id;
            //IUnitOfWork transaction = this.getUnitOfWork();
            //this.Remove(c);
            ////id = idCentrePrecedent;
            ////c = this.FindBy(id);
            //if (transaction != null)
            //    transaction.Commit();
            //}
            //else
            {
                //07/05/2015 PLE Si l'historique n'est pas conservé, on historise pas           
                //if (userMode)
                if (!deleteHisto)
                {
                    //On historise
                    if (transaction != null)
                    {
                        this._histoService.setUnitOfWorks(transaction);
                    }

                    this._histoService.historise(c);
                }
            }
            //On supprime l'utilisateur temporaire créer lors de la notification (centre indépendant)
            rUtilisateur u = new rUtilisateur();

            if (((c.RMotifModification.Id.Equals(Convert.ToString(rMotifModification.TypeModification.NOTVL)) ||
              c.RMotifModification.Id.Equals(Convert.ToString(rMotifModification.TypeModification.CHEXPLVL)) ||
              c.RMotifModification.Id.Equals(Convert.ToString(rMotifModification.TypeModification.CHLOCAVL)) ||
              c.RMotifModification.Id.Equals(Convert.ToString(rMotifModification.TypeModification.CHRATTVL)))
              && c.ReseauRattachementVL.Id.Equals(Convert.ToString(rReseau.IdCodeReseau.Z))))
            {
                u = this._utilisateurService.FindBy(c.CodAgremVl.ToLower());
                if (u != null)
                {
                    if (u.MotDePasseTemporaire != null)
                    {
                        this._utilisateurService.supprimerUtilisateur(c.CodAgremVl.ToLower());
                    }
                }
            }
            if (((c.RMotifModification.Id.Equals(Convert.ToString(rMotifModification.TypeModification.NOTPL)) ||
              c.RMotifModification.Id.Equals(Convert.ToString(rMotifModification.TypeModification.CHEXPLPL)) ||
              c.RMotifModification.Id.Equals(Convert.ToString(rMotifModification.TypeModification.CHLOCAPL)) ||
              c.RMotifModification.Id.Equals(Convert.ToString(rMotifModification.TypeModification.CHRATTPL)))
              && c.ReseauRattachementPL.Id.Equals(Convert.ToString(rReseau.IdCodeReseau.Z))))
            {
                u = this._utilisateurService.FindBy(c.CodAgremPl.ToLower());
                if (u != null)
                {
                    if (u.MotDePasseTemporaire != null)
                    {
                        this._utilisateurService.supprimerUtilisateur(c.CodAgremPl.ToLower());
                    }
                }
            }
            if (((c.RMotifModification.Id.Equals(Convert.ToString(rMotifModification.TypeModification.NOTCL)) ||
              c.RMotifModification.Id.Equals(Convert.ToString(rMotifModification.TypeModification.CHEXPLCL)) ||
              c.RMotifModification.Id.Equals(Convert.ToString(rMotifModification.TypeModification.CHLOCACL)) ||
              c.RMotifModification.Id.Equals(Convert.ToString(rMotifModification.TypeModification.CHRATTCL)))
            && c.ReseauRattachementCL.Id.Equals(Convert.ToString(rReseau.IdCodeReseau.Z))))
            {
                //21/02/2024 PLE supression ToLower pour la catégorie L 
                u = this._utilisateurService.FindBy(c.CodAgremCl);
                if (u != null)
                {
                    if (u.MotDePasseTemporaire != null)
                    {
                        this._utilisateurService.supprimerUtilisateur(c.CodAgremCl);
                    }
                }
            }

            if (!ExistHistorique)
            {
                // on est dans le cas d'un retour arrière sur une fiche créée automatiquement sur à un nouvel agrément, il n'y a donc pas d'historique
                // sur cette fiche. Il faut la supprimer et revenir à la fiche précédente
                //int idCentrePrecedent = c.PCentreEncourPrecedent.Id;
                //IUnitOfWork transaction = this.getUnitOfWork();
                this.Remove(c);
                //id = idCentrePrecedent;
                //c = this.FindBy(id);
                if (transaction != null)
                    transaction.Commit();
                return;
            }

            //On met à jour les données du centre à partir de l'histo
            c = majFromHistorique(c, centrehisto);

            if (!deleteHisto)
            {
                //données "systèmes" de suivi
                c.RUtilisateur = this._utilisateurService.FindBy(codeUtilisateur);
                c.DateMiseaJour = System.DateTime.Now;
                //c.DateEffetModification = System.DateTime.Now;
                c.RMotifModification = this._motifModificationService.FindBy(rMotifModification.TypeModification.RETOUR.ToString());
            }

            //On sauvegarde le centre
            base.Save(c);

            //On supprime l'historique (retour en cascade)
            if (deleteHisto)
            {
                this._histoService.Remove(centrehisto);
            }

            if (transaction != null)
                transaction.Commit();

        }

        private pCentreEncour majFromHistorique(pCentreEncour c, pCentreHisto centrehisto)
        {
            c.AncienNumeroAgrementPl = centrehisto.AncienNumeroAgrementPl;
            c.AncienNumeroAgrementVl = centrehisto.AncienNumeroAgrementVl;
            c.AncienNumeroAgrementCl = centrehisto.AncienNumeroAgrementCl;
            c.AvisOtcPl = centrehisto.AvisOtcPl;
            c.AvisOtcVl = centrehisto.AvisOtcVl;
            c.CodAgremPl = centrehisto.CodAgremPl;
            c.CodAgremVl = centrehisto.CodAgremVl;
            c.CodAgremCl = centrehisto.CodAgremCl;
            c.CodDepartement = centrehisto.CodDepartement;
            c.CommentaireAvisOtcPl = centrehisto.CommentaireAvisOtcPl;
            c.CommentaireAvisOtcVl = centrehisto.CommentaireAvisOtcVl;
            c.CpCentre = centrehisto.CpCentre;
            c.DateAvisOtcPl = centrehisto.DateAvisOtcPl;
            c.DateAvisOtcVl = centrehisto.DateAvisOtcVl;
            c.DateEcheanceAgrementPl = centrehisto.DateEcheanceAgrementPl;
            c.DateEffetModification = centrehisto.DateEffetModification;
            c.DateEffetNotificationPl = centrehisto.DateEffetNotificationPl;
            c.DateEffetNotificationVl = centrehisto.DateEffetNotificationVl;
            c.DateEffetNotificationCl = centrehisto.DateEffetNotificationCl;
            c.DateFinAgrementPl = centrehisto.DateFinAgrementPl;
            c.DateFinAgrementVl = centrehisto.DateFinAgrementVl;
            c.DateFinAgrementCl = centrehisto.DateFinAgrementCl;
            c.EnseigneCommerciale = centrehisto.EnseigneCommerciale;
            c.EtatAgrementPl = centrehisto.EtatAgrementPl;
            c.EtatAgrementVl = centrehisto.EtatAgrementVl;
            c.EtatAgrementCl = centrehisto.EtatAgrementCl;
            c.L1Adresse = centrehisto.L1Adresse;
            c.L2Adresse = centrehisto.L2Adresse;
            c.MotifFinAgrementPl = centrehisto.MotifFinAgrementPl;
            c.MotifFinAgrementVl = centrehisto.MotifFinAgrementVl;
            c.MotifFinAgrementCl = centrehisto.MotifFinAgrementCl;
            c.NomRepresentantLegal = centrehisto.NomRepresentantLegal;
            c.NumFax = centrehisto.NumFax;
            c.NumOrdrePl = centrehisto.NumOrdrePl;
            c.NumOrdreVl = centrehisto.NumOrdreVl;
            c.NumOrdreCl = centrehisto.NumOrdreCl;
            c.NumSiren = centrehisto.NumSiren;
            c.NumTel = centrehisto.NumTel;
            c.RaisonSociale = centrehisto.RaisonSociale;
            c.ReferenceAvisOtcPl = centrehisto.ReferenceAvisOtcPl;
            c.ReferenceAvisOtcVl = centrehisto.ReferenceAvisOtcVl;
            //c.ReferenceAvisOtcCl = centrehisto.ReferenceAvisOtcCl;
            c.RMotifModification = centrehisto.RMotifModification;
            c.RPrefecture = centrehisto.RPrefecture;
            c.RRegroupementIndependant = centrehisto.RRegroupementIndependant;
            //EG le 11/08/2011 suppresion de la colonne cod_reseau
            //c.RReseau = centrehisto.RReseau;
            c.RStatutJuridique = centrehisto.RStatutJuridique;
            c.RUtilisateur = centrehisto.RUtilisateur;
            c.TypeCentre = centrehisto.TypeCentre;
            c.Ville = centrehisto.Ville;
            //EG le 11/08/2011 nouvelle colonnes + suppression de la colonne cod_resau
            c.ReseauRattachementVL = centrehisto.ReseauRattachementVL;
            c.ReseauRattachementPL = centrehisto.ReseauRattachementPL;
            c.ReseauRattachementCL = centrehisto.ReseauRattachementCL;
            c.NumAccuseReceptionVL = centrehisto.NumAccuseReceptionVL;
            c.NumAccuseReceptionPL = centrehisto.NumAccuseReceptionPL;
            c.NumAccuseReceptionCL = centrehisto.NumAccuseReceptionCL;
            //PLE le 19/01/2024 Prise encompte catgorie L
            c.ExtAgremClCatVeh = centrehisto.ExtAgremClCatVeh;
            c.ExtAgremClDateDecla = centrehisto.ExtAgremClDateDecla;
            c.ExtAgremClDateValid = centrehisto.ExtAgremClDateValid;
            c.ExtAgremClStatut = centrehisto.ExtAgremClStatut;
            //PLE le 08/04/2025 PLE ajout SVA
            c.SvaVL = centrehisto.SvaVL;
            c.SvaPL = centrehisto.SvaPL;
            c.SvaCL = centrehisto.SvaCL;

            return c;
        }

        public bool aUnCertificatValide(int id, rCategorieVehicule.CategorieEnum categorieVehicule)
        {
            pCertificat c;
            c = this._gestionCertificatService.findLastValide(id, categorieVehicule);
            if (c != null)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool aUnCertificatValide(pCentreEncour centre, rCategorieVehicule.CategorieEnum categorieVehicule)
        {
            return this.aUnCertificatValide(centre.Id, categorieVehicule);

        }

        public RechercheCentreAgreeResultat estValide(string codAgrement, rCategorieVehicule.CategorieEnum categorieVehicule, bool userVerification)
        {
            RechercheCentreAgreeResultat r = new RechercheCentreAgreeResultat();
            pCentreEncour c = null;
            switch (categorieVehicule)
            {
                case rCategorieVehicule.CategorieEnum.VL: c = this.findByCodeAgrementVL(codAgrement); break;
                case rCategorieVehicule.CategorieEnum.PL: c = this.findByCodeAgrementPL(codAgrement); break;
                case rCategorieVehicule.CategorieEnum.CL: c = this.findByCodeAgrementCL(codAgrement); break;
            }

            if (c == null)
            {
                //Le code agrément selon la catégorie spécifiée est inexistant, on regarde quand même s'il existe pour une autre catégorie
                c = this.findByCodeAgrement(codAgrement);
                if (c == null)
                {
                    r.Resultat = pCentreEncour.ResultatRechercheCentre.CENTRE_INEXISTANT;
                    r.EstRechercheValide = false;
                }
                else
                {
                    r.Resultat = pCentreEncour.ResultatRechercheCentre.CENTRE_EXISTANT_POUR_AUTRE_CATEGORIE;
                    r.EstRechercheValide = false;
                }
            }
            else
            {
                //EG le 17/10/2011 : on sauvegarde le centre trouve
                r.CentreTrouve = c;

                //Le centre existe , est-il agréé ?
                string etatAgrement = null;
                switch (categorieVehicule)
                {
                    case rCategorieVehicule.CategorieEnum.VL: etatAgrement = c.EtatAgrementVl; break;
                    case rCategorieVehicule.CategorieEnum.PL: etatAgrement = c.EtatAgrementPl; break;
                    case rCategorieVehicule.CategorieEnum.CL: etatAgrement = c.EtatAgrementCl; break;
                }

                if ("1".Equals(etatAgrement))
                {
                    //EG le 19/07/2011
                    if (userVerification)
                    {
                        //centre agréé
                        //On vérifit si un utilisateur existe deja
                        if (c.RUtilisateur != null)
                        {
                            //PLE 21/02/2024 Remplacement recherche utilisateur
                            //rUtilisateur u = this._utilisateurService.FindBy(codAgrement.ToLower());
                            IList<rUtilisateur> u = this._utilisateurService.GetUserByCentre(c, categorieVehicule);

                            if ((u != null) && (u.Count() != 0))
                            {
                                //existence d'1 utilisateur
                                r.Resultat = pCentreEncour.ResultatRechercheCentre.UTILISATEUR_EXISTE_DEJA;
                                r.EstRechercheValide = false;
                            }
                            else
                            {
                                //Utilisateur inexistant
                                r.Resultat = pCentreEncour.ResultatRechercheCentre.CENTRE_AGREE;
                                r.EstRechercheValide = true;
                            }
                        }
                        else
                        {
                            //Utilisateur inexistant
                            r.Resultat = pCentreEncour.ResultatRechercheCentre.CENTRE_AGREE;
                            r.EstRechercheValide = true;
                        }
                    }
                    else
                    {
                        //pas besoin de vérifier si un user est attaché
                        r.Resultat = pCentreEncour.ResultatRechercheCentre.CENTRE_AGREE;
                        r.EstRechercheValide = true;
                    }

                }
                else
                {
                    r.Resultat = pCentreEncour.ResultatRechercheCentre.CENTRE_NON_AGREE;
                    r.EstRechercheValide = false;
                }

                //EG le 14/10/2011 : vérification si c'est un centre auxiliaire
                r.EstCentreAuxiliaire = "A".Equals(c.TypeCentre);
            }

            return r;
        }

        public pCentreEncour findByCodeAgrementVL(string codeAgrement)
        {
            Query q = new Query();
            q.Criteria.Add(new Criteria("CodAgremVl", codeAgrement, CriteriaOperator.EQUALS));

            IList<pCentreEncour> resultat = this.FindBy(q);
            if (resultat.Count > 0)
            {
                return resultat.ElementAt(0);
            }
            else
            {
                return null;
            }

        }

        public pCentreEncour findByCodeAgrementPL(string codeAgrement)
        {
            Query q = new Query();
            q.Criteria.Add(new Criteria("CodAgremPl", codeAgrement, CriteriaOperator.EQUALS));

            IList<pCentreEncour> resultat = this.FindBy(q);
            if (resultat.Count > 0)
            {
                return resultat.ElementAt(0);
            }
            else
            {
                return null;
            }
        }

        public pCentreEncour findByCodeAgrementCL(string codeAgrement)
        {
            Query q = new Query();
            q.Criteria.Add(new Criteria("CodAgremCl", codeAgrement, CriteriaOperator.EQUALS));

            IList<pCentreEncour> resultat = this.FindBy(q);
            if (resultat.Count > 0)
            {
                return resultat.ElementAt(0);
            }
            else
            {
                return null;
            }
        }

        public pCentreEncour findByCodeAgrement(string codeAgrement)
        {
            pCentreEncour c = null;
            c = this.findByCodeAgrementVL(codeAgrement);
            if (c == null)
            {
                c = this.findByCodeAgrementPL(codeAgrement);
            }

            if (c == null)
            {
                c = this.findByCodeAgrementCL(codeAgrement);
            }

            return c;
        }

        public void notificationAgrement(object id, pCentreEncour centre, rCategorieVehicule.CategorieEnum categVehicule, string codeUtilisateur)
        {
            //récupération du centre
            pCentreEncour c = this.FindBy(id);

            IUnitOfWork transaction = this.getUnitOfWork();
            if (transaction != null)
            {
                this._histoService.setUnitOfWorks(transaction);
                this._numOrdreAgrementService.setUnitOfWorks(transaction);
            }

            //on historise
            this._histoService.historise(c);

            string numagrement = null;
            rProfil profil = new rProfil();
            rProfil2 profil2 = new rProfil2();
            profil = this._profilService.FindBy((int)rProfil.CodeProfil.CENTRE_INDEPENDANT);
            profil2 = this._profil2Service.FindBy((int)rProfil2.CodeProfil2.CENTRE_INDEPENDANT);

            PasswordGenerator pGenerateur = PasswordGenerator.getInstance();
            string passWord = pGenerateur.GenerateForDesizedSize();
            //EG le 06/12/2011 pour numéro accusé réception : 4 premier caractères du mdp, pourquoi pas , c pour pas gâcher.... ;)
            string numeroAccuseReception = passWord.Substring(0, 4).ToLower();

            //EG le 03/01/2012 : le mot de passe est en minuscule
            passWord = passWord.ToLower();

            switch (categVehicule)
            {
                case rCategorieVehicule.CategorieEnum.VL:
                    if (centre.CodAgremVl == null)
                    {
                        //récupération du dernier n° d'ordre disponible
                        rNumordreAgrement n = this._numOrdreAgrementService.FindBy(c.RPrefecture.Id);
                        string numordre = n.NumOrdreCentre.ToString();

                        //enregistrer le compteur dans la base
                        n.NumOrdreCentre = n.NumOrdreCentre + 1;
                        this._numOrdreAgrementService.Save(n);

                        //constitution du n° d'agrément
                        string depCentre = c.CodDepartement;
                        if (depCentre.Length == 1) depCentre = "00" + depCentre;
                        if (depCentre.Length == 2) depCentre = "0" + depCentre;

                        if (numordre.Length == 1) numordre = "00" + numordre;
                        if (numordre.Length == 2) numordre = "0" + numordre;

                        numagrement = string.Format("{0}{1}{2}{3}", c.TypeCentre, depCentre, c.ReseauRattachementVL.Id, numordre);

                        // création d'un utilisateur tenporaire pour les centres indépendants
                        if (c.ReseauRattachementVL.Id == Convert.ToString(rReseau.IdCodeReseau.Z))
                        {
                            this._utilisateurService.CreateUserTemporaire(numagrement.ToLower(), passWord, profil, profil2, codeUtilisateur, categVehicule, c);
                        }

                    }
                    else
                    {
                        numagrement = centre.CodAgremVl;
                    }
                    //EG le 06/12/2011 : prise en compte acces public + génération du numéro d'accusé réception
                    c.AccesPublicVl = false;
                    if (!rReseau.IdCodeReseau.Z.ToString().Equals(c.ReseauRattachementVL.Id))
                    {
                        c.NumAccuseReceptionVL = numeroAccuseReception;
                    }
                    break;

                case rCategorieVehicule.CategorieEnum.PL:
                    if (centre.CodAgremPl == null)
                    {
                        //récupération du dernier n° d'ordre disponible
                        rNumordreAgrement n = this._numOrdreAgrementService.FindBy(c.RPrefecture.Id);
                        string numordre = n.NumOrdreCentre.ToString();

                        //enregistrer le compteur dans la base
                        n.NumOrdreCentre = n.NumOrdreCentre + 1;
                        this._numOrdreAgrementService.Save(n);

                        //constitution du n° d'agrément
                        string depCentre = c.CodDepartement;
                        if (depCentre.Length == 1) depCentre = "00" + depCentre;
                        if (depCentre.Length == 2) depCentre = "0" + depCentre;

                        if (numordre.Length == 1) numordre = "00" + numordre;
                        if (numordre.Length == 2) numordre = "0" + numordre;

                        numagrement = string.Format("{0}{1}{2}{3}", c.TypeCentre, depCentre, c.ReseauRattachementPL.Id, numordre);

                        // création d'un utilisateur tenporaire pour les centres indépendants
                        if (c.ReseauRattachementPL.Id == Convert.ToString(rReseau.IdCodeReseau.Z))
                        {
                            this._utilisateurService.CreateUserTemporaire(numagrement.ToLower(), passWord, profil, profil2, codeUtilisateur, categVehicule, c);
                        }
                    }
                    else
                    {
                        numagrement = centre.CodAgremPl;
                    }
                    // 10/06/2013 PLE (Correction Fiche 123) Ne pas générer une ACCUSE RECEPTION si il s'agit d'un agrément dans le cadre d'un renouvellement 
                    if (c.EtatAgrementPl != Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_RENOUVELEMENT_EN_COURS))
                    {
                        //EG le 06/12/2011 : prise en compte acces public + génération du numéro d'accusé réception
                        c.AccesPublicPl = false;
                        if (!rReseau.IdCodeReseau.Z.ToString().Equals(c.ReseauRattachementPL.Id))
                        {
                            c.NumAccuseReceptionPL = numeroAccuseReception;
                        }
                    }
                    break;

                case rCategorieVehicule.CategorieEnum.CL:
                    if (centre.CodAgremCl == null)
                    {
                        //récupération du dernier n° d'ordre disponible
                        rNumordreAgrement n = this._numOrdreAgrementService.FindBy(c.RPrefecture.Id);
                        string numordre = n.NumOrdreCentre.ToString();

                        //enregistrer le compteur dans la base
                        n.NumOrdreCentre = n.NumOrdreCentre + 1;
                        this._numOrdreAgrementService.Save(n);

                        //constitution du n° d'agrément
                        string depCentre = c.CodDepartement;
                        if (depCentre.Length == 1) depCentre = "00" + depCentre;
                        if (depCentre.Length == 2) depCentre = "0" + depCentre;

                        if (numordre.Length == 1) numordre = "00" + numordre;
                        if (numordre.Length == 2) numordre = "0" + numordre;

                        //PLE le 14/02/2024 Agrément CL : Changement de format L001Z001
                        //numagrement = string.Format("{0}{1}{2}{3}", c.TypeCentre, depCentre, c.ReseauRattachementCL.Id, numordre);
                        numagrement = string.Format("{0}{1}{2}{3}", "L", depCentre, c.ReseauRattachementCL.LettreReseau, numordre);

                        // création d'un utilisateur tenporaire pour les centres indépendants
                        if (c.ReseauRattachementCL.Id == Convert.ToString(rReseau.IdCodeReseau.Z))
                        {
                            //PLE le 14/02/2024 Agrément CL : Login = Code agrément centre
                            //this._utilisateurService.CreateUserTemporaire(numagrement.ToLower(), passWord, profil, profil2, codeUtilisateur, categVehicule, c);
                            this._utilisateurService.CreateUserTemporaire(numagrement, passWord, profil, profil2, codeUtilisateur, categVehicule, c);
                        }
                    }
                    else
                    {
                        numagrement = centre.CodAgremCl;
                    }
                    c.AccesPublicCl = false;
                    if (!rReseau.IdCodeReseau.Z.ToString().Equals(c.ReseauRattachementCL.Id))
                    {
                        c.NumAccuseReceptionCL = numeroAccuseReception;
                    }
                    break;
            }

            //On met à jour les données du centre
            int etat = new int();
            switch (categVehicule)
            {
                case rCategorieVehicule.CategorieEnum.VL:
                    etat = (int)pCentreEncour.CodeEtatAgrementVl.AGREMENT_OK;
                    c.EtatAgrementVl = Convert.ToString(etat);
                    c.DateEffetNotificationVl = centre.DateEffetNotificationVl;
                    c.CodAgremVl = numagrement;
                    c.RMotifModification = this._motifModificationService.FindBy(rMotifModification.TypeModification.NOTVL.ToString());
                    //08/04/2025 -PLE- mise en place SVA 
                    c.SvaVL = centre.SvaVL;
                    break;

                case rCategorieVehicule.CategorieEnum.PL:
                    if (c.EtatAgrementPl == Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_RENOUVELEMENT_EN_COURS))
                    {
                        etat = (int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_RENOUVELE_OK;
                        c.EtatAgrementPl = Convert.ToString(etat);
                        c.DateEffetNotificationPl = centre.DateEffetNotificationPl;
                        c.DateEcheanceAgrementPl = centre.DateEcheanceAgrementPl;
                        c.CodAgremPl = numagrement;
                        c.RMotifModification = this._motifModificationService.FindBy(rMotifModification.TypeModification.NOTRPL.ToString());
                    }
                    else
                    {
                        etat = (int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_OK;
                        c.EtatAgrementPl = Convert.ToString(etat);
                        c.DateEffetNotificationPl = centre.DateEffetNotificationPl;
                        c.DateEcheanceAgrementPl = centre.DateEcheanceAgrementPl;
                        c.CodAgremPl = numagrement;
                        c.RMotifModification = this._motifModificationService.FindBy(rMotifModification.TypeModification.NOTPL.ToString());
                        //08/04/2025 -PLE- mise en place SVA 
                        c.SvaPL = centre.SvaPL;
                    }
                    break;

                case rCategorieVehicule.CategorieEnum.CL:
                    etat = (int)pCentreEncour.CodeEtatAgrementCl.AGREMENT_OK;
                    c.EtatAgrementCl = Convert.ToString(etat);
                    c.DateEffetNotificationCl = centre.DateEffetNotificationCl;
                    c.CodAgremCl = numagrement;
                    c.RMotifModification = this._motifModificationService.FindBy(rMotifModification.TypeModification.NOTCL.ToString());
                    //08/04/2025 -PLE- mise en place SVA 
                    c.SvaCL = centre.SvaCL;
                    break;
            }

            //données système de suivi
            c.DateMiseaJour = System.DateTime.Now;
            //EG le 29/02/2012 : Rien à faire ici :( . Bug détecté par PB le 29/02/2012
            //c.DateEffetModification = System.DateTime.Now;
            c.RUtilisateur = this._utilisateurService.FindBy(codeUtilisateur);

            //enregistrement du centre
            this.Save(c);

            if (transaction != null)
                transaction.Commit();
        }

        public void rejetAgrement(object id, pCentreEncour centre, rCategorieVehicule.CategorieEnum categVehicule, string codeUtilisateur)
        {
            //récupération du centre
            pCentreEncour c = this.FindBy(id);

            IUnitOfWork transaction = this.getUnitOfWork();
            if (transaction != null)
            {
                this._histoService.setUnitOfWorks(transaction);
            }

            //on historise
            this._histoService.historise(c);

            //On met à jour les données du centre
            switch (categVehicule)
            {
                case rCategorieVehicule.CategorieEnum.VL:
                    c.CommentaireAvisOtcVl = centre.CommentaireAvisOtcVl;
                    c.EtatAgrementVl = Convert.ToString((int)pCentreEncour.CodeEtatAgrementVl.AGREMENT_REJETE);
                    c.RMotifModification = this._motifModificationService.FindBy(rMotifModification.TypeModification.REJVL.ToString());
                    break;

                case rCategorieVehicule.CategorieEnum.PL:
                    c.CommentaireAvisOtcPl = centre.CommentaireAvisOtcPl;
                    c.EtatAgrementPl = Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_REJETE);
                    c.RMotifModification = this._motifModificationService.FindBy(rMotifModification.TypeModification.REJPL.ToString());
                    break;

                case rCategorieVehicule.CategorieEnum.CL:
                    c.CommentaireAvisOtcCl = centre.CommentaireAvisOtcCl;
                    c.EtatAgrementCl = Convert.ToString((int)pCentreEncour.CodeEtatAgrementCl.AGREMENT_REJETE);
                    c.RMotifModification = this._motifModificationService.FindBy(rMotifModification.TypeModification.REJCL.ToString());
                    break;
            }

            //données système de suivi
            c.DateMiseaJour = System.DateTime.Now;
            //c.DateEffetModification = System.DateTime.Now;
            c.RUtilisateur = this._utilisateurService.FindBy(codeUtilisateur);

            //enregistrement du centre
            this.Save(c);

            if (transaction != null)
                transaction.Commit();

        }

        public void rejetRenouvellementAgrement(object id, pCentreEncour centre, rCategorieVehicule.CategorieEnum categVehicule, string codeUtilisateur)
        {
            //récupération du centre
            pCentreEncour c = this.FindBy(id);

            IUnitOfWork transaction = this.getUnitOfWork();

            //on historise
            this._histoService.historise(c);

            //On met à jour les données du centre
            //EG le 30/01/2012 la date de fin est automatiquement définie à partir de la date d'échéance
            //c.DateFinAgrementPl = centre.DateFinAgrementPl;
            c.DateFinAgrementPl = c.DateEcheanceAgrementPl;
            c.MotifFinAgrementPl = centre.MotifFinAgrementPl;
            c.EtatAgrementPl = Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_RENOUVELE_REJETE);
            c.RMotifModification = this._motifModificationService.FindBy(rMotifModification.TypeModification.REJRPL.ToString());
            c.CommentaireAvisOtcPl = centre.CommentaireAvisOtcPl;

            //données système de suivi
            c.DateMiseaJour = System.DateTime.Now;
            //c.DateEffetModification = System.DateTime.Now;
            c.RUtilisateur = this._utilisateurService.FindBy(codeUtilisateur);

            //enregistrement du centre
            this.Save(c);

            if (transaction != null)
                transaction.Commit();
        }

        public void annulerAgrement(object id, pCentreEncour centre, rCategorieVehicule.CategorieEnum categVehicule, string codeUtilisateur)
        {
            //récupération du centre
            pCentreEncour c = this.FindBy(id);

            IUnitOfWork transaction = this.getUnitOfWork();
            if (transaction != null)
            {
                this._histoService.setUnitOfWorks(transaction);
            }

            //on historise
            this._histoService.historise(c);

            //On met à jour les données du centre
            switch (categVehicule)
            {
                case rCategorieVehicule.CategorieEnum.VL:
                    c.DateFinAgrementVl = centre.DateFinAgrementVl;
                    c.MotifFinAgrementVl = centre.MotifFinAgrementVl;
                    c.EtatAgrementVl = Convert.ToString((int)pCentreEncour.CodeEtatAgrementVl.AGREMENT_KO);
                    c.RMotifModification = this._motifModificationService.FindBy(rMotifModification.TypeModification.ANUVL.ToString());

                    // Si le motif sélectionné est "2" ou "3" et un autre agrément est valide il faut annuler tous les agréments valides
                    if (c.MotifFinAgrementVl == Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementVL.CHANGEMENT_EXPLOITANT) ||
                        c.MotifFinAgrementVl == Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementVL.CHANGEMENT_LOCALISATION))
                    {
                        // Supprimer l'agrément PL si valide
                        if (Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_OK) == c.EtatAgrementPl ||
                            Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_RENOUVELE_OK) == c.EtatAgrementPl)
                        {   //mettre à jour les données du centre pour l'agrément PL
                            c.DateFinAgrementPl = centre.DateFinAgrementVl;
                            c.MotifFinAgrementPl = centre.MotifFinAgrementVl;
                            c.EtatAgrementPl = Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_KO);
                            //EG le 28/02/2012
                            this.fermerDerogation(c, rCategorieVehicule.CategorieEnum.PL);
                        }
                        // Supprimer l'agrément CL si valide
                        if (Convert.ToString((int)pCentreEncour.CodeEtatAgrementCl.AGREMENT_OK) == c.EtatAgrementCl)
                        {   //mettre à jour les données du centre pour l'agrément CL
                            c.DateFinAgrementCl = centre.DateFinAgrementVl;
                            c.MotifFinAgrementCl = centre.MotifFinAgrementVl;
                            c.EtatAgrementCl = Convert.ToString((int)pCentreEncour.CodeEtatAgrementCl.AGREMENT_KO);
                            //EG le 28/02/2012
                            this.fermerDerogation(c, rCategorieVehicule.CategorieEnum.CL);
                        }
                    }

                    break;

                case rCategorieVehicule.CategorieEnum.PL:
                    c.DateFinAgrementPl = centre.DateFinAgrementPl;
                    c.MotifFinAgrementPl = centre.MotifFinAgrementPl;
                    c.EtatAgrementPl = Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_KO);
                    c.RMotifModification = this._motifModificationService.FindBy(rMotifModification.TypeModification.ANUPL.ToString());

                    // Si le motif sélectionné est "2" ou "3" et un autre agrément est valide il faut annuler tous les agréments valides
                    if (c.MotifFinAgrementPl == Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementPL.CHANGEMENT_EXPLOITANT) ||
                        c.MotifFinAgrementPl == Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementPL.CHANGEMENT_LOCALISATION))
                    {
                        // Supprimer l'agrément VL si valide
                        if (Convert.ToString((int)pCentreEncour.CodeEtatAgrementVl.AGREMENT_OK) == c.EtatAgrementVl)
                        {   //mettre à jour les données du centre pour l'agrément PL
                            c.DateFinAgrementVl = centre.DateFinAgrementPl;
                            c.MotifFinAgrementVl = centre.MotifFinAgrementPl;
                            c.EtatAgrementVl = Convert.ToString((int)pCentreEncour.CodeEtatAgrementVl.AGREMENT_KO);
                            //EG le 28/02/2012
                            this.fermerDerogation(c, rCategorieVehicule.CategorieEnum.VL);
                        }
                        // Supprimer l'agrément CL si valide
                        if (Convert.ToString((int)pCentreEncour.CodeEtatAgrementCl.AGREMENT_OK) == c.EtatAgrementCl)
                        {   //mettre à jour les données du centre pour l'agrément CL
                            c.DateFinAgrementCl = centre.DateFinAgrementPl;
                            c.MotifFinAgrementCl = centre.MotifFinAgrementPl;
                            c.EtatAgrementCl = Convert.ToString((int)pCentreEncour.CodeEtatAgrementCl.AGREMENT_KO);
                            //EG le 28/02/2012
                            this.fermerDerogation(c, rCategorieVehicule.CategorieEnum.CL);
                        }
                    }
                    break;

                case rCategorieVehicule.CategorieEnum.CL:
                    c.DateFinAgrementCl = centre.DateFinAgrementCl;
                    c.MotifFinAgrementCl = centre.MotifFinAgrementCl;
                    c.EtatAgrementCl = Convert.ToString((int)pCentreEncour.CodeEtatAgrementCl.AGREMENT_KO);
                    c.RMotifModification = this._motifModificationService.FindBy(rMotifModification.TypeModification.ANUCL.ToString());

                    // Si le motif sélectionné est "2" ou "3" et un autre agrément est valide il faut annuler tous les agréments valides
                    if (c.MotifFinAgrementCl == Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementCL.CHANGEMENT_EXPLOITANT) ||
                        c.MotifFinAgrementCl == Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementCL.CHANGEMENT_LOCALISATION))
                    {
                        // Supprimer l'agrément VL si valide
                        if (Convert.ToString((int)pCentreEncour.CodeEtatAgrementVl.AGREMENT_OK) == c.EtatAgrementVl)
                        {   //mettre à jour les données du centre pour l'agrément PL
                            c.DateFinAgrementVl = centre.DateFinAgrementCl;
                            c.MotifFinAgrementVl = centre.MotifFinAgrementCl;
                            c.EtatAgrementVl = Convert.ToString((int)pCentreEncour.CodeEtatAgrementVl.AGREMENT_KO);
                            //EG le 28/02/2012
                            this.fermerDerogation(c, rCategorieVehicule.CategorieEnum.VL);
                        }
                        // Supprimer l'agrément PL si valide
                        if (Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_OK) == c.EtatAgrementPl ||
                            Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_RENOUVELE_OK) == c.EtatAgrementPl)
                        {   //mettre à jour les données du centre pour l'agrément PL
                            c.DateFinAgrementPl = centre.DateFinAgrementCl;
                            c.MotifFinAgrementPl = centre.MotifFinAgrementCl;
                            c.EtatAgrementPl = Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_KO);
                            //EG le 28/02/2012
                            this.fermerDerogation(c, rCategorieVehicule.CategorieEnum.PL);
                        }
                    }
                    break;
            }

            //EG le 28/02/2012 : on ferme les dérogations : NB ici car la date de fin d'agrément doit être initialisée avec la bonne valeur
            this.fermerDerogation(c, categVehicule);

            //données système de suivi
            c.DateMiseaJour = System.DateTime.Now;
            //c.DateEffetModification = System.DateTime.Now;
            c.RUtilisateur = this._utilisateurService.FindBy(codeUtilisateur);

            //enregistrement du centre
            this.Save(c);

            if (transaction != null)
                transaction.Commit();
        }

        //EG le 28/02/2012: pour gérer l'annulation de l'agrément
        private void fermerDerogation(pCentreEncour centre, rCategorieVehicule.CategorieEnum categorie)
        {
            if (centre.PDerogationCentres != null && centre.PDerogationCentres.Count > 0)
            {
                foreach (var derogation in centre.PDerogationCentres)
                {
                    if (derogation.CategorieVehicule.Equals(categorie))
                    {
                        DateTime dateFin = categorie.Equals(rCategorieVehicule.CategorieEnum.VL) ? centre.DateFinAgrementVl.Value :
                                           categorie.Equals(rCategorieVehicule.CategorieEnum.PL) ? centre.DateFinAgrementPl.Value : centre.DateFinAgrementCl.Value;

                        if (derogation.DateFin == null || (derogation.DateFin.HasValue && derogation.DateFin.Value.CompareTo(dateFin) > 0))
                        {
                            derogation.DateFin = dateFin;
                        }
                    }
                }
            }

        }

        public void annulerAgrementSuiteSanction(pCentreEncour centre, pSanctionCentre sanction, string codeUtilisateur)
        {
            //On met à jour les données du centre
            if (sanction.IsAgrementAffecteVL)
            {
                centre.DateFinAgrementVl = sanction.DateDebut;
                centre.MotifFinAgrementVl = Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementVL.RETRAIT_SANCTION);
                centre.EtatAgrementVl = Convert.ToString((int)pCentreEncour.CodeEtatAgrementVl.AGREMENT_KO);
                //centre.RMotifModification = this._motifModificationService.FindBy(rMotifModification.TypeModification.ANUVL.ToString());
                centre.RMotifModification = this._motifModificationService.FindBy(rMotifModification.TypeModification.SANCT.ToString());
            }

            if (sanction.IsAgrementAffectePL)
            {
                centre.DateFinAgrementPl = sanction.DateDebut;
                centre.MotifFinAgrementPl = Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementPL.RETRAIT_SANCTION);
                centre.EtatAgrementPl = Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_KO);
                //centre.RMotifModification = this._motifModificationService.FindBy(rMotifModification.TypeModification.ANUPL.ToString());
                centre.RMotifModification = this._motifModificationService.FindBy(rMotifModification.TypeModification.SANCT.ToString());
            }

            if (sanction.IsAgrementAffecteCL)
            {
                centre.DateFinAgrementCl = sanction.DateDebut;
                centre.MotifFinAgrementCl = Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementCL.RETRAIT_SANCTION);
                centre.EtatAgrementCl = Convert.ToString((int)pCentreEncour.CodeEtatAgrementCl.AGREMENT_KO);
                //centre.RMotifModification = this._motifModificationService.FindBy(rMotifModification.TypeModification.ANUCL.ToString());
                centre.RMotifModification = this._motifModificationService.FindBy(rMotifModification.TypeModification.SANCT.ToString());
            }
            //données système de suivi
            centre.DateMiseaJour = System.DateTime.Now;
            //centre.DateEffetModification = System.DateTime.Now;
            centre.RUtilisateur = this._utilisateurService.FindBy(codeUtilisateur);

            //enregistrement du centre
            this.Save(centre);

        }

        public pCentreEncour nouveauCentre(pCentreEncour centre, rCategorieVehicule.CategorieEnum categorie, string codeUtilisateur)
        {
            //1 récupération du centre
            pCentreEncour c = new pCentreEncour();

            IUnitOfWork transaction = this.getUnitOfWork();
            if (transaction != null)
            {
                this._histoService.setUnitOfWorks(transaction);
            }

            //2 On modifie les informations du centre 
            c.RaisonSociale = centre.RaisonSociale.ToUpper();
            c.NumSiren = centre.NumSiren;
            c.EnseigneCommerciale = centre.EnseigneCommerciale;
            c.L1Adresse = centre.L1Adresse;
            c.L2Adresse = centre.L2Adresse;
            c.CpCentre = centre.CpCentre;
            c.Ville = centre.Ville.ToUpper();
            c.NumTel = centre.NumTel;
            if (centre.NomRepresentantLegal != null)
            {
                c.NomRepresentantLegal = centre.NomRepresentantLegal.ToUpper();
            }
            c.TypeCentre = centre.TypeCentre;
            c.CodDepartement = centre.CodDepartement;

            c.EtatAgrementVl = Convert.ToString((int)pCentreEncour.CodeEtatAgrementVl.AGREMENT_KO);
            c.EtatAgrementPl = Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_KO);
            c.EtatAgrementCl = Convert.ToString((int)pCentreEncour.CodeEtatAgrementCl.AGREMENT_KO);

            switch (categorie)
            {
                case rCategorieVehicule.CategorieEnum.VL:
                    c.EtatAgrementVl = Convert.ToString((int)pCentreEncour.CodeEtatAgrementVl.AGREMENT_DEMANDE_EN_COURS);
                    c.EtatAgrementPl = Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_KO);
                    c.EtatAgrementCl = Convert.ToString((int)pCentreEncour.CodeEtatAgrementCl.AGREMENT_KO);
                    //EG le 11/08/2011 pour suppression de la colonne cod_reseau
                    c.ReseauRattachementVL = this._reseauService.FindBy(centre.ReseauRattachementVL.Id);
                    break;
                case rCategorieVehicule.CategorieEnum.PL:
                    c.EtatAgrementVl = Convert.ToString((int)pCentreEncour.CodeEtatAgrementVl.AGREMENT_KO);
                    c.EtatAgrementPl = Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_DEMANDE_EN_COURS);
                    c.EtatAgrementCl = Convert.ToString((int)pCentreEncour.CodeEtatAgrementCl.AGREMENT_KO);
                    //EG le 11/08/2011 pour suppression de la colonne cod_reseau
                    c.ReseauRattachementPL = this._reseauService.FindBy(centre.ReseauRattachementPL.Id);
                    break;
                case rCategorieVehicule.CategorieEnum.CL:
                    c.EtatAgrementVl = Convert.ToString((int)pCentreEncour.CodeEtatAgrementVl.AGREMENT_KO);
                    c.EtatAgrementPl = Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_KO);
                    c.EtatAgrementCl = Convert.ToString((int)pCentreEncour.CodeEtatAgrementCl.AGREMENT_DEMANDE_EN_COURS);
                    //EG le 11/08/2011 pour suppression de la colonne cod_reseau
                    c.ReseauRattachementCL = this._reseauService.FindBy(centre.ReseauRattachementCL.Id);
                    break;
            }

            //rPrefecture pref ;
            //pref = this._prefectureService.findByCodeDepartement(c.CodDepartement);
            //c.RPrefecture = pref;
            c.RPrefecture = this._prefectureService.findByCodeDepartement(centre.CodDepartement);
            if (centre.RStatutJuridique.Id != "-1")
                c.RStatutJuridique = this._statutJuridiqueService.FindBy(centre.RStatutJuridique.Id);
            //EG le 11/08/2011 suppression de la colonne co_d_reseau 
            //c.RReseau = this._reseauService.FindBy(centre.RReseau.Id);

            //recherche du rutilisateur
            c.RUtilisateur = this._utilisateurService.FindBy(codeUtilisateur);

            //données système de suivi
            c.DateMiseaJour = System.DateTime.Now;
            c.RUtilisateur = this._utilisateurService.FindBy(codeUtilisateur);

            switch (categorie)
            {
                case rCategorieVehicule.CategorieEnum.VL:
                    c.RMotifModification = this._motifModificationService.FindBy(rMotifModification.TypeModification.CRECEVL.ToString());
                    break;
                case rCategorieVehicule.CategorieEnum.PL:
                    c.RMotifModification = this._motifModificationService.FindBy(rMotifModification.TypeModification.CRECEPL.ToString());
                    break;
                case rCategorieVehicule.CategorieEnum.CL:
                    c.RMotifModification = this._motifModificationService.FindBy(rMotifModification.TypeModification.CRECECL.ToString());
                    break;
            }

            //4 On sauvegarde le centre
            this.Save(c);

            if (transaction != null)
                transaction.Commit();
            return c;
        }

        public void historiserCentre(pCentreEncour centre)
        {
            // on historise le centre
            //EG le 30/11/2011  pour assurer la continuité de la transaction
            IUnitOfWork u = this.getUnitOfWork();
            this._histoService.setUnitOfWorks(u);

            this._histoService.historise(centre);
        }

        public void modifSuiviCentre(pCentreEncour centre, rMotifModification motif, string codeUtilisateur)
        {
            //données système de suivi
            centre.DateMiseaJour = System.DateTime.Now;
            //centre.DateEffetModification = System.DateTime.Now;
            centre.RUtilisateur = this._utilisateurService.FindBy(codeUtilisateur);
            centre.RMotifModification = motif;
            //enregistrement du centre
            this.Save(centre);
        }

        //YM le 21/09/2011
        //PLE le 19/05/2022
        public Dictionary<int, string> ListeMotifAnnulationAgrement(rCategorieVehicule.CategorieEnum categorie, bool avecRetraitSanction)
        {
            Dictionary<int, string> ListeModification = new Dictionary<int, string>();
            string motif;

            switch (categorie)
            {
                case rCategorieVehicule.CategorieEnum.VL:
                    foreach (pCentreEncour.MotifAnnulerAgrementVL m in Enum.GetValues(typeof(pCentreEncour.MotifAnnulerAgrementVL)))
                    {
                        //PLE le 19/05/2022 : suppression du motif "changement de localisation"
                        if ((m.ToString().Equals("RETRAIT_SANCTION") && avecRetraitSanction == false) || (m.ToString() == "CHANGEMENT_LOCALISATION"))
                        {
                            continue;
                        }
                        motif = m.ToString();
                        if (m.ToString() == "FERMETURE_DEFINITIVE") motif = "Fermeture définitive";
                        if (m.ToString() == "CHANGEMENT_EXPLOITANT") motif = "Changement de titulaire de l'agrément avec modification du numéro unique d'identification";
                        //PLE le 19/05/2022 : suppression du motif "changement de localisation"
                        //if (m.ToString() == "CHANGEMENT_LOCALISATION") motif = "Changement de localisation";
                        if (m.ToString() == "CHANGEMENT_RESEAU") motif = "Changement du réseau de rattachement";
                        if (m.ToString() == "RETRAIT_SANCTION") motif = "Retrait suite à sanction";
                        ListeModification.Add((int)m, motif);
                        //ListeModification.Add((int)m, m.ToString());
                    }
                    break;

                case rCategorieVehicule.CategorieEnum.PL:
                    foreach (pCentreEncour.MotifAnnulerAgrementPL m in Enum.GetValues(typeof(pCentreEncour.MotifAnnulerAgrementPL)))
                    {
                        //PLE le 19/05/2022 : suppression du motif "changement de localisation"
                        if ((m.ToString() == "RETRAIT_SANCTION" && avecRetraitSanction == false) || (m.ToString() == "CHANGEMENT_LOCALISATION"))
                        {
                            continue;
                        }
                        motif = m.ToString();
                        if (m.ToString() == "FERMETURE_DEFINITIVE") motif = "Fermeture définitive";
                        if (m.ToString() == "CHANGEMENT_EXPLOITANT") motif = "Changement de titulaire de l'agrément avec modification du numéro unique d'identification";
                        //PLE le 19/05/2022 : suppression du motif "changement de localisation"
                        //if (m.ToString() == "CHANGEMENT_LOCALISATION") motif = "Changement de localisation";
                        if (m.ToString() == "CHANGEMENT_RESEAU") motif = "Changement du réseau de rattachement";
                        if (m.ToString() == "RETRAIT_SANCTION") motif = "Retrait suite à sanction";
                        if (m.ToString() == "NON_RENOUVELLEMENT") motif = "Non renouvellement";
                        ListeModification.Add((int)m, motif);
                        //ListeModification.Add((int)m, m.ToString());
                    }
                    break;

                case rCategorieVehicule.CategorieEnum.CL:
                    foreach (pCentreEncour.MotifAnnulerAgrementCL m in Enum.GetValues(typeof(pCentreEncour.MotifAnnulerAgrementCL)))
                    {
                        //PLE le 19/05/2022 : suppression du motif "changement de localisation"
                        if ((m.ToString() == "RETRAIT_SANCTION" && avecRetraitSanction == false) || (m.ToString() == "CHANGEMENT_LOCALISATION"))
                        {
                            continue;
                        }
                        motif = m.ToString();
                        if (m.ToString() == "FERMETURE_DEFINITIVE") motif = "Fermeture définitive";
                        if (m.ToString() == "CHANGEMENT_EXPLOITANT") motif = "Changement de titulaire de l'agrément avec modification du numéro unique d'identification";
                        //PLE le 19/05/2022 : suppression du motif "changement de localisation"
                        //if (m.ToString() == "CHANGEMENT_LOCALISATION") motif = "Changement de localisation";
                        if (m.ToString() == "CHANGEMENT_RESEAU") motif = "Changement du réseau de rattachement";
                        if (m.ToString() == "RETRAIT_SANCTION") motif = "Retrait suite à sanction";
                        ListeModification.Add((int)m, motif);
                        //ListeModification.Add((int)m, m.ToString());
                    }
                    break;
            }
            return ListeModification;
        }

        public void renouvellementAgrementPl(object id, string codeUtilisateur)
        {
            //récupération du centre
            pCentreEncour c = this.FindBy(id);

            IUnitOfWork transaction = this.getUnitOfWork();

            //on historise
            this._histoService.historise(c);

            //On met à jour les données du centre
            c.EtatAgrementPl = Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_RENOUVELEMENT_EN_COURS);
            c.RMotifModification = this._motifModificationService.FindBy(rMotifModification.TypeModification.DERPL.ToString());

            //données système de suivi
            c.DateMiseaJour = System.DateTime.Now;
            //c.DateEffetModification = System.DateTime.Now;
            c.RUtilisateur = this._utilisateurService.FindBy(codeUtilisateur);

            //enregistrement du centre
            this.Save(c);

            if (transaction != null)
                transaction.Commit();
        }

        public void deleteFiche(pCentreEncour centre)
        {
            IUnitOfWork transaction = this.getUnitOfWork();

            this.Remove(centre);

            if (transaction != null)
                transaction.Commit();
        }

        //YM 26/01/2012
        public IList<pCentreEncour> FindAllCentreNouvellementAgreeRegion(rRegion region, string datedeb)
        {
            IpCentreEncourRepository repository = this._Repository as IpCentreEncourRepository;
            return repository.FindAllCentreNouvellementAgreeRegion(region, datedeb);
        }

        //YM 26/01/2012
        public IList<pCentreEncour> FindAllCentreAgreeSansControleurRegion(rRegion region)
        {
            IpCentreEncourRepository repository = this._Repository as IpCentreEncourRepository;
            return repository.FindAllCentreAgreeSansControleurRegion(region);
        }

        //EG le 21/02/2012
        public override void Remove(pCentreEncour centre)
        {
            //On remove d'abord l'historique
            IList<pCentreHisto> historique = centre.PCentreHisto;
            if (historique != null && historique.Count > 0)
            {
                this._histoService.removeAll(centre);
            }

            //EG le 22/02/2012 : on détache tous les controleurs rattachés
            //VL
            if (centre.ControleurVLs.Count > 0)
            {
                foreach (var item in centre.ControleurVLs)
                {
                    item.PCentreEncourVL = null;
                }
            }

            //PL
            if (centre.ControleurPLs.Count > 0)
            {
                foreach (var item in centre.ControleurPLs)
                {
                    item.PCentreEncourPL = null;
                }
            }

            //CL
            if (centre.ControleurCLs.Count > 0)
            {
                foreach (var item in centre.ControleurCLs)
                {
                    item.PCentreEncourCL = null;
                }
            }

            base.Remove(centre);
        }
        //PLE le 22/01/2024 Extension catégorie L
        public void EnregistrerExtensionAgrementCatL(object id, pCentreEncour centre, rCategorieVehicule.CategorieEnum categVehicule, string codeUtilisateur)
        {
            //récupération du centre
            pCentreEncour c = this.FindBy(id);

            IUnitOfWork transaction = this.getUnitOfWork();
            if (transaction != null)
            {
                this._histoService.setUnitOfWorks(transaction);
            }

            //on historise
            this._histoService.historise(c);

            //On met à jour les données du centre
            c.ExtAgremClStatut = centre.ExtAgremClStatut;
            c.ExtAgremClDateValid = centre.ExtAgremClDateValid;

            if (categVehicule.Equals(rCategorieVehicule.CategorieEnum.VL))
            { c.ExtAgremClCatVeh = (short?)rCategorieVehicule.ExtCatvehEnum.VL; }
            else
            { c.ExtAgremClCatVeh = (short?)rCategorieVehicule.ExtCatvehEnum.PL; }

            rMotifModification.TypeModification typeModification;
            if (centre.ExtAgremClStatut.Equals((short?)1))
            {
                typeModification = rMotifModification.TypeModification.EXTDECLA;
            }
            else
            {
                typeModification = rMotifModification.TypeModification.EXTRETRA;
            }
            c.RMotifModification = this._motifModificationService.FindBy(typeModification.ToString());

            //données système de suivi
            c.DateMiseaJour = System.DateTime.Now;
            c.ExtAgremClDateDecla = c.DateMiseaJour;

            //c.DateEffetModification = System.DateTime.Now;
            c.RUtilisateur = this._utilisateurService.FindBy(codeUtilisateur);

            //PLE le 08/03/2024 Duplication des logins dans la table r_utilisateur ctcl_prod provenant soit du VL ou PL
            //Uniquement dans le cas de la déclaration pour un centre indépendant VL ou PL
            if (centre.ExtAgremClStatut.Equals((short?)1))
            {
                switch (categVehicule)
                {
                    case rCategorieVehicule.CategorieEnum.VL:
                        if (c.ReseauRattachementVL.Id.Equals(rReseau.IdCodeReseau.Z.ToString()))
                        {
                            UtilisateurWebService utilisateurWebService = null;
                            utilisateurWebService = this._utilisateurWebServiceCL.FindByLogin(c.CodAgremVl);
                            //Le login VL n'est pas présent dans la base CL
                            if (utilisateurWebService == null)
                            {
                                //Récupération du login dans la base VL
                                utilisateurWebService = this._utilisateurWebServiceVL.FindByLogin(c.CodAgremVl);
                                var newUser = new UtilisateurWebServiceCL();
                                newUser.Login = utilisateurWebService.Login;
                                newUser.Password = utilisateurWebService.Password;
                                newUser.CodReseau = utilisateurWebService.CodReseau;
                                this._utilisateurWebServiceCL.ajouterUtilisateur(newUser);
                            }
                        }
                        break;

                    case rCategorieVehicule.CategorieEnum.PL:
                        if (c.ReseauRattachementPL.Id.Equals(rReseau.IdCodeReseau.Z.ToString()))
                        {
                            UtilisateurWebService utilisateurWebService = null;
                            utilisateurWebService = this._utilisateurWebServiceCL.FindByLogin(c.CodAgremPl);
                            //Le login VL n'est pas présent dans la base CL
                            if (utilisateurWebService == null)
                            {
                                //Récupération du login dans la base PL
                                utilisateurWebService = this._utilisateurWebServicePL.FindByLogin(c.CodAgremPl);
                                var newUser = new UtilisateurWebServiceCL();
                                newUser.Login = utilisateurWebService.Login;
                                newUser.Password = utilisateurWebService.Password;
                                newUser.CodReseau = utilisateurWebService.CodReseau;
                                this._utilisateurWebServiceCL.ajouterUtilisateur(newUser);
                            }
                        }
                        break;


                }


            }

            //enregistrement du centre
            this.Save(c);

            if (transaction != null)
                transaction.Commit();
        }


    }
}
