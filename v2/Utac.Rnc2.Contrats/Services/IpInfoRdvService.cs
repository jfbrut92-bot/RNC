using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Contrats.Services
{
    public interface IpInfoRdvService : IService<pInfoRdv>
    {
        void nouveauRdv(pCentreEncour c, pInfoRdv pInfoRdv, string codeUtilisateur);
        void modifierInfoRdv(pCentreEncour c, pInfoRdv pInfoRdv, string codeUtilisateur);

        void nouveauRdv(rReseau reseau, pInfoRdv pInfoRdv, string codeUtilisateur);
        void modifierInfoRdv(rReseau reseau, pInfoRdv pInfoRdv, string codeUtilisateur);
    }
}
