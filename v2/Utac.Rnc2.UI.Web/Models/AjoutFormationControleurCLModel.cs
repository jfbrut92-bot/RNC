using System.Collections.Generic;
using Utac.Rnc2.BusinessObjects;

namespace Utac.Rnc2.UI.Web.Models
{
    public class AjoutFormationControleurCLModel
    {
        public pFormationControleurCL Formation { get; set; }
        public pContoleurEncour Controleur { get; set; }
        public IList<rTypeFormation> ListeFormation { get; set; }
    }
}
