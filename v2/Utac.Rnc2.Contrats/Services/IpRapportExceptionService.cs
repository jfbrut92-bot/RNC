using System.Collections.Generic;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Contrats.Services
{
    public interface IpRapportExceptionService : IService<pRapportException>
    {
        IList<pRapportException> FindAllForCNR();
        IList<pRapportException> FindAllByCentre(pCentreEncour centre);
        IList<pRapportException> FindAllByCentre(int idCentre);
        void traceTelechargement(int id, string codeUtilisateur);
        void traceTelechargement(pRapportException rapport, string codeUtilisateur);
        IList<pRapportException> findListRapportPL(string q);
        IList<pRapportException> FindAllByReseau(string cod_reseau);

    }
}
