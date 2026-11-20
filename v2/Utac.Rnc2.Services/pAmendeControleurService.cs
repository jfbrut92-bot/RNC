using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Repositories;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Services
{
    public class pAmendeControleurService : BaseService<pAmendeControleur>, IpAmendeControleurService
    {

        private readonly IpControleurEnCourService _controleurEnCourService;

        public pAmendeControleurService(
            IpAmendeControleurRepository repository,
            IUnitOfWork UnitOfWork,
            IpControleurEnCourService controleurEnCourService
            )
            : base(repository, UnitOfWork)
        {
            this._controleurEnCourService = controleurEnCourService;
        }

        public pAmendeControleur nouvelleAmende(pContoleurEncour controleur, pAmendeControleur amende, string codeUtilisateur)
        {
            // chargement du centre
            pContoleurEncour c = _controleurEnCourService.FindBy(controleur.Id);
            IUnitOfWork transaction = this.getUnitOfWork();

            // On renseigne les informations de l'amende
            amende.PContoleurEncour = controleur;

            // On sauvegarde l'amende
            this.Save(amende);

            // on modifie les données du centre
            this._controleurEnCourService.setUnitOfWorks(transaction);
            this._controleurEnCourService.modifSuivi(controleur, rTypeDemandeControleur.TypeModification.AMEND, codeUtilisateur);

            if (transaction != null)
                transaction.Commit();

            return amende;
        }

        public pAmendeControleur modifierAmende(pAmendeControleur amende, string codeUtilisateur)
        {
            // chargement du centre
            pContoleurEncour c = this._controleurEnCourService.FindBy(amende.PContoleurEncour.Id);

            IUnitOfWork transaction = this.getUnitOfWork();

            // On sauvegarde l'amende
            this.Save(amende);

            if (transaction != null)
                transaction.Commit();

            return amende;
        }

        public void deleteAmende(pAmendeControleur a)
        {
            IUnitOfWork transaction = this.getUnitOfWork();

            this.Remove(a);

            if (transaction != null)
                transaction.Commit();

        }

    }

}
