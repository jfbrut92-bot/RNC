using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Repositories;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Services
{
    public class pAuditControleurCLService : BaseService<pAuditControleurCL>, IpAuditControleurCLService
    {

        private readonly IpControleurEnCourService _controleurService;
        private readonly IrUtilisateurService _utilisateurService;

        public pAuditControleurCLService(
            IpAuditControleurCLRepository repository,
            IUnitOfWork UnitOfWork,
            IpControleurEnCourService controleurService,
            IrUtilisateurService utilisateurService
            )
            : base(repository, UnitOfWork)
        {
            this._controleurService = controleurService;
            this._utilisateurService = utilisateurService;
        }

        public pAuditControleurCL nouvelAudit(pContoleurEncour controleur, pAuditControleurCL audit, string codeUtilisateur)
        {
            IUnitOfWork transaction = this.getUnitOfWork();

            // On renseigne la cle primaire de l'audit
            audit.Controleur = controleur;
            audit.DateMiseaJour = System.DateTime.Now;

            //utilisateur qui a fait la modification
            audit.RUtilisateur = this._utilisateurService.FindBy(codeUtilisateur);
            // On sauvegarde l'audit
            this.Save(audit);

            // on modifie les données du controleurForm
            this._controleurService.setUnitOfWorks(transaction);
            this._controleurService.modifSuivi(controleur, rTypeDemandeControleur.TypeModification.AUDITVL, codeUtilisateur);

            if (transaction != null)
                transaction.Commit();

            return audit;
        }

        public pAuditControleurCL modifierAudit(int id, pAuditControleurCL audit, string codeUtilisateur)
        {

            IUnitOfWork transaction = this.getUnitOfWork();

            // On sauvegarde l'audit
            pAuditControleurCL a = this.FindBy(id);
            //a.CodAgremCntr = audit.CodAgremCntr;
            a.DateAudit = audit.DateAudit;
            a.DateMiseaJour = System.DateTime.Now;
            //a.ImmatVehicule = audit.ImmatVehicule;
            //a.NumPv = audit.NumPv;
            //a.RCategorieVehicule = audit.RCategorieVehicule;
            a.ResultatAudit = audit.ResultatAudit;
            a.RUtilisateur = this._utilisateurService.FindBy(codeUtilisateur);
            this.Save(a);

            if (transaction != null)
                transaction.Commit();

            return a;
        }

        public void deleteAudit(pAuditControleurCL audit)
        {
            IUnitOfWork transaction = this.getUnitOfWork();

            this.Remove(audit);

            if (transaction != null)
                transaction.Commit();
        }

    }
}
