using System.Collections.Generic;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.BusinessObjects.statistiques;
using Utac.Rnc2.Contrats.Repositories.statistiques;
using Utac.Rnc2.Contrats.Services;

namespace Utac.Rnc2.Services
{
    public class StatistiquesCentreService : IStatistiquesCentreService
    {
        private readonly IStatistiquesCentreRepository _statistiquesRepository;
        //IpCumulActivitePlJrnTrtService _cumulActiviteTraitementService;
        //IpControleurEnCourService _controleurService;

        public StatistiquesCentreService(IStatistiquesCentreRepository repository
            //, IpCumulActivitePlJrnTrtService cumulActiviteTraitementService, IpControleurEnCourService controleurService
            )
        {
            this._statistiquesRepository = repository;
            //this._cumulActiviteTraitementService = cumulActiviteTraitementService;
            //this._controleurService = controleurService;
        }

        //public List<NombreControleursParRegionEtDepartement> getNombreControleurParRegionEtDepartement(rRegion region)
        //{
        //    return this._statistiquesRepository.getNombreControleurParRegionEtDepartement(region);
        //}

        public IList<ItemListeDesInstallationsVéhiculesLegersELourdsSelonRegion> listeDesInstallationsVéhiculesLegersELourdsSelonRegion(BusinessObjects.rRegion region, rUtilisateur utilisateur)
        {
            return this._statistiquesRepository.listeDesInstallationsVéhiculesLegersELourdsSelonRegion(region, utilisateur);
        }

        //public IList<ItemListeDesControleursVéhiculesLegersEtLourdsSelonRegion> listeDesControleursVehiculesLegersEtLourdsSelonRegion(BusinessObjects.rRegion region)
        //{
        //    return this._statistiquesRepository.listeDesControleursVehiculesLegersEtLourdsSelonRegion(region);
        //}

        //public IList<ItemListeDesControleursRattachesAuxCentresDuReseau> listeDesControleursRattachesAuxCentresDuReseau(BusinessObjects.rUtilisateur utilisateur)
        //{
        //    return this._statistiquesRepository.listeDesControleursRattachesAuxCentresDuReseau(utilisateur);
        //}

        public IList<ItemListeDesInstallationsVehiculesLegersSelonRegion> listeDesInstallationsVehiculesLegersSelonRegion(BusinessObjects.rRegion region, BusinessObjects.rPrefecture prefecture)
        {
            return this._statistiquesRepository.listeDesInstallationsVehiculesLegersSelonRegion(region, prefecture);
        }

        public IList<ItemListeDesInstallations> ListeDesInstallationsVehiculesLourdsSelonDepartement(BusinessObjects.rPrefecture prefecture)
        {
            return this._statistiquesRepository.ListeDesInstallationsVehiculesLourdsSelonDepartement(prefecture);
        }

        public IList<ItemListeDesInstallations> ListeDesInstallationsVehiculesLegersSelonDepartement(BusinessObjects.rPrefecture prefecture)
        {
            return this._statistiquesRepository.ListeDesInstallationsVehiculesLegersSelonDepartement(prefecture);
        }

        //public IList<ItemListeDesControleursVLCentreRattachementSelonDepartement> listeDesControleursVLCentreRattachementSelonDepartement(BusinessObjects.rPrefecture prefecture)
        //{
        //    return this._statistiquesRepository.listeDesControleursVLCentreRattachementSelonDepartement(prefecture);
        //}

        //public IList<ItemListeDesControleursPLCentreRattachementSelonDepartement> listeDesControleursPLCentreRattachementSelonDepartement(rPrefecture prefecture)
        //{
        //    return this._statistiquesRepository.listeDesControleursPLCentreRattachementSelonDepartement(prefecture);
        //}

        //public IList<ItemListeSuiviQualificationPL> ListeDesSuivisQualificationControleursPLSelonRegion(BusinessObjects.rRegion region)
        //{
        //    IList<ItemListeSuiviQualificationPL> listeQualif = new List<ItemListeSuiviQualificationPL>();
        //    ItemListeSuiviQualificationPLQuery query = new ItemListeSuiviQualificationPLQuery();
        //    query.CumulActiviteTraitement = this._cumulActiviteTraitementService.FindFirstMax();

        //    IList<pContoleurEncour> listeControleurs = this._controleurService.FindAllForListeDesSuivisQualificationControleursPLSelonRegion(region);

        //    if ( listeControleurs != null ) 
        //    { 
        //        foreach (var controleur in listeControleurs)
        //        {
        //            query.Controleur = controleur;
        //            ItemListeSuiviQualificationPL item = this._statistiquesRepository.FindItemListeSuiviQualificationPL(query);
        //            if ( item != null)
        //                listeQualif.Add(item);
        //        }
        //    }

        //    return listeQualif;

        //}

        //public IList<ItemListeSuiviQualificationPL> ListeDesSuivisQualificationControleursPLSelonReseaux(IList<BusinessObjects.rReseau> listeReseaux)
        //{
        //    IList<ItemListeSuiviQualificationPL> listeQualif = new List<ItemListeSuiviQualificationPL>();
        //    ItemListeSuiviQualificationPLQuery query = new ItemListeSuiviQualificationPLQuery();
        //    query.CumulActiviteTraitement = this._cumulActiviteTraitementService.FindFirstMax();

        //    IList<pContoleurEncour> listeControleurs = this._controleurService.FindAllForListeDesSuivisQualificationControleursPLSelonReseaux(listeReseaux);

        //    if (listeControleurs != null)
        //    {
        //        foreach (var controleur in listeControleurs)
        //        {
        //            query.Controleur = controleur;
        //            ItemListeSuiviQualificationPL item = this._statistiquesRepository.FindItemListeSuiviQualificationPL(query);
        //            if (item != null)
        //                listeQualif.Add(item);
        //        }
        //    }

        //    return listeQualif;
        //}

        public IList<ItemListeDesInstallations> ListeDesInstallationsVehiculesLourdsSelonRegionDepartement(rRegion region, rPrefecture prefecture)
        {
            return this._statistiquesRepository.ListeDesInstallationsVehiculesLourdsSelonRegionDepartement(region, prefecture);
        }

        //public IList<ItemListeDesControleursVLCentreRattachementSelonRegion> listeDesControleursVLCentreRattachementSelonRegion(rRegion region, rPrefecture prefecture)
        //{
        //    return this._statistiquesRepository.listeDesControleursVLCentreRattachementSelonRegion(region, prefecture);
        //}

        public IList<ItemNombreInstallations> NombreInstallationsVehiculesLegersSelonRegion(rRegion region, rPrefecture prefecture)
        {
            return this._statistiquesRepository.NombreInstallationsVehiculesLegersSelonRegion(region, prefecture);
        }

        public IList<ItemNombreInstallations> NombreInstallationsVehiculesLourdsSelonRegion(rRegion region, rPrefecture prefecture)
        {
            return this._statistiquesRepository.NombreInstallationsVehiculesLourdsSelonRegion(region, prefecture);
        }
    }
}
