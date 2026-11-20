using System.Collections.Generic;
using Utac.Rnc2.BusinessObjects;

namespace Utac.Rnc2.Contrats.Services
{
    public interface IrRegionService
    {
        List<rRegion> FindAll();
        rRegion FindBy(object Id);
        void Save(rRegion obj);
        void Remove(rRegion obj);
    }
}
