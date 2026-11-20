using System;
using System.Collections.Generic;
using System.Text;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Repositories;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Services
{
    public class rRegionService : IrRegionService
    {
        private readonly IrRegionRepository _regionRepository;
        private readonly IUnitOfWork _UnitOfWork;

        public rRegionService(IrRegionRepository regionRepository, IUnitOfWork UnitOfWork)
        {
            this._regionRepository = regionRepository;
            this._regionRepository.Inject(UnitOfWork);
            this._UnitOfWork = UnitOfWork;
        }

        public List<rRegion> FindAll()
        {
            return (List<rRegion>)_regionRepository.FindAll();
        }

        public void Save(rRegion region)
        {
            if (region.GetBrokenRules().Count == 0)
            {
                this._regionRepository.Save(region);
                _UnitOfWork.Commit();
            }
            else
            {

                string brokenRules = BuildErrorListFrom(region);

                throw new InvalidOperationException(string.Format("You cannot save the product because it has broken business rules. {0}", brokenRules));
            }
        }

        private string BuildErrorListFrom(rRegion region)
        {

            StringBuilder brokenRulesList = new StringBuilder();

            foreach (BrokenRule brokenRule in region.GetBrokenRules())
            {
                brokenRulesList.Append(brokenRule.Rule);
                brokenRulesList.Append(Environment.NewLine);
            }

            return brokenRulesList.ToString();
        }

        public rRegion FindBy(object Id)
        {
            return this._regionRepository.FindBy(Id);
        }

        public void Remove(rRegion region)
        {
            _regionRepository.Remove(region);
            _UnitOfWork.Commit();
        }

    }
}
