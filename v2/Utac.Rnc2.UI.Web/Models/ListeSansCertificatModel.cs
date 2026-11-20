using System.Collections.Generic;
using Utac.Rnc2.BusinessObjects;

namespace Utac.Rnc2.UI.Web.Models
{
    public class ListeSansCertificatModel
    {
        public IList<pCentreEncour> Liste { get; set; }
        public rCategorieVehicule.CategorieEnum Categorie { get; set; }

    }
}