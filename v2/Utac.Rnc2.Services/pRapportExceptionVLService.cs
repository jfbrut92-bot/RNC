using System;
using System.Collections.Generic;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Repositories;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Services
{
    public class pRapportExceptionVLService : BaseService<pRapportExceptionVl>, IpRapportExceptionVLService
    {

        public pRapportExceptionVLService(IpRapportExceptionVLRepository repository, IUnitOfWork UnitOfWork) : base(repository, UnitOfWork)
        {
        }

        public void traceTelechargement(int id, string codeUtilisateur)
        {
            IUnitOfWork transaction = this.getUnitOfWork();
            if (transaction != null)
            {
                transaction.BeginTran();
            }

            pRapportExceptionVl rapport = this.FindBy(id);

            rapport.DateTelechargementRapport = System.DateTime.Now;
            rapport.CodUtilisateurTelechargement = codeUtilisateur;

            this.Save(rapport);
            if (transaction != null)
            {
                transaction.CommitTran();
            }

        }

        public IList<pRapportExceptionVl> FindAllByCentre(pCentreEncour centre)
        {
            if (centre == null) return null;
            return this.FindAllByCentre(centre.Id);
        }

        public IList<pRapportExceptionVl> FindAllByCentre(int idCentre)
        {
            //PLE 20/06/2012
            //Query q = new Query();
            //q.Criteria.Add(new Criteria("PCentreEncour.Id" , idCentre , CriteriaOperator.EQUALS));
            //q.OrderBy.Add(new OrderBy() { PropertyName = "DateDebutTransmissionPv", Ascending = false });
            //return this.FindBy(q);

            return this.findListRapportVL(" where id_centre_encours = " + Convert.ToString(idCentre));
        }

        public IList<pRapportExceptionVl> findListRapportVL(string q)
        {
            IpRapportExceptionVLRepository repository = this._Repository as IpRapportExceptionVLRepository;
            return repository.findListRapportVL(q);
        }

    }
}
