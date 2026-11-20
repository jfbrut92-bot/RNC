using System;
using System.Text;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Repositories;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Services
{
    public class rProfilService : BaseService<rProfil>, IrProfilService
    {
        private readonly IrProfilRepository _ProfilRepository;
        private readonly IUnitOfWork _UnitOfWork;

        public rProfilService(IrProfilRepository ProfilRepository, IUnitOfWork UnitOfWork) : base(ProfilRepository, UnitOfWork)
        {
            this._ProfilRepository = ProfilRepository;
            this._ProfilRepository.Inject(UnitOfWork);
            this._UnitOfWork = UnitOfWork;
        }

        private string BuildErrorListFrom(rProfil Profil)
        {

            StringBuilder brokenRulesList = new StringBuilder();

            foreach (BrokenRule brokenRule in Profil.GetBrokenRules())
            {
                brokenRulesList.Append(brokenRule.Rule);
                brokenRulesList.Append(Environment.NewLine);
            }

            return brokenRulesList.ToString();
        }

    }
}
