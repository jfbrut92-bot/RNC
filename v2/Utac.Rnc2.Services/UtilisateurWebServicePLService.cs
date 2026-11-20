using System.Collections.Generic;
using System.Linq;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Repositories;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Infrastructure;
using Utac.Rnc2.Infrastructure.Queries;

namespace Utac.Rnc2.Services
{
    public partial class UtilisateurWebServicePLService : BaseService<UtilisateurWebServicePL>, IUtilisateurWebServicePLService
    {

        public UtilisateurWebServicePLService(IUtilisateurWebServicePLRepository repository, IUnitOfWork UnitOfWork) : base(repository, UnitOfWork)
        {

        }

        public UtilisateurWebServicePL FindByCodeReseau(string codeReseau)
        {
            Query q = new Query();
            q.Criteria.Add(new Criteria("CodReseau", codeReseau, CriteriaOperator.EQUALS));
            IList<UtilisateurWebServicePL> liste = this.FindBy(q);
            if (liste != null && liste.Count > 0)
            {
                return liste.ElementAt(0);
            }

            return null;
        }

        public UtilisateurWebServicePL FindByLogin(string login)
        {
            Query q = new Query();
            q.Criteria.Add(new Criteria("Login", login, CriteriaOperator.EQUALS));
            IList<UtilisateurWebServicePL> liste = this.FindBy(q);
            if (liste != null && liste.Count > 0)
            {
                return liste.ElementAt(0);
            }

            return null;
        }

        public void ajouterUtilisateur(UtilisateurWebServicePL newUser)
        {
            newUser.DateCreation = System.DateTime.Now;
            this.Save(newUser);

            IUnitOfWork transaction = this.getUnitOfWork();
            if (transaction != null)
            {
                transaction.Commit();
            }
        }
    }

}
