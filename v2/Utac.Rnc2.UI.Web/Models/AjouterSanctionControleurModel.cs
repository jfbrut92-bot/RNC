using System.Collections.Generic;
using Utac.Rnc2.BusinessObjects;

namespace Utac.Rnc2.UI.Web.Models
{
    public class AjouterSanctionControleurModel
    {
        public pContoleurEncour Controleur { get; set; }

        public pSanctionControleur Sanction { get; set; }

        public IList<rTypeSanctionControleur> ListeSanction { get; set; }

        public IList<rTypeOrganismeDemandeur> ListeOrganismeDemandeur { get; set; }

    }
}