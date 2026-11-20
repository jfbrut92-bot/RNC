using System.Collections.Generic;
using System.Linq;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Repositories;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Infrastructure;
using Utac.Rnc2.Infrastructure.Queries;

namespace Utac.Rnc2.Services
{
    public class rReseauAppartenanceService : BaseService<rReseauAppartenance>, IrReseauAppartenanceService
    {

        public rReseauAppartenanceService(IrReseauAppartenanceRepository repository, IUnitOfWork UnitOfWork) : base(repository, UnitOfWork)
        {
        }

        public void supprimeAppartenance(rUtilisateur utilisateur, rReseau reseau)
        {
            supprimeAppartenance(utilisateur.Id, reseau.Id);
        }

        public void supprimeAppartenance(string codeUtilisateur, string codeReseau)
        {
            Query q = new Query();
            q.Criteria.Add(new Criteria("CodUtilisateur", codeUtilisateur, CriteriaOperator.EQUALS));
            q.Criteria.Add(new Criteria("CodReseau", codeReseau, CriteriaOperator.EQUALS));

            IList<rReseauAppartenance> liste = this.FindBy(q);
            if (liste.Count > 0)
            {
                rReseauAppartenance r = this.FindBy(q).ElementAt(0);
                this.Remove(r);
            }

        }

        public void ajouteAppartenance(string codeUtilisateur, string codeReseau)
        {

            rReseauAppartenance r = new rReseauAppartenance() { CodUtilisateur = codeUtilisateur, CodReseau = codeReseau };
            this.Save(r);

        }
    }
}
