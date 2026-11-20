using System;
using System.Collections.Generic;
using System.Linq;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Repositories;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Infrastructure;
using Utac.Rnc2.Infrastructure.Queries;
using Utac.Rnc2.Infrastructure.Search;

namespace Utac.Rnc2.Services
{
    public class pControleurEnCourService : BaseService<pContoleurEncour>, IpControleurEnCourService
    {

        private readonly IpControleurHistoService _histoService;

        private readonly IrTypeDemandeControleurService _typeDemandeService;

        private readonly IrUtilisateurService _utilisateurService;

        private readonly IrNumordreAgrementService _numOrdreAgrementService;

        private readonly IpCentreEncourService _centreService;

        private readonly IrHabilitationService _habilitationService;

        private readonly IrPrefectureService _prefectureService;

        private readonly IrReseauService _reseauService;

        public pControleurEnCourService(
                IpControleurEnCoursRepository repository,
                IUnitOfWork UnitOfWork,
                IpControleurHistoService histoService,
                IrTypeDemandeControleurService typeDemandeService,
                IrUtilisateurService utilisateurService,
                IrNumordreAgrementService numOrdreAgrementService,
                IpCentreEncourService centreService,
                IrHabilitationService habilitationService,
                IrPrefectureService prefectureService,
                IrReseauService reseauService
            )
            : base(repository, UnitOfWork)
        {
            this._histoService = histoService;
            this._typeDemandeService = typeDemandeService;
            this._utilisateurService = utilisateurService;
            this._numOrdreAgrementService = numOrdreAgrementService;
            this._centreService = centreService;
            this._habilitationService = habilitationService;
            this._prefectureService = prefectureService;
            this._reseauService = reseauService;
        }

        public override void Save(pContoleurEncour controleurModifie)
        {
            ////0 récupération de la valeur initiale
            //pContoleurEncour controleurInitial = this.FindBy(controleurModifie.Id);

            ////1 on historise via un autre service
            //this._histoService.historise(controleurInitial);

            ///** TODO modifie la date de mise a jour de controleurModifie */

            ////2 on sauvegarde en appelant la méthode de la classe de base
            //base.Save(controleurModifie);
            //if ( this.getUnitOfWork() != null )
            //    this.getUnitOfWork().Commit();
            throw new Exception("non autorisé !"); //a cause de l'historisation
        }

        public void modifyEtatCivil(object id, pContoleurEncour model, string codeUtilisateur)
        {
            //1 récupération du controleurForm
            pContoleurEncour c = this.FindBy(id);

            IUnitOfWork transaction = this.getUnitOfWork();
            if (transaction != null)
            {
                this._histoService.setUnitOfWorks(transaction);
            }

            //2 on historise
            //c.PControleurHistoriques.Add(this._histoService.historise(c));
            this._histoService.historise(c);

            //3 On modifie les informations du controleurForm 
            c.Nom = model.Nom.ToUpper();
            c.Prenom = model.Prenom;
            c.DateNaissance = model.DateNaissance;

            //données système de suivi
            c.DateMiseaJour = System.DateTime.Now;
            c.RTypeDemandeControleur = this._typeDemandeService.FindBy(rTypeDemandeControleur.TypeModification.MODCO.ToString());
            //EG le 09/05/2011
            c.RUtilisateur = this._utilisateurService.FindBy(codeUtilisateur);

            //4 On sauvegarde le controleurForm
            base.Save(c);

            if (transaction != null)
                transaction.Commit();

        }

        //Retour arriere et Annulation modification : methode commune
        //PLE 07/05/2015 
        public void retourArriere(object id, bool userMode, string codeUtilisateur, bool deleteHisto)
        {
            //On recupere selon l'id
            pContoleurEncour c = this.FindBy(id);

            //On récupère le dernier histo
            pControleurHisto controleurhisto = this._histoService.findLastBy(id);

            IUnitOfWork transaction = this.getUnitOfWork();

            //PLE 07/05/2014
            //if (userMode)
            if (!deleteHisto)
            {
                //On historise
                if (transaction != null)
                {
                    this._histoService.setUnitOfWorks(transaction);
                    this._numOrdreAgrementService.setUnitOfWorks(transaction);
                }

                this._histoService.historise(c);
            }
            //EG le 19/10/2011 on décrémente le compteur des numéro d'ordre si dernière modification = NOTIFICATION
            rTypeDemandeControleur.TypeModification dernièreModification = (rTypeDemandeControleur.TypeModification)Enum.Parse(typeof(rTypeDemandeControleur.TypeModification), c.RTypeDemandeControleur.Id);
            if (dernièreModification.Equals(rTypeDemandeControleur.TypeModification.NOTVL))
            {
                try
                {
                    int numOrdreAgrement = Convert.ToInt16(c.CodAgremVl.Substring(4, 4)) + 1;
                    rNumordreAgrement numOrdre = c.PCentreEncourVL.RPrefecture.RNumordreAgrement;
                    if (numOrdre.NumOrdreCntlrVl == numOrdreAgrement)
                    {
                        numOrdre.NumOrdreCntlrVl--;
                        this._numOrdreAgrementService.Save(numOrdre);
                    }
                }
                catch (Exception e)
                {
                    //TODO on loggue
                }
            }
            else
            {
                if (dernièreModification.Equals(rTypeDemandeControleur.TypeModification.NOTPL))
                {
                    try
                    {
                        int numOrdreAgrement = Convert.ToInt16(c.CodAgremPl.Substring(4, 4)) + 1;
                        rNumordreAgrement numOrdre = c.PCentreEncourPL.RPrefecture.RNumordreAgrement;
                        if (numOrdre.NumOrdreCntlrPl == numOrdreAgrement)
                        {
                            numOrdre.NumOrdreCntlrPl--;
                            this._numOrdreAgrementService.Save(numOrdre);
                        }
                    }
                    catch (Exception e)
                    {
                        //TODO on loggue
                    }
                }
                else
                {
                    if (dernièreModification.Equals(rTypeDemandeControleur.TypeModification.NOTCL))
                    {
                        try
                        {
                            int numOrdreAgrement = Convert.ToInt16(c.CodAgremCl.Substring(4, 4)) + 1;
                            rNumordreAgrement numOrdre = c.PCentreEncourCL.RPrefecture.RNumordreAgrement;
                            if (numOrdre.NumOrdreCntlrCl == numOrdreAgrement)
                            {
                                numOrdre.NumOrdreCntlrCl--;
                                this._numOrdreAgrementService.Save(numOrdre);
                            }
                        }
                        catch (Exception e)
                        {
                            //TODO on loggue
                        }
                    }
                }
            }

            //On met à jour les données du controleurForm à partir de l'histo
            this.updateControleurFromHistorique(c, controleurhisto);

            if (!deleteHisto)
            {
                //données "systèmes" de suivi
                c.DateMiseaJour = System.DateTime.Now;
                c.RTypeDemandeControleur = this._typeDemandeService.FindBy(rTypeDemandeControleur.TypeModification.RETOUR.ToString());
                c.RUtilisateur = this._utilisateurService.FindBy(codeUtilisateur);
            }

            //supprimer le dernier historique
            if (deleteHisto)
            {
                this._histoService.Remove(controleurhisto);
            }

            //On sauvegarde le controleurForm
            base.Save(c);

            if (transaction != null)
                transaction.Commit();

        }

