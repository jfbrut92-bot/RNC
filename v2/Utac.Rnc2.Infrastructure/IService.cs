using System;
using System.Collections.Generic;
using Utac.Rnc2.Infrastructure.Queries;

namespace Utac.Rnc2.Infrastructure
{

    public interface IService<T>
    {
        List<T> FindAll();
        T FindBy(Guid Id);
        T FindBy(object Id);
        List<T> FindBy(Query q);
        List<T> FindBy(Query q, String Alias);
        List<T> FindByPage(Query q, int page, int pageSize, out long count);
        void Save(T obj);
        void Update(T obj);
        void Remove(T obj);
        IUnitOfWork getUnitOfWork();
        void setUnitOfWorks(IUnitOfWork value);

    }

}
