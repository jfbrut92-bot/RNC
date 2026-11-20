using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Contrats.Services
{
    public interface IpGestionCertificatService : IService<pGestionCertificat>
    {
        pCertificat findLastValide(int id, rCategorieVehicule.CategorieEnum categorieVehicule);
        pCertificat findLastValide(pCentreEncour centre, rCategorieVehicule.CategorieEnum categorieVehicule);
        pCertificat findLastValideOuNonValide(int id, rCategorieVehicule.CategorieEnum categorieVehicule);
        pCertificat findLastValideOuNonValide(pCentreEncour centre, rCategorieVehicule.CategorieEnum categorieVehicule);
        //EG le 22/11/2011
        pCertificat findLastValide(rReseau reseau);
        pCertificat findLastValideOuNonValide(rReseau reseau);

    }
}

