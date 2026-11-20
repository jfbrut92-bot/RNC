using System;
using System.Collections.Generic;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Infrastructure;

namespace Utac.Rnc2.Contrats.Services

{
    public partial interface IpCourrierEnvoyeService : IService<pCourrierEnvoye>
    {
        pCourrierEnvoye nouveau(rUtilisateur declencheur, rUtilisateur destinataire);
        void modifier(pCourrierEnvoye courrier);

        IList<pCourrierEnvoye> FindAllByDate(DateTime debut, DateTime fin);
    }
}
