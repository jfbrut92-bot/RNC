using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Repositories;
using Utac.Rnc2.Repository.Nhibernate;

namespace Utac.Rnc2.Repository
{
    public class rRegionRepository : BaseRepository<rRegion>, IrRegionRepository
    {
        public override System.Collections.Generic.IList<rRegion> FindAll()
        {
            NHibernate.ICriteria CriteriaQuery = SessionFactory.GetCurrentSession().CreateCriteria(typeof(rRegion));
            NHibernate.Criterion.Order order = new NHibernate.Criterion.Order("LibelleRegion", true);
            CriteriaQuery.AddOrder(order);

            return CriteriaQuery.List<rRegion>();
        }
    }
}
