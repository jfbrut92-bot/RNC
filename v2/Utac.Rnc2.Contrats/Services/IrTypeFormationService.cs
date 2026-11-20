using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Infrastructure;
using System.Collections.Generic;

namespace Utac.Rnc2.Contrats.Services
{
    public interface IrTypeFormationService : IService<rTypeFormation>
    {
        List<rTypeFormation> FindSelonCodCategorieVeh(rCategorieVehicule.CategorieEnum categorie);
    }
}
