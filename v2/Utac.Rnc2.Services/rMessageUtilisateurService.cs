using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Repositories;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Services
{
    public class rMessageUtilisateurService : BaseService<rMessageUtilisateur>, IrMessageUtilisateurService
    {
        public rMessageUtilisateurService(IrMessageUtilisateurRepository repository, IUnitOfWork transaction) : base(repository, transaction)
        {
        }
    }
}
