using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Contrats.Services
{
    public interface IpHoraireOuvertureService : IService<pHoraireOuverture>
    {

        void modifierHoraire(pCentreEncour model, pHoraireOuverture pHoraireOuverture, string codeUtilisateur);
        void nouvelleHoraire(pCentreEncour model, pHoraireOuverture pHoraireOuverture, string codeUtilisateur);
    }
}
