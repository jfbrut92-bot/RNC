using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Contrats.Repositories
{
    public interface IpCentreHistoRepository : IRepository<pCentreHisto>
    {
        pCentreHisto findLastBy(object id);
        void removeAll(pCentreEncour centre);
    }
}