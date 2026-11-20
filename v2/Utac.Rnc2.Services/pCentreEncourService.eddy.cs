using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.BusinessObjects.certificats;
using Utac.Rnc2.Contrats.Repositories;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Infrastructure;
using Utac.Rnc2.Infrastructure.Queries;
using Utac.Rnc2.Infrastructure.Search;
using Utac.Rnc2.Infrastructure.Utilitaires;

namespace Utac.Rnc2.Services
{
    public partial class pCentreEncourService : BaseService<pCentreEncour>, IpCentreEncourService
    {

        private readonly ILog log = LogManager.GetLogger(typeof(pCentreEncourService));

        //EG le 09/08/2011 pour gestion des droits dans GestionDroitsExecution
        public bool isAncienCentre(pCentreEncour centre)
        {

            return this.isAncienCentre(centre.Id);
        }

        public bool isAncienCentre(int centreId)
        {
            Query q = new Query();
            q.Criteria.Add(new Criteria("PCentreEncourPrecedent.Id", centreId, CriteriaOperator.EQUALS));
            List<pCentreEncour> listeCentres = this.FindBy(q);
            return listeCentres.Count > 0;

        }

        private struct Filtre
        {
            public string ColonneEtatAgrement;
            public string ColonneAccesPublic;
        }

        public List<pCentreEncour> FindByFiltreProfilReseau(FiltreRechercheProfilReseau form, out Boolean estDepartementErrone)
        {
            Query q = new Query();
            //EG le 15/12/2011 : changement de type de  dictionnaire pour prise en compte des centres visibles ou non
            Dictionary<string, Filtre> etatAgrementProps = new Dictionary<string, Filtre>();
            etatAgrementProps.Add("ReseauRattachementVL.Id", new Filtre() { ColonneEtatAgrement = "EtatAgrementVl", ColonneAccesPublic = "AccesPublicVl" });
            etatAgrementProps.Add("ReseauRattachementPL.Id", new Filtre() { ColonneEtatAgrement = "EtatAgrementPl", ColonneAccesPublic = "AccesPublicPl" });
            etatAgrementProps.Add("ReseauRattachementCL.Id", new Filtre() { ColonneEtatAgrement = "EtatAgrementCl", ColonneAccesPublic = "AccesPublicCl" });

            //Quel réseau
            rReseau reseauFiltre = this._reseauService.FindBy(form.CodeReseau);
            string propertyName = reseauFiltre.TypeReseauVl ? "ReseauRattachementVL.Id" : reseauFiltre.TypeReseauPl ? "ReseauRattachementPL.Id" : "ReseauRattachementCL.Id";

            q.Criteria.Add(new Criteria(propertyName, form.CodeReseau, CriteriaOperator.EQUALS));

            //Correction code département de la forme 2b
            if (!string.IsNullOrEmpty(form.CodeDepartement))
                form.CodeDepartement = form.CodeDepartement.ToUpper();

            //Correction code département de la forme 1 pour 01
            if (!string.IsNullOrEmpty(form.CodeDepartement))
            {
                int result;
                if (Int32.TryParse(form.CodeDepartement, out result))
                {
                    if (result < 10)
                    {
                        form.CodeDepartement = "0" + result;
                    }
                }
                //Supression du 0 devant si 3 caractères
                if (form.CodeDepartement.Length.Equals(3) && form.CodeDepartement.StartsWith("0"))
                {
                    form.CodeDepartement = form.CodeDepartement.Substring(1, 2);
                }
            }


            q.Criteria.Add(new Criteria(etatAgrementProps[propertyName].ColonneEtatAgrement, "1", CriteriaOperator.EQUALS));
            //EG le 15/12/2011 : Ajout filtre pour prise en compte des centres visibles ou non
            q.Criteria.Add(new Criteria(etatAgrementProps[propertyName].ColonneAccesPublic, true, CriteriaOperator.EQUALS));

            estDepartementErrone = false;
            if (!string.IsNullOrWhiteSpace(form.CodeDepartement))
            {
                rPrefecture pref = this._prefectureService.findByCodeDepartement(form.CodeDepartement);                
                if (pref == null)
                {
                    estDepartementErrone = true;
                    return null;
                }
                q.Criteria.Add(new Criteria("RPrefecture.Id", pref.Id, CriteriaOperator.EQUALS));
            }

            if (!string.IsNullOrWhiteSpace(form.TypeCentre))
            {
                q.Criteria.Add(new Criteria("TypeCentre", form.TypeCentre, CriteriaOperator.EQUALS));
            }

            return this.FindBy(q);
        }

