using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Contrats.Services

{
    public partial interface IpAuditControleurService : IService<pAuditControleur>
    {

        pAuditControleur nouvelAudit(pContoleurEncour controleur, pAuditControleur audit, string codeUtilisateur);

        pAuditControleur modifierAudit(int id, pAuditControleur audit, string codeUtilisateur);

        void deleteAudit(pAuditControleur audit);
    }
}
