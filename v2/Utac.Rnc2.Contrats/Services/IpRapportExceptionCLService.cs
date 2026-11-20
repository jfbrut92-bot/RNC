using System.Collections.Generic;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Contrats.Services
{
    public interface IpRapportExceptionCLService : IService<pRapportExceptionCl>
    {
        //EG le 28/11/2011
        void traceTelechargement(int id, string codeUtilisateur);
        IList<pRapportExceptionCl> FindAllByCentre(pCentreEncour centre);
        IList<pRapportExceptionCl> FindAllByCentre(int idCentre);
    }
}