        public void demandeAgrement(object id, pContoleurEncour controleur, rCategorieVehicule.CategorieEnum categorieVehicule, string codeUtilisateur)
        {
            //1 récupération du controleurForm
            pContoleurEncour c = this.FindBy(id);

            IUnitOfWork transaction = this.getUnitOfWork();
            if (transaction != null)
            {
                this._histoService.setUnitOfWorks(transaction);
            }

            //2 on historise
            this._histoService.historise(c);

            //3 On modifie les informations du controleurForm 
            string typeModification = null;
            switch (categorieVehicule)
            {
                case rCategorieVehicule.CategorieEnum.VL:
                    c.CodAgremRattachementVl = (string.IsNullOrEmpty(controleur.CodAgremRattachementVl) ? null : controleur.CodAgremRattachementVl);
                    if (!string.IsNullOrEmpty(controleur.CodAgremRattachementVl))
                    {
                        c.PCentreEncourVL = this._centreService.findByCodeAgrementVL(controleur.CodAgremRattachementVl);
                    }
                    else
                    {
                        c.PCentreEncourVL = null;
                    }
                    typeModification = rTypeDemandeControleur.TypeModification.DEAVL.ToString();
                    c.EtatAgrementVl = Convert.ToString((int)pContoleurEncour.CodeEtatAgrementVl.AGREMENT_DEMANDE_EN_COURS);
                    //EG le 19/10/2011 réinitialisation des champs
                    c.DateDebutRattachementVl = null;
                    c.DateFinRattachementVl = null;
                    c.DateEffetNotificationVl = null;
                    c.CodAgremVl = null;
                    c.DateFinAgrementVl = null;
                    c.MotifFinAgrementVl = null;
                    c.CommentaireAvisOtcVl = null;
                    break;
                case rCategorieVehicule.CategorieEnum.PL:
                    c.CodAgremRattachementPl = (string.IsNullOrEmpty(controleur.CodAgremRattachementPl) ? null : controleur.CodAgremRattachementPl);
                    if (!string.IsNullOrEmpty(controleur.CodAgremRattachementPl))
                    {
                        c.PCentreEncourPL = this._centreService.findByCodeAgrementPL(controleur.CodAgremRattachementPl);
                    }
                    else
                    {
                        c.PCentreEncourPL = null;
                    }
                    typeModification = rTypeDemandeControleur.TypeModification.DEAPL.ToString();
                    c.EtatAgrementPl = Convert.ToString((int)pContoleurEncour.CodeEtatAgrementPl.AGREMENT_DEMANDE_EN_COURS);
                    //EG le 19/10/2011 réinitialisation des champs
                    c.DateDebutRattachementPl = null;
                    c.DateFinRattachementPl = null;
                    c.DateEffetNotificationPl = null;
                    c.CodAgremPl = null;
                    c.DateFinAgrementPl = null;
                    c.MotifFinAgrementPl = null;
                    c.CommentaireAvisOtcPl = null;
                    break;
                case rCategorieVehicule.CategorieEnum.CL:
                    c.CodAgremRattachementCl = (string.IsNullOrEmpty(controleur.CodAgremRattachementCl) ? null : controleur.CodAgremRattachementCl);
                    if (!string.IsNullOrEmpty(controleur.CodAgremRattachementCl))
                    {
                        c.PCentreEncourCL = this._centreService.findByCodeAgrementCL(controleur.CodAgremRattachementCl);
                    }
                    else
                    {
                        c.PCentreEncourCL = null;
                    }
                    typeModification = rTypeDemandeControleur.TypeModification.DEACL.ToString();
                    c.EtatAgrementCl = Convert.ToString((int)pContoleurEncour.CodeEtatAgrementCl.AGREMENT_DEMANDE_EN_COURS);
                    //EG le 19/10/2011 réinitialisation des champs
                    c.DateDebutRattachementCl = null;
                    c.DateFinRattachementCl = null;
                    c.DateEffetNotificationCl = null;
                    c.CodAgremCl = null;
                    c.DateFinAgrementCl = null;
                    c.MotifFinAgrementCl = null;
                    c.CommentaireAvisOtcCl = null;
                    break;

            }

            //données système de suivi
            c.DateMiseaJour = System.DateTime.Now;
            c.RTypeDemandeControleur = this._typeDemandeService.FindBy(typeModification);
            c.RUtilisateur = this._utilisateurService.FindBy(codeUtilisateur);

            //4 On sauvegarde le controleurForm
            base.Save(c);

            if (transaction != null)
                transaction.Commit();

        }

        public pContoleurEncour creerNouveau(pContoleurEncour c, rCategorieVehicule.CategorieEnum categorie, string codeUtilisateur)
        {
            IUnitOfWork transaction = this.getUnitOfWork();

            string typeModification = null;
            c.EtatAgrementVl = Convert.ToString((int)pContoleurEncour.CodeEtatAgrementVl.AGREMENT_KO);
            c.EtatAgrementPl = Convert.ToString((int)pContoleurEncour.CodeEtatAgrementPl.AGREMENT_KO);
            c.EtatAgrementCl = Convert.ToString((int)pContoleurEncour.CodeEtatAgrementCl.AGREMENT_KO);

            switch (categorie)
            {
                case rCategorieVehicule.CategorieEnum.VL:
                    typeModification = rTypeDemandeControleur.TypeModification.CRECOVL.ToString();
                    c.EtatAgrementVl = Convert.ToString((int)pContoleurEncour.CodeEtatAgrementVl.AGREMENT_DEMANDE_EN_COURS);
                    if (!string.IsNullOrEmpty(c.CodAgremRattachementVl))
                    {
                        c.PCentreEncourVL = this._centreService.findByCodeAgrementVL(c.CodAgremRattachementVl);
                    }
                    break;
                case rCategorieVehicule.CategorieEnum.PL:
                    typeModification = rTypeDemandeControleur.TypeModification.CRECOPL.ToString();
                    c.EtatAgrementPl = Convert.ToString((int)pContoleurEncour.CodeEtatAgrementPl.AGREMENT_DEMANDE_EN_COURS);
                    if (!string.IsNullOrEmpty(c.CodAgremRattachementPl))
                    {
                        c.PCentreEncourPL = this._centreService.findByCodeAgrementPL(c.CodAgremRattachementPl);
                    }
                    break;
                case rCategorieVehicule.CategorieEnum.CL:
                    typeModification = rTypeDemandeControleur.TypeModification.CRECOCL.ToString();
                    c.EtatAgrementCl = Convert.ToString((int)pContoleurEncour.CodeEtatAgrementCl.AGREMENT_DEMANDE_EN_COURS);
                    if (!string.IsNullOrEmpty(c.CodAgremRattachementCl))
                    {
                        c.PCentreEncourCL = this._centreService.findByCodeAgrementCL(c.CodAgremRattachementCl);
                    }
                    break;

            }

            //EG le 17/10/2011 : Conversion du nom en majuscule
            if (!string.IsNullOrEmpty(c.Nom))
                c.Nom = c.Nom.ToUpper().Trim();

            //données système de suivi
            c.DateMiseaJour = System.DateTime.Now;
            c.RTypeDemandeControleur = this._typeDemandeService.FindBy(typeModification);
            c.RUtilisateur = this._utilisateurService.FindBy(codeUtilisateur);

            //4 On sauvegarde le controleurForm
            base.Save(c);

            if (transaction != null)
                transaction.Commit();

            return c;
        }

