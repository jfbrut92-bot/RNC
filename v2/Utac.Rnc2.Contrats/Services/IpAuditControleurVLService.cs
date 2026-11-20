using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Contrats.Services

{
    public partial interface IpAuditControleurVLService : IService<pAuditControleurVL>
    {

        pAuditControleurVL nouvelAudit(pContoleurEncour controleur, pAuditControleurVL audit, string codeUtilisateur);

        pAuditControleurVL modifierAudit(int id, pAuditControleurVL audit, string codeUtilisateur);

        void deleteAudit(pAuditControleurVL audit);
    }
}
