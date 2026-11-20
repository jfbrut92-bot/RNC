using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Contrats.Services
{
    public interface IpCertificatService : IService<pCertificat>
    {
        pCertificat findByIdAndNumeroSerie(int idReq, string numeroSerie);
        void nouveauCertificat(pCertificat certificat, pCentreEncour centre, rCategorieVehicule.CategorieEnum categorieVehicule, string codeUtilisateur, bool estInitial);
        //EG le 22/11/2011
        void nouveauCertificat(pCertificat certificat, rReseau reseau, string codeUtilisateur, bool estInitial);
        void traceTelechargementCertificat(pCertificat certificat, rReseau reseau, string codeUtilisateur);
        void traceTelechargementCertificat(pCertificat certificat, pCentreEncour centre, rCategorieVehicule.CategorieEnum categorieVehicule, string codeUtilisateur);
        //EG le 02/12/2011
        void revocationCertificat(pCertificat certificat, pCentreEncour centre, rCategorieVehicule.CategorieEnum categorieVehicule, bool autorisationRenew, string codeUtilisateur);
        void autorisationCertificat(pCertificat certificat, pCentreEncour centre, rCategorieVehicule.CategorieEnum categorieVehicule, string codeUtilisateur);
        void revocationCertificat(pCertificat certificat, rReseau reseau, bool autorisationRenew, string codeUtilisateur);
        void autorisationCertificat(pCertificat certificat, rReseau reseau, string codeUtilisateur);

    }
}