        public void notificationAgrement(object id, pContoleurEncour controleur, rCategorieVehicule.CategorieEnum categorie, string codeUtilisateur)
        {
            //récupération du centre
            pContoleurEncour c = this.FindBy(id);

            IUnitOfWork transaction = this.getUnitOfWork();
            if (transaction != null)
            {
                this._histoService.setUnitOfWorks(transaction);
                this._numOrdreAgrementService.setUnitOfWorks(transaction);
            }

            //on historise
            this._histoService.historise(c);

            string numagrement = null;
            string typeModification = null;
            //Mise a jour des données du controleurForm
            switch (categorie)
            {
                case rCategorieVehicule.CategorieEnum.VL:
                    numagrement = controleur.CodAgremVl;
                    c.EtatAgrementVl = Convert.ToString((int)pContoleurEncour.CodeEtatAgrementVl.AGREMENT_OK);
                    c.DateEffetNotificationVl = controleur.DateEffetNotificationVl;
                    //EG le 17/09/2011
                    c.DateDebutRattachementVl = c.DateEffetNotificationVl;
                    if (!string.IsNullOrEmpty(controleur.CodAgremRattachementVl))
                    {
                        c.PCentreEncourVL = this._centreService.findByCodeAgrementVL(controleur.CodAgremRattachementVl);
                        c.CodAgremRattachementVl = controleur.CodAgremRattachementVl;
                    }
                    //Pour le cas d'un ré-agrément
                    c.DateFinAgrementVl = null;
                    typeModification = rTypeDemandeControleur.TypeModification.NOTVL.ToString();
                    //EG le 19/10/2011
                    //PLE le 11/04/2025 Ajout SVA
                    c.SvaVL = controleur.SvaVL;
                    break;
                case rCategorieVehicule.CategorieEnum.PL:
                    numagrement = controleur.CodAgremPl;
                    c.EtatAgrementPl = Convert.ToString((int)pContoleurEncour.CodeEtatAgrementPl.AGREMENT_OK);
                    c.DateEffetNotificationPl = controleur.DateEffetNotificationPl;
                    //EG le 17/09/2011
                    c.DateDebutRattachementPl = c.DateEffetNotificationPl;
                    if (!string.IsNullOrEmpty(controleur.CodAgremRattachementPl))
                    {
                        c.PCentreEncourPL = this._centreService.findByCodeAgrementPL(controleur.CodAgremRattachementPl);
                        c.CodAgremRattachementPl = controleur.CodAgremRattachementPl;
                    }
                    //Pour le cas d'un ré-agrément
                    c.DateFinAgrementPl = null;
                    typeModification = rTypeDemandeControleur.TypeModification.NOTPL.ToString();
                    //EG le 19/10/2011 : pour l'habilitation par défaut
                    if (c.RHabilitation == null)
                    {
                        c.RHabilitation = this._habilitationService.FindBy(rHabilitation.HabilitationEnum.Q1.ToString());
                    }
                    //PLE le 11/04/2025 Ajout SVA
                    c.SvaPL = controleur.SvaPL;
                    break;
                case rCategorieVehicule.CategorieEnum.CL:
                    numagrement = controleur.CodAgremCl;
                    c.EtatAgrementCl = Convert.ToString((int)pContoleurEncour.CodeEtatAgrementCl.AGREMENT_OK);
                    c.DateEffetNotificationCl = controleur.DateEffetNotificationCl;
                    //EG le 17/09/2011
                    c.DateDebutRattachementCl = c.DateEffetNotificationCl;
                    if (!string.IsNullOrEmpty(controleur.CodAgremRattachementCl))
                    {
                        c.PCentreEncourCL = this._centreService.findByCodeAgrementCL(controleur.CodAgremRattachementCl);
                        c.CodAgremRattachementCl = controleur.CodAgremRattachementCl;
                    }
                    //Pour le cas d'un ré-agrément
                    c.DateFinAgrementCl = null;
                    typeModification = rTypeDemandeControleur.TypeModification.NOTCL.ToString();
                    //PLE le 11/04/2025 Ajout SVA
                    c.SvaCL = controleur.SvaCL;
                    break;
            }

            //doit-on calculer le numéro d'agrément du contrôleur ?
            if (string.IsNullOrEmpty(numagrement))
            {
                //On récupère le centre de rattachement associé
                string codCentre = null;
                switch (categorie)
                {
                    case rCategorieVehicule.CategorieEnum.VL: codCentre = controleur.CodAgremRattachementVl; break;
                    case rCategorieVehicule.CategorieEnum.PL: codCentre = controleur.CodAgremRattachementPl; break;
                    case rCategorieVehicule.CategorieEnum.CL: codCentre = controleur.CodAgremRattachementCl; break;
                }
                pCentreEncour centreRattachement = this._centreService.findByCodeAgrement(codCentre);

                if (centreRattachement == null)
                {
                    //est-ce possible ?
                    throw new Exception(string.Format("Le centre de rattachement({0}) est inconnu !", codCentre));
                }
                //récupération du dernier n° d'ordre disponible + incrémentation
                rNumordreAgrement n = this._numOrdreAgrementService.FindBy(centreRattachement.RPrefecture.Id);
                int numordre = 0;
                string depCentre = centreRattachement.CodDepartement;
                if (depCentre.Length == 1) depCentre = "00" + depCentre;
                if (depCentre.Length == 2) depCentre = "0" + depCentre;

                //EG le 11/08/2011 suppression de la colonne cod_reseau
                //string lettreCentre = centreRattachement.RReseau.Id;
                string lettreCentre = string.Empty;

                switch (categorie)
                {
                    case rCategorieVehicule.CategorieEnum.VL:
                        numordre = n.NumOrdreCntlrVl;
                        n.NumOrdreCntlrVl++;
                        //EG le 11/08/2011 suppression de la colonne cod_reseau
                        lettreCentre = centreRattachement.ReseauRattachementVL.Id;
                        break;
                    case rCategorieVehicule.CategorieEnum.PL:
                        numordre = n.NumOrdreCntlrPl;
                        n.NumOrdreCntlrPl++;
                        //EG le 11/08/2011 suppression de la colonne cod_reseau
                        lettreCentre = centreRattachement.ReseauRattachementPL.Id;
                        break;
                    case rCategorieVehicule.CategorieEnum.CL:
                        numordre = n.NumOrdreCntlrCl;
                        n.NumOrdreCntlrCl++;
                        //EG le 11/08/2011 suppression de la colonne cod_reseau
                        //PLE le 02/03/2024 Lettre réseau
                        //lettreCentre = centreRattachement.ReseauRattachementCL.Id;
                        lettreCentre = centreRattachement.ReseauRattachementCL.LettreReseau;
                        break;
                }

                //enregistrer le compteur dans la base
                this._numOrdreAgrementService.Save(n);

                //constitution du n° d'agrément , exemple 001D0001 , dep du centre + lettre du centre + numordre
                numagrement = string.Format("{0}{1}{2}", depCentre, lettreCentre, string.Format("{0:0000}", numordre));

            }

            switch (categorie)
            {
                case rCategorieVehicule.CategorieEnum.VL: c.CodAgremVl = numagrement; break;
                case rCategorieVehicule.CategorieEnum.PL: c.CodAgremPl = numagrement; break;
                case rCategorieVehicule.CategorieEnum.CL: c.CodAgremCl = numagrement; break;
            }

            //données système de suivi
            c.DateMiseaJour = System.DateTime.Now;
            c.RUtilisateur = this._utilisateurService.FindBy(codeUtilisateur);
            c.RTypeDemandeControleur = this._typeDemandeService.FindBy(typeModification);

            //enregistrement du controleurForm
            base.Save(c);

            if (transaction != null)
                transaction.Commit();

        }

