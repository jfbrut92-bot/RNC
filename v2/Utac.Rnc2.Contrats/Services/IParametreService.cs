using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Contrats.Services

{
    public partial interface IParametreService : IService<Parametre>
    {

        void modifyParametre(int id, Parametre p);

        void suppression(Parametre p);
        void suppression(int id);
    }
}
