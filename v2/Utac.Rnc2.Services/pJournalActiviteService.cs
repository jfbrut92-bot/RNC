using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Repositories;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Services
{
    public class pJournalActiviteService : BaseService<pJournalActivite>, IpJournalActiviteService
    {
        private readonly IpCentreEncourService _centreEncourService;
        private readonly IpControleurEnCourService _controleurEncourService;
        private readonly IrUtilisateurService _utilisateurService;

        public pJournalActiviteService(
            IpJournalActiviteRepository repository,
            IUnitOfWork UnitOfWork,
            IpCentreEncourService centreEncourService,
            IpControleurEnCourService controleurEncourService,
            IrUtilisateurService utilisateurService
            )
            : base(repository, UnitOfWork)
        {
            this._centreEncourService = centreEncourService;
            this._controleurEncourService = controleurEncourService;
            this._utilisateurService = utilisateurService;
        }

        public pJournalActivite nouveauJournal(pCentreEncour centreForm, pJournalActivite journal, string codeUtilisateur)
        {
            // chargement du centre
            pCentreEncour centre = _centreEncourService.FindBy(centreForm.Id);

            IUnitOfWork transaction = this.getUnitOfWork();

            // On renseigne les informations du journal
            journal.CentreEncour = centre;
            journal.UtilisateurCreateur = this._utilisateurService.FindBy(codeUtilisateur);
            journal.DateCreation = System.DateTime.Now;

            // On sauvegarde le journal
            this.Save(journal);

            if (transaction != null)
                transaction.Commit();

            return journal;

        }

        public pJournalActivite nouveauJournal(pContoleurEncour controleurForm, pJournalActivite journal, string codeUtilisateur)
        {
            // chargement du centre
            pContoleurEncour controleur = this._controleurEncourService.FindBy(controleurForm.Id);

            IUnitOfWork transaction = this.getUnitOfWork();

            // On renseigne les informations du journal
            journal.ContoleurEncour = controleur;
            journal.UtilisateurCreateur = this._utilisateurService.FindBy(codeUtilisateur);
            journal.DateCreation = System.DateTime.Now;

            // On sauvegarde le journal
            this.Save(journal);

            if (transaction != null)
                transaction.Commit();

            return journal;
        }

        public void modifierJournal(pJournalActivite journalForm, string codeUtilisateur)
        {
            pJournalActivite journal = this.FindBy(journalForm.Id);

            journal.Detail = journalForm.Detail;
            journal.UtilisateurModification = this._utilisateurService.FindBy(codeUtilisateur);
            journal.DateModification = System.DateTime.Now;
            journal.Detail = journalForm.Detail;

            this.Save(journal);

            IUnitOfWork transaction = this.getUnitOfWork();

            if (transaction != null)
                transaction.Commit();

        }

        public void deleteJournal(pJournalActivite journal)
        {
            IUnitOfWork transaction = this.getUnitOfWork();

            this.Remove(journal);

            if (transaction != null)
                transaction.Commit();

        }
    }

}
