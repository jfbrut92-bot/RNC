using System.Collections.Generic;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Contrats.Services
{
    public interface IrProfil2Service : IService<rProfil2>
    {

        IList<rProfil2> findAllByProfil(rProfil profil);

        rProfil2 FindByLibelle(string libelleProfil);
    }
}
