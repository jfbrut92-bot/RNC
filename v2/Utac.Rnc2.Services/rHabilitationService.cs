using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Repositories;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Services
{
    public class rHabilitationService : BaseService<rHabilitation>, IrHabilitationService
    {

        public rHabilitationService(IrHabilitationRepository repository, IUnitOfWork u)
            : base(repository, u)
        {

        }

    }
}