        public IList<pCentreEncour> findAllCentrePourAccuseReception(rReseau r)
        {

            Query q = new Query();
            if (r.TypeReseauVl)
            {
                q.Criteria.Add(new Criteria("NumAccuseReceptionVL", null, CriteriaOperator.IS_NOT_NULL));
                q.Criteria.Add(new Criteria("AccesPublicVl", false, CriteriaOperator.EQUALS));
                q.Criteria.Add(new Criteria("EtatAgrementVl", Convert.ToString((int)pCentreEncour.CodeEtatAgrementVl.AGREMENT_OK), CriteriaOperator.EQUALS));
            }

            if (r.TypeReseauPl)
            {
                q.Criteria.Add(new Criteria("NumAccuseReceptionPL", null, CriteriaOperator.IS_NOT_NULL));
                q.Criteria.Add(new Criteria("AccesPublicPl", false, CriteriaOperator.EQUALS));
                q.Criteria.Add(new Criteria("EtatAgrementPl", Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_OK), CriteriaOperator.EQUALS));
            }

            if (r.TypeReseauCl)
            {
                q.Criteria.Add(new Criteria("NumAccuseReceptionCL", null, CriteriaOperator.IS_NOT_NULL));
                q.Criteria.Add(new Criteria("AccesPublicCl", false, CriteriaOperator.EQUALS));
                q.Criteria.Add(new Criteria("EtatAgrementCl", Convert.ToString((int)pCentreEncour.CodeEtatAgrementCl.AGREMENT_OK), CriteriaOperator.EQUALS));
            }

            return this.FindBy(q);
        }

        public bool accuseReception(pCentreEncour centre, rCategorieVehicule.CategorieEnum categorie, string codAgrementCentre, string numeroAccuseReception)
        {
            bool enregistrementOK = false;

            pCentreEncour c = this.FindBy(centre.Id);
            switch (categorie)
            {
                case rCategorieVehicule.CategorieEnum.VL:
                    if (numeroAccuseReception.Equals(c.NumAccuseReceptionVL) && codAgrementCentre.Equals(c.CodAgremVl))
                    {
                        c.AccesPublicVl = true;
                        enregistrementOK = true;
                    }
                    break;

                case rCategorieVehicule.CategorieEnum.PL:
                    if (numeroAccuseReception.Equals(c.NumAccuseReceptionPL) && codAgrementCentre.Equals(c.CodAgremPl))
                    {
                        c.AccesPublicPl = true;
                        enregistrementOK = true;
                    }
                    break;

                case rCategorieVehicule.CategorieEnum.CL:
                    if (numeroAccuseReception.Equals(c.NumAccuseReceptionCL) && codAgrementCentre.Equals(c.CodAgremCl))
                    {
                        c.AccesPublicCl = true;
                        enregistrementOK = true;
                    }
                    break;
            }

            if (enregistrementOK)
            {
                base.Save(c);
                IUnitOfWork u = this.getUnitOfWork();
                if (u != null)
                {
                    u.Commit();
                }
            }
            return enregistrementOK;
        }

