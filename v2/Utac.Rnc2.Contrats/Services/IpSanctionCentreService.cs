using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Contrats.Services

{
    public partial interface IpSanctionCentreService : IService<pSanctionCentre>
    {
        pSanctionCentre nouvelleSanction(pCentreEncour centre, pSanctionCentre sanction, string codeUtilisateur);

        pSanctionCentre modifierSanction(pSanctionCentre sanction, string codeUtilisateur);

        void deleteSanction(pSanctionCentre sanction);
    }
}
