using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Repositories;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Services
{
    public class pAmendeCentreService : BaseService<pAmendeCentre>, IpAmendeCentreService
    {

        private readonly IpCentreEncourService _centreEncours;
        private readonly IrMotifModificationService _motifModificationService;

        public pAmendeCentreService(
            IpAmendeCentreRepository repository,
            IUnitOfWork UnitOfWork,
            IpCentreEncourService centreEncourService,
            IrMotifModificationService motifModificationService
            )
            : base(repository, UnitOfWork)
        {
            this._centreEncours = centreEncourService;
            this._motifModificationService = motifModificationService;
        }

        public pAmendeCentre nouvelleAmende(pCentreEncour centre, pAmendeCentre amende, string codeUtilisateur)
        {
            // chargement du centre
            pCentreEncour c = _centreEncours.FindBy(centre.Id);

            IUnitOfWork transaction = this.getUnitOfWork();

            // on historise
            this._centreEncours.historiserCentre(c);

            // On renseigne les informations de l'amende
            amende.PCentreEncour = centre;

            // On sauvegarde l'amende
            this.Save(amende);

            // on modifie les données du centre
            rMotifModification motif = this._motifModificationService.FindBy(rMotifModification.TypeModification.AMEND.ToString());
            this._centreEncours.modifSuiviCentre(centre, motif, codeUtilisateur);

            if (transaction != null)
                transaction.Commit();

            return amende;
        }

        public pAmendeCentre modifierAmende(pAmendeCentre amende, string codeUtilisateur)
        {
            // chargement du centre
            pCentreEncour c = _centreEncours.FindBy(amende.PCentreEncour.Id);

            IUnitOfWork transaction = this.getUnitOfWork();

            // On sauvegarde l'amende
            this.Save(amende);

            if (transaction != null)
                transaction.Commit();

            return amende;
        }

        public void deleteAmende(pAmendeCentre a)
        {
            IUnitOfWork transaction = this.getUnitOfWork();

            this.Remove(a);

            if (transaction != null)
                transaction.Commit();

        }

    }

}
