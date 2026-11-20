using System.Collections.Generic;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Contrats.Repositories
{
    public interface IpFactureRepository : IRepository<pFacture>
    {
        void traceTelechargement(pFacture facture, string codeUtilisateur);
        IList<pFacture> FindListFacture(string q);
    }
}

