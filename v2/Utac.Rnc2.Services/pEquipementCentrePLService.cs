using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Repositories;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Services
{
    public class pEquipementCentrePLService : BaseService<pEquipementCentrePL>, IpEquipementCentrePLService
    {

        private readonly IpCentreEncourService _centreService;
        private readonly IrMotifModificationService _motifModificationService;

        public pEquipementCentrePLService(
            IpEquipementCentrePLRepository repository,
            IUnitOfWork UnitOfWork,
            IpCentreEncourService centreEncourService,
            IrMotifModificationService motifModificationService
            )
            : base(repository, UnitOfWork)
        {
            this._centreService = centreEncourService;
            this._motifModificationService = motifModificationService;
        }

        public void nouveauEquipementPL(pCentreEncour centre, pEquipementCentrePL equipement, string codeUtilisateur)
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

            //centre.EquipementCentrePL = equipement;

            // on modifie les données du centre
            rMotifModification motif = this._motifModificationService.FindBy(rMotifModification.TypeModification.EQUIPPL.ToString());
            this._centreService.modifSuiviCentre(centre, motif, codeUtilisateur);

            if (transaction != null)
                transaction.Commit();

        }

        public void modifierEquipementPL(pCentreEncour centre, pEquipementCentrePL equipement, string codeUtilisateur)
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
            pEquipementCentrePL e = this.FindBy(centre.Id);
            e.ControleTCP = equipement.ControleTCP;
            e.ControleTMD = equipement.ControleTMD;
            e.Decelerometre = equipement.Decelerometre;
            e.LieuStockagePV = equipement.LieuStockagePV;

            // on modifie les données du centre
            rMotifModification motif = this._motifModificationService.FindBy(rMotifModification.TypeModification.EQUIPPL.ToString());
            this._centreService.modifSuiviCentre(centre, motif, codeUtilisateur);

            // On sauvegarde l'équipement
            this.Save(e);

            if (transaction != null)
                transaction.CommitTran();
        }

    }

}