        public void rejetAgrement(object id, pContoleurEncour controleur, rCategorieVehicule.CategorieEnum categorie, string codeUtilisateur)
        {
            //récupération du centre
            pContoleurEncour c = this.FindBy(id);

            IUnitOfWork transaction = this.getUnitOfWork();
            if (transaction != null)
            {
                this._histoService.setUnitOfWorks(transaction);
                this._numOrdreAgrementService.setUnitOfWorks(transaction);
            }

            //on historise
            this._histoService.historise(c);

            string typeModification = null;
            //Mise a jour des données du controleurForm
            switch (categorie)
            {
                case rCategorieVehicule.CategorieEnum.VL:
                    c.EtatAgrementVl = Convert.ToString((int)pContoleurEncour.CodeEtatAgrementVl.AGREMENT_REJETE);
                    c.CommentaireAvisOtcVl = controleur.CommentaireAvisOtcVl;
                    typeModification = rTypeDemandeControleur.TypeModification.REJVL.ToString();
                    break;
                case rCategorieVehicule.CategorieEnum.PL:
                    c.EtatAgrementPl = Convert.ToString((int)pContoleurEncour.CodeEtatAgrementPl.AGREMENT_REJETE);
                    c.CommentaireAvisOtcPl = controleur.CommentaireAvisOtcPl;
                    typeModification = rTypeDemandeControleur.TypeModification.REJPL.ToString();
                    break;
                case rCategorieVehicule.CategorieEnum.CL:
                    c.EtatAgrementCl = Convert.ToString((int)pContoleurEncour.CodeEtatAgrementCl.AGREMENT_REJETE);
                    c.CommentaireAvisOtcCl = controleur.CommentaireAvisOtcCl;
                    typeModification = rTypeDemandeControleur.TypeModification.REJCL.ToString();
                    break;
            }

            //données système de suivi
            c.DateMiseaJour = System.DateTime.Now;
            c.RUtilisateur = this._utilisateurService.FindBy(codeUtilisateur);
            c.RTypeDemandeControleur = this._typeDemandeService.FindBy(typeModification);

            //enregistrement du controleurForm
            base.Save(c);

            if (transaction != null)
                transaction.Commit();
        }

        public void annulerAgrement(object id, pContoleurEncour controleur, rCategorieVehicule.CategorieEnum categorie, string codeUtilisateur)
        {
            //récupération du centre
            pContoleurEncour c = this.FindBy(id);

            IUnitOfWork transaction = this.getUnitOfWork();
            if (transaction != null)
            {
                this._histoService.setUnitOfWorks(transaction);
                this._numOrdreAgrementService.setUnitOfWorks(transaction);
            }

            //on historise
            this._histoService.historise(c);

            string typeModification = null;
            //Mise a jour des données du controleurForm
            switch (categorie)
            {
                case rCategorieVehicule.CategorieEnum.VL:
                    c.EtatAgrementVl = Convert.ToString((int)pContoleurEncour.CodeEtatAgrementVl.AGREMENT_KO);
                    c.MotifFinAgrementVl = Convert.ToString((int)pContoleurEncour.CodeMotifFinAgrementVl.CESSATION_ACTIVITE);
                    c.DateFinAgrementVl = controleur.DateFinAgrementVl;
                    typeModification = rTypeDemandeControleur.TypeModification.ANUVL.ToString();
                    //EG le 19/10/2011 : Alimenter l'attribut : date_fin_rattachement_vl ou pl ou cl <--- date_fin_agrement_vl ou pl ou cl
                    c.DateFinRattachementVl = c.DateFinAgrementVl;
                    //EG le 19/10/2011 : annulation de l'habilitation GAZ VL
                    if (c.HabilitationGazVL != null)
                    {
                        if (c.HabilitationGazVL == 1)
                        {
                            c.HabilitationGazVL = 0;
                            c.DateDebutFinHabilitationGazVL = c.DateFinAgrementVl;
                        }
                    }
                    break;
                case rCategorieVehicule.CategorieEnum.PL:
                    c.EtatAgrementPl = Convert.ToString((int)pContoleurEncour.CodeEtatAgrementPl.AGREMENT_KO);
                    c.MotifFinAgrementPl = Convert.ToString((int)pContoleurEncour.CodeMotifFinAgrementPl.CESSATION_ACTIVITE);
                    c.DateFinAgrementPl = controleur.DateFinAgrementPl;
                    typeModification = rTypeDemandeControleur.TypeModification.ANUPL.ToString();
                    //EG le 19/10/2011 : Alimenter l'attribut : date_fin_rattachement_vl ou pl ou cl <--- date_fin_agrement_vl ou pl ou cl
                    c.DateFinRattachementPl = c.DateFinAgrementPl;
                    break;
                case rCategorieVehicule.CategorieEnum.CL:
                    c.EtatAgrementCl = Convert.ToString((int)pContoleurEncour.CodeEtatAgrementCl.AGREMENT_KO);
                    c.MotifFinAgrementCl = Convert.ToString((int)pContoleurEncour.CodeMotifFinAgrementCl.CESSATION_ACTIVITE);
                    c.DateFinAgrementCl = controleur.DateFinAgrementCl;
                    typeModification = rTypeDemandeControleur.TypeModification.ANUCL.ToString();
                    //EG le 19/10/2011 : Alimenter l'attribut : date_fin_rattachement_vl ou pl ou cl <--- date_fin_agrement_vl ou pl ou cl
                    c.DateFinRattachementCl = c.DateFinAgrementCl;
                    break;
            }

            //données système de suivi
            c.DateMiseaJour = System.DateTime.Now;
            c.RUtilisateur = this._utilisateurService.FindBy(codeUtilisateur);
            c.RTypeDemandeControleur = this._typeDemandeService.FindBy(typeModification);

            //enregistrement du controleurForm
            base.Save(c);

            if (transaction != null)
                transaction.Commit();
        }

