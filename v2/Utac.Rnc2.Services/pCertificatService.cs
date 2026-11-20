using System.Collections.Generic;
using System.Linq;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Repositories;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Infrastructure;
using Utac.Rnc2.Infrastructure.Queries;

namespace Utac.Rnc2.Services
{
    public class pCertificatService : BaseService<pCertificat>, IpCertificatService
    {

        private readonly IpGestionCertificatService _gestionCertificatService;
        private readonly IpCentreEncourService _centreEncourService;
        private readonly IrActionCertificatService _rActionCertificateService;
        private readonly IrUtilisateurService _utilisateurService;

        public pCertificatService(
                IpCertificatRepository repository,
                IUnitOfWork UnitOfWork,
                IpGestionCertificatService gestionCertificatService,
                IpCentreEncourService centreEncourService,
                IrActionCertificatService rActionCertificatService,
                IrUtilisateurService utilisateurService
            )
            : base(repository, UnitOfWork)
        {
            this._gestionCertificatService = gestionCertificatService;
            this._centreEncourService = centreEncourService;
            this._rActionCertificateService = rActionCertificatService;
            this._utilisateurService = utilisateurService;
        }

        /*
        public IList<pCertificat> findAllExpireOuRevoque(Query q)
        {
           
        }
        */

        public pCertificat findByIdAndNumeroSerie(int idReq, string numeroSerie)
        {
            IList<pCertificat> certificats;
            Query q = new Query();
            q.Criteria.Add(new Criteria("IdReq", idReq, CriteriaOperator.EQUALS));
            q.Criteria.Add(new Criteria("NumSerie", numeroSerie, CriteriaOperator.EQUALS));

            certificats = this.FindBy(q);

            if (certificats.Count() > 0)
            {
                return certificats.ElementAt(0);
            }
            else
            {
                return null;
            }
        }

        public void nouveauCertificat(pCertificat certificat, pCentreEncour centre, rCategorieVehicule.CategorieEnum categorieVehicule, string codeUtilisateur, bool estInitial)
        {
            IUnitOfWork transaction = this.getUnitOfWork();
            if (transaction != null)
            {
                this._gestionCertificatService.setUnitOfWorks(transaction);
                this._centreEncourService.setUnitOfWorks(transaction);
            }

            //créer l'enregistrement dans gestion certificat
            pGestionCertificat g = new pGestionCertificat();
            pCentreEncour c = this._centreEncourService.FindBy(centre.Id);

            g.PCertificat = certificat;
            if (estInitial)
                g.RActionCertificat = this._rActionCertificateService.FindBy((short)pGestionCertificat.CodeGestionCertificat.TELECHARGER_CERTI_INITIAL);
            else
                g.RActionCertificat = this._rActionCertificateService.FindBy((short)pGestionCertificat.CodeGestionCertificat.TELECHARGER_CERTI_RENOUVELE);
            switch (categorieVehicule)
            {
                case rCategorieVehicule.CategorieEnum.VL:
                    //EG commenté le 22/11/2011 car pas requis g.RReseau = c.ReseauRattachementVL;
                    g.PCentreEncourVL = c;
                    break;
                case rCategorieVehicule.CategorieEnum.PL:
                    //EG commenté le 22/11/2011 car pas requis g.RReseau = c.ReseauRattachementPL;
                    g.PCentreEncourPL = c;
                    break;
                case rCategorieVehicule.CategorieEnum.CL:
                    //EG commenté le 22/11/2011 car pas requis g.RReseau = c.ReseauRattachementCL;
                    g.PCentreEncourCL = c;
                    break;
            }

            g.DateMaj = System.DateTime.Now;
            g.RUtilisateur = this._utilisateurService.FindBy(codeUtilisateur);

            certificat.PGestionCertificats.Add(g);

            //On sauvegarde le certificat et les enfants (pgestioncertificat)
            base.Save(certificat);

            if (transaction != null)
                transaction.Commit();

        }

