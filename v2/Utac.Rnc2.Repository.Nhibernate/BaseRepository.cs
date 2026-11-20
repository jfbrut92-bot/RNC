using System;
using System.Collections.Generic;
using log4net;
using NHibernate;
using Utac.Rnc2.Infrastructure;
using Utac.Rnc2.Infrastructure.CustomExceptions;
using Utac.Rnc2.Infrastructure.Queries;

namespace Utac.Rnc2.Repository.Nhibernate
{
    public class BaseRepository<T> : IRepository<T> where T : IAggregateRoot
    {

        private readonly ILog log = LogManager.GetLogger(typeof(BaseRepository<T>));

        protected IUnitOfWork _UnitOfWork;

        public void Inject(IUnitOfWork UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;
        }

        public virtual System.Collections.Generic.IList<T> FindAll()
        {

            NHibernate.ICriteria CriteriaQuery = SessionFactory.GetCurrentSession().CreateCriteria(typeof(T));

            return CriteriaQuery.List<T>();
        }

        public System.Collections.Generic.IList<T> FindBy(Query Query)
        {

            NHibernate.ICriteria criteria = SessionFactory.GetCurrentSession().CreateCriteria(typeof(T));
          
            return QueryTranslator.GetCriteriaFrom(criteria, Query).List<T>();
        }

        public System.Collections.Generic.IList<T> FindBy(Query Query, string alias)
        {

            NHibernate.ICriteria criteria = SessionFactory.GetCurrentSession().CreateCriteria(typeof(T));
            //03/09/2021 PLE - Suppression createAlias + Ajout Fetch + SetResultTransformer
            //criteria.CreateAlias(alias, "aliasNom", NHibernate.SqlCommand.JoinType.LeftOuterJoin);
            criteria.Fetch(SelectMode.Fetch, alias);
            criteria.SetResultTransformer(new NHibernate.Transform.DistinctRootEntityResultTransformer());
         
            return QueryTranslator.GetCriteriaFrom(criteria, Query).List<T>();
        }

        //public T FindBy(Guid Id) 
        public T FindBy(object Id)
        {

            // Load doesn't return null if no entity found 
            //Return NHSession.Load(Of T)(Id) 
            if (Id != null)
            {
                //EG le 18/02/2012 : pour tracer le problème de cache Nhibernate (mauvais businnes object retourné malgré l'id passé)
                //PLE le 15/07/2026 :Suppression des logs
                //log.Debug("FindBy ID demandé = " + Id);
                T businness = SessionFactory.GetCurrentSession().Get<T>(Id);
                //log.Debug("FindBy ID objet retourné = " + (businness != null ? Convert.ToString(businness.getId()) : "null object"));
                if (businness != null)
                {
                    if (!Id.Equals(businness.getId()))
                    {
                        log.Warn(string.Format("FindBy les ID sont différents ID0 = {0} , ID1 = {1}", Id, businness.getId()));
                    }
                }
                return businness;
            }
            return default(T);

        }

        public void Remove(T Entity)
        {
            if (_UnitOfWork == null)
            {

                using (ITransaction Transaction = SessionFactory.GetCurrentSession().BeginTransaction())
                {

                    SessionFactory.GetCurrentSession().Delete(Entity);
                    try
                    {
                        Transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        Transaction.Rollback();
                        // TODO: Log exception 
                        throw ex;
                    }

                }
            }
            else
            {
                _UnitOfWork.Delete(Entity);
            }
        }

        public void Save(T Entity)
        {
            if (_UnitOfWork == null)
            {

                using (ITransaction Transaction = SessionFactory.GetCurrentSession().BeginTransaction())
                {
                    SessionFactory.GetCurrentSession().SaveOrUpdate(Entity);

                    try
                    {
                        Transaction.Commit();
                    }
                    catch (NHibernate.StaleObjectStateException ex)
                    {
                        // Catch StaleObjectStateException: 
                        // Entity was changed while we were viewing it, 
                        // Wrap Nhibernate exception and then send it 
                        // back up the calling stack 
                        throw new ConcurrencyException(string.Format("Please reload the {0} as changes have been made to it since you have loaded it.", Entity.ToString()), ex);
                    }
                    catch (Exception ex)
                    {
                        Transaction.Rollback();
                        // TODO: Log exception
                        throw ex;

                    }

                }
            }
            else
            {
                _UnitOfWork.Save(Entity);
            }
        }

        public void Update(T Entity)
        {
            if (_UnitOfWork == null)
            {

                using (ITransaction Transaction = SessionFactory.GetCurrentSession().BeginTransaction())
                {
                    SessionFactory.GetCurrentSession().Update(Entity);

                    try
                    {
                        Transaction.Commit();
                    }
                    catch (NHibernate.StaleObjectStateException ex)
                    {
                        // Catch StaleObjectStateException: 
                        // Entity was changed while we were viewing it, 
                        // Wrap Nhibernate exception and then send it 
                        // back up the calling stack 
                        throw new ConcurrencyException(string.Format("Please reload the {0} as changes have been made to it since you have loaded it.", Entity.ToString()), ex);
                    }
                    catch (Exception ex)
                    {
                        Transaction.Rollback();
                        // TODO: Log exception
                        throw ex;

                    }

                }
            }
            else
            {
                _UnitOfWork.Save(Entity);
            }
        }

        public System.Collections.Generic.IList<T> FindByPage(Query q, int page, int pageSize, out long count)
        {
            //Récupération de la liste en fonction de la pagination
            NHibernate.ICriteria criteria = SessionFactory.GetCurrentSession().CreateCriteria(typeof(T));
            criteria = QueryTranslator.GetCriteriaFrom(criteria, q);
            IList<T> liste = criteria.SetFirstResult((page - 1) * pageSize).SetMaxResults(pageSize).List<T>();

            //Récupération count
            criteria = SessionFactory.GetCurrentSession().CreateCriteria(typeof(T));
            criteria = QueryTranslator.GetCriteriaFrom(criteria, q);

            System.Collections.IList tmp = criteria.SetProjection(new NHibernate.Criterion.RowCountInt64Projection()).List();
            count = (long)tmp[0];

            return liste;
        }
    }

}
