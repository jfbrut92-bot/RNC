using System.Collections.Generic;
using System.Linq;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Repositories;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Infrastructure;
using Utac.Rnc2.Infrastructure.Queries;

namespace Utac.Rnc2.Services
{
    public class rProfil2Service : BaseService<rProfil2>, IrProfil2Service
    {

        public rProfil2Service(IrProfil2Repository ProfilRepository, IUnitOfWork UnitOfWork) : base(ProfilRepository, UnitOfWork)
        {

        }

        public IList<rProfil2> findAllByProfil(rProfil profil)
        {
            Query q = new Query();
            q.Criteria.Add(new Criteria("RProfil.Id", profil.Id, CriteriaOperator.EQUALS));

            return this.FindBy(q);
        }

        public rProfil2 FindByLibelle(string libelleProfil)
        {
            Query q = new Query();
            q.Criteria.Add(new Criteria("LibelleProfil", libelleProfil, CriteriaOperator.EQUALS));

            List<rProfil2> l = this.FindBy(q);
            if (l != null && l.Count > 0)
            {
                return l.ElementAt(0);
            }

            return null;
        }
    }
}
