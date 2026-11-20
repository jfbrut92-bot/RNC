using System;
using System.Collections.Generic;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Repositories;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Infrastructure;
using Utac.Rnc2.Infrastructure.Queries;

namespace Utac.Rnc2.Services
{
    public partial class CommuneInseeService : BaseService<CommuneInsee>, ICommuneInseeService
    {

        public CommuneInseeService(
                                    ICommuneInseeRepository repository,
                                    IUnitOfWork UnitOfWork

            )
            : base(repository, UnitOfWork)
        {

        }

        public List<CommuneInsee> findByCodePostal(string codePostal)
        {
            Query q = new Query();
            q.Criteria.Add(new Criteria("CodePostal", codePostal + "%", CriteriaOperator.LIKE));

            return this.FindBy(q);
        }

        public List<CommuneInsee> findByVille(string ville)
        {
            Query q = new Query();
            q.Criteria.Add(new Criteria("NomCommune", ville + "%", CriteriaOperator.LIKE));

            return this.FindBy(q);
        }

        public Array findByCodePostalArray(string codePostal)
        {
            List<CommuneInsee> liste = this.findByCodePostal(codePostal);
            Array a = Array.CreateInstance(typeof(CommuneInseeResult), liste.Count);
            int i = 0;
            foreach (CommuneInsee c in liste)
            {
                a.SetValue(new CommuneInseeResult { CodePostal = c.CodePostal, Ville = c.NomCommune }, i);
                i++;
            }
            return a;
        }

        public Array findByVilleArray(string ville)
        {
            List<CommuneInsee> liste = this.findByVille(ville);
            Array a = Array.CreateInstance(typeof(CommuneInseeResult), liste.Count);
            int i = 0;
            foreach (CommuneInsee c in liste)
            {
                a.SetValue(new CommuneInseeResult { CodePostal = c.CodePostal, Ville = c.NomCommune }, i);
                i++;
            }
            return a;
        }

        private struct CommuneInseeResult
        {
            public string CodePostal;
            public string Ville;
        }

    }

}
