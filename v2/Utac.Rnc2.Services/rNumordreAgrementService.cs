using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Repositories;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Services
{
    public class rNumordreAgrementService : BaseService<rNumordreAgrement>, IrNumordreAgrementService
    {

        public rNumordreAgrementService(IrNumordreAgrementRepository repository, IUnitOfWork UnitOfWork)
            : base(repository, UnitOfWork)
        {
        }

        public rNumordreAgrementService(IrNumordreAgrementRepository repository, IUnitOfWork UnitOfWork, IpControleurHistoService histoService)
            : base(repository, UnitOfWork)
        {

        }

    }
}
