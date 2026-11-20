using System.Collections.Generic;
using Utac.Rnc2.Infrastructure.Queries;

namespace Utac.Rnc2.Infrastructure
{
    public interface IRepository<T> where T : IAggregateRoot
    {
        void Save(T Entity);
        void Remove(T Entity);
        void Update(T Entity);
        T FindBy(object Id);
        IList<T> FindBy(Query q);
        IList<T> FindByPage(Query q, int page, int pageSize, out long count);
        IList<T> FindAll();
        void Inject(IUnitOfWork UnitOfWork);
        IList<T> FindBy(Query q, string alias);
    }
}
