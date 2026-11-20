using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Contrats.Services

{
    public partial interface IpFormationControleurPLService : IService<pFormationControleurPL>
    {

        pFormationControleurPL nouvelFormation(pContoleurEncour controleur, pFormationControleurPL formation, string codeUtilisateur);

        pFormationControleurPL modifierFormation(int id, pFormationControleurPL formation, string codeUtilisateur);

        void deleteFormation(pFormationControleurPL audit);
    }
}
