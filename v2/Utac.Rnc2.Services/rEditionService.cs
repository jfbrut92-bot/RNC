using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Infrastructure;
using Utac.Rnc2.Repository;
using System.Collections.Generic;
using Utac.Rnc2.Infrastructure.Queries;

namespace Utac.Rnc2.Services
{
    public class rEditionService : BaseService<rEdition>, IrEditionService
    {
        public rEditionService(rEditionRepository repository, IUnitOfWork UnitOfWork)
            : base(repository, UnitOfWork)
        {
        }

        public List<rEdition>  FindAllActif()
        {
            Query q = new Query();
            q.Criteria.Add(new Criteria("this.EstActif", true, CriteriaOperator.EQUALS));
            return this.FindBy(q);
        }
    }
}
