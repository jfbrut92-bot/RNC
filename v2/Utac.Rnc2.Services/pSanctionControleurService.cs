using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Repositories;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Services
{
    public class pSanctionControleurService : BaseService<pSanctionControleur>, IpSanctionControleurService
    {

        private readonly IpControleurEnCourService _controleurEncourService;
        private readonly IrUtilisateurService _utilisateurService;

        public pSanctionControleurService(
            IpSanctionControleurRepository repository,
            IUnitOfWork UnitOfWork,
            IpControleurEnCourService controleurEncourService,
            IrUtilisateurService utilisateurService
            )
            : base(repository, UnitOfWork)
        {
            this._controleurEncourService = controleurEncourService;
            this._utilisateurService = utilisateurService;
        }

        public pSanctionControleur nouvelleSanction(pContoleurEncour controleur, pSanctionControleur sanction, string codeUtilisateur)
        {
            // chargement du controleurForm
            pContoleurEncour c = _controleurEncourService.FindBy(controleur.Id);

            IUnitOfWork transaction = this.getUnitOfWork();

            //Dans tous les cas on modifie les données du contrôleur pour suivi des changements, cette méthode historise le controleurForm
            this._controleurEncourService.modifSuivi(controleur, rTypeDemandeControleur.TypeModification.SANCT, codeUtilisateur);

            // si sanction RETRAIT_DEFINITIF = 1
            if (sanction.RTypeSanctionControleur.Id == (int)rTypeSanctionControleur.MotifSanctionEnum.RETRAIT_DEFINITIF)
            {
                // on annule l'agrément , on modifie les données du centre
                this._controleurEncourService.annulerAgrementSuiteSanction(controleur, sanction, codeUtilisateur);
            }

            // On renseigne les informations de la sanction 
            sanction.PContoleurEncour = controleur;

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

        public pSanctionControleur modifierSanction(pSanctionControleur sanction, string codeUtilisateur)
        {
            // chargement du centre
            pContoleurEncour c = _controleurEncourService.FindBy(sanction.PContoleurEncour.Id);

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

        public void deleteSanction(pSanctionControleur s)
        {
            IUnitOfWork transaction = this.getUnitOfWork();

            this.Remove(s);

            if (transaction != null)
                transaction.Commit();

        }

    }
}
