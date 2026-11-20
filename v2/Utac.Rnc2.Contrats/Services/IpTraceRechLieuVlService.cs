using System;
using System.Collections.Generic;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Contrats.Services

{
    public partial interface IpTraceRechLieuVlService : IService<pTraceRechLieuVl>
    {
        void TracerAppel(pTraceRechLieuVl trace);
        IList<pTraceRechLieuVl> FindAllByDate(DateTime debut, DateTime fin);
    }
}
