using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Contrats.Services

{
    public partial interface IpFormationControleurCLService : IService<pFormationControleurCL>
    {

        pFormationControleurCL nouvelFormation(pContoleurEncour controleur, pFormationControleurCL formation, string codeUtilisateur);

        pFormationControleurCL modifierFormation(int id, pFormationControleurCL formation, string codeUtilisateur);

        void deleteFormation(pFormationControleurCL audit);
    }
}
