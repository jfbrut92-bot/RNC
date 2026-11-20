using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Repositories;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Services
{
    public class pFormationControleurCLService : BaseService<pFormationControleurCL>, IpFormationControleurCLService
    {

        private readonly IpControleurEnCourService _controleurEnCourService;
        private readonly IrUtilisateurService _utilisateurService;

        public pFormationControleurCLService(
            IpFormationControleurCLRepository repository,
            IUnitOfWork UnitOfWork,
            IpControleurEnCourService controleurEnCourService,
            IrUtilisateurService utilisateurService
            )
            : base(repository, UnitOfWork)
        {
            this._controleurEnCourService = controleurEnCourService;
            this._utilisateurService = utilisateurService;
        }

        public pFormationControleurCL nouvelFormation(pContoleurEncour controleur, pFormationControleurCL formation, string codeUtilisateur)
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
            this._controleurEnCourService.modifSuivi(controleur, rTypeDemandeControleur.TypeModification.FORMACL, codeUtilisateur);

            if (transaction != null)
                transaction.Commit();

            return formation;
        }

        public pFormationControleurCL modifierFormation(int id, pFormationControleurCL formation, string codeUtilisateur)
        {

            IUnitOfWork transaction = this.getUnitOfWork();

            // On sauvegarde la formation
            pFormationControleurCL f = this.FindBy(id);
            f.DateDebut = formation.DateDebut;
            f.DateFin = formation.DateFin;
            f.DateMiseaJour = System.DateTime.Now;
            f.ResultatFormation = formation.ResultatFormation;
            f.RUtilisateur = this._utilisateurService.FindBy(codeUtilisateur);
            this.Save(f);

            if (transaction != null)
                transaction.Commit();

            return f;
        }

        public void deleteFormation(pFormationControleurCL formation)
        {
            IUnitOfWork transaction = this.getUnitOfWork();

            this.Remove(formation);

            if (transaction != null)
                transaction.Commit();

        }

    }

}
