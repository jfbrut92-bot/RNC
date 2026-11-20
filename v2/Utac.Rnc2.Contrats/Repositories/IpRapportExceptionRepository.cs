using System.Collections.Generic;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Contrats.Repositories
{
    public interface IpRapportExceptionRepository : IRepository<pRapportException>
    {

        void traceTelechargement(pRapportException rapport, string codeUtilisateur);
        //PLE le 18/06/2012
        IList<pRapportException> findListRapportPL(string q);
    }
}