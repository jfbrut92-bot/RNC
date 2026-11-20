using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Repositories;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Services
{
    public class pInfoRdvService : BaseService<pInfoRdv>, IpInfoRdvService
    {
        private readonly IpCentreEncourService _centreEncourService;
        private readonly IrMotifModificationService _motifModificationService;

        public pInfoRdvService(
            IpInfoRdvRepository repository,
            IUnitOfWork UnitOfWork,
            IpCentreEncourService centreEncourService,
            IrMotifModificationService motifModificationService
            )
            : base(repository, UnitOfWork)
        {
            this._centreEncourService = centreEncourService;
            this._motifModificationService = motifModificationService;
        }

        //Pour les centres indépendants
        public void nouveauRdv(pCentreEncour centreForm, pInfoRdv pInfoRdv, string codeUtilisateur)
        {
            // chargement du centre
            pCentreEncour centre = _centreEncourService.FindBy(centreForm.Id);

            IUnitOfWork transaction = this.getUnitOfWork();

            // on historise
            this._centreEncourService.historiserCentre(centre);

            // On renseigne les informations de la prise de RDV
            pInfoRdv.PCentreEncour = centre;

            // On sauvegarde l'amende
            this.Save(pInfoRdv);

            // on modifie les données du centre
            centre.NumFax = centreForm.NumFax;
            centre.NumTel = centreForm.NumTel;
            rMotifModification motif = this._motifModificationService.FindBy(rMotifModification.TypeModification.RDV.ToString());
            this._centreEncourService.modifSuiviCentre(centre, motif, codeUtilisateur);

            if (transaction != null)
                transaction.Commit();

        }

        public void modifierInfoRdv(pCentreEncour centreForm, pInfoRdv pInfoRdv, string codeUtilisateur)
        {
            // chargement du centre
            pCentreEncour centre = _centreEncourService.FindBy(centreForm.Id);

            IUnitOfWork transaction = this.getUnitOfWork();

            // on historise
            this._centreEncourService.historiserCentre(centre);

            // On renseigne les informations de la prise de RDV
            pInfoRdv infoRDV = centre.PInfoRdvs[0];
            infoRDV.NumTelCentrale = pInfoRdv.NumTelCentrale;
            infoRDV.NumTelTarif = pInfoRdv.NumTelTarif;
            infoRDV.ServeurMinitel = pInfoRdv.ServeurMinitel;
            infoRDV.SiteWeb = pInfoRdv.SiteWeb;

            // On sauvegarde les informations de la prise de RDV
            this.Save(infoRDV);

            // on modifie les données du centre
            centre.NumFax = centreForm.NumFax;
            centre.NumTel = centreForm.NumTel;
            rMotifModification motif = this._motifModificationService.FindBy(rMotifModification.TypeModification.RDV.ToString());
            this._centreEncourService.modifSuiviCentre(centre, motif, codeUtilisateur);

            if (transaction != null)
                transaction.Commit();
        }

        //Pour les réseaux
        public void nouveauRdv(rReseau reseau, pInfoRdv pInfoRdv, string codeUtilisateur)
        {
            IUnitOfWork transaction = this.getUnitOfWork();

            //On associe avec le reseau
            pInfoRdv.RReseau = reseau;

            // On sauvegarde l'amende
            this.Save(pInfoRdv);

            if (transaction != null)
                transaction.Commit();

        }

        public void modifierInfoRdv(rReseau reseau, pInfoRdv pInfoRdv, string codeUtilisateur)
        {
            IUnitOfWork transaction = this.getUnitOfWork();

            // On renseigne les informations de la prise de RDV
            pInfoRdv infoRDV = this.FindBy(pInfoRdv.Id); //Pour avoir l'objet gérer par hibernate

            infoRDV.NumTelCentrale = pInfoRdv.NumTelCentrale;
            infoRDV.NumTelTarif = pInfoRdv.NumTelTarif;
            infoRDV.ServeurMinitel = pInfoRdv.ServeurMinitel;
            infoRDV.SiteWeb = pInfoRdv.SiteWeb;

            // On sauvegarde les informations de la prise de RDV
            this.Save(infoRDV);

            if (transaction != null)
                transaction.Commit();
        }
    }

}
