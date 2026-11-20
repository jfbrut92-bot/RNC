using System.Collections.Generic;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Repositories;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Infrastructure;
using Utac.Rnc2.Infrastructure.Queries;

namespace Utac.Rnc2.Services
{
    public class rReseauService : BaseService<rReseau>, IrReseauService
    {

        private readonly IpGestionCertificatService _gestionCertificatService;

        public rReseauService(IrReseauRepository repository, IUnitOfWork UnitOfWork, IpGestionCertificatService gestionCertificatService) : base(repository, UnitOfWork)
        {
            this._gestionCertificatService = gestionCertificatService;
        }

        public List<rReseau> FindAllReseauPLSansZ()
        {
            Query q = new Query();
            q.Criteria.Add(new Criteria("this.TypeReseauPl", true, CriteriaOperator.EQUALS));
            q.Criteria.Add(new Criteria("this.Id", "Z", CriteriaOperator.NOT_EQUALS));
            return this.FindBy(q);
        }

        public List<rReseau> FindAllReseauVLSansZ()
        {
            Query q = new Query();
            q.Criteria.Add(new Criteria("this.TypeReseauVl", true, CriteriaOperator.EQUALS));
            q.Criteria.Add(new Criteria("this.Id", "Z", CriteriaOperator.NOT_EQUALS));
            return this.FindBy(q);
        }

        public List<rReseau> FindAllReseauCLSansW()
        {
            Query q = new Query();
            q.Criteria.Add(new Criteria("this.TypeReseauCl", true, CriteriaOperator.EQUALS));
            q.Criteria.Add(new Criteria("this.Id", "W", CriteriaOperator.NOT_EQUALS));
            return this.FindBy(q);
        }

        public List<rReseau> FindAllReseauPLAvecZ()
        {
            Query q = new Query();
            q.Criteria.Add(new Criteria("this.TypeReseauPl", true, CriteriaOperator.EQUALS));
            return this.FindBy(q);
        }

        public List<rReseau> FindAllReseauVLAvecZ()
        {
            Query q = new Query();
            q.Criteria.Add(new Criteria("this.TypeReseauVl", true, CriteriaOperator.EQUALS));
            return this.FindBy(q);
        }

        public List<rReseau> FindAllReseauCLAvecW()
        {
            Query q = new Query();
            q.Criteria.Add(new Criteria("this.TypeReseauCl", true, CriteriaOperator.EQUALS));
            return this.FindBy(q);
        }

        //EG le 10/11/2011
        public List<rReseau> FindAllReseauSansA()
        {
            Query q = new Query();
            q.Criteria.Add(new Criteria("this.Id", "A", CriteriaOperator.NOT_EQUALS));
            return this.FindBy(q);
        }

        //EG le 16/11/2011
        public List<rReseau> FindAllReseauSansAWZ()
        {
            Query q = new Query();
            string[] reseauFiltre = new string[] { "A", "W", "Z" };
            q.Criteria.Add(new Criteria("this.Id", reseauFiltre, CriteriaOperator.NOT_IN));
            return this.FindBy(q);
        }

        //EG le 22/11/2011
        public bool aUnCertificatValide(rReseau r)
        {
            pCertificat c;
            c = this._gestionCertificatService.findLastValide(r);
            if (c != null)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public rReseau ModifierParametreFacturationReseau(rReseau reseau, bool facturePapier, string adresseMail)
        {
            IUnitOfWork transaction = this.getUnitOfWork();

            reseau.FacturePapier = facturePapier;
            reseau.EmailFacturation = adresseMail;

            // On sauvegarde le réseau
            this.Save(reseau);

            if (transaction != null)
                transaction.Commit();

            return reseau;
        }

    }
}