        public void nouveauCertificat(pCertificat certificat, rReseau reseau, string codeUtilisateur, bool estInitial)
        {
            IUnitOfWork transaction = this.getUnitOfWork();
            if (transaction != null)
            {
                this._gestionCertificatService.setUnitOfWorks(transaction);
                this._centreEncourService.setUnitOfWorks(transaction);
            }

            //créer l'enregistrement dans gestion certificat
            pGestionCertificat g = new pGestionCertificat();

            g.PCertificat = certificat;
            if (estInitial)
                g.RActionCertificat = this._rActionCertificateService.FindBy((short)pGestionCertificat.CodeGestionCertificat.TELECHARGER_CERTI_INITIAL);
            else
                g.RActionCertificat = this._rActionCertificateService.FindBy((short)pGestionCertificat.CodeGestionCertificat.TELECHARGER_CERTI_RENOUVELE);
            g.RReseau = reseau;
            g.DateMaj = System.DateTime.Now;
            g.RUtilisateur = this._utilisateurService.FindBy(codeUtilisateur);

            certificat.PGestionCertificats.Add(g);

            //On sauvegarde le certificat et les enfants (pgestioncertificat)
            base.Save(certificat);

            if (transaction != null)
                transaction.Commit();

        }

        public void traceTelechargementCertificat(pCertificat certificat, rReseau reseau, string codeUtilisateur)
        {
            IUnitOfWork transaction = this.getUnitOfWork();
            if (transaction != null)
            {
                this._gestionCertificatService.setUnitOfWorks(transaction);
                this._centreEncourService.setUnitOfWorks(transaction);
            }

            //créer l'enregistrement dans gestion certificat
            pGestionCertificat g = new pGestionCertificat();

            g.PCertificat = certificat;
            g.RActionCertificat = this._rActionCertificateService.FindBy((short)pGestionCertificat.CodeGestionCertificat.TELECHARGER_COIPIE_CERTI);
            g.RReseau = reseau;
            g.DateMaj = System.DateTime.Now;
            g.RUtilisateur = this._utilisateurService.FindBy(codeUtilisateur);

            certificat.PGestionCertificats.Add(g);

            //On sauvegarde le certificat et les enfants (pgestioncertificat)
            base.Save(certificat);

            if (transaction != null)
                transaction.Commit();

        }

        public void traceTelechargementCertificat(pCertificat certificat, pCentreEncour centre, rCategorieVehicule.CategorieEnum categorieVehicule, string codeUtilisateur)
        {
            IUnitOfWork transaction = this.getUnitOfWork();
            if (transaction != null)
            {
                this._gestionCertificatService.setUnitOfWorks(transaction);
                this._centreEncourService.setUnitOfWorks(transaction);
            }

            //créer l'enregistrement dans gestion certificat
            pGestionCertificat g = new pGestionCertificat();
            pCentreEncour c = this._centreEncourService.FindBy(centre.Id);

            g.PCertificat = certificat;
            g.RActionCertificat = this._rActionCertificateService.FindBy((short)pGestionCertificat.CodeGestionCertificat.TELECHARGER_COIPIE_CERTI);
            switch (categorieVehicule)
            {
                case rCategorieVehicule.CategorieEnum.VL:
                    g.PCentreEncourVL = c;
                    break;
                case rCategorieVehicule.CategorieEnum.PL:
                    g.PCentreEncourPL = c;
                    break;
                case rCategorieVehicule.CategorieEnum.CL:
                    g.PCentreEncourCL = c;
                    break;
            }

            g.DateMaj = System.DateTime.Now;
            g.RUtilisateur = this._utilisateurService.FindBy(codeUtilisateur);

            certificat.PGestionCertificats.Add(g);

            //On sauvegarde le certificat et les enfants (pgestioncertificat)
            base.Save(certificat);

            if (transaction != null)
                transaction.Commit();

        }

        public void revocationCertificat(pCertificat certificat, pCentreEncour centre, rCategorieVehicule.CategorieEnum categorieVehicule, bool autorisationRenew, string codeUtilisateur)
        {
            IUnitOfWork transaction = this.getUnitOfWork();
            if (transaction != null)
            {
                this._gestionCertificatService.setUnitOfWorks(transaction);
                this._centreEncourService.setUnitOfWorks(transaction);
            }

            //créer l'enregistrement dans gestion certificat
            pGestionCertificat g = new pGestionCertificat();
            pCentreEncour c = this._centreEncourService.FindBy(centre.Id);

            g.PCertificat = certificat;
            g.RActionCertificat = null;
            if (autorisationRenew)
                g.RActionCertificat = this._rActionCertificateService.FindBy((short)pGestionCertificat.CodeGestionCertificat.REVOCATION_CERTI);
            else
                g.RActionCertificat = this._rActionCertificateService.FindBy((short)pGestionCertificat.CodeGestionCertificat.REVOCATION_CERTI_SANS_RENOUVELLEMENT);

            switch (categorieVehicule)
            {
                case rCategorieVehicule.CategorieEnum.VL:
                    g.PCentreEncourVL = c;
                    break;
                case rCategorieVehicule.CategorieEnum.PL:
                    g.PCentreEncourPL = c;
                    break;
                case rCategorieVehicule.CategorieEnum.CL:
                    g.PCentreEncourCL = c;
                    break;
            }

            g.DateMaj = System.DateTime.Now;
            g.RUtilisateur = this._utilisateurService.FindBy(codeUtilisateur);

            certificat.PGestionCertificats.Add(g);

            //On sauvegarde le certificat et les enfants (pgestioncertificat)
            base.Save(certificat);

            if (transaction != null)
                transaction.Commit();

        }

