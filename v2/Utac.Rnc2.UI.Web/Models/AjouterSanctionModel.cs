using System.Collections.Generic;
using Utac.Rnc2.BusinessObjects;

namespace Utac.Rnc2.UI.Web.Models
{
    public class AjouterSanctionModel
    {
        public pCentreEncour Centre { get; set; }

        public pSanctionCentre Sanction { get; set; }

        public IList<rTypeSanctionCentre> ListeSanction { get; set; }

        public IList<rTypeOrganismeDemandeur> ListeOrganismeDemandeur { get; set; }

    }
}