using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Contrats.Services
{
    public interface IpJournalActiviteService : IService<pJournalActivite>
    {
        pJournalActivite nouveauJournal(pCentreEncour centre, pJournalActivite journal, string codeUtilisateur);
        pJournalActivite nouveauJournal(pContoleurEncour controleur, pJournalActivite journal, string codeUtilisateur);
        void modifierJournal(pJournalActivite journal, string codeUtilisateur);
        void deleteJournal(pJournalActivite journal);
    }
}
