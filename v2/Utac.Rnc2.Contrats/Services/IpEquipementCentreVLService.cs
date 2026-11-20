using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Contrats.Services

{
    public partial interface IpEquipementCentreVLService : IService<pEquipementCentreVL>
    {
        void nouveauEquipementVL(pCentreEncour centre, pEquipementCentreVL equipement, string codeUtilisateur);
        void modifierEquipementVL(pCentreEncour centre, pEquipementCentreVL equipement, string codeUtilisateur);

    }
}
