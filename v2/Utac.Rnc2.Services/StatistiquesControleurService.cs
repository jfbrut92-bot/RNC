using System;
using System.Collections.Generic;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.BusinessObjects.statistiques;
using Utac.Rnc2.Contrats.Repositories.statistiques;
using Utac.Rnc2.Contrats.Services;

namespace Utac.Rnc2.Services
{
    public class StatistiquesService : IStatistiquesControleurService
    {
        private readonly IStatistiquesControleurRepository _statistiquesRepository;
        readonly IpCumulActivitePlJrnTrtService _cumulActiviteTraitementService;
        readonly IpControleurEnCourService _controleurService;

        public StatistiquesService(IStatistiquesControleurRepository repository, IpCumulActivitePlJrnTrtService cumulActiviteTraitementService, IpControleurEnCourService controleurService)
        {
            this._statistiquesRepository = repository;
            this._cumulActiviteTraitementService = cumulActiviteTraitementService;
            this._controleurService = controleurService;
        }

        public List<NombreControleursParRegionEtDepartement> getNombreControleurParRegionEtDepartement(rRegion region)
        {
            return this._statistiquesRepository.getNombreControleurParRegionEtDepartement(region);
        }

        //public IList<ItemListeDesInstallationsVéhiculesLegersELourdsSelonRegion> listeDesInstallationsVéhiculesLegersELourdsSelonRegion(BusinessObjects.rRegion region)
        //{
        //    return this._statistiquesRepository.listeDesInstallationsVéhiculesLegersELourdsSelonRegion(region);
        //}

        public IList<ItemListeDesControleursVéhiculesLegersEtLourds> listeDesControleursVehiculesLegersEtLourdsSelonRegion(BusinessObjects.rRegion region)
        {
            return this._statistiquesRepository.listeDesControleursVehiculesLegersEtLourdsSelonRegion(region);
        }

        public IList<ItemListeDesControleurs> listeDesControleursVehiculesLourdsSelonRegion(rRegion region)
        {
            return this._statistiquesRepository.listeDesControleursVehiculesLourdsSelonRegion(region);
        }

        public IList<ItemListeDesControleurs> listeDesControleursVehiculesLegersSelonRegion(rRegion region)
        {
            return this._statistiquesRepository.listeDesControleursVehiculesLegersSelonRegion(region);
        }

        public IList<ItemNombreDeControleurs> NombreDeControleursVehiculesLegersEtLourdsSelonRegion(rRegion region, rPrefecture prefecture)
        {
            return this._statistiquesRepository.nombreDeControleursVehiculesLegersEtLourdsSelonRegion(region, prefecture);
        }

        public IList<ItemListeDesControleursRattachesAuxCentresDuReseau> listeDesControleursRattachesAuxCentresDuReseau(BusinessObjects.rUtilisateur utilisateur)
        {
            return this._statistiquesRepository.listeDesControleursRattachesAuxCentresDuReseau(utilisateur);
        }

        //public IList<ItemListeDesInstallationsVehiculesLegersSelonRegion> listeDesInstallationsVehiculesLegersSelonRegion(BusinessObjects.rRegion region)
        //{
        //    return this._statistiquesRepository.listeDesInstallationsVehiculesLegersSelonRegion(region);
        //}

        //public IList<ItemListeDesInstallations> ListeDesInstallationsVehiculesLourdsSelonDepartement(BusinessObjects.rPrefecture prefecture)
        //{
        //    return this._statistiquesRepository.ListeDesInstallationsVehiculesLourdsSelonDepartement(prefecture);
        //}

        //public IList<ItemListeDesInstallations> ListeDesInstallationsVehiculesLegersSelonDepartement(BusinessObjects.rPrefecture prefecture)
        //{
        //    return this._statistiquesRepository.ListeDesInstallationsVehiculesLegersSelonDepartement(prefecture);
        //}

        public IList<ItemListeDesControleursVLCentreRattachementSelonDepartement> listeDesControleursVLCentreRattachementSelonDepartement(rPrefecture prefecture)
        {
            return this._statistiquesRepository.listeDesControleursVLCentreRattachementSelonDepartement(prefecture);
        }

        public IList<ItemListeDesControleursPLCentreRattachementSelonDepartement> listeDesControleursPLCentreRattachementSelonDepartement(rPrefecture prefecture)
        {
            return this._statistiquesRepository.listeDesControleursPLCentreRattachementSelonDepartement(prefecture);
        }

        public IList<ItemListeDesControleursPLCentreRattachementSelonDepartement> listeDesControleursPLCentreRattachementSelonRegionDepartement(rRegion region, rPrefecture prefecture)
        {
            return this._statistiquesRepository.listeDesControleursPLCentreRattachementSelonRegionDepartement(region, prefecture);
        }

        public IList<ItemListeSuiviQualificationPL> ListeDesSuivisQualificationControleursPLSelonRegion(BusinessObjects.rRegion region)
        {
            IList<ItemListeSuiviQualificationPL> listeQualif = new List<ItemListeSuiviQualificationPL>();
            ItemListeSuiviQualificationPLQuery query = new ItemListeSuiviQualificationPLQuery();
            query.CumulActiviteTraitement = this._cumulActiviteTraitementService.FindFirstMax();

            IList<pContoleurEncour> listeControleurs = this._controleurService.FindAllForListeDesSuivisQualificationControleursPLSelonRegion(region);

            if (listeControleurs != null)
            {
                foreach (var controleur in listeControleurs)
                {
                    query.Controleur = controleur;
                    ItemListeSuiviQualificationPL item = this._statistiquesRepository.FindItemListeSuiviQualificationPL(query);
                    if (item != null)
                        listeQualif.Add(item);
                }
            }

            return listeQualif;

        }

