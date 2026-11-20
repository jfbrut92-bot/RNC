using System.Collections.Generic;
using System.Linq;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Repositories;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Infrastructure;
using Utac.Rnc2.Infrastructure.Queries;

namespace Utac.Rnc2.Services
{
    public class rPrefectureService : BaseService<rPrefecture>, IrPrefectureService
    {

        public rPrefectureService(IrPrefectureRepository repository, IUnitOfWork UnitOfWork)
            : base(repository, UnitOfWork)
        {

        }

        public rPrefecture findByCodeDepartement(string codeDepartement)
        {
            Query q = new Query();
            q.Criteria.Add(new Criteria("CodDepartement", codeDepartement, CriteriaOperator.EQUALS));

            //YM 04/10/2011 eviter index hors limite si codeDepartement inconnu
            IList<rPrefecture> p = this.FindBy(q);

            if (p.Count() == 0)
            {
                return null;
            }
            else
            {
                return p.ElementAt(0);
            }

            //return this.FindBy(q).ElementAt(0);
        }
    }
}