        public void changementCentreRattachement(int id, pContoleurEncour controleurForm, rCategorieVehicule.CategorieEnum categorie, string codeUtilisateur)
        {
            //récupération du centre
            pContoleurEncour controleurEnCours = this.FindBy(id);

            IUnitOfWork transaction = this.getUnitOfWork();
            if (transaction != null)
            {
                this._histoService.setUnitOfWorks(transaction);
            }

            //on historise
            //this._histoService.historise(controleurEnCours);
            //EG le 18/10/2011 : historisation spéciale
            rTypeDemandeControleur.TypeModification typeModification = default(rTypeDemandeControleur.TypeModification);
            switch (categorie)
            {
                case rCategorieVehicule.CategorieEnum.VL:
                    typeModification = rTypeDemandeControleur.TypeModification.CHCEVL;
                    break;
                case rCategorieVehicule.CategorieEnum.PL:
                    typeModification = rTypeDemandeControleur.TypeModification.CHCEPL;
                    break;
                case rCategorieVehicule.CategorieEnum.CL:
                    typeModification = rTypeDemandeControleur.TypeModification.CHCECL;
                    break;
            }

            this._histoService.historise(controleurEnCours, controleurForm, typeModification);

            //Mise a jour des données du controleurForm
            switch (categorie)
            {
                case rCategorieVehicule.CategorieEnum.VL:
                    //EG le 18/10/2011 : pour la gestion de la perte de l'habilitation gaz
                    pCentreEncour oldCentre = null;
                    oldCentre = controleurEnCours.PCentreEncourVL;
                    controleurEnCours.CodAgremRattachementVl = controleurForm.CodAgremRattachementVl;
                    if (!string.IsNullOrEmpty(controleurForm.CodAgremRattachementVl))
                    {
                        controleurEnCours.PCentreEncourVL = this._centreService.findByCodeAgrementVL(controleurForm.CodAgremRattachementVl);
                    }
                    controleurEnCours.DateEffetNotificationVl = controleurForm.DateEffetNotificationVl;
                    //EG le 17/10/2011
                    controleurEnCours.DateDebutRattachementVl = controleurForm.DateEffetNotificationVl;

                    //EG le 18/10/2011 : pour la gestion de la perte de l'habilitation gaz
                    if (controleurEnCours.HabilitationGazVL != null && controleurEnCours.HabilitationGazVL == 1)
                    {
                        if ("Z".Equals(controleurEnCours.PCentreEncourVL.ReseauRattachementVL.Id) || !oldCentre.ReseauRattachementVL.Id.Equals(controleurEnCours.PCentreEncourVL.Id))
                        {
                            controleurEnCours.HabilitationGazVL = 0;
                            controleurEnCours.DateDebutFinHabilitationGazVL = controleurEnCours.DateEffetNotificationVl;

                        }
                    }
                    break;
                case rCategorieVehicule.CategorieEnum.PL:
                    controleurEnCours.CodAgremRattachementPl = controleurForm.CodAgremRattachementPl;
                    if (!string.IsNullOrEmpty(controleurForm.CodAgremRattachementPl))
                    {
                        controleurEnCours.PCentreEncourPL = this._centreService.findByCodeAgrementPL(controleurForm.CodAgremRattachementPl);
                    }
                    controleurEnCours.DateEffetNotificationPl = controleurForm.DateEffetNotificationPl;
                    //EG le 17/10/2011
                    controleurEnCours.DateDebutRattachementPl = controleurForm.DateEffetNotificationPl;
                    break;
                case rCategorieVehicule.CategorieEnum.CL:
                    controleurEnCours.CodAgremRattachementCl = controleurForm.CodAgremRattachementCl;
                    if (!string.IsNullOrEmpty(controleurForm.CodAgremRattachementCl))
                    {
                        controleurEnCours.PCentreEncourCL = this._centreService.findByCodeAgrementCL(controleurForm.CodAgremRattachementCl);
                    }
                    controleurEnCours.DateEffetNotificationCl = controleurForm.DateEffetNotificationCl;
                    //EG le 17/10/2011
                    controleurEnCours.DateDebutRattachementCl = controleurForm.DateEffetNotificationCl;
                    break;
            }

            //données système de suivi
            controleurEnCours.DateMiseaJour = System.DateTime.Now;
            controleurEnCours.RUtilisateur = this._utilisateurService.FindBy(codeUtilisateur);
            controleurEnCours.RTypeDemandeControleur = this._typeDemandeService.FindBy(typeModification.ToString());

            //enregistrement du controleurForm
            base.Save(controleurEnCours);

            if (transaction != null)
                transaction.Commit();
        }

        public void declarerHabilitationGaz(int id, pContoleurEncour controleur, string codeUtilisateur)
        {
            //récupération du controleurForm
            pContoleurEncour c = this.FindBy(id);

            IUnitOfWork transaction = this.getUnitOfWork();
            if (transaction != null)
            {
                this._histoService.setUnitOfWorks(transaction);
            }

            //on historise
            this._histoService.historise(c);

            //données du formulaire
            c.HabilitationGazVL = controleur.HabilitationGazVL;
            c.DateDebutFinHabilitationGazVL = controleur.DateDebutFinHabilitationGazVL;

            //données système de suivi
            c.DateMiseaJour = System.DateTime.Now;
            c.RUtilisateur = this._utilisateurService.FindBy(codeUtilisateur);
            string typeModification = rTypeDemandeControleur.TypeModification.HABILGAZ.ToString();
            c.RTypeDemandeControleur = this._typeDemandeService.FindBy(typeModification);

            //enregistrement du controleurForm
            base.Save(c);

            if (transaction != null)
                transaction.Commit();

        }

        public void annulerAgrementSuiteSanction(pContoleurEncour controleur, pSanctionControleur sanction, string codeUtilisateur)
        {

            //Pas d'historisation car appelé dans une méthode qui historise déjà

            //On met à jour les données du controleurForm
            if (sanction.IsAgrementAffecteVL)
            {
                controleur.DateFinAgrementVl = sanction.DateDebut;
                controleur.MotifFinAgrementVl = Convert.ToString((int)pContoleurEncour.CodeMotifFinAgrementVl.RETRAIT_DEFINITIF_SUITE_SANCTION);
                controleur.EtatAgrementVl = Convert.ToString((int)pContoleurEncour.CodeEtatAgrementVl.AGREMENT_KO);
                //PLE 12/03/2012 Mise à niveau comme la méthode "AnnulerAgrément"
                controleur.DateFinRattachementVl = controleur.DateFinAgrementVl;
                if (controleur.HabilitationGazVL != null)
                {
                    if (controleur.HabilitationGazVL == 1)
                    {
                        controleur.HabilitationGazVL = 0;
                        controleur.DateDebutFinHabilitationGazVL = controleur.DateFinAgrementVl;
                    }
                }

            }

            if (sanction.IsAgrementAffectePL)
            {
                controleur.DateFinAgrementPl = sanction.DateDebut;
                controleur.MotifFinAgrementPl = Convert.ToString((int)pContoleurEncour.CodeMotifFinAgrementPl.RETRAIT_DEFINITIF_SUITE_SANCTION);
                controleur.EtatAgrementPl = Convert.ToString((int)pContoleurEncour.CodeEtatAgrementPl.AGREMENT_KO);
                //PLE 12/03/2012 Mise à niveau comme la méthode "AnnulerAgrément"
                controleur.DateFinRattachementPl = controleur.DateFinAgrementPl;
            }

            if (sanction.IsAgrementAffecteCL)
            {
                controleur.DateFinAgrementCl = sanction.DateDebut;
                controleur.MotifFinAgrementCl = Convert.ToString((int)pContoleurEncour.CodeMotifFinAgrementCl.RETRAIT_DEFINITIF_SUITE_SANCTION);
                controleur.EtatAgrementCl = Convert.ToString((int)pContoleurEncour.CodeEtatAgrementPl.AGREMENT_KO);
                //PLE 12/03/2012 Mise à niveau comme la méthode "AnnulerAgrément"
                controleur.DateFinRattachementCl = controleur.DateFinAgrementCl;
            }
            //données système de suivi
            controleur.DateMiseaJour = System.DateTime.Now;
            controleur.RUtilisateur = this._utilisateurService.FindBy(codeUtilisateur);

            //enregistrement du centre
            base.Save(controleur);

            //PAS DE COMMIT, controleurEnCours'est l'appelant qui fait le commit

        }

