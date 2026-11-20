using System;
using System.Collections.Generic;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Repositories;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Services
{
    public class pRapportExceptionService : BaseService<pRapportException>, IpRapportExceptionService
    {

        public pRapportExceptionService(IpRapportExceptionRepository repository, IUnitOfWork UnitOfWork) : base(repository, UnitOfWork)
        {
        }

        public IList<pRapportException> FindAllForCNR()
        {
            //PLE le 18/06/2012
            //Query q = new Query();
            //q.Criteria.Add(new Criteria("CodAgremPl", null, CriteriaOperator.IS_NOT_NULL));
            //q.OrderBy.Add(new OrderBy() { PropertyName = "CodAgremPl" , Ascending = true });
            //q.OrderBy.Add(new OrderBy() { PropertyName = "DateDebutTransmissionPv", Ascending = true });
            //return this.FindBy(q);
            return this.findListRapportPL(" where not (p_rapport_exception.cod_agrem_ind = null)");

        }

        public IList<pRapportException> FindAllByCentre(pCentreEncour centre)
        {
            if (centre == null) return null;
            return this.FindAllByCentre(centre.Id);
        }

        public IList<pRapportException> FindAllByCentre(int idCentre)
        {
            //PLE le 18/06/2012
            //Query q = new Query();
            //q.Criteria.Add(new Criteria("PCentreEncour.Id", idCentre, CriteriaOperator.EQUALS));
            //q.OrderBy.Add(new OrderBy() { PropertyName = "DateDebutTransmissionPv", Ascending = false });
            //return this.FindBy(q);
            return this.findListRapportPL(" where p_rapport_exception.id_centre_encours = " + Convert.ToString(idCentre));
        }

        public IList<pRapportException> FindAllByReseau(string cod_reseau)
        {
            //PLE le 18/06/2012
            //Query q = new Query();
            //q.Criteria.Add(new Criteria("PCentreEncour.Id", idCentre, CriteriaOperator.EQUALS));
            //q.OrderBy.Add(new OrderBy() { PropertyName = "DateDebutTransmissionPv", Ascending = false });
            //return this.FindBy(q);
            return this.findListRapportPL(" where p_rapport_exception.cod_reseau = '" + Convert.ToString(cod_reseau) + "'");
        }

        public void traceTelechargement(int id, string codeUtilisateur)
        {

            pRapportException rapport = this.FindBy(id);
            this.traceTelechargement(rapport, codeUtilisateur);
        }

        public void traceTelechargement(pRapportException rapport, string codeUtilisateur)
        {
            IUnitOfWork transaction = this.getUnitOfWork();
            if (transaction != null)
            {
                transaction.BeginTran();
            }

            /*
            rapport.DateTelechargementRapport = System.DateTime.Now;
            rapport.CodUtilisateurTelechargement = codeUtilisateur;

            this.Save(rapport);
             */
            var repository = this._Repository as IpRapportExceptionRepository;
            repository.traceTelechargement(rapport, codeUtilisateur);

            if (transaction != null)
            {
                transaction.CommitTran();
            }
        }

        public IList<pRapportException> findListRapportPL(string q)
        {
            IpRapportExceptionRepository repository = this._Repository as IpRapportExceptionRepository;
            return repository.findListRapportPL(q);
        }
    }
}
