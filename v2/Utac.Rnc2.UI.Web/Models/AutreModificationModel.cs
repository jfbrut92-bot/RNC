using System.Collections.Generic;
using Utac.Rnc2.BusinessObjects;

namespace Utac.Rnc2.UI.Web.Models
{
    public class AutreModificationModel
    {
        public pCentreEncour Centre { get; set; }
        public IList<rStatutJuridique> ListeStatutJuridique { get; set; }
    }
}

