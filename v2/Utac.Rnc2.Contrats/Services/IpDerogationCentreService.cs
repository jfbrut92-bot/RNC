using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Contrats.Services

{
    public partial interface IpDerogationCentreService : IService<pDerogationCentre>
    {
        pDerogationCentre nouvelleDerogation(pCentreEncour centre, pDerogationCentre derogation, rCategorieVehicule.CategorieEnum categorie, string codeUtilisateur);

        pDerogationCentre modifierDerogation(pDerogationCentre derogation, string codeUtilisateur);

        void deleteDerogation(pDerogationCentre derogation, string codeUtilisateur);
    }
}
