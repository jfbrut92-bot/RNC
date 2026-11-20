using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Contrats.Services

{
    public partial interface IpAuditControleurCLService : IService<pAuditControleurCL>
    {

        pAuditControleurCL nouvelAudit(pContoleurEncour controleur, pAuditControleurCL audit, string codeUtilisateur);

        pAuditControleurCL modifierAudit(int id, pAuditControleurCL audit, string codeUtilisateur);

        void deleteAudit(pAuditControleurCL audit);
    }
}
