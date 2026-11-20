using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Contrats.Services

{
    public partial interface IpEquipementCentrePLService : IService<pEquipementCentrePL>
    {
        void nouveauEquipementPL(pCentreEncour centre, pEquipementCentrePL equipement, string codeUtilisateur);
        void modifierEquipementPL(pCentreEncour centre, pEquipementCentrePL equipement, string codeUtilisateur);
    }
}
