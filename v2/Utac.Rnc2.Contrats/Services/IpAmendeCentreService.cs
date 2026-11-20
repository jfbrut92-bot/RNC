using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Contrats.Services

{
    public partial interface IpAmendeCentreService : IService<pAmendeCentre>
    {
        pAmendeCentre nouvelleAmende(pCentreEncour centre, pAmendeCentre amende, string codeUtilisateur);
        pAmendeCentre modifierAmende(pAmendeCentre amende, string codeUtilisateur);
        void deleteAmende(pAmendeCentre amende);
    }
}
