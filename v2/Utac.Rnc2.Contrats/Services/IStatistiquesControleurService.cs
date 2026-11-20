using System;
using System.Collections.Generic;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.BusinessObjects.statistiques;

namespace Utac.Rnc2.Contrats.Services
{
    public interface IStatistiquesControleurService
    {
        List<NombreControleursParRegionEtDepartement> getNombreControleurParRegionEtDepartement(rRegion region);
        IList<ItemListeDesControleursVéhiculesLegersEtLourds> listeDesControleursVehiculesLegersEtLourdsSelonRegion(rRegion region);
        IList<ItemListeDesControleurs> listeDesControleursVehiculesLourdsSelonRegion(rRegion region);
        IList<ItemListeDesControleurs> listeDesControleursVehiculesLegersSelonRegion(rRegion region);
        IList<ItemListeDesControleursRattachesAuxCentresDuReseau> listeDesControleursRattachesAuxCentresDuReseau(rUtilisateur utilisateur);
        IList<ItemListeDesControleursVLCentreRattachementSelonDepartement> listeDesControleursVLCentreRattachementSelonDepartement(rPrefecture prefecture);
        IList<ItemListeDesControleursVLCentreRattachementSelonDepartement> listeDesControleursVLCentreRattachementSelonRegion(rRegion region, rPrefecture prefecture);
        IList<ItemListeSuiviQualificationPL> ListeDesSuivisQualificationControleursPLSelonRegion(rRegion region);
        //YM 23/05/2013
        IList<ItemListeSuiviQualificationVL> ListeDesSuivisQualificationControleursVLSelonRegion(rRegion region);
        IList<ItemListeSuiviQualificationPL> ListeDesSuivisQualificationControleursPLSelonReseaux(IList<rReseau> listeReseaux);
        IList<ItemListeDesControleursPLCentreRattachementSelonDepartement> listeDesControleursPLCentreRattachementSelonDepartement(rPrefecture prefecture);
        ItemListeSuiviQualificationPL SuivisQualificationControleurs(pContoleurEncour controleur, ref DateTime? dateDebutPeriode, ref DateTime? dateFinPeriode, ref int anneeNMoins1);
        //YM 24/05/2013
        ItemListeSuiviQualificationVL SuivisQualificationControleursVL(pContoleurEncour controleur, pActiviteControleurVL activite);
        IList<ItemListeDesControleursPLCentreRattachementSelonDepartement> listeDesControleursPLCentreRattachementSelonRegionDepartement(rRegion region, rPrefecture prefecture);
        IList<ItemNombreDeControleurs> NombreDeControleursVehiculesLegersEtLourdsSelonRegion(rRegion region, rPrefecture prefecture);
        //IList<ItemListeDesControleursPLCentreRattachementSelonDepartement> listeDesControleursPLCentreRattachementSelonRegionDepartement(rRegion region, rPrefecture prefecture);
        //YM 15/10/2013
        IList<ItemListeSuiviActiviteVL> ListeDesSuivisActiviteControleursVLSelonRegion(rRegion region);
        IList<ItemListeSuiviActiviteVL> ListeDesSuivisActiviteControleursVLRattachesAuxCentresDuReseau(rUtilisateur utilisateur);

    }
}

