using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Contrats.Services
{
    public interface IrReseauAppartenanceService : IService<rReseauAppartenance>
    {
        void supprimeAppartenance(rUtilisateur utilisateur, rReseau reseau);
        void supprimeAppartenance(string codeUtilisateur, string codeReseau);
        void ajouteAppartenance(string codeUtilisateur, string codeReseau);
    }
}
