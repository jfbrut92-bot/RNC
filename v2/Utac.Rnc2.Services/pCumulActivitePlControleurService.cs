using System.Collections.Generic;
using System.Linq;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Infrastructure;
using Utac.Rnc2.Infrastructure.Queries;
using Utac.Rnc2.Repository;

namespace Utac.Rnc2.Services
{
    public partial class pCumulActivitePlJrnTrtService : BaseService<pCumulActivitePlJrnTrt>, IpCumulActivitePlJrnTrtService
    {

        public pCumulActivitePlJrnTrtService(pCumulActivitePlJrnTrtRepository repository, IUnitOfWork UnitOfWork)
            : base(repository, UnitOfWork)
        {

        }

        public pCumulActivitePlJrnTrt FindFirstMax()
        {
            Query q = new Query();
            q.Criteria.Add(new Criteria("DateDebutTraitement", null, CriteriaOperator.IS_NOT_NULL));
            q.Criteria.Add(new Criteria("DateFinTraitement", null, CriteriaOperator.IS_NOT_NULL));
            q.OrderBy.Add(new OrderBy() { PropertyName = "Id", Ascending = false });

            IList<pCumulActivitePlJrnTrt> liste = this.FindBy(q);
            if (liste != null && liste.Count > 0)
            {
                return liste.ElementAt(0);
            }

            return null;

        }
    }

}
