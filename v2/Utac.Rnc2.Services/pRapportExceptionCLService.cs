using System;
using System.Collections.Generic;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Repositories;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Services
{
    public class pRapportExceptionCLService : BaseService<pRapportExceptionCl>, IpRapportExceptionCLService
    {

        public pRapportExceptionCLService(IpRapportExceptionCLRepository repository, IUnitOfWork UnitOfWork) : base(repository, UnitOfWork)
        {
        }

        public void traceTelechargement(int id, string codeUtilisateur)
        {
            IUnitOfWork transaction = this.getUnitOfWork();
            if (transaction != null)
            {
                transaction.BeginTran();
            }

            pRapportExceptionCl rapport = this.FindBy(id);

            rapport.DateTelechargementRapport = System.DateTime.Now;
            rapport.CodUtilisateurTelechargement = codeUtilisateur;

            this.Save(rapport);
            if (transaction != null)
            {
                transaction.CommitTran();
            }

        }

        public IList<pRapportExceptionCl> FindAllByCentre(pCentreEncour centre)
        {
            if (centre == null) return null;
            return this.FindAllByCentre(centre.Id);
        }

        public IList<pRapportExceptionCl> FindAllByCentre(int idCentre)
        {
            //PLE 20/06/2012
            //Query q = new Query();
            //q.Criteria.Add(new Criteria("PCentreEncour.Id" , idCentre , CriteriaOperator.EQUALS));
            //q.OrderBy.Add(new OrderBy() { PropertyName = "DateDebutTransmissionPv", Ascending = false });
            //return this.FindBy(q);

            return this.findListRapportCL(" where id_centre_encours = " + Convert.ToString(idCentre));
        }

        public IList<pRapportExceptionCl> findListRapportCL(string q)
        {
            IpRapportExceptionCLRepository repository = this._Repository as IpRapportExceptionCLRepository;
            return repository.findListRapportCL(q);
        }

    }
}
