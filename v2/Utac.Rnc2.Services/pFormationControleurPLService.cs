using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Repositories;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Services
{
    public class pFormationControleurPLService : BaseService<pFormationControleurPL>, IpFormationControleurPLService
    {

        private readonly IpControleurEnCourService _controleurEnCourService;
        private readonly IrUtilisateurService _utilisateurService;

        public pFormationControleurPLService(
            IpFormationControleurPLRepository repository,
            IUnitOfWork UnitOfWork,
            IpControleurEnCourService controleurEnCourService,
            IrUtilisateurService utilisateurService
            )
            : base(repository, UnitOfWork)
        {
            this._controleurEnCourService = controleurEnCourService;
            this._utilisateurService = utilisateurService;
        }

        public pFormationControleurPL nouvelFormation(pContoleurEncour controleur, pFormationControleurPL formation, string codeUtilisateur)
        {
            // chargement du centre
            pContoleurEncour c = _controleurEnCourService.FindBy(controleur.Id);
            IUnitOfWork transaction = this.getUnitOfWork();

            // On renseigne les informations de la formation
            formation.Controleur = controleur;
            formation.RUtilisateur = this._utilisateurService.FindBy(codeUtilisateur);
            formation.DateMiseaJour = System.DateTime.Now;

            // On sauvegarde l'amende
            this.Save(formation);

            // on modifie les données du centre
            this._controleurEnCourService.setUnitOfWorks(transaction);
            this._controleurEnCourService.modifSuivi(controleur, rTypeDemandeControleur.TypeModification.FORMAPL, codeUtilisateur);

            if (transaction != null)
                transaction.Commit();

            return formation;
        }

        public pFormationControleurPL modifierFormation(int id, pFormationControleurPL formation, string codeUtilisateur)
        {

            IUnitOfWork transaction = this.getUnitOfWork();

            // On sauvegarde la formation
            pFormationControleurPL f = this.FindBy(id);
            f.DateDebut = formation.DateDebut;
            f.DateFin = formation.DateFin;
            f.DateMiseaJour = System.DateTime.Now;
            f.RTypeFormation = formation.RTypeFormation;
            f.ResultatFormation = formation.ResultatFormation;
            f.RUtilisateur = this._utilisateurService.FindBy(codeUtilisateur);
            this.Save(f);

            if (transaction != null)
                transaction.Commit();

            return f;
        }

        public void deleteFormation(pFormationControleurPL formation)
        {
            IUnitOfWork transaction = this.getUnitOfWork();

            this.Remove(formation);

            if (transaction != null)
                transaction.Commit();

        }

    }

}
