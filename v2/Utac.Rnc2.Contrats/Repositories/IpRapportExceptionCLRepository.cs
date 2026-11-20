using System.Collections.Generic;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Contrats.Repositories
{
    public interface IpRapportExceptionCLRepository : IRepository<pRapportExceptionCl>
    {
        IList<pRapportExceptionCl> findListRapportCL(string q);
    }
}
