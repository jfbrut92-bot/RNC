using System.Collections.Generic;
using Utac.Rnc2.BusinessObjects;

namespace Utac.Rnc2.UI.Web.Models
{
    public class AjoutFormationControleurPLModel
    {
        public pFormationControleurPL Formation { get; set; }
        public pContoleurEncour Controleur { get; set; }
        public IList<rTypeFormation> ListeFormation { get; set; }
        public System.String ReturnLettreReseau { get; set; }
        public System.String ReturnCodDep { get; set; }
    }
}
