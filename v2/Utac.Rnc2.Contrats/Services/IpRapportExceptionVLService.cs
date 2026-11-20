using System.Collections.Generic;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Contrats.Services
{
    public interface IpRapportExceptionVLService : IService<pRapportExceptionVl>
    {
        //EG le 28/11/2011
        void traceTelechargement(int id, string codeUtilisateur);
        IList<pRapportExceptionVl> FindAllByCentre(pCentreEncour centre);
        IList<pRapportExceptionVl> FindAllByCentre(int idCentre);
    }
}
