using System.Collections.Generic;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.BusinessObjects.statistiques;

namespace Utac.Rnc2.Contrats.Repositories.statistiques
{
    public interface IStatistiquesControleurRepository
    {
        List<NombreControleursParRegionEtDepartement> getNombreControleurParRegionEtDepartement(rRegion region);
        //IList<ItemListeDesInstallationsVéhiculesLegersELourdsSelonRegion> listeDesInstallationsVéhiculesLegersELourdsSelonRegion(rRegion region);
        IList<ItemListeDesControleursVéhiculesLegersEtLourds> listeDesControleursVehiculesLegersEtLourdsSelonRegion(rRegion region);
        IList<ItemListeDesControleursRattachesAuxCentresDuReseau> listeDesControleursRattachesAuxCentresDuReseau(rUtilisateur utilisateur);
        //IList<ItemListeDesInstallationsVehiculesLegersSelonRegion> listeDesInstallationsVehiculesLegersSelonRegion(rRegion region);
        //IList<ItemListeDesInstallations> ListeDesInstallationsVehiculesLourdsSelonDepartement(rPrefecture prefecture);
        //IList<ItemListeDesInstallations> ListeDesInstallationsVehiculesLegersSelonDepartement(rPrefecture prefecture);
        IList<ItemListeDesControleursVLCentreRattachementSelonDepartement> listeDesControleursVLCentreRattachementSelonDepartement(rPrefecture prefecture);
        IList<ItemListeDesControleursVLCentreRattachementSelonDepartement> listeDesControleursVLCentreRattachementSelonRegion(rRegion region, rPrefecture prefecture);
        ItemListeSuiviQualificationPL FindItemListeSuiviQualificationPL(ItemListeSuiviQualificationPLQuery query);
        //YM 23/05/2013
        ItemListeSuiviQualificationVL FindItemListeSuiviQualificationVL(ItemListeSuiviQualificationVLQuery query);
        //IList<ItemListeDesInstallations> ListeDesInstallationsVehiculesLourdsSelonRegionDepartement(rRegion region, rPrefecture prefecture);
        IList<ItemListeDesControleursPLCentreRattachementSelonDepartement> listeDesControleursPLCentreRattachementSelonDepartement(rPrefecture prefecture);
        IList<ItemListeDesControleursPLCentreRattachementSelonDepartement> listeDesControleursPLCentreRattachementSelonRegionDepartement(rRegion region, rPrefecture prefecture);

        IList<ItemListeDesControleurs> listeDesControleursVehiculesLourdsSelonRegion(rRegion region);
        IList<ItemListeDesControleurs> listeDesControleursVehiculesLegersSelonRegion(rRegion region);
        IList<ItemNombreDeControleurs> nombreDeControleursVehiculesLegersEtLourdsSelonRegion(rRegion region, rPrefecture prefecture);
        //YM 15/10/2013
        IList<ItemListeSuiviActiviteVL> ListeDesSuivisActiviteControleursVLSelonRegion(rRegion region);
        IList<ItemListeSuiviActiviteVL> ListeDesSuivisActiviteControleursVLRattachesAuxCentresDuReseau(rUtilisateur utilisateur);

        //todo
        //Nombre de contrôleurs véhicules Légers et Lourds par région/département
        //Nombre d'installations véhicules Légers par région/département
        //Nombre d'installations véhicules Lourds par région/département
        //Liste des contrôleurs Véhicules Légers et Lourds selon une région ou toutes les régions
        //Liste des installations Véhicules Légers et Lourds selon une région ou toutes les régions
        //Liste des suivis de qualification des contrôleurs PL selon une ou toutes les régions (!!nouveau!!)
        //Liste des installations véhicules Légers selon le département
        //Liste des installations véhicules Légers selon la région/département
        //Liste des contrôleurs véhicules Légers par centre de rattachement et selon le département
        //Liste des contrôleurs véhicules Légers par centre de rattachement et selon la région/département
        //Liste des contrôleurs véhicules Légers selon le département
        //Liste des contrôleurs véhicules Légers selon la région/département
        //Liste des installations Véhicules Lourds selon la région/département
        //Liste des contrôleurs véhicules Lourds par centre de rattachement et selon le département
        //Liste des contrôleurs véhicules Lourds par centre de rattachement et selon la région/département
        //Liste des contrôleurs véhicules Lourds selon le département
        //Liste des contrôleurs véhicules Lourds selon la région/département
        //Liste des suivis de qualification des contrôleurs PL selon une région (!!nouveau!!)

    }
}
