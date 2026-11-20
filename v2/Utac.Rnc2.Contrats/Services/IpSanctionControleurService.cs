using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Contrats.Services

{
    public partial interface IpSanctionControleurService : IService<pSanctionControleur>
    {

        pSanctionControleur nouvelleSanction(pContoleurEncour centre, pSanctionControleur sanction, string codeUtilisateur);

        pSanctionControleur modifierSanction(pSanctionControleur sanction, string codeUtilisateur);

        void deleteSanction(pSanctionControleur sanction);
    }
}
