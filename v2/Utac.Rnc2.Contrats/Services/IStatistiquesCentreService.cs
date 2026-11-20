using System.Collections.Generic;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.BusinessObjects.statistiques;

namespace Utac.Rnc2.Contrats.Services
{
    public interface IStatistiquesCentreService
    {
        IList<ItemListeDesInstallationsVéhiculesLegersELourdsSelonRegion> listeDesInstallationsVéhiculesLegersELourdsSelonRegion(rRegion region, rUtilisateur utilisateur);
        IList<ItemListeDesInstallationsVehiculesLegersSelonRegion> listeDesInstallationsVehiculesLegersSelonRegion(rRegion region, rPrefecture prefecture);
        IList<ItemListeDesInstallations> ListeDesInstallationsVehiculesLourdsSelonDepartement(rPrefecture prefecture);
        IList<ItemListeDesInstallations> ListeDesInstallationsVehiculesLegersSelonDepartement(rPrefecture prefecture);
        IList<ItemListeDesInstallations> ListeDesInstallationsVehiculesLourdsSelonRegionDepartement(rRegion region, rPrefecture prefecture);
        IList<ItemNombreInstallations> NombreInstallationsVehiculesLourdsSelonRegion(rRegion region, rPrefecture prefecture);
        IList<ItemNombreInstallations> NombreInstallationsVehiculesLegersSelonRegion(rRegion region, rPrefecture prefecture);
    }
}
