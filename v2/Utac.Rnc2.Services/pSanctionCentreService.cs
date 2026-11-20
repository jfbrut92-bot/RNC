using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Repositories;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Services
{
    public class pSanctionCentreService : BaseService<pSanctionCentre>, IpSanctionCentreService
    {

        private readonly IpCentreEncourService _centreEncours;
        private readonly IrMotifModificationService _motifModificationService;
        private readonly IrUtilisateurService _utilisateurService;

        public pSanctionCentreService(
            IpSanctionCentreRepository repository,
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

        public pSanctionCentre nouvelleSanction(pCentreEncour centre, pSanctionCentre sanction, string codeUtilisateur)
        {
            // chargement du centre
            pCentreEncour c = _centreEncours.FindBy(centre.Id);

            IUnitOfWork transaction = this.getUnitOfWork();

            // on historise
            this._centreEncours.historiserCentre(c);

            // si sanction RETRAIT_DEFINITIF = 1
            if (sanction.RTypeSanctionCentre.Id == (int)rTypeSanctionCentre.MotifSanctionEnum.RETRAIT_DEFINITIF)
            {
                // on annule l'agrément , on modifie les données du centre
                this._centreEncours.annulerAgrementSuiteSanction(centre, sanction, codeUtilisateur);
            }
            else
            {
                // on modifie les données du centre
                rMotifModification motif = this._motifModificationService.FindBy(rMotifModification.TypeModification.SANCT.ToString());
                this._centreEncours.modifSuiviCentre(centre, motif, codeUtilisateur);
            }

            // On renseigne les informations de la sanction 
            sanction.PCentreEncour = centre;

            if (sanction.IsAgrementAffecteVL)
            {
                sanction.CodAgremVl = c.CodAgremVl;
            }
            else
            {
                sanction.CodAgremVl = null;
            }

            if (sanction.IsAgrementAffectePL)
            {
                sanction.CodAgremPl = c.CodAgremPl;
            }
            else
            {
                sanction.CodAgremPl = null;
            }

            if (sanction.IsAgrementAffecteCL)
            {
                sanction.CodAgremCl = c.CodAgremCl;
            }
            else
            {
                sanction.CodAgremCl = null;
            }

            sanction.RUtilisateur = this._utilisateurService.FindBy(codeUtilisateur);

            // On sauvegarde la sanction
            this.Save(sanction);

            if (transaction != null)
                transaction.Commit();
            return sanction;
        }

        public pSanctionCentre modifierSanction(pSanctionCentre sanction, string codeUtilisateur)
        {
            // chargement du centre
            pCentreEncour c = _centreEncours.FindBy(sanction.PCentreEncour.Id);

            IUnitOfWork transaction = this.getUnitOfWork();

            if (sanction.IsAgrementAffecteVL)
            {
                sanction.CodAgremVl = c.CodAgremVl;
            }
            else
            {
                sanction.CodAgremVl = null;
            }

            if (sanction.IsAgrementAffectePL)
            {
                sanction.CodAgremPl = c.CodAgremPl;
            }
            else
            {
                sanction.CodAgremPl = null;
            }

            if (sanction.IsAgrementAffecteCL)
            {
                sanction.CodAgremCl = c.CodAgremCl;
            }
            else
            {
                sanction.CodAgremCl = null;
            }

            sanction.RUtilisateur = this._utilisateurService.FindBy(codeUtilisateur);

            // On sauvegarde la sanction
            this.Save(sanction);

            if (transaction != null)
                transaction.Commit();
            return sanction;
        }

        public void deleteSanction(pSanctionCentre s)
        {
            IUnitOfWork transaction = this.getUnitOfWork();

            this.Remove(s);

            if (transaction != null)
                transaction.Commit();

        }

    }
}