        //EG le 09/12/2011
        public pCentreEncour nouvelAgrement(object id, pCentreEncour centreForm, rCategorieVehicule.CategorieEnum categorie, string codeUtilisateur)
        {
            //récupération du centre
            log.Debug("nouvelAgrement recherche du centre ID = " + id);
            pCentreEncour centreEnCours = this.FindBy(id);
            log.Debug("nouvelAgrement recherche du centre ID = " + id + " ...OK");
            /*Si non existence d'un autre agrem ok  alors …
          …Créer un nouveau centre à partir des données de la fiche actuelle + saisie formulaire … (nouvel id_centre)
          ...Sinon mettre à jour la fiche centre actuelle.*/

            pCentreEncour newCentre = centreEnCours;
            bool nouvelleFiche = true;
            switch (categorie)
            {
                case rCategorieVehicule.CategorieEnum.VL:
                    if (centreEnCours.EstAgrementValidePL || centreEnCours.EstAgrementValideCL) nouvelleFiche = false;
                    break;
                case rCategorieVehicule.CategorieEnum.PL:
                    if (centreEnCours.EstAgrementValideVL || centreEnCours.EstAgrementValideCL) nouvelleFiche = false;
                    break;
                case rCategorieVehicule.CategorieEnum.CL:
                    if (centreEnCours.EstAgrementValideVL || centreEnCours.EstAgrementValidePL) nouvelleFiche = false;
                    break;
            }

            //Cas changement exploitant ou localisation
            //PLE 13/06/2102 Ne pas limiter le nombre de changement d'exploitant
            //if (centreEnCours.PCentreEncourPrecedent != null)
            //{
            //    nouvelleFiche = false;
            //}

            if (nouvelleFiche)
            {
                //création d'une nouvelle fiche
                log.Debug("nouvelAgrement CREATION NOUVELLE FICHE");
                newCentre = this.createNewCentre(centreEnCours);
            }
            else
            {
                log.Debug("nouvelAgrement ON RESTE SUR LA FICHE");
            }

            IUnitOfWork transaction = this.getUnitOfWork();
            if (!nouvelleFiche)
            {
                if (transaction != null)
                {
                    this._histoService.setUnitOfWorks(transaction);
                }
                //on historise
                this._histoService.historise(centreEnCours);
            }

            //On met à jour les données du centre
            //… En initialisant les rubriques suivantes (mise à nulle) : cod_agrem_vl, date_fin agrément_vl, motif_fin_agrement_vl …
            newCentre.RaisonSociale = centreForm.RaisonSociale;
            newCentre.NumSiren = centreForm.NumSiren;
            newCentre.EnseigneCommerciale = centreForm.EnseigneCommerciale;
            newCentre.NomRepresentantLegal = centreForm.NomRepresentantLegal;
            newCentre.L1Adresse = centreForm.L1Adresse;
            newCentre.L2Adresse = centreForm.L2Adresse;
            newCentre.CpCentre = centreForm.CpCentre;
            /* Le code département reste inchangé 
            newCentre.CodDepartement = centreForm.CpCentre.Substring(0, 2);
            short codDep;
            if (Int16.TryParse(newCentre.CodDepartement, out codDep))
            {
                if (codDep > 95)
                {
                    newCentre.CodDepartement = centreForm.CpCentre.Substring(0, 3);
                }
            }
            */

            newCentre.Ville = centreForm.Ville;
            newCentre.NumTel = centreForm.NumTel;
            newCentre.RStatutJuridique = centreForm.RStatutJuridique;

            switch (categorie)
            {
                case rCategorieVehicule.CategorieEnum.VL:
                    newCentre.DateEffetNotificationVl = centreForm.DateEffetNotificationVl;
                    newCentre.ReseauRattachementVL = centreForm.ReseauRattachementVL;
                    //10/04/2025 PLE Ajout SAV : intialisation à false
                    newCentre.SvaVL = false;
                    break;
                case rCategorieVehicule.CategorieEnum.PL:
                    newCentre.DateEffetNotificationPl = centreForm.DateEffetNotificationPl;
                    newCentre.ReseauRattachementPL = centreForm.ReseauRattachementPL;
                    //10/04/2025 PLE Ajout SAV : intialisation à false
                    newCentre.SvaPL = false;
                    break;
                case rCategorieVehicule.CategorieEnum.CL:
                    newCentre.DateEffetNotificationCl = centreForm.DateEffetNotificationCl;
                    newCentre.ReseauRattachementCL = centreForm.ReseauRattachementCL;
                    //10/04/2025 PLE Ajout SAV : intialisation à false
                    newCentre.SvaCL = false;
                    break;
            }

            PasswordGenerator pGenerateur = PasswordGenerator.getInstance();
            string passWord = pGenerateur.GenerateForDesizedSize();
            string numeroAccuseReception = passWord.Substring(0, 4).ToLower();

            switch (categorie)
            {
                case rCategorieVehicule.CategorieEnum.VL:
                    //if (nouvelleFiche)
                    newCentre.AncienNumeroAgrementVl = centreEnCours.CodAgremVl;
                    newCentre.DateEffetNotificationVl = centreForm.DateEffetNotificationVl;
                    newCentre.DateFinAgrementVl = null;
                    newCentre.CommentaireAvisOtcVl = null;
                    newCentre.EtatAgrementVl = Convert.ToString((int)pCentreEncour.CodeEtatAgrementVl.AGREMENT_OK);
                    if (newCentre.MotifFinAgrementVl == Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementVL.CHANGEMENT_EXPLOITANT))
                    {
                        newCentre.RMotifModification = this._motifModificationService.FindBy(rMotifModification.TypeModification.CHEXPLVL.ToString());
                    }
                    if (newCentre.MotifFinAgrementVl == Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementVL.CHANGEMENT_LOCALISATION))
                    {
                        newCentre.RMotifModification = this._motifModificationService.FindBy(rMotifModification.TypeModification.CHLOCAVL.ToString());
                    }
                    if (newCentre.MotifFinAgrementVl == Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementVL.CHANGEMENT_RESEAU))
                    {
                        newCentre.RMotifModification = this._motifModificationService.FindBy(rMotifModification.TypeModification.CHRATTVL.ToString());
                    }
                    if (centreForm.CodAgremVl == null)
                    {
                        newCentre.CodAgremVl = this.generateNewAgrem(newCentre, categorie);
                    }
                    else
                    {
                        newCentre.CodAgremVl = centreForm.CodAgremVl;
                    }
                    newCentre.AccesPublicVl = false;
                    if (!rReseau.IdCodeReseau.Z.ToString().Equals(newCentre.ReseauRattachementVL.Id))
                    {
                        newCentre.NumAccuseReceptionVL = numeroAccuseReception;
                    }
                    newCentre.MotifFinAgrementVl = null;
                    break;

                case rCategorieVehicule.CategorieEnum.PL:
                    //if (nouvelleFiche)
                    newCentre.AncienNumeroAgrementPl = centreEnCours.CodAgremPl;
                    newCentre.DateEffetNotificationPl = centreForm.DateEffetNotificationPl;
                    newCentre.DateFinAgrementPl = null;
                    newCentre.CommentaireAvisOtcPl = null;
                    newCentre.EtatAgrementPl = Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_OK);
                    if (newCentre.MotifFinAgrementPl == Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementPL.CHANGEMENT_EXPLOITANT))
                    {
                        newCentre.RMotifModification = this._motifModificationService.FindBy(rMotifModification.TypeModification.CHEXPLPL.ToString());
                    }
                    if (newCentre.MotifFinAgrementPl == Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementPL.CHANGEMENT_LOCALISATION))
                    {
                        newCentre.RMotifModification = this._motifModificationService.FindBy(rMotifModification.TypeModification.CHLOCAPL.ToString());
                    }
                    if (newCentre.MotifFinAgrementPl == Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementPL.CHANGEMENT_RESEAU))
                    {
                        newCentre.RMotifModification = this._motifModificationService.FindBy(rMotifModification.TypeModification.CHRATTPL.ToString());
                    }
                    if (centreForm.CodAgremPl == null)
                    {
                        newCentre.CodAgremPl = this.generateNewAgrem(newCentre, categorie);
                    }
                    else
                    {
                        newCentre.CodAgremPl = centreForm.CodAgremPl;
                    }
                    newCentre.AccesPublicPl = false;
                    if (!rReseau.IdCodeReseau.Z.ToString().Equals(newCentre.ReseauRattachementPL.Id))
                    {
                        newCentre.NumAccuseReceptionPL = numeroAccuseReception;
                    }
                    newCentre.MotifFinAgrementPl = null;
                    break;

                case rCategorieVehicule.CategorieEnum.CL:
                    //if (nouvelleFiche) 
                    newCentre.AncienNumeroAgrementCl = centreEnCours.CodAgremCl;
                    newCentre.DateEffetNotificationCl = centreForm.DateEffetNotificationCl;
                    newCentre.DateFinAgrementCl = null;
                    newCentre.CommentaireAvisOtcCl = null;
                    newCentre.EtatAgrementCl = Convert.ToString((int)pCentreEncour.CodeEtatAgrementCl.AGREMENT_OK);
                    if (newCentre.MotifFinAgrementCl == Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementCL.CHANGEMENT_EXPLOITANT))
                    {
                        newCentre.RMotifModification = this._motifModificationService.FindBy(rMotifModification.TypeModification.CHEXPLCL.ToString());
                    }
                    if (newCentre.MotifFinAgrementCl == Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementCL.CHANGEMENT_LOCALISATION))
                    {
                        newCentre.RMotifModification = this._motifModificationService.FindBy(rMotifModification.TypeModification.CHLOCACL.ToString());
                    }
                    if (newCentre.MotifFinAgrementCl == Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementCL.CHANGEMENT_RESEAU))
                    {
                        newCentre.RMotifModification = this._motifModificationService.FindBy(rMotifModification.TypeModification.CHRATTCL.ToString());
                    }
                    if (centreForm.CodAgremCl == null)
                    {
                        newCentre.CodAgremCl = this.generateNewAgrem(newCentre, categorie);
                    }
                    else
                    {
                        newCentre.CodAgremCl = centreForm.CodAgremCl;
                    }
                    newCentre.AccesPublicCl = false;
                    if (!rReseau.IdCodeReseau.Z.ToString().Equals(newCentre.ReseauRattachementCL.Id))
                    {
                        newCentre.NumAccuseReceptionCL = numeroAccuseReception;
                    }
                    newCentre.MotifFinAgrementCl = null;
                    break;
            }

            //données système de suivi
            newCentre.DateMiseaJour = System.DateTime.Now;
            //newCentre.DateEffetModification = System.DateTime.Now;
            newCentre.RUtilisateur = this._utilisateurService.FindBy(codeUtilisateur);

            //enregistrement du centre (ancien ou nouveau)

            this.Save(newCentre);
            //Création de l'utilisateur temporaire si besoin
            //EG le 16/02/2012 à cause du bug de la création du user sur la violation de contrainte sur la foreign key 
            //EG le 17/02/2012 à cause du bug du "switch" d'identity entre deux sessions....
            //if (newCentre.Id == 0)
            if (nouvelleFiche)
            {
                log.Debug("nouvelAgrement ID de la nouvelle fiche avant recherche ID = " + newCentre.Id);
                int idNew = newCentre.Id;
                newCentre = this.FindNewCentreFor(centreEnCours.Id);
                if (newCentre != null)
                {
                    string idCentrePrec = newCentre.PCentreEncourPrecedent != null ? Convert.ToString(newCentre.PCentreEncourPrecedent.Id) : "??";
                    log.Debug(string.Format("nouvelAgrement ID de la nouvelle fiche après recherche ID = {0} , IDPREC = {1} , IDENCOURS = {2} ", newCentre.Id, idCentrePrec, centreEnCours.Id));
                    if (idNew != newCentre.Id)
                    {
                        log.Warn(string.Format("nouvelAgrement les ID sont différents entre deux requêtes ID0 = {0} , ID1 = {1}", idNew, newCentre.Id));
                    }
                }
                else
                {
                    log.Error("nouvelAgrement() : Erreur dans la récupération de la nouvelle fiche pour centre ID = " + centreEnCours.Id);
                }
            }

            if (newCentre != null)
                createUserTemporaire(newCentre, categorie, codeUtilisateur);

            if (transaction != null)
                transaction.Commit();

            log.Debug("nouvelAgrement ID du CENTRE = " + newCentre.Id);
            return newCentre;

        }

        private void createUserTemporaire(pCentreEncour centre, rCategorieVehicule.CategorieEnum categVehicule, string codeUtilisateur)
        {
            rProfil profil = new rProfil();
            rProfil2 profil2 = new rProfil2();
            profil = this._profilService.FindBy((int)rProfil.CodeProfil.CENTRE_INDEPENDANT);
            profil2 = this._profil2Service.FindBy((int)rProfil2.CodeProfil2.CENTRE_INDEPENDANT);

            PasswordGenerator pGenerateur = PasswordGenerator.getInstance();
            string passWord = pGenerateur.GenerateForDesizedSize();
            string numeroAccuseReception = passWord.Substring(0, 4).ToLower();

            //EG le 03/01/2012 : mot de passe en minuscule
            passWord = passWord.ToLower();

            switch (categVehicule)
            {
                case rCategorieVehicule.CategorieEnum.VL:
                    // création d'un utilisateur tenporaire pour les centres indépendants
                    if (Convert.ToString(rReseau.IdCodeReseau.Z).Equals(centre.ReseauRattachementVL.Id))
                    {
                        this._utilisateurService.CreateUserTemporaire(centre.CodAgremVl.ToLower(), passWord, profil, profil2, codeUtilisateur, categVehicule, centre);
                    }
                    break;
                case rCategorieVehicule.CategorieEnum.PL:
                    if (Convert.ToString(rReseau.IdCodeReseau.Z).Equals(centre.ReseauRattachementPL.Id))
                    {
                        this._utilisateurService.CreateUserTemporaire(centre.CodAgremPl.ToLower(), passWord, profil, profil2, codeUtilisateur, categVehicule, centre);
                    }
                    break;
                case rCategorieVehicule.CategorieEnum.CL:
                    if (Convert.ToString(rReseau.IdCodeReseau.Z).Equals(centre.ReseauRattachementCL.Id))
                    {
                        //PLE le 21/02/2024 : le user CL reste en majuscule
                        //this._utilisateurService.CreateUserTemporaire(centre.CodAgremCl.ToLower(), passWord, profil, profil2, codeUtilisateur, categVehicule, centre);
                        this._utilisateurService.CreateUserTemporaire(centre.CodAgremCl, passWord, profil, profil2, codeUtilisateur, categVehicule, centre);
                    }
                    break;

            }
        }

        private pCentreEncour createNewCentre(pCentreEncour centre)
        {
            pCentreEncour nouveauCentre = new pCentreEncour();
            nouveauCentre.AccesPublicCl = centre.AccesPublicCl;
            nouveauCentre.AccesPublicPl = centre.AccesPublicPl;
            nouveauCentre.AccesPublicVl = centre.AccesPublicVl;
            nouveauCentre.AncienNumeroAgrementCl = centre.AncienNumeroAgrementCl;
            nouveauCentre.AncienNumeroAgrementPl = centre.AncienNumeroAgrementPl;
            nouveauCentre.AncienNumeroAgrementVl = centre.AncienNumeroAgrementVl;
            nouveauCentre.CodAgremCl = centre.CodAgremCl;
            nouveauCentre.CodAgremPl = centre.CodAgremPl;
            nouveauCentre.CodAgremVl = centre.CodAgremVl;
            nouveauCentre.CodDepartement = centre.CodDepartement;
            nouveauCentre.CommentaireAvisOtcCl = centre.CommentaireAvisOtcCl;
            nouveauCentre.CommentaireAvisOtcPl = centre.CommentaireAvisOtcPl;
            nouveauCentre.CommentaireAvisOtcVl = centre.CommentaireAvisOtcVl;
            nouveauCentre.CpCentre = centre.CpCentre;
            nouveauCentre.DateEcheanceAgrementPl = centre.DateEcheanceAgrementPl;
            nouveauCentre.DateEffetModification = null;
            nouveauCentre.DateEffetNotificationCl = centre.DateEffetNotificationCl;
            nouveauCentre.DateEffetNotificationPl = centre.DateEffetNotificationPl;
            nouveauCentre.DateEffetNotificationVl = centre.DateEffetNotificationVl;
            nouveauCentre.DateFinAgrementCl = centre.DateFinAgrementCl;
            nouveauCentre.DateFinAgrementPl = centre.DateFinAgrementPl;
            nouveauCentre.DateFinAgrementVl = centre.DateFinAgrementVl;
            nouveauCentre.DateMiseaJour = centre.DateMiseaJour;
            nouveauCentre.EnseigneCommerciale = centre.EnseigneCommerciale;
            nouveauCentre.EtatAgrementCl = centre.EtatAgrementCl;
            nouveauCentre.EtatAgrementPl = centre.EtatAgrementPl;
            nouveauCentre.EtatAgrementVl = centre.EtatAgrementVl;
            nouveauCentre.L1Adresse = centre.L1Adresse;
            nouveauCentre.L2Adresse = centre.L2Adresse;
            nouveauCentre.MotifFinAgrementCl = centre.MotifFinAgrementCl;
            nouveauCentre.MotifFinAgrementPl = centre.MotifFinAgrementPl;
            nouveauCentre.MotifFinAgrementVl = centre.MotifFinAgrementVl;
            nouveauCentre.NomRepresentantLegal = centre.NomRepresentantLegal;
            nouveauCentre.NumAccuseReceptionCL = centre.NumAccuseReceptionCL;
            nouveauCentre.NumAccuseReceptionPL = centre.NumAccuseReceptionPL;
            nouveauCentre.NumAccuseReceptionVL = centre.NumAccuseReceptionVL;
            nouveauCentre.NumFax = centre.NumFax;
            nouveauCentre.NumOrdreCl = centre.NumOrdreCl;
            nouveauCentre.NumOrdrePl = centre.NumOrdrePl;
            nouveauCentre.NumOrdreVl = centre.NumOrdreVl;
            nouveauCentre.NumSiren = centre.NumSiren;
            nouveauCentre.NumTel = centre.NumTel;
            nouveauCentre.PCentreEncourPrecedent = centre;
            nouveauCentre.RaisonSociale = centre.RaisonSociale;
            nouveauCentre.ReseauRattachementCL = centre.ReseauRattachementCL;
            nouveauCentre.ReseauRattachementPL = centre.ReseauRattachementPL;
            nouveauCentre.ReseauRattachementVL = centre.ReseauRattachementVL;
            nouveauCentre.RMotifModification = centre.RMotifModification;
            nouveauCentre.RPrefecture = centre.RPrefecture;
            nouveauCentre.RRegroupementIndependant = centre.RRegroupementIndependant;
            nouveauCentre.RStatutJuridique = centre.RStatutJuridique;
            nouveauCentre.RUtilisateur = centre.RUtilisateur;
            nouveauCentre.TypeCentre = centre.TypeCentre;
            nouveauCentre.Ville = centre.Ville;

            return nouveauCentre;
        }

        private string generateNewAgrem(pCentreEncour centre, rCategorieVehicule.CategorieEnum categorie)
        {
            //récupération du dernier n° d'ordre disponible
            rNumordreAgrement n = this._numOrdreAgrementService.FindBy(centre.RPrefecture.Id);
            string numordre = n.NumOrdreCentre.ToString();

            //enregistrer le compteur dans la base
            n.NumOrdreCentre = n.NumOrdreCentre + 1;
            this._numOrdreAgrementService.Save(n);

            //constitution du n° d'agrément
            string depCentre = centre.CodDepartement;
            if (depCentre.Length == 1) depCentre = "00" + depCentre;
            if (depCentre.Length == 2) depCentre = "0" + depCentre;

            if (numordre.Length == 1) numordre = "00" + numordre;
            if (numordre.Length == 2) numordre = "0" + numordre;

            string codeReseauRattachement = rCategorieVehicule.CategorieEnum.VL.Equals(categorie) ?
                centre.ReseauRattachementVL.Id : rCategorieVehicule.CategorieEnum.PL.Equals(categorie) ?
                centre.ReseauRattachementPL.Id : centre.ReseauRattachementCL.Id;

            //PLE le 21/02/2024 Utiliisation de la lettre réseau
            rReseau reseau = this._reseauService.FindBy(codeReseauRattachement);
            string lettreReseauRattachement = reseau.LettreReseau;

            //PLE le 21/02/2024 première lettre L
            string typeCentre;
            switch (categorie)
            {
                case rCategorieVehicule.CategorieEnum.CL :
                    typeCentre = "L";
                    break;
                default:
                    typeCentre = centre.TypeCentre;
                    break;
            }

            //PLE le 21/02/2024 première lettre L
            //string numagrement = string.Format("{0}{1}{2}{3}", centre.TypeCentre, depCentre, codeReseauRattachement, numordre);
            string numagrement = string.Format("{0}{1}{2}{3}", typeCentre, depCentre, lettreReseauRattachement, numordre);

            return numagrement;
        }

        public IList<ItemListeDesInfosCertificatsExpireOuRevoque> findAllAgreeAvecCertificatExpireOuRevoque(rCategorieVehicule.CategorieEnum categorie)
        {
            var repository = this._Repository as IpCentreEncourRepository;
            return repository.findAllAgreeAvecCertificatExpireOuRevoque(categorie);
        }

        public pCentreEncour FindNewCentreFor(pCentreEncour centreOld)
        {
            if (centreOld == null) return null;
            return this.FindNewCentreFor(centreOld.Id);
        }

        public pCentreEncour FindNewCentreFor(int centreId)
        {
            Query q = new Query();
            q.Criteria.Add(new Criteria("PCentreEncourPrecedent.Id", centreId, CriteriaOperator.EQUALS));

            IList<pCentreEncour> liste = this.FindBy(q);
            if (liste != null && liste.Count > 0)
            {
                return liste.ElementAt(0);
            }
            return null;
        }

        public IList<pCentreEncour> FindAllDemandeEnCours(rPrefecture prefecture, rCategorieVehicule.CategorieEnum categorie)
        {
            Dictionary<rCategorieVehicule.CategorieEnum, string> queryColumns = new Dictionary<rCategorieVehicule.CategorieEnum, string>();
            queryColumns.Add(rCategorieVehicule.CategorieEnum.VL, "EtatAgrementVl");
            queryColumns.Add(rCategorieVehicule.CategorieEnum.PL, "EtatAgrementPl");
            queryColumns.Add(rCategorieVehicule.CategorieEnum.CL, "EtatAgrementCl");

            Query q = new Query();
            q.Criteria.Add(new Criteria(queryColumns[categorie], Convert.ToString((int)pCentreEncour.CodeEtatAgrementVl.AGREMENT_DEMANDE_EN_COURS), CriteriaOperator.EQUALS));
            q.Criteria.Add(new Criteria("RPrefecture", prefecture, CriteriaOperator.EQUALS));

            return this.FindBy(q);

        }

        public IList<pCentreEncour> FindAllDemandeEnCours(rPrefecture prefecture)
        {
            IList<pCentreEncour> liste = new List<pCentreEncour>();
            IEnumerable<pCentreEncour> retListe = liste.Union(this.FindAllDemandeEnCours(prefecture, rCategorieVehicule.CategorieEnum.VL))
                                                             .Union(this.FindAllDemandeEnCours(prefecture, rCategorieVehicule.CategorieEnum.PL))
                                                             .Union(this.FindAllDemandeEnCours(prefecture, rCategorieVehicule.CategorieEnum.CL));

            return retListe.ToList<pCentreEncour>();

        }

        public IList<pCentreEncour> FindCentreCLPourSiteWeb(rPrefecture prefecture)
        {
            Query qCentreCL = new Query();
            qCentreCL.Criteria.Add(new Criteria("RPrefecture", prefecture, CriteriaOperator.EQUALS));
            qCentreCL.Criteria.Add(new Criteria("EtatAgrementCl", "1", CriteriaOperator.EQUALS));
            qCentreCL.Criteria.Add(new Criteria("AccesPublicCl", true, CriteriaOperator.EQUALS));

            Query qExtVL = new Query();
            qExtVL.Criteria.Add(new Criteria("RPrefecture", prefecture, CriteriaOperator.EQUALS));
            qExtVL.Criteria.Add(new Criteria("EtatAgrementVl", "1", CriteriaOperator.EQUALS));
            qExtVL.Criteria.Add(new Criteria("AccesPublicVl", true, CriteriaOperator.EQUALS));
            qExtVL.Criteria.Add(new Criteria("ExtAgremClCatVeh", (short)1, CriteriaOperator.EQUALS));
            qExtVL.Criteria.Add(new Criteria("ExtAgremClStatut", (short)1, CriteriaOperator.EQUALS)); 

            IList<pCentreEncour> liste = new List<pCentreEncour>();
            IEnumerable<pCentreEncour> retListe = liste.Union(this.FindBy(qCentreCL))
                                                       .Union(this.FindBy(qExtVL));

            return retListe.ToList<pCentreEncour>();
        }

        public IList<pCentreEncour> FindAllNouvellementAgree(rPrefecture prefecture)
        {
            IList<pCentreEncour> liste = new List<pCentreEncour>();
            IEnumerable<pCentreEncour> retListe = liste.Union(this.FindAllNouvellementAgree(prefecture, rCategorieVehicule.CategorieEnum.VL))
                                                       .Union(this.FindAllNouvellementAgree(prefecture, rCategorieVehicule.CategorieEnum.PL))
                                                       .Union(this.FindAllNouvellementAgree(prefecture, rCategorieVehicule.CategorieEnum.CL));

            return retListe.ToList<pCentreEncour>();
        }

        public IList<pCentreEncour> FindAllNouvellementAgree(rPrefecture prefecture, rCategorieVehicule.CategorieEnum categorie)
        {
            Dictionary<rCategorieVehicule.CategorieEnum, string[]> queryColumns = new Dictionary<rCategorieVehicule.CategorieEnum, string[]>();
            queryColumns.Add(rCategorieVehicule.CategorieEnum.VL, new string[] { "EtatAgrementVl", "DateEffetNotificationVl" });
            queryColumns.Add(rCategorieVehicule.CategorieEnum.PL, new string[] { "EtatAgrementPl", "DateEffetNotificationPl" });
            queryColumns.Add(rCategorieVehicule.CategorieEnum.CL, new string[] { "EtatAgrementCl", "DateEffetNotificationCl" });

            Query q = new Query();
            q.Criteria.Add(new Criteria(queryColumns[categorie][0], Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_OK), CriteriaOperator.EQUALS));
            q.Criteria.Add(new Criteria("RPrefecture", prefecture, CriteriaOperator.EQUALS));
            DateTime fin = DateTime.Now.AddDays(1);
            DateTime debut = fin.AddMonths(-1).AddDays(-fin.Day);
            q.Criteria.Add(new Criteria(queryColumns[categorie][1], debut, CriteriaOperator.GE));
            q.Criteria.Add(new Criteria(queryColumns[categorie][1], fin, CriteriaOperator.LE));

            return this.FindBy(q);
        }
    }
}
