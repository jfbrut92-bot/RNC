using Utac.Rnc2.BusinessObjects;
using System.Collections.Generic;
using Utac.Rnc2.Contrats.Repositories;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Infrastructure;
using Utac.Rnc2.Infrastructure.Queries;

namespace Utac.Rnc2.Services
{
    public class rTypeFormationService : BaseService<rTypeFormation>, IrTypeFormationService
    {

        public rTypeFormationService(IrTypeFormationRepository repository, IUnitOfWork UnitOfWork)
            : base(repository, UnitOfWork)
        {
        }

        public List<rTypeFormation> FindSelonCodCategorieVeh(rCategorieVehicule.CategorieEnum categorie)
        {
            Query q = new Query();
            q.Criteria.Add(new Criteria("this.CodCategorieVeh", categorie.ToString(), CriteriaOperator.EQUALS));
            return this.FindBy(q);
        }

    }
}
