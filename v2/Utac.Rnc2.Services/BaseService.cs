using System;
using System.Collections.Generic;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Services
{
    public class BaseService<T> : IService<T> where T : IAggregateRoot
    {
        protected IRepository<T> _Repository;
        private IUnitOfWork _UnitOfWork;

        public IUnitOfWork getUnitOfWork()
        {
            return _UnitOfWork;
        }

        public void setUnitOfWorks(IUnitOfWork value)
        {
            _UnitOfWork = value;
            this._Repository.Inject(value);
        }

        public BaseService(IRepository<T> repository, IUnitOfWork UnitOfWork)
        {
            this._Repository = repository;
            this._Repository.Inject(UnitOfWork);
            this._UnitOfWork = UnitOfWork;
        }

        public BaseService()
        {

        }

        public virtual List<T> FindAll()
        {
            return (List<T>)_Repository.FindAll();
        }

        public virtual T FindBy(Guid Id)
        {
            throw new NotImplementedException();
        }

        public virtual T FindBy(object Id)
        {
            //IUnitOfWork transaction = this.getUnitOfWork();
            //if (transaction != null) transaction.BeginTran();
            T o = (T)_Repository.FindBy(Id);
            //if (transaction != null) transaction.CommitTran();
            return o;

        }

        public virtual void Save(T obj)
        {
            this._Repository.Save(obj);

        }

        public virtual void Update(T obj)
        {
            this._Repository.Update(obj);

        }

        public virtual void Remove(T obj)
        {
            this._Repository.Remove(obj);
        }

        public virtual List<T> FindBy(Utac.Rnc2.Infrastructure.Queries.Query q)
        {
            return (List<T>)this._Repository.FindBy(q);
        }

        public virtual List<T> FindBy(Utac.Rnc2.Infrastructure.Queries.Query q, String alias)
        {
            return (List<T>)this._Repository.FindBy(q, alias);
        }


        public List<T> FindByPage(Infrastructure.Queries.Query q, int page, int pageSize, out long count)
        {
            return (List<T>)this._Repository.FindByPage(q, page, pageSize, out count);
        }
    }
}
