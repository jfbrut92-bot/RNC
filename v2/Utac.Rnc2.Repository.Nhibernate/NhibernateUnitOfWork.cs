using System;
using NHibernate;
using Utac.Rnc2.Infrastructure;
using Utac.Rnc2.Infrastructure.CustomExceptions;

namespace Utac.Rnc2.Repository.Nhibernate
{
    public class NhibernateUnitOfWork : IUnitOfWork
    {
        // TODO ajouter une propriété pour gérer la transaction en cours pour un UnitOfWork partagé

        private readonly ISession _Session;
        private ITransaction _Transaction;

        public NhibernateUnitOfWork()
        {
            _Session = SessionFactory.GetCurrentSession();
        }

        //EG le 14/01/2011 ajouter pour le TDD
        public NhibernateUnitOfWork(ISession session)
        {
            _Session = session;
        }

        public void Save(IAggregateRoot Entity)
        {
            //TODO: Use CurrentSession, this will be from the HttpContext 
            _Session.SaveOrUpdate(Entity);
        }

        public void Update(IAggregateRoot Entity)
        {
            //TODO: Use CurrentSession, this will be from the HttpContext 
            _Session.Update(Entity);
        }

        public void SaveNew(IAggregateRoot Entity)
        {
            //TODO: Use CurrentSession, this will be from the HttpContext 
            _Session.Save(Entity);
        }

        public void Commit()
        {

            using (ITransaction transaction = _Session.BeginTransaction())
            {
                try
                {

                    // flush the session and commit the transaction 
                    transaction.Commit();
                }
                catch (StaleObjectStateException NHex)
                {

                    transaction.Rollback();
                    // TODO: Log exception 

                    // Catch StaleObjectStateException: 
                    // Entity was changed while we were viewing it, 
                    // Wrap Nhibernate exception and then send it 
                    // back up the calling stack 

                    throw new ConcurrencyException("Please reload the screen as changes have been made to an entity since first viewing.", NHex);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    // TODO: Log exception 
                    throw ex;

                }

            }

        }

        public void BeginTran()
        {

            if (_Transaction == null)
            {
                _Session.Flush();
                _Session.FlushMode = FlushMode.Commit;
                _Transaction = _Session.BeginTransaction();

            }
            else
            {
                throw new Exception("Une transaction est déjà ouverte");
            }

        }

        public void CommitTran()
        {

            if (_Transaction != null)
            {
                try
                {

                    // flush the session and commit the transaction 
                    _Session.Flush();
                    _Transaction.Commit();
                    //_Transaction.Dispose();
                    //_Transaction = null;
                }
                catch (StaleObjectStateException NHex)
                {

                    _Transaction.Rollback();
                    // TODO: Log exception 

                    // Catch StaleObjectStateException: 
                    // Entity was changed while we were viewing it, 
                    // Wrap Nhibernate exception and then send it 
                    // back up the calling stack 

                    throw new ConcurrencyException("Please reload the screen as changes have been made to an entity since first viewing.", NHex);
                }
                catch (Exception ex)
                {
                    _Transaction.Rollback();
                    // TODO: Log exception 
                    throw ex;

                }
            }
            else
            {
                throw new Exception("Aucune transaction ouverte");
            }

        }

        public void Delete(IAggregateRoot Entity)
        {
            _Session.Delete(Entity);
        }

    }

}