        public void modifSuivi(pContoleurEncour c, rTypeDemandeControleur.TypeModification typeModification, string codeUtilisateur)
        {
            //Attention le controleurForm doit être préalablement initialisé à partir de la base de données

            /*IUnitOfWork transaction = this.getUnitOfWork();
            if (transaction != null)
            {
                this._histoService.setUnitOfWorks(transaction);
            }*/

            //on historise
            this._histoService.historise(c);

            //données système de suivi
            c.DateMiseaJour = System.DateTime.Now;
            c.RUtilisateur = this._utilisateurService.FindBy(codeUtilisateur);
            c.RTypeDemandeControleur = this._typeDemandeService.FindBy(typeModification.ToString());

            //enregistrement du controleurForm
            base.Save(c);

            //ATTENTION : est appelé par une méthode qui fait un commit()
            /*if (transaction != null)
                transaction.Commit();
            */
        }

        public void declarerHabilitationPL(int id, pContoleurEncour controleur, string codeUtilisateur)
        {
            //récupération du controleurForm
            pContoleurEncour c = this.FindBy(id);

            IUnitOfWork transaction = this.getUnitOfWork();
            if (transaction != null)
            {
                this._histoService.setUnitOfWorks(transaction);
            }

            //on historise
            this._histoService.historise(c);

            //données du formulaire
            c.RHabilitation = controleur.RHabilitation;
            c.DateDebutHabilitationPl = controleur.DateDebutHabilitationPl;

            //données système de suivi
            c.DateMiseaJour = System.DateTime.Now;
            c.RUtilisateur = this._utilisateurService.FindBy(codeUtilisateur);
            string typeModification = rTypeDemandeControleur.TypeModification.HABIL.ToString();
            c.RTypeDemandeControleur = this._typeDemandeService.FindBy(typeModification);

            //enregistrement du controleurForm
            base.Save(c);

            if (transaction != null)
                transaction.Commit();

        }

        public IList<pContoleurEncour> findAllVL(pCentreEncour centre)
        {
            Query q = new Query();
            //pas de certificats
            q.Criteria.Add(new Criteria("PCentreEncourVL", centre, CriteriaOperator.EQUALS));
            return this._Repository.FindBy(q);

        }

        public IList<pContoleurEncour> findAllPL(pCentreEncour centre)
        {
            Query q = new Query();
            //pas de certificats
            q.Criteria.Add(new Criteria("PCentreEncourPL", centre, CriteriaOperator.EQUALS));
            return this._Repository.FindBy(q);
        }

        public IList<pContoleurEncour> findAllCL(pCentreEncour centre)
        {
            Query q = new Query();
            //pas de certificats
            q.Criteria.Add(new Criteria("PCentreEncourCL", centre, CriteriaOperator.EQUALS));
            return this._Repository.FindBy(q);

        }

        public void modifFiche(int id, pContoleurEncour model, string codeUtilisateur)
        {
            //récupération du controleurForm
            pContoleurEncour c = this.FindBy(id);

            IUnitOfWork transaction = this.getUnitOfWork();

            //on n'historise pas

            //données du formulaire
            c.Nom = model.Nom.ToUpper();
            c.Prenom = model.Prenom;
            c.DateNaissance = model.DateNaissance;
            c.EstPrestataire = model.EstPrestataire;
            c.CodAgremRattachementVl = model.CodAgremRattachementVl;
            //PLE 04/12/2013 Synchronisation de id_centrevl avec le code agrément du centre de rattachement VL
            if (!string.IsNullOrEmpty(model.CodAgremRattachementVl))
            {
                c.PCentreEncourVL = this._centreService.findByCodeAgrementVL(model.CodAgremRattachementVl);
            }
            else
            {
                c.PCentreEncourVL = null;
            }

            c.DateEffetNotificationVl = model.DateEffetNotificationVl;
            c.CodAgremVl = model.CodAgremVl;
            c.DateFinAgrementVl = model.DateFinAgrementVl;
            c.CommentaireAvisOtcVl = model.CommentaireAvisOtcVl;

            c.CodAgremRattachementPl = model.CodAgremRattachementPl;
            //PLE 04/12/2013 Synchronisation de id_centrepl avec le code agrément du centre de rattachement PL
            if (!string.IsNullOrEmpty(model.CodAgremRattachementPl))
            {
                c.PCentreEncourPL = this._centreService.findByCodeAgrementPL(model.CodAgremRattachementPl);
            }
            else
            {
                c.PCentreEncourPL = null;
            }

            c.DateEffetNotificationPl = model.DateEffetNotificationPl;
            c.CodAgremPl = model.CodAgremPl;
            c.DateFinAgrementPl = model.DateFinAgrementPl;
            c.CommentaireAvisOtcPl = model.CommentaireAvisOtcPl;
            c.RHabilitation = null;
            if (model.RHabilitation != null)
            {
                if (model.RHabilitation.Id != null)
                    c.RHabilitation = model.RHabilitation;

            }

            c.DateDebutHabilitationPl = model.DateDebutHabilitationPl;

            c.CodAgremRattachementCl = model.CodAgremRattachementCl;
            //PLE 04/12/2013 Synchronisation de id_centrecl avec le code agrément du centre de rattachement CL
            if (!string.IsNullOrEmpty(model.CodAgremRattachementCl))
            {
                c.PCentreEncourCL = this._centreService.findByCodeAgrementCL(model.CodAgremRattachementCl);
            }
            else
            {
                c.PCentreEncourCL = null;
            }
            c.DateEffetNotificationCl = model.DateEffetNotificationCl;
            c.CodAgremCl = model.CodAgremCl;
            c.DateFinAgrementCl = model.DateFinAgrementCl;
            c.CommentaireAvisOtcCl = model.CommentaireAvisOtcCl;

            //PLE le 29/01/202 Extension agrément CL
            c.ExtAgremClStatut = model.ExtAgremClStatut;
            c.ExtAgremClDateValid = model.ExtAgremClDateValid;
            c.ExtAgremClDateDecla = model.ExtAgremClDateDecla;
            c.ExtAgremClCatVeh = model.ExtAgremClCatVeh;

            //données système de suivi
            c.DateMiseaJour = System.DateTime.Now;
            c.RUtilisateur = this._utilisateurService.FindBy(codeUtilisateur);

            //enregistrement du controleurForm
            base.Save(c);

            if (transaction != null)
                transaction.Commit();
        }

        public void supprimeFiche(pContoleurEncour controleur)
        {
            //On supprime le contrôleur... toutes les collections sont en cascade="all-delete-orphan" cf pContoleurEncour.hbm.xml
            //La transaction est gérée dans Remove()
            this.Remove(controleur);
        }