        public void autorisationCertificat(pCertificat certificat, pCentreEncour centre, rCategorieVehicule.CategorieEnum categorieVehicule, string codeUtilisateur)
        {
            IUnitOfWork transaction = this.getUnitOfWork();
            if (transaction != null)
            {
                this._gestionCertificatService.setUnitOfWorks(transaction);
                this._centreEncourService.setUnitOfWorks(transaction);
            }

            //créer l'enregistrement dans gestion certificat
            pGestionCertificat g = new pGestionCertificat();
            pCentreEncour c = this._centreEncourService.FindBy(centre.Id);

            g.PCertificat = certificat;
            g.RActionCertificat = this._rActionCertificateService.FindBy((short)pGestionCertificat.CodeGestionCertificat.AUTORISATION_RENOUVELLEMENT);

            switch (categorieVehicule)
            {
                case rCategorieVehicule.CategorieEnum.VL:
                    g.PCentreEncourVL = c;
                    break;
                case rCategorieVehicule.CategorieEnum.PL:
                    g.PCentreEncourPL = c;
                    break;
                case rCategorieVehicule.CategorieEnum.CL:
                    g.PCentreEncourCL = c;
                    break;
            }

            g.DateMaj = System.DateTime.Now;
            g.RUtilisateur = this._utilisateurService.FindBy(codeUtilisateur);

            certificat.PGestionCertificats.Add(g);

            //On sauvegarde le certificat et les enfants (pgestioncertificat)
            base.Save(certificat);

            if (transaction != null)
                transaction.Commit();

        }

        public void revocationCertificat(pCertificat certificat, rReseau reseau, bool autorisationRenew, string codeUtilisateur)
        {
            IUnitOfWork transaction = this.getUnitOfWork();
            if (transaction != null)
            {
                this._gestionCertificatService.setUnitOfWorks(transaction);
                this._centreEncourService.setUnitOfWorks(transaction);
            }

            //créer l'enregistrement dans gestion certificat
            pGestionCertificat g = new pGestionCertificat();

            g.PCertificat = certificat;
            g.RActionCertificat = null;
            if (autorisationRenew)
                g.RActionCertificat = this._rActionCertificateService.FindBy((short)pGestionCertificat.CodeGestionCertificat.REVOCATION_CERTI);
            else
                g.RActionCertificat = this._rActionCertificateService.FindBy((short)pGestionCertificat.CodeGestionCertificat.REVOCATION_CERTI_SANS_RENOUVELLEMENT);

            g.RReseau = reseau;
            g.DateMaj = System.DateTime.Now;
            g.RUtilisateur = this._utilisateurService.FindBy(codeUtilisateur);

            certificat.PGestionCertificats.Add(g);

            //On sauvegarde le certificat et les enfants (pgestioncertificat)
            base.Save(certificat);

            if (transaction != null)
                transaction.Commit();

        }

        public void autorisationCertificat(pCertificat certificat, rReseau reseau, string codeUtilisateur)
        {
            IUnitOfWork transaction = this.getUnitOfWork();
            if (transaction != null)
            {
                this._gestionCertificatService.setUnitOfWorks(transaction);
                this._centreEncourService.setUnitOfWorks(transaction);
            }

            //créer l'enregistrement dans gestion certificat
            pGestionCertificat g = new pGestionCertificat();

            g.PCertificat = certificat;
            g.RActionCertificat = this._rActionCertificateService.FindBy((short)pGestionCertificat.CodeGestionCertificat.AUTORISATION_RENOUVELLEMENT);

            g.RReseau = reseau;

            g.DateMaj = System.DateTime.Now;
            g.RUtilisateur = this._utilisateurService.FindBy(codeUtilisateur);

            certificat.PGestionCertificats.Add(g);

            //On sauvegarde le certificat et les enfants (pgestioncertificat)
            base.Save(certificat);

            if (transaction != null)
                transaction.Commit();

        }
    }
}
