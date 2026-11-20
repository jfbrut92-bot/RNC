using Utac.Rnc2.BusinessObjects;

namespace Utac.Rnc2.UI.Web.Models
{
    public class AccuseReceptionModel
    {
        public string NumAccuseReception { get; set; }
        public string CodAgrementCentre { get; set; }
        public int CentreId { get; set; }
        public rCategorieVehicule.CategorieEnum CategorieVehicule { get; set; }
    }
}