        public void annulerModification(pContoleurEncour controleur)
        {
            IUnitOfWork transation = this.getUnitOfWork();

            if (transation != null)
            {
                this._histoService.setUnitOfWorks(transation);
            }

            //recopier le dernier historique à la place de la fiche courante
            pControleurHisto histo = this._histoService.findLastBy(controleur.Id);
            this.updateControleurFromHistorique(controleur, histo);

            //supprimer le dernier historique
            this._histoService.Remove(histo);

            //On sauvegarde la fiche "retour arrière"
            base.Save(controleur);

            if (transation != null)
            {
                transation.Commit();
            }

        }

        //On met à jour les données du contrôleur à partir de l'historique
        private void updateControleurFromHistorique(pContoleurEncour controleur, pControleurHisto historique)
        {
            //EG le 18/10/2011 : prise en compte du retour arrière suite à un changement de rattachement de centre 
            rTypeDemandeControleur.TypeModification dernierModification = (rTypeDemandeControleur.TypeModification)Enum.Parse(typeof(rTypeDemandeControleur.TypeModification), controleur.RTypeDemandeControleur.Id);

            controleur.AvisOtcPl = historique.AvisOtcPl;
            controleur.AvisOtcVl = historique.AvisOtcVl;
            controleur.CodAgremPl = historique.CodAgremPl;
            controleur.CodAgremRattachementPl = historique.CodAgremRattachementPl;
            controleur.CodAgremRattachementVl = historique.CodAgremRattachementVl;
            controleur.CodAgremVl = historique.CodAgremVl;
            controleur.CodDepartement = historique.CodDepartement;
            controleur.CommentaireAvisOtcPl = historique.CommentaireAvisOtcPl;
            controleur.CommentaireAvisOtcVl = historique.CommentaireAvisOtcVl;
            controleur.DateAvisOtcPl = historique.DateAvisOtcPl;
            controleur.DateAvisOtcVl = historique.DateAvisOtcVl;
            controleur.DateDebutHabilitationPl = historique.DateDebutHabilitationPl;
            controleur.DateDebutRattachementPl = historique.DateDebutRattachementPl;
            controleur.DateDebutRattachementVl = historique.DateDebutRattachementVl;
            controleur.DateEffetNotificationPl = historique.DateEffetNotificationPl;
            controleur.DateEffetNotificationVl = historique.DateEffetNotificationVl;
            controleur.DateFinAgrementPl = historique.DateFinAgrementPl;
            controleur.DateFinAgrementVl = historique.DateFinAgrementVl;
            controleur.DateFinRattachementPl = historique.DateFinRattachementPl;
            controleur.DateFinRattachementVl = historique.DateFinRattachementVl;
            controleur.DateFinRattachementCl = historique.DateFinRattachementCl;
            controleur.DateMiseaJour = historique.DateHisto;
            controleur.DateNaissance = historique.DateNaissance;
            controleur.EtatAgrementPl = historique.EtatAgrementPl;
            controleur.EtatAgrementVl = historique.EtatAgrementVl;
            controleur.MotifFinAgrementPl = historique.MotifFinAgrementPl;
            controleur.MotifFinAgrementVl = historique.MotifFinAgrementVl;
            controleur.Nom = historique.Nom;
            controleur.NumOrdrePl = historique.NumOrdrePl;
            controleur.NumOrdreVl = historique.NumOrdreVl;
            controleur.Prenom = historique.Prenom;
            controleur.ReferenceAvisOtcPl = historique.ReferenceAvisOtcPl;
            controleur.ReferenceAvisOtcVl = historique.ReferenceAvisOtcVl;
            controleur.RHabilitation = historique.RHabilitation;
            controleur.RPrefecture = historique.RPrefecture;
            controleur.RTypeDemandeControleur = historique.RTypeDemandeControleur;
            controleur.RUtilisateur = historique.RUtilisateur;
            //controleur.PCentreEncourCL = historique.PCentreEncourCL;
            controleur.CodAgremRattachementCl = historique.CodAgremRattachementCl;
            controleur.DateDebutRattachementCl = historique.DateDebutRattachementCl;
            controleur.DateEffetNotificationCl = historique.DateEffetNotificationCl;
            controleur.CodAgremCl = historique.CodAgremCl;
            controleur.DateFinAgrementCl = historique.DateFinAgrementCl;
            controleur.MotifFinAgrementCl = historique.MotifFinAgrementCl;
            controleur.EtatAgrementCl = historique.EtatAgrementCl;
            controleur.CommentaireAvisOtcCl = historique.CommentaireAvisOtcCl;
            controleur.NumOrdreCl = historique.NumOrdreCl;
            controleur.HabilitationGazVL = historique.HabilitationGazVL;
            controleur.DateDebutFinHabilitationGazVL = historique.DateDebutFinHabilitationGazVL;
            //PLE le 27/01/2024 Extension categorie L
            controleur.ExtAgremClStatut = historique.ExtAgremClStatut;
            controleur.ExtAgremClDateValid = historique.ExtAgremClDateValid;
            controleur.ExtAgremClDateDecla = historique.ExtAgremClDateDecla;
            controleur.ExtAgremClCatVeh = historique.ExtAgremClCatVeh;
            //PLE 11/04/2025 Prise en compte SVA
            controleur.SvaVL = historique.SvaVL;
            controleur.SvaPL = historique.SvaPL;
            controleur.SvaCL = historique.SvaCL;

            //07/05/2015 PLE - Recherche des centres de rattachement des contrôleurs
            if (historique.CodAgremRattachementVl != null)
            {
                controleur.PCentreEncourVL = this._centreService.findByCodeAgrementVL(historique.CodAgremRattachementVl);
            }
            if (historique.CodAgremRattachementPl != null)
            {
                controleur.PCentreEncourPL = this._centreService.findByCodeAgrementPL(historique.CodAgremRattachementPl);
            }
            if (historique.CodAgremRattachementCl != null)
            {
                controleur.PCentreEncourCL = this._centreService.findByCodeAgrementCL(historique.CodAgremRattachementCl);
            }

            switch (dernierModification)
            {
                case rTypeDemandeControleur.TypeModification.CHCEVL:
                    controleur.DateFinRattachementVl = null;
                    break;

                case rTypeDemandeControleur.TypeModification.CHCEPL:
                    controleur.DateFinRattachementPl = null;
                    break;

                case rTypeDemandeControleur.TypeModification.CHCECL:
                    controleur.DateFinRattachementCl = null;
                    break;
            }

        }

        //EG le 18/11/2011
        public IList<pContoleurEncour> FindByFiltreProfilReseau(FiltreRechercheProfilReseau form, out Boolean estDepartementErrone)
        {
            IpControleurEnCoursRepository repository = this._Repository as IpControleurEnCoursRepository;
            rReseau reseauFiltre = this._reseauService.FindBy(form.CodeReseau);

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

            rPrefecture prefFiltre = this._prefectureService.findByCodeDepartement(form.CodeDepartement);
            estDepartementErrone = false;
            if (prefFiltre == null)
            {
                estDepartementErrone = true;
                return null;
            }
            return repository.FindByFiltreProfilReseau(form, reseauFiltre, prefFiltre);
        }

        public IList<pContoleurEncour> FindAllForListeDesSuivisQualificationControleursPLSelonRegion(rRegion region)
        {
            IpControleurEnCoursRepository repository = this._Repository as IpControleurEnCoursRepository;
            return repository.FindAllForListeDesSuivisQualificationControleursPLSelonRegion(region);
        }

