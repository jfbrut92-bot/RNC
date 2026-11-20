using System;
using System.Collections.Generic;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Contrats.Services

{
    public partial interface IpTraceRechPvVlService : IService<pTraceRechPvVl>
    {
        void TracerAppel(pTraceRechPvVl trace, string codeUtilisateur);

        //pTraceRechPvVl nouvelleTrace(pTraceRechPvVl trace, string codeUtilisateur);

        //pTraceRechPvVl modifierTrace(int id, pTraceRechPvVl trace, string codeUtilisateur);

        //void deleteTrace(pTraceRechPvVl trace);

        IList<pTraceRechPvVl> FindAllByDate(DateTime debut, DateTime fin);
    }
}
