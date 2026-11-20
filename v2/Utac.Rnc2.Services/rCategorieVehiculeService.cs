using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Repositories;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Services
{
    public class rCategorieVehiculeService : BaseService<rCategorieVehicule>, IrCategorieVehiculeService
    {

        public rCategorieVehiculeService(IrCategorieVehiculeRepository repository, IUnitOfWork UnitOfWork) : base(repository, UnitOfWork)
        {

        }

    }
}