        //YM 24/05/2013
        public IList<pContoleurEncour> FindAllForListeDesSuivisQualificationControleursVLSelonRegion(rRegion region)
        {
            IpControleurEnCoursRepository repository = this._Repository as IpControleurEnCoursRepository;
            return repository.FindAllForListeDesSuivisQualificationControleursVLSelonRegion(region);
        }

        public IList<pContoleurEncour> FindAllForListeDesSuivisQualificationControleursPLSelonReseaux(IList<rReseau> reseaux)
        {
            IpControleurEnCoursRepository repository = this._Repository as IpControleurEnCoursRepository;
            return repository.FindAllForListeDesSuivisQualificationControleursPLSelonReseaux(reseaux);
        }

        public IList<pContoleurEncour> FindAllAgreesCentreFerme(rCategorieVehicule.CategorieEnum categorieVehicule, rPrefecture prefecture)
        {
            IpControleurEnCoursRepository repository = this._Repository as IpControleurEnCoursRepository;
            return repository.FindAllAgreesCentreFerme(categorieVehicule, prefecture);

        }

        public IList<pContoleurEncour> FindAllAgreesCentreFermeReseau(rCategorieVehicule.CategorieEnum categorieVehicule, IList<rReseau> reseaux)
        {
            IpControleurEnCoursRepository repository = this._Repository as IpControleurEnCoursRepository;
            return repository.FindAllAgreesCentreFermeReseau(categorieVehicule, reseaux);

        }

        public IList<pContoleurEncour> FindAllAgreesCentreFermeRegion(rCategorieVehicule.CategorieEnum categorieVehicule, rRegion region)
        {
            IpControleurEnCoursRepository repository = this._Repository as IpControleurEnCoursRepository;
            return repository.FindAllAgreesCentreFermeRegion(categorieVehicule, region);

        }

        public IList<pContoleurEncour> FindAllDemandeEnCours(rCategorieVehicule.CategorieEnum categorieVehicule, rPrefecture prefecture)
        {
            IpControleurEnCoursRepository repository = this._Repository as IpControleurEnCoursRepository;
            return repository.FindAllDemandeEnCours(categorieVehicule, prefecture);
        }

        public IList<pContoleurEncour> FindAllDemandeEnCours(rPrefecture prefecture)
        {
            IList<pContoleurEncour> liste = new List<pContoleurEncour>();
            return liste.Union(this.FindAllDemandeEnCours(rCategorieVehicule.CategorieEnum.VL, prefecture)).
                         Union(this.FindAllDemandeEnCours(rCategorieVehicule.CategorieEnum.PL, prefecture)).
                         Union(this.FindAllDemandeEnCours(rCategorieVehicule.CategorieEnum.CL, prefecture)).ToList<pContoleurEncour>();
        }

        public IList<pContoleurEncour> FindAllNouvellementAgree(rPrefecture prefecture, rCategorieVehicule.CategorieEnum categorie)
        {
            IpControleurEnCoursRepository repository = this._Repository as IpControleurEnCoursRepository;
            DateTime fin = System.DateTime.Now.AddDays(1);
            DateTime debut = fin.AddMonths(-1).AddDays(-fin.Day);
            return repository.FindAllNouvellementAgree(prefecture, categorie, debut, fin); //Attention à l'ordre à cause du between
        }

        public IList<pContoleurEncour> FindAllNouvellementAgree(rPrefecture prefecture)
        {
            IList<pContoleurEncour> liste = new List<pContoleurEncour>();
            return liste.Union(this.FindAllNouvellementAgree(prefecture, rCategorieVehicule.CategorieEnum.VL)).
                         Union(this.FindAllNouvellementAgree(prefecture, rCategorieVehicule.CategorieEnum.PL)).
                         Union(this.FindAllNouvellementAgree(prefecture, rCategorieVehicule.CategorieEnum.CL)).ToList<pContoleurEncour>();
        }

        //EG le 21/02/2012
        public override void Remove(pContoleurEncour controleur)
        {
            IUnitOfWork transaction = this.getUnitOfWork();
            if (transaction != null)
                transaction.BeginTran();

            //On remove d'abord l'historique
            IList<pControleurHisto> historique = controleur.PControleurHistoriques;
            if (historique != null && historique.Count > 0)
            {
                if (transaction != null) this._histoService.setUnitOfWorks(transaction);
                this._histoService.removeAll(controleur);
            }
            base.Remove(controleur);
            if (transaction != null)
                transaction.CommitTran();
        }

        public IList<pContoleurEncour> FindAllAgreesSansCentreRattachement(rCategorieVehicule.CategorieEnum categorieVehicule, rPrefecture prefecture)
        {
            IpControleurEnCoursRepository repository = this._Repository as IpControleurEnCoursRepository;
            return repository.FindAllAgreesSansCentreRattachement(categorieVehicule, prefecture);
        }

        //YM le 23/10/2013 liste des prestataires Européens
        public IList<pContoleurEncour> FindAllPrestatairesEuropeensVL(rPrefecture prefecture)
        {
            IpControleurEnCoursRepository repository = this._Repository as IpControleurEnCoursRepository;
            return repository.FindAllPrestatairesEuropeensVL(prefecture);
        }

        public IList<pContoleurEncour> FindAllPrestatairesEuropeensPL(rPrefecture prefecture)
        {
            IpControleurEnCoursRepository repository = this._Repository as IpControleurEnCoursRepository;
            return repository.FindAllPrestatairesEuropeensPL(prefecture);
        }

        //PLE le 22/01/2024 Extension catégorie L
        public void EnregistrerExtensionAgrementCatL(object id, pContoleurEncour controleur, rCategorieVehicule.CategorieEnum categVehicule, string codeUtilisateur)
        {
            //récupération du centre
            pContoleurEncour c = this.FindBy(id);

            IUnitOfWork transaction = this.getUnitOfWork();
            if (transaction != null)
            {
                this._histoService.setUnitOfWorks(transaction);
            }

            //on historise
            this._histoService.historise(c);

            //On met à jour les données du centre
            c.ExtAgremClStatut = controleur.ExtAgremClStatut;
            c.ExtAgremClDateValid = controleur.ExtAgremClDateValid;

            if (categVehicule.Equals(rCategorieVehicule.CategorieEnum.VL))
            { c.ExtAgremClCatVeh = (short?)rCategorieVehicule.ExtCatvehEnum.VL; }
            else
            { c.ExtAgremClCatVeh = (short?)rCategorieVehicule.ExtCatvehEnum.PL; }

            rTypeDemandeControleur.TypeModification typeModification;
            if (controleur.ExtAgremClStatut.Equals((short?)1))
            {
                typeModification = rTypeDemandeControleur.TypeModification.EXTDECLA;
            }
            else
            {
                typeModification = rTypeDemandeControleur.TypeModification.EXTRETRA;
            }
            c.RTypeDemandeControleur = this._typeDemandeService.FindBy(typeModification.ToString());

            //données système de suivi
            c.DateMiseaJour = System.DateTime.Now;
            c.ExtAgremClDateDecla = c.DateMiseaJour;

            //c.DateEffetModification = System.DateTime.Now;
            c.RUtilisateur = this._utilisateurService.FindBy(codeUtilisateur);

            //enregistrement du centre
            base.Save(c);

            if (transaction != null)
                transaction.Commit();
        }

    }

}
