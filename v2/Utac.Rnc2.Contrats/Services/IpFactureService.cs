using System.Collections.Generic;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Contrats.Services

{
    public interface IpFactureService : IService<pFacture>
    {
        IList<pFacture> FindAllForCNR();
        IList<pFacture> FindAllByCentre(pCentreEncour centre);
        IList<pFacture> FindAllByCentre(int idCentre);
        void traceTelechargement(int id, string codeUtilisateur);
        void traceTelechargement(pFacture facture, string codeUtilisateur);
        IList<pFacture> FindListFacture(string q);
        IList<pFacture> FindAllByReseau(string cod_reseau);
    }
}
