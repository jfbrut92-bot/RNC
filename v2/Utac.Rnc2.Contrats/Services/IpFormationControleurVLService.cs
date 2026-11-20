using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Contrats.Services

{
    public partial interface IpFormationControleurVLService : IService<pFormationControleurVL>
    {

        pFormationControleurVL nouvelFormation(pContoleurEncour controleur, pFormationControleurVL formation, string codeUtilisateur);

        pFormationControleurVL modifierFormation(int id, pFormationControleurVL formation, string codeUtilisateur);

        void deleteFormation(pFormationControleurVL audit);
    }
}
