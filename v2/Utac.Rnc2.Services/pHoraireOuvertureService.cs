using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Repositories;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Services
{
    public class pHoraireOuvertureService : BaseService<pHoraireOuverture>, IpHoraireOuvertureService
    {
        private readonly IpCentreEncourService _centreEncourService;
        private readonly IrMotifModificationService _motifModificationService;

        public pHoraireOuvertureService(
            IpHoraireOuvertureRepository repository,
            IUnitOfWork UnitOfWork,
            IpCentreEncourService centreEncourService,
            IrMotifModificationService motifModificationService
            )
            : base(repository, UnitOfWork)
        {
            this._centreEncourService = centreEncourService;
            this._motifModificationService = motifModificationService;
        }

        public void modifierHoraire(pCentreEncour model, pHoraireOuverture pHoraireOuverture, string codeUtilisateur)
        {
            // chargement du centre
            pCentreEncour centre = _centreEncourService.FindBy(model.Id);

            IUnitOfWork transaction = this.getUnitOfWork();

            // on historise
            this._centreEncourService.historiserCentre(centre);

            // On renseigne les informations des horaires
            pHoraireOuverture horaire = centre.PHoraireOuvertures[0];
            horaire.LundiMatin = pHoraireOuverture.LundiMatin;
            horaire.LundiSoir = pHoraireOuverture.LundiSoir;
            horaire.MardiMatin = pHoraireOuverture.MardiMatin;
            horaire.MardiSoir = pHoraireOuverture.MardiSoir;
            horaire.MercrediMatin = pHoraireOuverture.MercrediMatin;
            horaire.MercrediSoir = pHoraireOuverture.MercrediSoir;
            horaire.JeudiMatin = pHoraireOuverture.JeudiMatin;
            horaire.JeudiSoir = pHoraireOuverture.JeudiSoir;
            horaire.VendrediMatin = pHoraireOuverture.VendrediMatin;
            horaire.VendrediSoir = pHoraireOuverture.VendrediSoir;
            horaire.SamediMatin = pHoraireOuverture.SamediMatin;
            horaire.SamediSoir = pHoraireOuverture.SamediSoir;

            // On sauvegarde les informations de la prise de RDV
            this.Save(horaire);

            // on modifie les données du centre
            rMotifModification motif = this._motifModificationService.FindBy(rMotifModification.TypeModification.CHHORAI.ToString());
            this._centreEncourService.modifSuiviCentre(centre, motif, codeUtilisateur);

            if (transaction != null)
                transaction.Commit();
        }

        public void nouvelleHoraire(pCentreEncour model, pHoraireOuverture pHoraireOuverture, string codeUtilisateur)
        {
            // chargement du centre
            pCentreEncour centre = _centreEncourService.FindBy(model.Id);

            IUnitOfWork transaction = this.getUnitOfWork();

            // on historise
            this._centreEncourService.historiserCentre(centre);

            // On renseigne les informations de la prise de RDV
            pHoraireOuverture.Id = centre.Id;

            // On sauvegarde l'amende
            this.Save(pHoraireOuverture);

            // on modifie les données du centre
            rMotifModification motif = this._motifModificationService.FindBy(rMotifModification.TypeModification.RDV.ToString());
            this._centreEncourService.modifSuiviCentre(centre, motif, codeUtilisateur);

            if (transaction != null)
                transaction.Commit();
        }
    }

}
