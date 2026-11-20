using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Contrats.Repositories
{
    public interface IpControleurHistoRepository : IRepository<pControleurHisto>
    {
        void removeAll(pContoleurEncour controleur);
    }
}