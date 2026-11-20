using Utac.Rnc2.BusinessObjects;
using System.Collections.Generic;

namespace Utac.Rnc2.UI.Web.Models
{
    public class AjoutFormationControleurVLModel
    {
        public pFormationControleurVL Formation { get; set; }
        public pContoleurEncour Controleur { get; set; }
        public IList<rTypeFormation> ListeFormation { get; set; }

        public System.String ReturnLettreReseau { get; set; }

        public System.String ReturnCodDep { get; set; }
    }
}
