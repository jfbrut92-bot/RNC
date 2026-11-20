using System;
using System.Collections.Generic;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Contrats.Services

{
    public partial interface ICommuneInseeService : IService<CommuneInsee>
    {
        List<CommuneInsee> findByCodePostal(string codePostal);
        Array findByCodePostalArray(string codePostal);
        //EG le 07/12/2011
        List<CommuneInsee> findByVille(string ville);
        Array findByVilleArray(string ville);
    }
}
