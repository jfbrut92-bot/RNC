namespace Utac.Rnc2.Infrastructure
{
    public interface IUnitOfWork
    {
        void Save(IAggregateRoot Entity);
        void Delete(IAggregateRoot Entity);
        void Commit();
        void BeginTran();
        void CommitTran();
    }
}
