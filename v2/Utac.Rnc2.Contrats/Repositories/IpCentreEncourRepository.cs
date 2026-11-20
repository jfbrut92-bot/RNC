using System.Collections.Generic;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.BusinessObjects.certificats;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Contrats.Repositories
{
    public interface IpCentreEncourRepository : IRepository<pCentreEncour>
    {
        IList<pCentreEncour> findAllPL();
        IList<pCentreEncour> findAllVL();
        //EG le 06/01/2012 : requête trop complexe
        IList<ItemListeDesInfosCertificatsExpireOuRevoque> findAllAgreeAvecCertificatExpireOuRevoque(rCategorieVehicule.CategorieEnum categorie);
        //YM 26/01/2012
        IList<pCentreEncour> FindAllCentreNouvellementAgreeRegion(rRegion region, string datedeb);
        //YM 27/01/2012
        IList<pCentreEncour> FindAllCentreAgreeSansControleurRegion(rRegion region);
    }
}
