using System.Collections.Generic;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Infrastructure;
using Utac.Rnc2.Infrastructure.Search;

namespace Utac.Rnc2.Contrats.Services
{
    public partial interface IpCentreEncourService : IService<pCentreEncour>
    {
        bool isAncienCentre(pCentreEncour centre);
        bool isAncienCentre(int centreId);
        List<pCentreEncour> FindByFiltreProfilReseau(FiltreRechercheProfilReseau form, out System.Boolean estDepartementErrone);
        //EG le 05/12/2011
        IList<pCentreEncour> findAllCentrePourAccuseReception(rReseau r);
        //EG le 06/12/2011
        bool accuseReception(pCentreEncour centre, rCategorieVehicule.CategorieEnum categorie, string codAgrementCentre, string numeroAccuseReception);
        //EG le 09/12/2011
        pCentreEncour nouvelAgrement(object id, pCentreEncour centre, rCategorieVehicule.CategorieEnum categorie, string codeUtilisateur);
        //EG le 07/01/2012
        pCentreEncour FindNewCentreFor(pCentreEncour centreOld);
        pCentreEncour FindNewCentreFor(int centreId);
        IList<pCentreEncour> FindAllDemandeEnCours(rPrefecture prefecture);
        IList<pCentreEncour> FindAllDemandeEnCours(rPrefecture prefecture, rCategorieVehicule.CategorieEnum categorie);
        IList<pCentreEncour> FindAllNouvellementAgree(rPrefecture rPrefecture);
        IList<pCentreEncour> FindAllNouvellementAgree(rPrefecture prefecture, rCategorieVehicule.CategorieEnum categorie);

        IList<pCentreEncour> FindCentreCLPourSiteWeb(rPrefecture prefecture);
    }
}
