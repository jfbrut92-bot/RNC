using System.Collections.Generic;
using System.Linq;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Repositories;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Infrastructure;
using Utac.Rnc2.Infrastructure.Queries;

namespace Utac.Rnc2.Services
{
    public class rTemplateMailService : BaseService<rTemplateMail>, IrTemplateMailService
    {

        public rTemplateMailService(
            IrTemplateMailRepository repository,
            IUnitOfWork UnitOfWork
            )
            : base(repository, UnitOfWork)
        {
        }

        public rTemplateMail ajouter(rTemplateMail template)
        {
            IUnitOfWork transaction = this.getUnitOfWork();

            this.Save(template);

            if (transaction != null)
            {
                transaction.Commit();
            }

            return template;
        }

        public void modifier(rTemplateMail template)
        {
            IUnitOfWork transaction = this.getUnitOfWork();

            this.Save(template);

            if (transaction != null)
            {
                transaction.Commit();
            }

        }

        public rTemplateMail FindByName(string codeTemplate)
        {
            Query q = new Query();
            q.Criteria.Add(new Criteria("NomTemplate", codeTemplate, CriteriaOperator.EQUALS));
            IList<rTemplateMail> liste = this.FindBy(q);

            if (liste != null && liste.Count > 0)
            {
                return liste.ElementAt(0);

            }
            return null;
        }
    }
}
