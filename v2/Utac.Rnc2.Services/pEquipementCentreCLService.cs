using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Repositories;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Services
{
    public class pEquipementCentreCLService : BaseService<pEquipementCentreCL>, IpEquipementCentreCLService
    {

        private readonly IpCentreEncourService _centreService;
        private readonly IrMotifModificationService _motifModificationService;

        public pEquipementCentreCLService(
            IpEquipementCentreCLRepository repository,
            IUnitOfWork UnitOfWork,
            IpCentreEncourService centreEncourService,
            IrMotifModificationService motifModificationService
            )
            : base(repository, UnitOfWork)
        {
            this._centreService = centreEncourService;
            this._motifModificationService = motifModificationService;
        }

        public void nouveauEquipementCL(pCentreEncour centre, pEquipementCentreCL equipement, string codeUtilisateur)
        {
            // chargement du centre
            pCentreEncour c = _centreService.FindBy(centre.Id);

            IUnitOfWork transaction = this.getUnitOfWork();

            // on historise
            this._centreService.setUnitOfWorks(transaction);
            this._centreService.historiserCentre(c);

            // On renseigne les informations de l'équipement
            equipement.PCentreEnCour = centre;
            equipement.Id = centre.Id;

            // On sauvegarde l'équipement
            this.Save(equipement);

            //centre.EquipementCentreCL = equipement;

            // on modifie les données du centre
            rMotifModification motif = this._motifModificationService.FindBy(rMotifModification.TypeModification.EQUIPCL.ToString());
            this._centreService.modifSuiviCentre(centre, motif, codeUtilisateur);

            if (transaction != null)
                transaction.Commit();

        }

        public void modifierEquipementCL(pCentreEncour centre, pEquipementCentreCL equipement, string codeUtilisateur)
        {
            // chargement du centre
            pCentreEncour c = _centreService.FindBy(centre.Id);

            IUnitOfWork transaction = this.getUnitOfWork();
            if (transaction != null)
            {
                this._centreService.setUnitOfWorks(transaction);
                transaction.BeginTran();
            }

            // on historise

            this._centreService.historiserCentre(c);

            // On renseigne les informations de l'équipement
            pEquipementCentreCL e = this.FindBy(centre.Id);
            e.ControleTricyles = equipement.ControleTricyles;
            e.LieuStockagePV = equipement.LieuStockagePV;

            // on modifie les données du centre
            rMotifModification motif = this._motifModificationService.FindBy(rMotifModification.TypeModification.EQUIPCL.ToString());
            this._centreService.modifSuiviCentre(centre, motif, codeUtilisateur);

            // On sauvegarde l'équipement
            this.Save(e);

            if (transaction != null)
                transaction.CommitTran();
        }

    }

}
