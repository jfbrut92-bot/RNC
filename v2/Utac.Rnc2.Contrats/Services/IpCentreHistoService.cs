using System;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Contrats.Services
{
    public interface IpCentreHistoService : IService<pCentreHisto>
    {

        void historise(pCentreEncour c);
        pCentreHisto findLastBy(object id);
        pCentreHisto findByIdAndDate(object id, DateTime dateHisto);
        void removeAll(pCentreEncour c);

    }
}