        //YM 23/05/2013 fiche 86
        public IList<ItemListeSuiviQualificationVL> ListeDesSuivisQualificationControleursVLSelonRegion(BusinessObjects.rRegion region)
        {
            IList<ItemListeSuiviQualificationVL> listeQualif = new List<ItemListeSuiviQualificationVL>();
            ItemListeSuiviQualificationVLQuery query = new ItemListeSuiviQualificationVLQuery();
            query.CumulActiviteTraitement = this._cumulActiviteTraitementService.FindFirstMax();

            IList<pContoleurEncour> listeControleurs = this._controleurService.FindAllForListeDesSuivisQualificationControleursVLSelonRegion(region);

            if (listeControleurs != null)
            {
                foreach (var controleur in listeControleurs)
                {
                    query.Controleur = controleur;
                    ItemListeSuiviQualificationVL item = this._statistiquesRepository.FindItemListeSuiviQualificationVL(query);
                    if (item != null)
                        listeQualif.Add(item);
                }
            }

            return listeQualif;

        }

        public IList<ItemListeSuiviQualificationPL> ListeDesSuivisQualificationControleursPLSelonReseaux(IList<BusinessObjects.rReseau> listeReseaux)
        {
            IList<ItemListeSuiviQualificationPL> listeQualif = new List<ItemListeSuiviQualificationPL>();
            ItemListeSuiviQualificationPLQuery query = new ItemListeSuiviQualificationPLQuery();
            query.CumulActiviteTraitement = this._cumulActiviteTraitementService.FindFirstMax();

            IList<pContoleurEncour> listeControleurs = this._controleurService.FindAllForListeDesSuivisQualificationControleursPLSelonReseaux(listeReseaux);

            if (listeControleurs != null)
            {
                foreach (var controleur in listeControleurs)
                {
                    query.Controleur = controleur;
                    ItemListeSuiviQualificationPL item = this._statistiquesRepository.FindItemListeSuiviQualificationPL(query);
                    if (item != null)
                        listeQualif.Add(item);
                }
            }

            return listeQualif;
        }

        //08/09/2012 PLE : Récupération du suivi de qualification UNITAIRE pour fiche de suivi de qualification controleur PL
        //27/08/2018 -PLE- Suivi qualification contrôleur PL  : ajout année n-1
        public ItemListeSuiviQualificationPL SuivisQualificationControleurs(pContoleurEncour controleur, ref DateTime? dateDebutPeriode, ref DateTime? dateFinPeriode, ref int anneeNMoins1)
        {
            ItemListeSuiviQualificationPL qualif = new ItemListeSuiviQualificationPL();
            ItemListeSuiviQualificationPLQuery query = new ItemListeSuiviQualificationPLQuery();
            query.CumulActiviteTraitement = this._cumulActiviteTraitementService.FindFirstMax();

            query.Controleur = controleur;
            qualif = this._statistiquesRepository.FindItemListeSuiviQualificationPL(query);
            dateDebutPeriode = query.DateMoisDebut;
            dateFinPeriode = query.DateMoisFin;
            anneeNMoins1 = query.annee_n_moins1;

            return qualif;
        }

        //24/05/2013 YM : Récupération du suivi de qualification UNITAIRE pour fiche de suivi de qualification controleur VL
        public ItemListeSuiviQualificationVL SuivisQualificationControleursVL(pContoleurEncour controleur, pActiviteControleurVL activite)
        {
            ItemListeSuiviQualificationVL qualif = new ItemListeSuiviQualificationVL();
            ItemListeSuiviQualificationVLQuery query = new ItemListeSuiviQualificationVLQuery();
            //query.CumulActiviteTraitement = this._cumulActiviteTraitementService.FindFirstMax();

            query.Controleur = controleur;
            qualif = this._statistiquesRepository.FindItemListeSuiviQualificationVL(query);

            return qualif;
        }

        //public IList<ItemListeDesInstallations> ListeDesInstallationsVehiculesLourdsSelonRegionDepartement(rRegion region , rPrefecture prefecture)
        //{
        //    return this._statistiquesRepository.ListeDesInstallationsVehiculesLourdsSelonRegionDepartement(region, prefecture);
        //}

        public IList<ItemListeDesControleursVLCentreRattachementSelonDepartement> listeDesControleursVLCentreRattachementSelonRegion(rRegion region, rPrefecture prefecture)
        {
            return this._statistiquesRepository.listeDesControleursVLCentreRattachementSelonRegion(region, prefecture);
        }

        //YMA 15/10/2013 Liste des activités des controleurs
        public IList<ItemListeSuiviActiviteVL> ListeDesSuivisActiviteControleursVLSelonRegion(rRegion region)
        {
            return this._statistiquesRepository.ListeDesSuivisActiviteControleursVLSelonRegion(region);
        }

        //YMA 16/10/2013 Liste des activités des controleurs par réseau
        public IList<ItemListeSuiviActiviteVL> ListeDesSuivisActiviteControleursVLRattachesAuxCentresDuReseau(rUtilisateur utilisateur)
        {
            return this._statistiquesRepository.ListeDesSuivisActiviteControleursVLRattachesAuxCentresDuReseau(utilisateur);
        }
    }
}
