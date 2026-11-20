using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Contrats.Services

{
    public partial interface IpEquipementCentreCLService : IService<pEquipementCentreCL>
    {
        void nouveauEquipementCL(pCentreEncour centre, pEquipementCentreCL equipement, string codeUtilisateur);
        void modifierEquipementCL(pCentreEncour centre, pEquipementCentreCL equipement, string codeUtilisateur);

    }
}
