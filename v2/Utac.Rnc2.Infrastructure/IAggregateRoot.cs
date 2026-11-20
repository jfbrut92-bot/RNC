namespace Utac.Rnc2.Infrastructure
{
    public interface IAggregateRoot
    {
        //EG le 18/02/2012 pour pouvoir tracer les ID retournés par Nhibernate
        object getId();
    }
}
