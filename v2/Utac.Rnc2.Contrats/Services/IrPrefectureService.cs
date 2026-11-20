using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Contrats.Services
{
    public interface IrPrefectureService : IService<rPrefecture>
    {

        rPrefecture findByCodeDepartement(string codeDepartement);
    }
}
