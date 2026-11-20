using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Repositories;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Services
{
    public class pDerogationCentreService : BaseService<pDerogationCentre>, IpDerogationCentreService
    {

        private readonly IpCentreEncourService _centreEncours;
        private readonly IrMotifModificationService _motifModificationService;
        private readonly IrUtilisateurService _utilisateurService;

        public pDerogationCentreService(
            IpDerogationCentreRepository repository,
            IUnitOfWork UnitOfWork,
            IpCentreEncourService centreEncourService,
            IrMotifModificationService motifModificationService,
            IrUtilisateurService utilisateurService
            )
            : base(repository, UnitOfWork)
        {
            this._centreEncours = centreEncourService;
            this._motifModificationService = motifModificationService;
            this._utilisateurService = utilisateurService;
        }

        public pDerogationCentre nouvelleDerogation(pCentreEncour centre, pDerogationCentre derogation, rCategorieVehicule.CategorieEnum categorie, string codeUtilisateur)
        {
            // chargement du centre
            pCentreEncour c = _centreEncours.FindBy(centre.Id);

            IUnitOfWork transaction = this.getUnitOfWork();

            // on historise
            this._centreEncours.historiserCentre(c);

            // On recherche l'utilisateur
            rUtilisateur u = _utilisateurService.FindBy(codeUtilisateur);

            // On renseigne les informations de la dérogation
            derogation.PCentreEncour = centre;
            derogation.RUtilisateur = u;
            //EG le 28/02/2012
            derogation.CategorieVehicule = categorie;

            // On sauvegarde l'amende
            this.Save(derogation);

            // on modifie les données du centre
            rMotifModification motif = this._motifModificationService.FindBy(rMotifModification.TypeModification.DEROG.ToString());
            this._centreEncours.modifSuiviCentre(centre, motif, codeUtilisateur);

            if (transaction != null)
                transaction.Commit();

            return derogation;
        }

        public pDerogationCentre modifierDerogation(pDerogationCentre derogationForm, string codeUtilisateur)
        {
            //chargement de de la dérog
            pDerogationCentre derogation = this.FindBy(derogationForm.Id);

            // chargement du centre
            pCentreEncour c = derogation.PCentreEncour;

            IUnitOfWork transaction = this.getUnitOfWork();

            // On recherche l'utilisateur
            rUtilisateur u = _utilisateurService.FindBy(codeUtilisateur);

            //On modifie les données de la dérogation
            derogation.Commentaire = derogationForm.Commentaire;
            derogation.DateDebut = derogationForm.DateDebut;
            derogation.DateFin = derogationForm.DateFin;

            // On enregistre l'utilisateur qui modifie la dérogation
            derogation.RUtilisateur = u;

            // On sauvegarde la dérogation
            this.Save(derogation);

            // on modifie les données du centre
            rMotifModification motif = this._motifModificationService.FindBy(rMotifModification.TypeModification.MODDEROG.ToString());
            this._centreEncours.modifSuiviCentre(c, motif, codeUtilisateur);

            if (transaction != null)
                transaction.Commit();

            return derogation;
        }

        public void deleteDerogation(pDerogationCentre derogation, string codeUtilisateur)
        {
            pCentreEncour c = _centreEncours.FindBy(derogation.PCentreEncour.Id);

            IUnitOfWork transaction = this.getUnitOfWork();

            this.Remove(derogation);

            // on modifie les données du centre
            rMotifModification motif = this._motifModificationService.FindBy(rMotifModification.TypeModification.SUPDEROG.ToString());
            this._centreEncours.modifSuiviCentre(c, motif, codeUtilisateur);

            if (transaction != null)
                transaction.Commit();

        }

    }
}
