using System.Collections.Generic;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Contrats.Repositories
{
    public interface IpRapportExceptionVLRepository : IRepository<pRapportExceptionVl>
    {
        IList<pRapportExceptionVl> findListRapportVL(string q);
    }
}